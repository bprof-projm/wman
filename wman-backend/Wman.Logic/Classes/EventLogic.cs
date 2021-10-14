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
    public class EventLogic : IEventLogic
    {
        IWorkEventRepo eventRepo;
        IMapper mapper;
        IAddressRepo address;
        UserManager<WmanUser> userManager;
        public EventLogic(IWorkEventRepo eventRepo, IMapper mapper , IAddressRepo address, UserManager<WmanUser> userManager)
        {
            this.eventRepo = eventRepo;
            this.mapper = mapper;
            this.userManager = userManager;
            this.address = address;
        }

        public async Task AssignUser(int id, string username)
        {
            var selectedEvent = await this.GetEvent(id);
            var selectedUser =  await userManager.Users.Where(x => x.UserName == username).SingleOrDefaultAsync();
            selectedEvent.AssignedUsers.Add(selectedUser);
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

        public async Task<IQueryable<WorkEvent>> GetAllEvents()
        {
            var output = eventRepo.GetAll();
            //foreach (var item in output)
            //{
            //    if (item.Address.Id == 0)
            //    {
            //        item.Address = await address.GetOne(item.AddressId);
            //    }
            //}
            return output;
        }

        public async Task<WorkEvent> GetEvent(int id)
        {
            var output = await eventRepo.GetOne(id);
            output.Address = await address.GetOne(output.AddressId);
            return output;
        }

        public async Task UpdateEvent(int Id, WorkEvent newWorkEvent)
        {
            await eventRepo.Update(Id, newWorkEvent);
        }

      

        public async Task<ICollection<UserDTO>> GetAllAssignedUsers(int id)
        {
            var selectedEvent = await GetEvent(id);
            return mapper.Map<List<UserDTO>>(selectedEvent.AssignedUsers);
        }

    }
}
