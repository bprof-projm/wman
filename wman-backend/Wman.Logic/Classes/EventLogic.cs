using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.DTO_Models;
using Wman.Logic.Helpers;
using Wman.Logic.Interfaces;
using Wman.Logic.Services;
using Wman.Repository.Interfaces;

namespace Wman.Logic.Classes
{
    public class EventLogic : IEventLogic
    {
        IWorkEventRepo eventRepo;
        IMapper mapper;
        IAddressRepo address;
        UserManager<WmanUser> userManager;
        private readonly IHubContext<NotifyHub> _hub;
        private NotifyHub _notifyHub;
        private IEmailService _email;

        public EventLogic(IWorkEventRepo eventRepo, IMapper mapper, IAddressRepo address, UserManager<WmanUser> userManager, IHubContext<NotifyHub> hub, NotifyHub notifyHub , IEmailService email)
        {
            this.eventRepo = eventRepo;
            this.mapper = mapper;
            this.address = address;
            this.userManager = userManager;
            _hub = hub;
            _notifyHub = notifyHub;
            _email = email;
        }

        public async Task AssignUser(int eventID, string username)
        {
            var selectedEvent = await eventRepo.GetOneWithTracking(eventID);
            if (selectedEvent == null)
            {
                throw new NotFoundException(WmanError.EventNotFound);
            }
            var selectedUser = await userManager.Users.Where(x => x.UserName == username).Include(y => y.WorkEvents).SingleOrDefaultAsync();
            if (selectedUser == null)
            {
                throw new NotFoundException(WmanError.UserNotFound);
            }
            if (await userManager.IsInRoleAsync(selectedUser, "Worker") == false)
            {
                throw new NotMemberOfRoleException(WmanError.NotAWorker);
            }

            bool testResult = await this.DoTasksOverlap(selectedUser.WorkEvents, selectedEvent);
            ;
            if (testResult)
            {
                throw new InvalidOperationException(WmanError.UserIsBusy);
            }
            else
            {
                selectedEvent.AssignedUsers.Add(selectedUser);
                await this.eventRepo.Update(eventID, selectedEvent);
                var notifyEvent = await eventRepo.GetOne(eventID);
                var n = mapper.Map<WorkEventForWorkCardDTO>(notifyEvent);
                var k = Newtonsoft.Json.JsonConvert.SerializeObject(n, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    StringEscapeHandling = StringEscapeHandling.Default,
                    Converters = { new Newtonsoft.Json.Converters.StringEnumConverter() }
                });
                if (DateTime.Now.Year == notifyEvent.EstimatedStartDate.Year && DateTime.Now.Month == notifyEvent.EstimatedStartDate.Month && DateTime.Now.Day == notifyEvent.EstimatedStartDate.Day)
                {
                    await _notifyHub.NotifyWorkerAboutEventForToday(selectedUser.UserName, k);
                }
                await _notifyHub.NotifyWorkerAboutEvent(selectedUser.UserName, k);
                await _email.AssigedToWorkEvent(notifyEvent, selectedUser);
            }
        }

