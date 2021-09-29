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
            debugRepo.Add(new WorkEvent { JobDescription = "Test 1" });
            debugRepo.Add(new WorkEvent { JobDescription = "Test 2" });
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
