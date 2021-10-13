using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Connection_Tables;
using Wman.Data.DB_Models;
using Wman.Logic.DTO_Models;
using Wman.Logic.Interfaces;
using Wman.Repository.Interfaces;

namespace Wman.Logic.Classes
{
    public class EventLogic : IEventLogic
    {
        IWorkEventRepo eventRepo;
        IMapper mapper;
        IAddressRepo address;
        public EventLogic(IWorkEventRepo eventRepo, IMapper mapper , IAddressRepo address)
        {
            this.eventRepo = eventRepo;
            this.mapper = mapper;
            this.address = address;
        }

        public async Task CreateEvent(CreateEventDTO workEvent)
        {
            if (workEvent.EstimatedStartDate < workEvent.EstimatedFinishDate && workEvent.EstimatedStartDate.Day == workEvent.EstimatedFinishDate.Day)
            {
                var result = mapper.Map<WorkEvent>(workEvent);
                var find = await (from x in address.GetAll()
                                  where x.Street == result.Address.Street && x.ZIPCode == result.Address.ZIPCode && x.City == result.Address.City
                                  select x).FirstOrDefaultAsync();
                if (find != null)
                {
                    result.AddressId = find.Id;
                    result.Address = null;
                }
                await eventRepo.Add(result);
            }
            else
            {
                throw new ArgumentException("Events are not at the same day or start is after the finishing date");
            }
            
        }

        public async Task DeleteEvent(int Id)
        {
            await eventRepo.Delete(Id);
        }

        public IQueryable<WorkEvent> GetAllEvents()
        {
            return eventRepo.GetAll();
        }

        public async Task<WorkEvent> GetEvent(int id)
        {
            return await eventRepo.GetOne(id);
        }

        public async Task UpdateEvent(int Id, WorkEvent newWorkEvent)
        {
            await eventRepo.Update(Id, newWorkEvent);
        }

        public async Task DnDEvent(int Id, DnDEventDTO newWorkEvent)
        {
            if (newWorkEvent.EstimatedStartDate < newWorkEvent.EstimatedFinishDate && newWorkEvent.EstimatedStartDate.Day == newWorkEvent.EstimatedFinishDate.Day)
            {
                var result = mapper.Map<WorkEvent>(newWorkEvent);
                var workEventInDb = await eventRepo.GetOne(Id);
                if (workEventInDb.AssignedUsers != null)
                {
                    var eventssAtDnDTime =await (from x in eventRepo.GetAll()
                                         where x.EstimatedStartDate >= workEventInDb.EstimatedStartDate && x.EstimatedStartDate <= workEventInDb.EstimatedFinishDate && x.EstimatedFinishDate >= workEventInDb.EstimatedStartDate && x.EstimatedFinishDate <= workEventInDb.EstimatedFinishDate
                                         select x).ToListAsync();
                    
                    List<int> dnDUserIds = new List<int>();
                    foreach (var item in workEventInDb.AssignedUsers)
                    {
                        dnDUserIds.Add(item.WmanUser.Id);
                    }
                    List<int> eventUserIdsAtDnDTime = new List<int>();

                    foreach (var item in eventssAtDnDTime)
                    {
                        foreach (var item2 in item.AssignedUsers)
                        {
                            eventUserIdsAtDnDTime.Add(item2.WmanUser.Id);
                        }
                    }
                    if(dnDUserIds.Any(x => eventUserIdsAtDnDTime.Any(y => y != x)))
                    {
                        result.JobDescription = workEventInDb.JobDescription;
                        await eventRepo.Update(Id, result);
                    }
                    else
                    {
                        throw new ArgumentException("Assigned user conflict(user already assigned to an event at this time)");
                    }


                }
                result.JobDescription = workEventInDb.JobDescription;
                await eventRepo.Update(Id, result);
            }
            else
            {
                throw new ArgumentException("Events are not at the same day or start is after the finishing date");
            }
        }
    }
}
