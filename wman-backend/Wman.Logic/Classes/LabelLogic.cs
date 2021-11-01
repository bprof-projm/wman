using AutoMapper;
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
    public class LabelLogic : ILabelLogic
    {
        ILabelRepo labelRepo;
        IMapper mapper;

        public LabelLogic(ILabelRepo labelRepo, IMapper mapper)
        {
            this.labelRepo = labelRepo;
            this.mapper = mapper;
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
                throw new ArgumentException("This Label already exists");
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
                    TextColor = InverseColor(item.Color),
                    BackgroundColor = item.Color,
                    Content = item.Content
                });
            }
            return labelsDTOs;
        }

        private string InverseColor(string color)
        {
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
