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
        IPicturesRepo picRepo;
        UserManager<WmanUser> userManager;
        public DBSeed(IWorkEventRepo eventRepo, IMapper mapper, IAddressRepo addressRepo, UserManager<WmanUser> userManager, IPicturesRepo picRepo)
        {
            this.eventRepo = eventRepo;
            this.mapper = mapper;
            this.addressRepo = addressRepo;
            this.userManager = userManager;
            this.picRepo = picRepo;

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
            AddPicture();
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
                FirstName = "Admin",
                LastName = "User",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            userManager.CreateAsync(user, "string").Wait();
            userManager.AddToRoleAsync(user, "Admin").Wait();
            user = new WmanUser
            {
                Email = "manager@gmail.com",
                UserName = "manager",
                FirstName = "Manager",
                LastName = "User",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            userManager.CreateAsync(user, "string").Wait();
            userManager.AddToRoleAsync(user, "Manager").Wait();

            user = new WmanUser
            {
                Email = "pelda@abc.com",
                UserName = "user1",
                FirstName = "First",
                LastName = "User",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            userManager.CreateAsync(user, "string").Wait();
            userManager.AddToRoleAsync(user, "Worker").Wait();

            user = new WmanUser
            {
                Email = "Pelda2@gmail.com",
                UserName = "user2",
                FirstName = "Second",
                LastName = "User",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            userManager.CreateAsync(user, "string").Wait();
            userManager.AddToRoleAsync(user, "Worker").Wait();

            user = new WmanUser
            {
                Email = "Pelda3@gmail.com",
                UserName = "user3",
                FirstName = "Third",
                LastName = "User",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            userManager.CreateAsync(user, "string").Wait();
            userManager.AddToRoleAsync(user, "Worker").Wait();

        }
        private void AddEvents()
        {
            eventRepo.Add(new WorkEvent
            {
                JobDescription = "Example event #1",
                EstimatedStartDate = DateTime.Today.AddDays(-1).AddHours(3),
                EstimatedFinishDate = DateTime.Today.AddDays(-1).AddHours(4),
                Address = addressRepo.GetAll().First(),
                Status = Status.awaiting
            }).Wait();

            eventRepo.Add(new WorkEvent
            {
                JobDescription = "Example event #2",
                EstimatedStartDate = DateTime.Today.AddDays(-2).AddHours(5),
                EstimatedFinishDate = DateTime.Today.AddDays(-2).AddHours(6).AddMinutes(30),
                WorkStartDate = DateTime.Today.AddDays(-2).AddHours(5).AddMinutes(7),
                Address = addressRepo.GetAll().ToList()[1],
                Status = Status.started
            }).Wait();

            eventRepo.Add(new WorkEvent
            {
                JobDescription = "Example event #3",
                EstimatedStartDate = DateTime.Today.AddDays(-3).AddHours(5),
                EstimatedFinishDate = DateTime.Today.AddDays(-3).AddHours(7),
                WorkStartDate = DateTime.Today.AddDays(-3).AddHours(5).AddMinutes(10),
                WorkFinishDate = DateTime.Today.AddDays(-3).AddHours(8).AddMinutes(30),
                Address = addressRepo.GetAll().ToList()[1],

                Status = Status.finished
            }).Wait();

            eventRepo.Add(new WorkEvent
            {
                JobDescription = "Example event #4",
                EstimatedStartDate = DateTime.Today.AddDays(2).AddHours(5),
                EstimatedFinishDate = DateTime.Today.AddDays(2).AddHours(9),
                Address = addressRepo.GetAll().ToList()[1],
                Status = Status.awaiting
            }).Wait();


            //eventRepo.Add(new WorkEvent
            //{
            //    /* This workevent violates the rule, that a job must start and finish on the same day.
            //        Therefore, it might cause unexpected behaviour in various parts of the code.
            //        However, it could be useful for debugging workload calculation.
            //     */
            //    JobDescription = "!!! 3+ day long workevent",
            //    EstimatedStartDate = DateTime.Today.AddDays(-3).AddHours(5),
            //    EstimatedFinishDate = DateTime.Today.AddHours(2),
            //    WorkStartDate = DateTime.Today.AddDays(-3).AddHours(5).AddMinutes(10),
            //    WorkFinishDate = DateTime.Today.AddHours(5).AddMinutes(10),
            //    Address = addressRepo.GetAll().ToList()[1],
            //    Status = Status.finished
            //}).Wait();

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
        private void AddPicture()
        {
            picRepo.Add(new Pictures()
            {
                Name = "Test pic",
                Url = "https://upload.wikimedia.org/wikipedia/commons/c/c5/Number-One.JPG",
                WmanUser = userManager.Users.Where(x => x.UserName == "user1").SingleOrDefault(),
                PicturesType = PicturesType.ProfilePic
            }).Wait();

            picRepo.Add(new Pictures()
            {
                Name = "Test pic2",
                Url = "https://upload.wikimedia.org/wikipedia/commons/d/d5/Number-two.JPG",
                WmanUser = userManager.Users.Where(x => x.UserName == "user2").SingleOrDefault(),
                PicturesType = PicturesType.ProfilePic
            }).Wait();
        }
    }

}
