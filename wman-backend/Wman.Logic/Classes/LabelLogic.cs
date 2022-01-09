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

        public async Task<List<ListLabelsDTO>> GetAllLabels()
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
                return labelsDTOs;
            
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
                throw new NotFoundException(WmanError.LabelNotFound);
            }
            
        }

        public async Task AssignLabelToWorkEvent(int eventId, int labelId)
        {
            var selectedEvent = await eventRepo.GetOneWithTracking(eventId);
            if (selectedEvent == null)
            {
                throw new NotFoundException(WmanError.EventNotFound);
            }
            var selectedLabel = await labelRepo.GetOne(labelId);
            if (selectedLabel == null)
            {
                throw new NotFoundException(WmanError.LabelNotFound);
            }
            selectedLabel.WorkEvents.Add(selectedEvent);
            await labelRepo.SaveDatabase();
        }

        public async Task MassAssignLabelToWorkEvent(int eventId, ICollection<int> labelIds)
        {
            var selectedEvent = await eventRepo.GetOneWithTracking(eventId);
            if (selectedEvent == null)
            {
                throw new NotFoundException(WmanError.EventNotFound);
            }

            List<Label> labels = new();
            foreach (var labelId in labelIds)
            {
                var label = await labelRepo.GetOne(labelId);
                labels.Add(label);
            }

            if (labels.Contains(null))
            {
                throw new NotFoundException(WmanError.LabelNotFound);
            }

            foreach (var label in labels)
            {
                label.WorkEvents.Add(selectedEvent);
            }

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
                throw new NotFoundException(WmanError.LabelNotFound);
            }
        }

        public static string InverseColor(string color)
        {
            Regex rgx = new Regex(@"^#(?:[0-9a-fA-F]{3}){1,2}$");
            if (!rgx.IsMatch(color))
            {
                throw new ArgumentException(WmanError.WrongColor);
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
