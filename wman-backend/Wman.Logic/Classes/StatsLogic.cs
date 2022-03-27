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

        public async Task<ICollection<StatsXlsModel>> GetStats(DateTime input)
        {
            var allCompletedThisMonth = await eventRepo.GetAll()
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
            await this.makexls(output);
            return output;
        }
        public async Task makexls(List<StatsXlsModel> input)
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
        public async void registerRecurringJob(string x)
        {
            if (!string.IsNullOrWhiteSpace(x))
            {
                var cronExpr = "0 12 1/INPUTVALUE * *";
                cronExpr = cronExpr.Replace("INPUTVALUE", x);
                RecurringJob.AddOrUpdate("scheduledXlsReport", () => this.GetStats(DateTime.Now), cronExpr);
                await fileRepo.DeleteOldFiles(this.GetPath(), ".xlsx", DateTime.Now.AddMonths(-1)); //Delete every .xlsx file older than a month
                Debug.WriteLine($"\n--- Scheduled xls generation&sending every {x} days starting from the 1st of the month!--- \n");
            }
            else
            {
                RecurringJob.RemoveIfExists("scheduledXlsReport");
                Debug.WriteLine("\n--- No \"xlsSchedule\" tag found in appsettings.json, scheduled xls sending is disabled!--- ");
                Debug.WriteLine("Valid example: \"xlsSchedule\": \"5\"\n");
            }
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
