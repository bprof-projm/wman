﻿using AutoMapper;
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

        public async Task AssignUser(int id, WmanUser user)
        {
            var selectedEvent = await this.GetEvent(id);
            var link = new WmanUserWorkEvent { WmanUser = user, WmanUserId = user.Id, WorkEvent = selectedEvent, WorkEventId = selectedEvent.Id };
            selectedEvent.AssignedUsers.Add(link);
            ;
           await this.eventRepo.Update(id, selectedEvent);
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
                throw new ArgumentException("Events are not at the same day or ");
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
    }
}
