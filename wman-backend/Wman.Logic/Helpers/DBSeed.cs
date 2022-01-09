using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data;
using Wman.Data.DB_Models;
using Wman.Repository.Interfaces;

namespace Wman.Logic.Helpers
{
    public class DBSeed
    {
        IWorkEventRepo eventRepo;
        IAddressRepo addressRepo;
        ILabelRepo labelRepo;
        UserManager<WmanUser> userManager;

        public DBSeed(IWorkEventRepo eventRepo, IAddressRepo addressRepo, UserManager<WmanUser> userManager, ILabelRepo labelRepo)
        {
            this.eventRepo = eventRepo;
            this.addressRepo = addressRepo;
            this.userManager = userManager;
            this.labelRepo = labelRepo;
        }

        /// <summary>
        /// Should only be used on an empty db. This and the related methods will keep expanding during development, as more data will be required, to simulate a basic seeded database state. 
        /// </summary>
        public void PopulateDB()
        {
            AddLabels();
            AddUsers();
            AddAddress();
            AddEvents();
        }

        private void AddUsers()
        {
            WmanUser user = new()
            {
                Email = "vilhkov@mail.com",
                UserName = "admin",
                FirstName = "Vilhelmi",
                LastName = "Kováč",
                PhoneNumber = "0690123456",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            userManager.CreateAsync(user, "string").Wait();
            userManager.AddToRoleAsync(user, "Admin").Wait();

            user = new()
            {
                Email = "harada@mail.com",
                UserName = "manager1",
                FirstName = "Harlow",
                LastName = "Adams",
                PhoneNumber = "06971234567",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            userManager.CreateAsync(user, "string").Wait();
            userManager.AddToRoleAsync(user, "Manager").Wait();

            user = new()
            {
                Email = "liagow@mail.com",
                UserName = "manager2",
                FirstName = "Lianne",
                LastName = "McGowan",
                PhoneNumber = "+1289765432",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            userManager.CreateAsync(user, "string").Wait();
            userManager.AddToRoleAsync(user, "Manager").Wait();

            user = new()
            {
                Email = "sulaiman.eklund@gmail.com",
                UserName = "worker1",
                FirstName = "Sulaiman",
                LastName = "Eklund",
                PhoneNumber = "+3489717652",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            userManager.CreateAsync(user, "string").Wait();
            userManager.AddToRoleAsync(user, "Worker").Wait();

            user = new()
            {
                Email = "delfred@mail.com",
                UserName = "worker2",
                FirstName = "Delma",
                LastName = "Fredriksson",
                PhoneNumber = "+6287419537",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            userManager.CreateAsync(user, "string").Wait();
            userManager.AddToRoleAsync(user, "Worker").Wait();

            user = new()
            {
                Email = "azatmad@mail.com",
                UserName = "worker3",
                FirstName = "Azat",
                LastName = "Mac Domhnaill",
                PhoneNumber = "+385496287",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            userManager.CreateAsync(user, "string").Wait();
            userManager.AddToRoleAsync(user, "Worker").Wait();
        }

        private void AddEvents()
        {
            var workers = new List<WmanUser>();
            workers.AddRange(userManager.GetUsersInRoleAsync("Worker").Result);

            var addresses = new List<AddressHUN>();
            addresses.AddRange(addressRepo.GetAll());

            var labelList = new List<Label>();
            labelList.AddRange(labelRepo.GetAll());

            int id = 1;

            eventRepo.Add(new WorkEvent
            {
                JobDescription = $"Example event #{id++}",
                EstimatedStartDate = DateTime.Today.AddDays(1).AddHours(8),
                EstimatedFinishDate = DateTime.Today.AddDays(1).AddHours(14),
                Address = addresses.Find(x => x.ZIPCode == "1055"),
                Status = Status.awaiting,
                Labels = labelList,
                AssignedUsers = workers
            }).Wait();

            eventRepo.Add(new WorkEvent
            {
                JobDescription = $"Example event #{id++}",
                EstimatedStartDate = DateTime.Today.AddHours(9),
                EstimatedFinishDate = DateTime.Today.AddHours(13).AddMinutes(30),
                Address = addresses.FirstOrDefault(x => x.Id == addresses[0].Id),
                WorkStartDate = DateTime.Today.AddHours(9).AddMinutes(10),
                Status = Status.started,
                Labels = labelList.FindAll(x => x.Id == labelList[1].Id),
                AssignedUsers = workers.FindAll(x => x.Id == workers[0].Id)
            }).Wait();

            eventRepo.Add(new WorkEvent
            {
                JobDescription = $"Example event #{id++}",
                EstimatedStartDate = DateTime.Today.AddHours(11).AddMinutes(30),
                EstimatedFinishDate = DateTime.Today.AddHours(14),
                Address = addresses.FirstOrDefault(x => x.Id == addresses[1].Id),
                WorkStartDate = DateTime.Today.AddHours(9).AddMinutes(10),
                Status = Status.started,
                Labels = labelList.FindAll(x => x.Id == labelList[2].Id),
                AssignedUsers = workers.FindAll(x => x.Id == workers[1].Id)
            }).Wait();

            eventRepo.Add(new WorkEvent
            {
                JobDescription = $"Example event #{id++}",
                EstimatedStartDate = DateTime.Today.AddHours(10),
                EstimatedFinishDate = DateTime.Today.AddHours(13),
                Address = addresses.FirstOrDefault(x => x.Id == addresses[3].Id),
                Status = Status.awaiting,
                Labels = labelList.FindAll(x => x.Id == labelList[2].Id),
                AssignedUsers = workers.FindAll(x => x.Id != workers[0].Id)
            }).Wait();

            eventRepo.Add(new WorkEvent
            {
                JobDescription = $"Example event #{id++}",
                EstimatedStartDate = DateTime.Today.AddHours(16),
                EstimatedFinishDate = DateTime.Today.AddHours(19),
                Address = addresses.FirstOrDefault(x => x.Id == addresses[2].Id),
                Status = Status.awaiting,
                Labels = labelList.FindAll(x => x.Id != labelList[2].Id && x.Id != labelList[3].Id)
            }).Wait();

            eventRepo.Add(new WorkEvent
            {
                JobDescription = $"Example event #{id++}",
                EstimatedStartDate = DateTime.Today.AddDays(-3).AddHours(5),
                EstimatedFinishDate = DateTime.Today.AddDays(-3).AddHours(7),
                WorkStartDate = DateTime.Today.AddDays(-3).AddHours(5).AddMinutes(10),
                WorkFinishDate = DateTime.Today.AddDays(-3).AddHours(8).AddMinutes(30),
                Address = addresses.FirstOrDefault(x => x.Id == addresses[1].Id),
                Labels = labelList.FindAll(x => x.Id <= labelList[0].Id + 2),
                Status = Status.finished,
                AssignedUsers = workers.FindAll(x => x.Id <= workers[0].Id + 1)
            }).Wait();

            eventRepo.Add(new WorkEvent
            {
                JobDescription = $"Example event #{id++}",
                EstimatedStartDate = DateTime.Today.AddDays(-3).AddHours(7),
                EstimatedFinishDate = DateTime.Today.AddDays(-3).AddHours(9),
                WorkStartDate = DateTime.Today.AddDays(-3).AddHours(7).AddMinutes(30),
                WorkFinishDate = DateTime.Today.AddDays(-3).AddHours(10),
                Address = addresses.FirstOrDefault(x => x.Id == addresses[0].Id),
                Labels = labelList.FindAll(x => x.Id <= labelList[0].Id + 2),
                Status = Status.finished,
                AssignedUsers = workers.FindAll(x => x.Id <= workers[0].Id + 1)
            }).Wait();

            eventRepo.Add(new WorkEvent
            {
                JobDescription = $"Example event #{id++}",
                EstimatedStartDate = DateTime.Today.AddDays(2).AddHours(5),
                EstimatedFinishDate = DateTime.Today.AddDays(2).AddHours(9),
                Address = addresses.FirstOrDefault(x => x.Id == addresses[1].Id),
                Status = Status.awaiting,
                AssignedUsers = workers.FindAll(x => x.Id == workers.Last().Id)
            }).Wait();

            eventRepo.Add(new WorkEvent
            {
                JobDescription = $"Example event #{id++}",
                EstimatedStartDate = DateTime.Today.AddDays(2).AddHours(6),
                EstimatedFinishDate = DateTime.Today.AddDays(2).AddHours(8),
                Address = addresses.FirstOrDefault(x => x.Id == addresses[2].Id),
                Status = Status.awaiting,
                AssignedUsers = workers.FindAll(x => x.Id != workers.Last().Id)
            }).Wait();

            eventRepo.Add(new WorkEvent
            {
                JobDescription = $"Example event #{id++}",
                EstimatedStartDate = DateTime.Today.AddDays(2).AddHours(7),
                EstimatedFinishDate = DateTime.Today.AddDays(2).AddHours(8),
                Address = addresses.FirstOrDefault(x => x.Id == addresses[0].Id),
                Status = Status.awaiting
            }).Wait();

            eventRepo.Add(new WorkEvent
            {
                JobDescription = $"Example event #{id++}",
                EstimatedStartDate = DateTime.Today.AddDays(3).AddHours(7),
                EstimatedFinishDate = DateTime.Today.AddDays(3).AddHours(8),
                Address = addresses.FirstOrDefault(x => x.Id == addresses[0].Id),
                Status = Status.awaiting
            }).Wait();

            eventRepo.Add(new WorkEvent
            {
                JobDescription = $"Example event #{id++}",
                EstimatedStartDate = DateTime.Today.AddDays(4).AddHours(10),
                EstimatedFinishDate = DateTime.Today.AddDays(4).AddHours(11),
                Address = addresses.FirstOrDefault(x => x.Id == addresses[1].Id),
                Status = Status.awaiting
            }).Wait();

            eventRepo.Add(new WorkEvent
            {
                JobDescription = $"Example event #{id++}",
                EstimatedStartDate = DateTime.Today.AddDays(5).AddHours(13),
                EstimatedFinishDate = DateTime.Today.AddDays(5).AddHours(15),
                Address = addresses.FirstOrDefault(x => x.Id == addresses[3].Id),
                Status = Status.awaiting
            }).Wait();

            eventRepo.Add(new WorkEvent
            {
                JobDescription = $"Example event a year before today #{id++}",
                EstimatedStartDate = DateTime.Today.AddYears(-1).AddHours(8),
                EstimatedFinishDate = DateTime.Today.AddYears(-1).AddHours(12),
                WorkStartDate = DateTime.Today.AddYears(-1).AddHours(9),
                WorkFinishDate = DateTime.Today.AddYears(-1).AddHours(13),
                Address = addresses.FirstOrDefault(x => x.Id == addresses[3].Id),
                Labels = labelList.FindAll(x => x.Id >= labelList[0].Id + 2),
                Status = Status.finished,
                AssignedUsers = workers.FindAll(x => x.Id <= workers[0].Id)
            }).Wait();

            eventRepo.Add(new WorkEvent
            {
                JobDescription = $"Example event a year after today #{id++}",
                EstimatedStartDate = DateTime.Today.AddYears(1).AddHours(13),
                EstimatedFinishDate = DateTime.Today.AddYears(1).AddHours(16),
                Address = addresses.FirstOrDefault(x => x.Id == addresses[0].Id),
                Labels = labelList.FindAll(x => x.Id == labelList.Last().Id),
                Status = Status.awaiting
            }).Wait();

            eventRepo.Add(new WorkEvent
            {
                JobDescription = $"Example event #{id++}",
                EstimatedStartDate = new DateTime(2021, 12, 31, 20, 0, 0),
                EstimatedFinishDate = new DateTime(2021, 12, 31, 21, 0, 0),
                Address = addresses.FirstOrDefault(x => x.Id == addresses[3].Id),
                Labels = labelList.FindAll(x => x.Id == labelList.Last().Id),
                Status = Status.awaiting
            }).Wait();

            eventRepo.Add(new WorkEvent
            {
                JobDescription = $"Example event #{id++}",
                EstimatedStartDate = new DateTime(2022, 01, 01, 20, 0, 0),
                EstimatedFinishDate = new DateTime(2022, 01, 01, 21, 0, 0),
                Address = addresses.FirstOrDefault(x => x.Id == addresses[0].Id),
                Labels = labelList.FindAll(x => x.Id == labelList.Last().Id),
                Status = Status.awaiting
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

            addressRepo.Add(new AddressHUN
            {
                ZIPCode = "1055",
                City = "Budapest",
                Street = "Kossuth Lajos tér",
                BuildingNumber = "1-3"
            }).Wait();

            addressRepo.Add(new AddressHUN
            {
                ZIPCode = "2000",
                City = "Szentendre",
                Street = "Áprily Lajos tér",
                BuildingNumber = "5"
            }).Wait();
        }

        private void AddLabels()
        {
            labelRepo.Add(new Label()
            {
                Color = "#E70000",
                Content = "ASAP",
            }).Wait();

            labelRepo.Add(new Label()
            {
                Color = "#2986CC",
                Content = "Plumber required"
            }).Wait();

            labelRepo.Add(new Label()
            {
                Color = "#8FCE00",
                Content = "Gasfitter required"
            }).Wait();

            labelRepo.Add(new Label()
            {
                Color = "#FFD966",
                Content = "Electrician required"
            }).Wait();

            labelRepo.Add(new Label()
            {
                Color = "#BF9000",
                Content = "Forklift required",
            }).Wait();
        }
    }

}
