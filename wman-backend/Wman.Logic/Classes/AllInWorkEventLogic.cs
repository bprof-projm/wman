using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Logic.DTO_Models;
using Wman.Logic.Helpers;
using Wman.Logic.Interfaces;
using Wman.Repository.Interfaces;

namespace Wman.Logic.Classes
{
    public class AllInWorkEventLogic : IAllInWorkEventLogic
    {
        IWorkEventRepo workEvent;
        ILabelRepo labelRepo;
        IMapper mapper;

        public AllInWorkEventLogic(IWorkEventRepo workEvent, ILabelRepo labelRepo, IMapper mapper)
        {
            this.workEvent = workEvent;
            this.labelRepo = labelRepo;
            this.mapper = mapper;
        }

        public async Task<WorkEventForWorkCardDTO> ForWorkCard(int eventId)
        {
            var selectedWorkEvent =await workEvent.GetOne(eventId);
            if (selectedWorkEvent == null)
            {
                throw new NotFoundException(WmanError.EventNotFound);
            }
            var result = mapper.Map<WorkEventForWorkCardDTO>(selectedWorkEvent);
            return result;
        }
    }
}
