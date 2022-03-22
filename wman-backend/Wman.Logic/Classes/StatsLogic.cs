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
            await this.makexls();
            return output;
        }
        public async Task makexls()
        {
            using (var workbook = new XLWorkbook())
            {
                var sheet = workbook.Worksheets.Add("test");
                sheet.Cell(1, 1).Value = "Hello world";

                using (var fileStream = new FileStream("asd.xlsx", FileMode.OpenOrCreate, FileAccess.Write))
                {
                    workbook.SaveAs(fileStream);
                    fileStream.Close();
                }
            }

        }
    }
}
