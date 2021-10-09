using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wman.Data.DB_Models;
using Wman.Repository;

namespace Wman.Logic.Classes
{
    public class DebugLogic
    {
        private IDebug debugRepo;
        public DebugLogic(IDebug inRepo)
        {
            this.debugRepo = inRepo;
        }

        public void testadd()
        {
            debugRepo.Add(new WorkEvent { JobDescription = "Test 1" , EstimatedStartDate = new DateTime(2021,10,4), EstimatedFinishDate = new DateTime(2021, 10, 4) });
            debugRepo.Add(new WorkEvent { JobDescription = "Test 2", EstimatedStartDate = new DateTime(2021, 10, 5), EstimatedFinishDate =  new DateTime(2021, 10, 5) });
            debugRepo.Add(new WorkEvent { JobDescription = "Test 2", EstimatedStartDate = new DateTime(2021, 10, 6), EstimatedFinishDate = new DateTime(2021, 10, 6) });
            debugRepo.Add(new WorkEvent { JobDescription = "Test 2", EstimatedStartDate = new DateTime(2021, 10, 3), EstimatedFinishDate = new DateTime(2021, 10, 3) });
            debugRepo.Add(new WorkEvent { JobDescription = "Test 2", EstimatedStartDate = new DateTime(2021, 10, 11), EstimatedFinishDate = new DateTime(2021, 10, 11) });
        }
        public List<WorkEvent> testlist()
        {
            var outputlist = new List<WorkEvent>();
            foreach (var item in debugRepo.GetAll())
            {
                outputlist.Add(item);
            }
            return outputlist;
        }

    }
}
