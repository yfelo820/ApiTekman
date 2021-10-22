using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace Api.SignalR
{
    [AllowAnonymous]
    public class LiveStudentHub : Hub
    {
        // User - GroupKey
        static List<KeyValuePair<string,string>> _connectedUsers = new List<KeyValuePair<string, string>>();
        // GroupKey - TeacherConnectionId
        static List<KeyValuePair<string,string>> _connectedTeachers = new List<KeyValuePair<string, string>>();

        public Task StartSession(string groupKey)
        {
            if(!_connectedTeachers.Any(teacher => teacher.Key.Equals(groupKey)))
                _connectedTeachers.Add(new KeyValuePair<string, string>(groupKey, Context.ConnectionId));
            return Clients.Caller.SendAsync("startSession", JsonConvert.SerializeObject(GetConnectedUsers(groupKey).Select(u => u.Key).ToList()));
        }

        public async Task StudentJoin(string userName, string groupKey)
        {
            var teacherConnection = _connectedTeachers.FirstOrDefault(c => c.Key.Equals(groupKey)).Value;
            if (!_connectedUsers.Any(user => user.Key.Equals(userName, StringComparison.CurrentCultureIgnoreCase) && user.Value.Equals(groupKey)))
                _connectedUsers.Add(new KeyValuePair<string, string>(userName, groupKey));
            await Groups.AddToGroupAsync(Context.ConnectionId, groupKey);
            await Clients.Client(teacherConnection).SendAsync("studentJoin", userName);
        }

        public async Task StudentDisconnect(string userName, string groupKey)
        {
            var teacherConnection = _connectedTeachers.FirstOrDefault(c => c.Key.Equals(groupKey)).Value;
            _connectedUsers.RemoveAll(user => user.Key.Equals(userName, StringComparison.CurrentCultureIgnoreCase) && user.Value.Equals(groupKey));
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupKey);
            await Clients.Client(teacherConnection).SendAsync("studentDisconnect", userName);
        }

        public async Task CloseSession(string groupKey)
        {
            _connectedTeachers.RemoveAll(teacher => teacher.Key.Equals(groupKey));
            _connectedUsers.RemoveAll(user => user.Value.Equals(groupKey));
            await Clients.Group(groupKey).SendAsync("closeSession");
        }

        private List<KeyValuePair<string, string>> GetConnectedUsers(string groupKey)
        {
            return _connectedUsers.Where(u => u.Value == groupKey).ToList();
        }
    }
}
