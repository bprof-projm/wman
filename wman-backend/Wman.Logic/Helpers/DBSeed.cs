using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Repository.Interfaces;

namespace Wman.Logic.Helpers
{
    public class DBSeed
    {
        IWorkEventRepo eventRepo;
        IMapper mapper;
        IAddressRepo addressRepo;
        UserManager<WmanUser> userManager;
        public DBSeed(IWorkEventRepo eventRepo, IMapper mapper, IAddressRepo addressRepo, UserManager<WmanUser> userManager)
        {
            this.eventRepo = eventRepo;
            this.mapper = mapper;
            this.addressRepo = addressRepo;
            this.userManager = userManager;

        }

        /// <summary>
        /// Should only be used on an empty db. This and the related methods will keep expanding during development, as more data will be required, to simulate a basic seeded database state. 
        /// </summary>
        public void PopulateDB()
        {
#if DEBUG
            AddUsers();
            AddAddress();
            AddEvents();
#else
        throw new InvalidOperationException("API is not running in debug mode!");
#endif    
        }
        public void clearDB()
        {
            throw new NotImplementedException();
        }
        private void AddUsers()
        {
            var user = new WmanUser
            {
                Email = "string@string.com",
                UserName = "string",
                FirstName = "First",
                LastName = "User",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            userManager.CreateAsync(user, "string").Wait();

            user = new WmanUser
            {
                Email = "Pelda@gmail.com",
                UserName = "string2",
                FirstName = "Second",
                LastName = "User",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            userManager.CreateAsync(user, "string2").Wait();

            user = new WmanUser
            {
                Email = "Pelda3@gmail.com",
                UserName = "string3",
                FirstName = "Third",
                LastName = "User",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            userManager.CreateAsync(user, "string3").Wait();
        }
        private void AddEvents()
        {
            eventRepo.Add(new WorkEvent
            {
                JobDescription = "Example event #1",
                EstimatedStartDate = DateTime.Now.AddDays(-1),
                EstimatedFinishDate = DateTime.Now.AddDays(-1).AddHours(1),
                Address = addressRepo.GetAll().First(),
                AddressId = addressRepo.GetAll().First().Id,
                Status = Status.awaiting
            }).Wait();

            eventRepo.Add(new WorkEvent
            {
                JobDescription = "Example event #2",
                EstimatedStartDate = DateTime.Now.AddDays(-2),
                EstimatedFinishDate = DateTime.Now.AddDays(-2).AddHours(2),
                WorkStartDate = DateTime.Now.AddDays(-2).AddMinutes(10),
                Address = addressRepo.GetAll().ToList()[1],
                AddressId = addressRepo.GetAll().ToList()[1].Id,
                Status = Status.started
            }).Wait();

            eventRepo.Add(new WorkEvent
            {
                JobDescription = "Example event #3",
                EstimatedStartDate = DateTime.Now.AddDays(-3),
                EstimatedFinishDate = DateTime.Now.AddDays(-3).AddHours(3),
                WorkStartDate = DateTime.Now.AddDays(-3).AddMinutes(10),
                Address = addressRepo.GetAll().ToList()[1],
                AddressId = addressRepo.GetAll().ToList()[1].Id,
                WorkFinishDate = DateTime.Now.AddDays(-3).AddHours(4),
                Status = Status.finished
            }).Wait();
        }
        private void AddAddress()
        {
            addressRepo.Add(new AddressHUN
            {
                ZIPCode = "1034",
                City = "Budapest",
                Street = "Bécsi út",
                BuildingNumber = "104-108."
                
            }).Wait();

            addressRepo.Add(new AddressHUN
            {
                ZIPCode = "1034",
                City = "Budapest",
                Street = "Doberdó út",
                BuildingNumber = "6/A."

            }).Wait();
        }
    }

    }
