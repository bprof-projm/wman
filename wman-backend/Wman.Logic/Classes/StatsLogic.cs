using ClosedXML.Excel;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.DTO_Models;
using Wman.Logic.Interfaces;
using Wman.Logic.Services;
using Wman.Repository.Interfaces;

namespace Wman.Logic.Classes
{
    public class StatsLogic : IStatsLogic
    {
        IWorkEventRepo eventRepo;
        IFileRepo fileRepo;
        IConfiguration configuration;
        IEmailService emailService;
        UserManager<WmanUser> userManager;
        public StatsLogic(IWorkEventRepo eventRepo, IFileRepo fileRepo,
            IConfiguration configuration, IEmailService emailService, UserManager<WmanUser> userManager)
        {
            this.eventRepo = eventRepo;
            this.fileRepo = fileRepo;
            this.configuration = configuration;
            this.emailService = emailService;
            this.userManager = userManager;
        }

        public async Task<ICollection<StatsXlsModel>> GetManagerStats(DateTime input)
        {
            List<WorkEvent> allCompletedThisMonth = new List<WorkEvent>();
            allCompletedThisMonth = await eventRepo.GetAll()
                .Where(x => x.Status == Status.finished &&
                x.WorkFinishDate.Year == input.Year &&
                x.WorkFinishDate.Month == input.Month)
                .ToListAsync();


            var output = new List<StatsXlsModel>();

            foreach (var job in allCompletedThisMonth)
            {
                foreach (var person in job.AssignedUsers)
                {
                    output.Add(new StatsXlsModel
                    {
                        JobDesc = job.JobDescription,
                        JobLocation = job.Address.ToString(),
                        JobStart = job.WorkStartDate,
                        JobEnd = job.WorkFinishDate,
                        WorkerName = person.LastName + " " + person.FirstName

                    });
                }
            }
            await this.makeManagerxls(output);
            return output;
        }

        public async Task<ICollection<ICollection<StatsXlsModel>>> GetWorkerStats(DateTime input)
        {
            var workers = await userManager.GetUsersInRoleAsync("Worker");
            var AllUsers = await userManager.Users
                .Include(x => x.WorkEvents)
                .ThenInclude(y => y.Address)
                .Include(z => z.WorkEvents)
                .ThenInclude(zs => zs.ProofOfWorkPic)
                .AsNoTracking()
                .ToListAsync();
            //Workaround, because userManager.GetUsersInRoleAsync("Worker") does not include the associated workevents, addresses, pictures...
            //The filtering for workers part happens on it's own, as theoretically only workers can be assigned to completed events.
            //Possible alternate solutions are:
            // 1.) Implementing our own, modified IUserStore<WmanUser>, which would include the events as well
            // 2.) Migrating to .NET / EFCORE 6, as it's possible to configure auto inclusion there

            var tempTest = new List<string>();
            var output = new List<ICollection<StatsXlsModel>>();
            foreach (var user in AllUsers)
            {
                var userCompletedJobs = user.WorkEvents.Where(x => x.Status == Status.finished &&
                x.WorkFinishDate.Year == input.Year &&
                x.WorkFinishDate.Month == input.Month);
                if (!userCompletedJobs.Any())
                {
                    continue;
                }
                var oneWorker = new List<StatsXlsModel>();
                foreach (var job in userCompletedJobs)
                {
                    var picUrls = string.Empty;
                    foreach (var pic in job.ProofOfWorkPic)
                    {
                        picUrls += pic.Url + ", ";
                    }
                    if (!string.IsNullOrWhiteSpace(picUrls))
                    {
                        picUrls = picUrls.Substring(0, picUrls.Length - 2); //remove last trailing comma
                    }
                    oneWorker.Add(new StatsXlsModel
                    {
                        JobDesc = job.JobDescription,
                        JobLocation = job.Address.ToString(),
                        JobStart = job.WorkStartDate,
                        JobEnd = job.WorkFinishDate,
                        WorkHours = 2,
                        PicUrl = picUrls,
                        WorkerName = user.UserName
                    });
                }
                output.Add(oneWorker);
                //await this.makeManagerxls(oneWorker); //TODO UNCOMMENT
            }
           
            return output;
        }

