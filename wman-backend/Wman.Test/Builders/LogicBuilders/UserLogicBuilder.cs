using System.Collections.Generic;
using Wman.Data.DB_Models;

namespace Wman.Test.Builders.LogicBuilders
{
    public class UserLogicBuilder
    {
        public static void AssignWorkEvents(List<WmanUser> userList, List<WorkEvent> helper) //couldnt think of a better way
        {
            userList[0].WorkEvents.Add(helper[0]);
            helper[0].AssignedUsers.Add(userList[0]);

            userList[0].WorkEvents.Add(helper[1]);
            helper[1].AssignedUsers.Add(userList[0]);

            userList[1].WorkEvents.Add(helper[2]);
            helper[2].AssignedUsers.Add(userList[1]);

            userList[2].WorkEvents.Add(helper[3]);
            helper[2].AssignedUsers.Add(userList[2]);

            userList[0].WorkEvents.Add(helper[4]);
            helper[4].AssignedUsers.Add(userList[0]);
        }
    }
}
