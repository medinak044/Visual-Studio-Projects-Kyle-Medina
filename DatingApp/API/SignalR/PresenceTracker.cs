namespace API.SignalR
{
    public class PresenceTracker
    {
        private static readonly Dictionary<string, List<string>> OnlineUsers = new Dictionary<string, List<string>>();

        public Task<bool> UserConnected(string username, string connectionId)
        {
            bool isOnline = false;
            // "lock" locks the Dictionary until it is finished completing its tasks
            lock (OnlineUsers) // "lock" prevents concurrent users from messing up the Dictionary (because it's not a thread-safe resource)
            {
                if (OnlineUsers.ContainsKey(username))
                { OnlineUsers[username].Add(connectionId); }
                else
                { 
                    OnlineUsers.Add(username, new List<string> { connectionId }); 
                    isOnline = true;
                }
            }

            return Task.FromResult(isOnline);
        }

        public Task<bool> UserDisconnected(string username, string connectionId)
        {
            bool isOffline = false;
            lock(OnlineUsers)
            {
                if (!OnlineUsers.ContainsKey(username)) return Task.FromResult(isOffline);

                OnlineUsers[username].Remove(connectionId);

                if (OnlineUsers[username].Count == 0)
                { 
                    OnlineUsers.Remove(username); 
                    isOffline = true;
                }
            }

            return Task.FromResult(isOffline);
        }

        public Task<string[]> GetOnlineUsers()
        {
            string[] onlineUsers;

            // This logic does not request data from database, instead uses Dictionary data from memory
            lock (OnlineUsers)
            { onlineUsers = OnlineUsers.OrderBy(k => k.Key).Select(k => k.Key).ToArray(); }

            return Task.FromResult(onlineUsers);
        }

        public Task<List<string>> GetConnectionsForUser(string username)
        {
            List<string> conncetionIds;
            lock(OnlineUsers)
            {
                conncetionIds = OnlineUsers.GetValueOrDefault(username);
            }

            return Task.FromResult(conncetionIds);
        }
    }
}
