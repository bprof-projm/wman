using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.DTO_Models;

namespace Wman.Logic.Services
{
    [Authorize(Roles = "Worker", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotifyHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("Connected", Context.ConnectionId);
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            Clients.Caller.SendAsync("Disconnected", Context.ConnectionId);
            return base.OnDisconnectedAsync(exception);
        }
        public async Task NotifyWorkerAboutEventForToday(string user, string we)
        {
            try
            {
                if (Clients != null)
                {
                    await Clients.User(user).SendAsync("UserAssignedCurrentDay", we);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }         
            
            
        }
        public async Task NotifyWorkerAboutEvent(string user, string we)
        {
            try
            {
                if (Clients != null)
                {
                    await Clients.User(user).SendAsync("UserAssigned", we);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }



        }
        public async Task NotifyWorkerAboutEventChangeForToday(string user, string we)
        {
            try
            {
                if (Clients != null)
                {
                    await Clients.User(user).SendAsync("EventChangedForToday", we);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
        public async Task NotifyWorkerAboutEventChange(string user, string we)
        {
            try
            {
                if (Clients != null)
                {
                    await Clients.User(user).SendAsync("EventChanged", we);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
        public async Task NotifyWorkerAboutEventChangeFromCurrentToOther(string user, string we)
        {
            try
            {
                if (Clients != null)
                {
                    await Clients.User(user).SendAsync("EventChangedFromTodayToNotToday", we);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
        public async Task NotifyWorkerAboutOtherWorkerStateChange(string user, string we)
        {
            try
            {
                if (Clients != null)
                {
                    await Clients.User(user).SendAsync("EventStateChanged", we);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        public async Task NotifyWorkerAboutWorkEventDelete(string user, string we)
        {
            try
            {
                if (Clients != null)
                {
                    await Clients.User(user).SendAsync("EventDeleted", we);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
    }
    
}
