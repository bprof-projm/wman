using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.DTO_Models;
using Wman.Logic.Interfaces;
using Wman.Repository.Interfaces;

namespace Wman.Logic.Classes
{
    public class AllInWorkEventLogic : IAllInWorkEventLogic
    {
        UserManager<WmanUser> userManager;
        IWorkEventRepo workEvent;
        ILabelRepo labelRepo;
        IMapper mapper;

        public AllInWorkEventLogic(UserManager<WmanUser> userManager, IWorkEventRepo workEvent, ILabelRepo labelRepo, IMapper mapper)
        {
            this.userManager = userManager;
            this.workEvent = workEvent;
            this.labelRepo = labelRepo;
            this.mapper = mapper;
        }

        public async Task<WorkEventForWorkCardDTO> ForWorkCard(int eventId)
        {
            var selectedWorkEvent =await workEvent.GetOne(eventId);

            var result = mapper.Map<WorkEventForWorkCardDTO>(selectedWorkEvent);
            return result;
        }

        public async Task<List<WorkerDTO>> Available (DateTime fromDate, DateTime toDate)
        {
            

            if (fromDate<toDate)
            {
                var allUsers = userManager.Users
                .Include(y => y.WorkEvents)
                .Include(z => z.ProfilePicture);

                var notavailableWorkers = await (from x in workEvent.GetAll()
                                              where fromDate <= x.EstimatedFinishDate && toDate >= x.EstimatedStartDate
                                              select x.AssignedUsers).ToListAsync();

                List<WorkerDTO> workersDTO = new List<WorkerDTO>();

                List<int> notAvailableWorkerID = new List<int>();
                foreach (var workers in notavailableWorkers)
                {
                    foreach (var item in workers)
                    {
                        var valami = await userManager.GetRolesAsync(item);
                        if (valami.FirstOrDefault() == "Worker")
                        {
                            notAvailableWorkerID.Add(item.Id);
                        }
                    }

                }
                List<int> workerID = new List<int>();
                foreach (var item in userManager.Users)
                {
                    var valami = await userManager.GetRolesAsync(item);
                    if (valami.FirstOrDefault() == "Worker")
                    {
                        workerID.Add(item.Id);
                    }   
                }

                List<int> rightWorkers = new List<int>();
                foreach (var item in workerID)
                {
                    if (!notAvailableWorkerID.Any(x => x == item))
                    {
                        var user = allUsers.Where(x => x.Id == item).FirstOrDefault();
                        workersDTO.Add(mapper.Map<WorkerDTO>(user));
                    }
                }

                return workersDTO;
            }
            else
            {
                throw new ArgumentException("Times not set appropriately");
            }
            
        }
    }
}