        public async Task MassAssignUser(int eventID, ICollection<string> usernames)
        {
            var selectedEvent = await eventRepo.GetOneWithTracking(eventID);
            if (selectedEvent == null)
            {
                throw new NotFoundException(WmanError.EventNotFound);
            }
            var okUsers = new List<WmanUser>();
            WmanUser selectedUser;
            bool testresult;
            foreach (var item in usernames)
            {
                selectedUser = await userManager.Users.Where(x => x.UserName == item).Include(y => y.WorkEvents).SingleOrDefaultAsync();
                if (selectedUser == null)
                {
                    throw new NotFoundException(WmanError.UserNotFound);
                }
                if (await userManager.IsInRoleAsync(selectedUser, "Worker") == false)
                {
                    throw new NotMemberOfRoleException(WmanError.NotAWorker);
                }
                testresult = await this.DoTasksOverlap(selectedUser.WorkEvents, selectedEvent);
                if (testresult)
                {
                    throw new InvalidOperationException(WmanError.UserIsBusy);
                }
                else
                {
                    okUsers.Add(selectedUser);
                }
            }
            var notifyEvent = await eventRepo.GetOne(eventID);
            foreach (var item in okUsers)
            {
                selectedEvent.AssignedUsers.Add(item);
                await this.eventRepo.Update(eventID, selectedEvent);
                var n = mapper.Map<WorkEventForWorkCardDTO>(notifyEvent);
                var k = Newtonsoft.Json.JsonConvert.SerializeObject(n, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    StringEscapeHandling = StringEscapeHandling.Default,
                    Converters = { new Newtonsoft.Json.Converters.StringEnumConverter() }
                });
                if (DateTime.Now.Year == notifyEvent.EstimatedStartDate.Year && DateTime.Now.Month == notifyEvent.EstimatedStartDate.Month && DateTime.Now.Day == notifyEvent.EstimatedStartDate.Day)
                {
                    await _notifyHub.NotifyWorkerAboutEventForToday(item.UserName, k);
                }
                await _notifyHub.NotifyWorkerAboutEvent(item.UserName, k);
                await _email.AssigedToWorkEvent(notifyEvent, item);
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
                throw new InvalidOperationException(WmanError.EventDateInvalid);
            }

        }

        public async Task DeleteEvent(int Id)
        {
            var test = await this.GetEvent(Id);
            await eventRepo.Delete(Id);
        }

        public async Task<IQueryable<WorkEvent>> GetAllEvents()
        {
            var output = eventRepo.GetAll();
            return output;
        }

        public async Task<WorkEvent> GetEvent(int id)
        {
            var output = await eventRepo.GetOne(id);
            if (output == null)
            {
                throw new NotFoundException(WmanError.EventNotFound);
            }
            return output;
        }

        public async Task UpdateEvent(UpdateEventDTO newWorkEvent)
        {
            var workevent =await eventRepo.GetOneWithTracking(newWorkEvent.Id);
            if (workevent == null)
            {
                throw new NotFoundException(WmanError.EventNotFound);
            }
            workevent.JobDescription = newWorkEvent.JobDescription;
            workevent.Address = mapper.Map<AddressHUN>(newWorkEvent.Address);
            
            bool movedToAnotherDayFromCurrent = false;

            if (newWorkEvent.EstimatedStartDate < newWorkEvent.EstimatedFinishDate && newWorkEvent.EstimatedStartDate.Day == newWorkEvent.EstimatedFinishDate.Day)
            {
                if (await WorkerTimeCheck(workevent.AssignedUsers.ToList(), newWorkEvent.EstimatedStartDate, newWorkEvent.EstimatedFinishDate))
                {
                    if (workevent.EstimatedStartDate.Date == DateTime.Today)
                    {
                        if (newWorkEvent.EstimatedStartDate.Date != DateTime.Today)
                        {
                            movedToAnotherDayFromCurrent = true;
                        }
                    }
                    workevent.EstimatedStartDate = newWorkEvent.EstimatedStartDate;
                    workevent.EstimatedFinishDate = newWorkEvent.EstimatedFinishDate;
                }
                else
                {
                    throw new InvalidOperationException(WmanError.UserIsBusy);
                }
                
            }
            else
            {
                throw new InvalidOperationException(WmanError.EventDateInvalid);
            }
            workevent.Status = newWorkEvent.Status;

            await eventRepo.SaveDatabase();
            var notifyEvent = await eventRepo.GetOne(workevent.Id);
            var n = mapper.Map<WorkEventForWorkCardDTO>(notifyEvent);
            var k = Newtonsoft.Json.JsonConvert.SerializeObject(n, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                StringEscapeHandling = StringEscapeHandling.Default,
                Converters = { new Newtonsoft.Json.Converters.StringEnumConverter() }
            });
            foreach (var item in notifyEvent.AssignedUsers)
            {
                if (movedToAnotherDayFromCurrent)
                {
                    await _notifyHub.NotifyWorkerAboutEventChangeFromCurrentToOther(item.UserName, k);
                }
                else if (DateTime.Now.Year == notifyEvent.EstimatedStartDate.Year && DateTime.Now.Month == notifyEvent.EstimatedStartDate.Month && DateTime.Now.Day == notifyEvent.EstimatedStartDate.Day)
                {

                    await _notifyHub.NotifyWorkerAboutEventChangeForToday(item.UserName, k);

                }
                await _notifyHub.NotifyWorkerAboutEventChange(item.UserName, k);
                await _email.WorkEventUpdated(notifyEvent, item);
            }
            
        }

        public async Task<ICollection<UserDTO>> GetAllAssignedUsers(int id)
        {
            var selectedEvent = await this.GetEvent(id);
            
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
                bool movedToAnotherDayFromCurrent = false;
   
                if (workEventInDb.EstimatedStartDate.Date == DateTime.Today)
                {
                    if (result.EstimatedStartDate.Date != DateTime.Today)
                    {
                        movedToAnotherDayFromCurrent = true;
                    }
                }
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
                        throw new InvalidOperationException(WmanError.UserIsBusy);
                    }
                    var notifyEvent = await eventRepo.GetOne(Id);
                    var n = mapper.Map<WorkEventForWorkCardDTO>(notifyEvent);
                    var k = Newtonsoft.Json.JsonConvert.SerializeObject(n, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver(),
                        StringEscapeHandling = StringEscapeHandling.Default,
                        Converters = { new Newtonsoft.Json.Converters.StringEnumConverter() }
                    });
                    
                    foreach (var item in notifyEvent.AssignedUsers)
                    {
                        if (movedToAnotherDayFromCurrent)
                        {
                            await _notifyHub.NotifyWorkerAboutEventChangeFromCurrentToOther(item.UserName, k);
                        }
                        else if (DateTime.Now.Year == notifyEvent.EstimatedStartDate.Year && DateTime.Now.Month == notifyEvent.EstimatedStartDate.Month && DateTime.Now.Day == notifyEvent.EstimatedStartDate.Day)
                        {

                            await _notifyHub.NotifyWorkerAboutEventChangeForToday(item.UserName, k);

                        }
                        await _notifyHub.NotifyWorkerAboutEventChange(item.UserName, k);
                        await _email.WorkEventUpdated(notifyEvent, item);
                    }

                }
                
            }
            else
            {
                throw new InvalidOperationException(WmanError.EventDateInvalid);
            }
        }
        public async Task<WorkEventForWorkCardDTO> StatusUpdater(int eventId)
        {
            var workevent = await eventRepo.GetOne(eventId);
            if (workevent.Status != Status.proofawait)
            {
                workevent.Status++;
            }
            if (workevent.Status == Status.finished)
            {
                throw new InvalidOperationException(WmanError.StatusFinished);
            }
            if (workevent.ProofOfWorkPic.Count > 0)
            {
                workevent.Status++;
            }
            else
            {
                throw new InvalidOperationException(WmanError.StatusPowMissing);
            }
            return mapper.Map<WorkEventForWorkCardDTO>(workevent);
        }
        private async Task<bool> WorkerTimeCheck(List<WmanUser> assignedUsers, DateTime startDate , DateTime finishDate)
        {
            if (assignedUsers.Count > 0)
            {
                var eventsAtUpdateTime = await(from x in eventRepo.GetAll()
                                             where x.EstimatedStartDate <= finishDate && x.EstimatedFinishDate >= startDate
                                               select x).ToListAsync();
                List<int> UpdateUserIds = new List<int>();
                foreach (var item in assignedUsers)
                {
                    UpdateUserIds.Add(item.Id);
                }
                List<int> eventUserIdsAtDnDTime = new List<int>();

                foreach (var item in eventsAtUpdateTime)
                {
                    foreach (var item2 in item.AssignedUsers)
                    {
                        eventUserIdsAtDnDTime.Add(item2.Id);
                    }
                }
                if (eventUserIdsAtDnDTime.Count == 0 || !UpdateUserIds.Any(x => eventUserIdsAtDnDTime.Any(y => y == x)))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return true;
            }
        } 
    }
}
