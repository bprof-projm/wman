﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.DTO_Models;

namespace Wman.Logic.Interfaces
{
    public interface IEventLogic
    {
        Task CreateEvent(CreateEventDTO workEvent);
        Task DeleteEvent(int Id);
        Task UpdateEvent(int Id, WorkEvent newWorkEvent);
        Task<WorkEvent> GetEvent(int id);
        Task<IQueryable<WorkEvent>> GetAllEvents();
        Task DnDEvent(int Id, DnDEventDTO newWorkEvent);
        Task AssignUser(int eventID, string username);

        Task<ICollection<UserDTO>> GetAllAssignedUsers(int eventID);

        Task MassAssignUser(int eventID, ICollection<string> usernames);
    }
}
