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

        public LabelLogic(ILabelRepo labelRepo)
        {
            this.labelRepo = labelRepo;
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
                result = find;
            }

        }
    }
}
