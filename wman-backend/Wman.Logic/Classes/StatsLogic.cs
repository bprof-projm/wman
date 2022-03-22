using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.DTO_Models;
using Wman.Logic.Interfaces;
using Wman.Repository.Interfaces;

namespace Wman.Logic.Classes
{
    public class StatsLogic : IStatsLogic
    {
        IWorkEventRepo eventRepo;

        public StatsLogic(IWorkEventRepo eventRepo)
        {
            this.eventRepo = eventRepo;
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
            var filename = "JobStat_";
            var currentdate = DateTime.Now.ToString("yyyy_MM_dd");
            filename += currentdate + ".xlsx";
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

                using (var fileStream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    workbook.SaveAs(fileStream);
                    fileStream.Close();
                }
            }

        }
    }
}
