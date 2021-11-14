using System.Collections.Generic;
using Wman.Data.DB_Models;

namespace Wman.Test.Builders.LogicBuilders
{
    public class UserLogicBuilder
    {
        public static void AssignWorkEvents(List<WmanUser> userList)
        {
            List<WorkEvent> helper = CalendarEventLogicBuilder.GetWorkEvents();

            userList[0].WorkEvents.Add(helper[0]);
            userList[0].WorkEvents.Add(helper[1]);
            userList[1].WorkEvents.Add(helper[2]);
            userList[2].WorkEvents.Add(helper[3]);
        }
    }
}
