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
        public EventLogic(IWorkEventRepo eventRepo, IMapper mapper, IAddressRepo address, UserManager<WmanUser> userManager)
        {
            this.eventRepo = eventRepo;
            this.mapper = mapper;
            this.address = address;
            this.userManager = userManager;
        }

        public async Task AssignUser(int eventID, string username)
        {
            var selectedEvent = await eventRepo.GetOneWithTracking(eventID);
            if (selectedEvent == null)
            {
                throw new ArgumentException("Event not found! ");
            }
            var selectedUser = await userManager.Users.Where(x => x.UserName == username).Include(y => y.WorkEvents).SingleOrDefaultAsync();
            if (selectedUser == null)
            {
                throw new ArgumentException("User not found! ");
            }
            bool testResult = await this.DoTasksOverlap(selectedUser.WorkEvents, selectedEvent);
            ;
            if (testResult)
            {
                throw new ArgumentException("User is already busy during this event's estimated timeframe! ");
            }
            else
            {
                selectedEvent.AssignedUsers.Add(selectedUser);
                await this.eventRepo.Update(eventID, selectedEvent);
            }
        }

        public async Task MassAssignUser(int eventID, ICollection<string> usernames)
        {
            var selectedEvent = await eventRepo.GetOneWithTracking(eventID);
            if (selectedEvent == null)
            {
                throw new ArgumentException("Event not found! ");
            }
            var okUsers = new List<WmanUser>();
            WmanUser selectedUser;
            bool testresult;
            foreach (var item in usernames)
            {
                selectedUser = await userManager.Users.Where(x => x.UserName == item).Include(y => y.WorkEvents).SingleOrDefaultAsync();
                if (selectedUser == null)
                {
                    throw new ArgumentException($"User: {0} not found", item);
                }

                testresult = await this.DoTasksOverlap(selectedUser.WorkEvents, selectedEvent);
                if (testresult)
                {
                    throw new InvalidOperationException(String.Format("User: {0} is busy during this event", selectedUser.UserName));
                }
                else
                {
                    okUsers.Add(selectedUser);
                }
            }
            foreach (var item in okUsers)
            {
                selectedEvent.AssignedUsers.Add(item);
                await this.eventRepo.Update(eventID, selectedEvent);
            }
        }

        public async Task CreateEvent(CreateEventDTO workEvent)
        {
            if (workEvent.EstimatedStartDate < workEvent.EstimatedFinishDate && workEvent.EstimatedStartDate.Day == workEvent.EstimatedFinishDate.Day)
            {
                var result = mapper.Map<WorkEvent>(workEvent);
                var find = await (from x in address.GetAll()
                                  where x.Street == result.Address.Street && x.ZIPCode == result.Address.ZIPCode && x.City == result.Address.City
                                  select x).FirstOrDefaultAsync();
                if (find == null)
                {
                    await address.Add(result.Address);
                }
                else
                {
                    result.Address = find;
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
            return output;
        }

        public async Task UpdateEvent(int Id, WorkEvent newWorkEvent)
        {
            await eventRepo.Update(Id, newWorkEvent);
        }

        public async Task<ICollection<UserDTO>> GetAllAssignedUsers(int id)
        {
            var selectedEvent = await GetEvent(id);
            if (selectedEvent == null)
            {
                throw new ArgumentException("Event not found! ");
            }
            return mapper.Map<List<UserDTO>>(selectedEvent.AssignedUsers);
        }

        /// <summary>
        /// Decides if a new workevent overlaps with any of the provided ones
        /// </summary>
        /// <param name="preExistingEvents">A collection of all the workevents to be checked against</param>
        /// <param name="newEvent">The new workevent we'd like to check</param>
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

        public async Task DnDEvent(int Id, DnDEventDTO newWorkEvent)
        {
            if (newWorkEvent.EstimatedStartDate < newWorkEvent.EstimatedFinishDate && newWorkEvent.EstimatedStartDate.Day == newWorkEvent.EstimatedFinishDate.Day)
            {
                var result = mapper.Map<WorkEvent>(newWorkEvent);
                var workEventInDb = await eventRepo.GetOneWithTracking(Id);

                workEventInDb.EstimatedStartDate = result.EstimatedStartDate;
                workEventInDb.EstimatedFinishDate = result.EstimatedFinishDate;
                
                if (workEventInDb.AssignedUsers.Count > 0)
                {
                    var eventssAtDnDTime =await (from x in eventRepo.GetAll()
                                         where x.EstimatedStartDate >= result.EstimatedStartDate && x.EstimatedStartDate <= result.EstimatedFinishDate && x.EstimatedFinishDate >= result.EstimatedStartDate && x.EstimatedFinishDate <= result.EstimatedFinishDate
                                         select x).ToListAsync();
                    
                    List<int> dnDUserIds = new List<int>();
                    foreach (var item in workEventInDb.AssignedUsers)
                    {
                        dnDUserIds.Add(item.Id);
                    }
                    List<int> eventUserIdsAtDnDTime = new List<int>();

                    foreach (var item in eventssAtDnDTime)
                    {
                        foreach (var item2 in item.AssignedUsers)
                        {
                            eventUserIdsAtDnDTime.Add(item2.Id);
                        }
                    }
                    if (eventUserIdsAtDnDTime.Count == 0 || !dnDUserIds.Any(x => eventUserIdsAtDnDTime.Any(y => y == x)))
                    {
                        await eventRepo.Update(Id, workEventInDb);
                    }
                    else
                    {
                        throw new ArgumentException("Assigned user conflict(user already assigned to an event at this time)");
                    }


                }
                await eventRepo.Update(Id, workEventInDb);
            }
            else
            {
                throw new ArgumentException("Events are not at the same day or start is after the finishing date");
            }
        }
    }
}
