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
        IWmanUserRepo wmanUserRepo;
        public EventLogic(IWorkEventRepo eventRepo, IMapper mapper, IAddressRepo address, IWmanUserRepo wmanUserRepo)
        {
            this.eventRepo = eventRepo;
            this.mapper = mapper;
            this.address = address;
            this.wmanUserRepo = wmanUserRepo;
        }

        public async Task AssignUser(int eventID, string username)
        {
            var selectedEvent = await this.GetEvent(eventID);
            if (selectedEvent == null)
            {
                throw new ArgumentException("Event not found! ");
            }
            var selectedUser = await wmanUserRepo.getUser(username);
            if (selectedUser == null)
            {
                throw new ArgumentException("User not found! ");
            }
            bool testResult = await this.DoTasksOverlap(selectedUser.WorkEvents, selectedEvent);
            ;
            if (testResult) //TODO: Teljesen blokkoljuk az ütközést, vagy csak figyelmeztessük a frontendet?
            {
                throw new ArgumentException("User is already busy during this event's estimated timeframe! ");
            }
            else
            {
                selectedEvent.AssignedUsers.Add(selectedUser);
                await this.eventRepo.Update(eventID, selectedEvent);
            }
        }

        public async Task<ICollection<UserDTO>> MassAssignUser(int eventID, ICollection<string> usernames)
        {
            var successList = new List<UserDTO>();
            var selectedEvent = await this.GetEvent(eventID);
            if (selectedEvent == null)
            {
                throw new ArgumentException("Event not found! ");
            }
            var selectedUser = new WmanUser();
            bool testresult;
            foreach (var item in usernames)
            {
                selectedUser = await wmanUserRepo.getUser(item);
                if (selectedUser == null)
                {
                    throw new ArgumentException($"User: {0} not found", item);
                }
                testresult = await this.DoTasksOverlap(selectedUser.WorkEvents, selectedEvent);
                if (!testresult)
                {
                    selectedEvent.AssignedUsers.Add(selectedUser);
                    await this.eventRepo.Update(eventID, selectedEvent);
                    successList.Add(mapper.Map<UserDTO>(selectedUser));
                }
            }
            return successList;
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

        /// <summary>
        /// Decides if a new job overlaps with any of the provided ones
        /// </summary>
        /// <param name="preExistingEvents">A collection of all the jobs to be checked against</param>
        /// <param name="newEvent">The new job we'd like to check</param>
        /// <returns>False, if the <paramref name="newEvent"/> doesn't overlap with any of the ones provided in <paramref name="preExistingEvents"/>, true if it does collide</returns>
        private async Task<bool> DoTasksOverlap(ICollection<WorkEvent> preExistingEvents, WorkEvent newEvent)
        {
            foreach (var item in preExistingEvents)
            {
                if (item.EstimatedStartDate < newEvent.EstimatedFinishDate && item.EstimatedFinishDate > newEvent.EstimatedStartDate)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
