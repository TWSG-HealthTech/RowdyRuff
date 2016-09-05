using System.Collections.Generic;

namespace RowdyRuff.Core.Common
{
    public class ClientProfile
    {
        public string Id { get; private set; }
        public string Email { get; private set; }
        public string ConnectionId { get; private set; }
        public List<Subscription> Subscriptions { get; private set; }
        public List<SocialConnection> Connections { get; private set; }

        public ClientProfile(string id, string email)
        {
            Id = id;
            Email = email;
            Subscriptions = new List<Subscription>();
            Connections = new List<SocialConnection>();
        }

        private ClientProfile() { }
    }
}
