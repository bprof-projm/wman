using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.DTO_Models;
using Wman.Logic.Helpers;
using Wman.Logic.Interfaces;
using Wman.Repository.Interfaces;

namespace Wman.Logic.Classes
{
    public class LabelLogic : ILabelLogic
    {
        ILabelRepo labelRepo;
        IWorkEventRepo eventRepo;
        IMapper mapper;

        public LabelLogic(ILabelRepo labelRepo, IMapper mapper, IWorkEventRepo eventRepo)
        {
            this.labelRepo = labelRepo;
            this.mapper = mapper;
            this.eventRepo = eventRepo;
        }

        public async Task CreateLabel(CreateLabelDTO label)
        {
            var result = mapper.Map<Label>(label);

            var find = (from x in labelRepo.GetAll()
                       where x.Color == result.Color && x.Content == result.Content
                       select x).FirstOrDefault();
            if (find == null)
            {
                await labelRepo.Add(result);
            }
            else
            {
                throw new InvalidOperationException(WmanError.LabelExists);
            }

        }

        public List<ListLabelsDTO> GetAllLabels()
        {
            List<ListLabelsDTO> labelsDTOs = new List<ListLabelsDTO>();
            var labels = labelRepo.GetAll();

            foreach (var item in labels)
            {
                labelsDTOs.Add(new ListLabelsDTO()
                {
                    Id = item.Id,
                    TextColor = InverseColor(item.Color),
                    BackgroundColor = item.Color,
                    Content = item.Content
                });
            }
            if (labelsDTOs !=null)
            {
                return labelsDTOs;
            }
            else
            {
                throw new ArgumentException("Currently there are no labels added");
            }
            
        }
        public async Task UpdateLabel(int Id, CreateLabelDTO NewLabel)
        {
            if (await labelRepo.GetOne(Id) != null)
            {
                var result = mapper.Map<Label>(NewLabel);
                await labelRepo.Update(Id, result);
            }
            else
            {
                throw new ArgumentException("Not found");
            }
            
        }

        public async Task AssignLabelToWorkEvent(int eventId, int labelId)
        {
            var selectedEvent = await eventRepo.GetOneWithTracking(eventId);
            if (selectedEvent == null)
            {
                throw new ArgumentException("Bad event Id");
            }
            var selectedLabel = await labelRepo.GetOne(labelId);
            if (selectedLabel == null)
            {
                throw new ArgumentException("Bad Label Id");
            }
            selectedLabel.WorkEvents.Add(selectedEvent);
            await labelRepo.SaveDatabase();
        }

        public async Task DeleteLabel(int Id)
        {
            if (await labelRepo.GetOne(Id) != null)
            {
                await labelRepo.Delete(Id);
            }
            else
            {
                throw new ArgumentException("Bad Label Id");
            }
        }

        public static string InverseColor(string color)
        {
            Regex rgx = new Regex(@"^#(?:[0-9a-fA-F]{3}){1,2}$");
            if (!rgx.IsMatch(color))
            {
                throw new ArgumentException("Wrong regular expresion");
            }
            string redstring = 255 - Convert.ToInt32(color.Substring(1, 2), 16) <= 15 ? "0"
                + Convert.ToString(255 - Convert.ToInt32(color.Substring(1, 2), 16), 16)
                : Convert.ToString(255 - Convert.ToInt32(color.Substring(1, 2), 16), 16);
            string greenstring = 255 - Convert.ToInt32(color.Substring(3, 2), 16) <= 15 ? "0"
                + Convert.ToString(255 - Convert.ToInt32(color.Substring(3, 2), 16), 16)
                : Convert.ToString(255 - Convert.ToInt32(color.Substring(3, 2), 16), 16);
            string bluestring = 255 - Convert.ToInt32(color.Substring(5, 2), 16) <= 15 ? "0"
                + Convert.ToString(255 - Convert.ToInt32(color.Substring(5, 2), 16), 16)
                : Convert.ToString(255 - Convert.ToInt32(color.Substring(5, 2), 16), 16);

            string finish = String.Concat("#", redstring, greenstring, bluestring);

            return finish;
        }
    }
}