        public async Task makeManagerxls(List<StatsXlsModel> input)
        {

            using (var workbook = new XLWorkbook())
            {
                var sheet = workbook.Worksheets.Add("ManagerStats");
                var rowIndex = 1;
                sheet.Cell(rowIndex, 1).Value = "Worker's name";
                sheet.Cell(rowIndex, 2).Value = "Job Description";
                sheet.Cell(rowIndex, 3).Value = "Location";
                sheet.Cell(rowIndex, 4).Value = "Started at";
                sheet.Cell(rowIndex, 5).Value = "Finished at";
                foreach (var item in input)
                {
                    rowIndex++;
                    sheet.Cell(rowIndex, 1).Value = item.WorkerName;
                    sheet.Cell(rowIndex, 2).Value = item.JobDesc;
                    sheet.Cell(rowIndex, 3).Value = item.JobLocation;
                    sheet.Cell(rowIndex, 4).Value = item.JobStart;
                    sheet.Cell(rowIndex, 5).Value = item.JobEnd;
                }
                sheet.Row(1).Cells().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                sheet.Row(1).Cells().Style.Font.SetBold();
                sheet.Range(2, 4, sheet.LastRowUsed().RowNumber(), 5).Style.NumberFormat.Format = "yyyy.MM.dd. HH:mm";
                sheet.Columns().AdjustToContents();
                using (var ms = new MemoryStream())
                {
                    workbook.SaveAs(ms);
                    await fileRepo.Create(this.GetPath() + this.GetFilename(), ms);
                }

            }
            await this.SendEmails(this.GetFilename());
        }
        public async Task SendEmails(string fileName = "")
        {
            if (String.IsNullOrWhiteSpace(fileName)) //If no specific file is selected, assume the latest is needed
            {
                var folderContents = await fileRepo.GetDetails(this.GetPath());
                fileName = folderContents.GetFiles()
                    .Where(x => x.Name.ToLower().EndsWith(".xlsx"))
                    .OrderByDescending(x => x.LastWriteTime)
                    .First()
                    .FullName;
            }
            else
            {
                fileName = GetPath() + fileName;
            }

            var managers = await userManager.GetUsersInRoleAsync("Manager");
            foreach (var item in managers)
            {
                await emailService.SendXls(item, fileName);
            }
        }
        public async void registerRecurringJob(string input)
        {
            if (!int.TryParse(input, out _) || int.Parse(input) > 31) //No valid config value, use defaults
            {
                RecurringJob.RemoveIfExists("scheduledXlsReport");
                RecurringJob.AddOrUpdate("defaultCaseXls", () => this.GetManagerStats(DateTime.Now.AddDays(-2)), Cron.Monthly, TimeZoneInfo.Local); //No schedule provided, default is sending the prev. month's stats on the first day of current month
                Debug.WriteLine("\n--- No valid \"xlsSchedule\" tag found in appsettings.json, using defaults (1st day of each month)!--- \n");
            }
            else if (int.Parse(input) <= 0) // 0 or less value, disable scheduled emails
            {
                RecurringJob.RemoveIfExists("scheduledXlsReport");
                RecurringJob.RemoveIfExists("defaultCaseXls");
                Debug.WriteLine("\n--- Scheduled xls sending is disabled by \"xlsSchedule\" tag in appsettings.json!--- \n");
            }
            else //Valid value, schedule based on input (Every x days counting from the first of month)
            {
                var cronExpr = $"0 12 1/{input} * *";
                RecurringJob.AddOrUpdate("scheduledXlsReport", () => this.GetManagerStats(DateTime.Now), cronExpr, TimeZoneInfo.Local);
                RecurringJob.RemoveIfExists("defaultCaseXls");

                Debug.WriteLine($"\n--- Scheduled xls generation&sending every {input} days (From the beginning of the month)!--- \n");
            }
            await fileRepo.DeleteOldFiles(this.GetPath(), ".xlsx", DateTime.Now.AddMonths(-1)); //Delete every .xlsx file older than a month
        }
        private string GetFilename()
        {
            return this.GetFilename(DateTime.Now);
        }
        private string GetFilename(DateTime input)
        {
            var currentdate = input.ToString("yyyy_MM_dd");
            var filename = "JobStat_" + currentdate + ".xlsx";
            return filename;

        }
        private string GetPath()
        {
            string path = configuration.GetValue<string>("OutputDir");
            if (String.IsNullOrWhiteSpace(path))
            {
                path = AppDomain.CurrentDomain.BaseDirectory;
                ;
            }
            else if (!path.EndsWith('/') && !path.EndsWith(@"\"))
            {
                path += "/";
            }
            return path;
        }
    }

}
