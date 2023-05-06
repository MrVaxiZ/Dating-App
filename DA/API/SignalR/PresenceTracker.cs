using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.SignalR
{
    public class PresenceTracker
    {
        private static readonly Dictionary<string, List<string>> OnlineUsers = new Dictionary<string, List<string>>();

        public Task<bool> UserConnected(string username, string connectionId)
        {
            bool IsOnline = false;

            lock(OnlineUsers)
            {
                if(OnlineUsers.ContainsKey(username))
                {
                    OnlineUsers[username].Add(connectionId);
                }
                else
                {
                    OnlineUsers.Add(username, new List<string>{connectionId});
                    IsOnline = true;
                }
            }

            return Task.FromResult(IsOnline);
        }

        public Task<bool> UserDisconnected(string username, string connectionId)
        {
            bool IsOnline = false;

            lock(OnlineUsers)
            {
                if(!OnlineUsers.ContainsKey(username)) return Task.FromResult(IsOnline);

                OnlineUsers[username].Remove(connectionId);

                if(OnlineUsers[username].Count == 0)
                {
                    OnlineUsers.Remove(username);
                    IsOnline = true;
                }
            }

            return Task.FromResult(IsOnline);
        }

        public Task<string[]> GetOnlineUsers() 
        {
            string[] onlineUsers;
            lock(OnlineUsers)
            {
                onlineUsers = OnlineUsers.OrderBy(k => k.Key).Select(k => k.Key).ToArray();
            }

            return Task.FromResult(onlineUsers);
        }

        public static Task<List<string>> GetConnectionsForUser(string username)
        {
            List<string> connectionIds;

            lock(OnlineUsers)
            {
                connectionIds = OnlineUsers.GetValueOrDefault(username);
            }

            return Task.FromResult(connectionIds);
        }
    }
}