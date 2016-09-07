using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RowdyRuff.Common.JSON;

namespace RowdyRuff.Core.Common
{
    public class SocialConnection
    {
        public int Id { get; private set; }
        public string ClientProfileId { get; private set; }
        public string Name { get; private set; }
        public string Skype { get; private set; }
        public string AvatarUrl { get; private set; }
        public List<string> Aliases { get; private set; }
        public string CalendarEmail { get; private set; }
        public string CalendarNames { get; private set; }

        [JsonConverter(typeof(JsonObjectAsStringConverter))]
        public string CalendarClientSecret { get; private set; }

        [JsonIgnore]
        public string SerializedAliases
        {
            get { return string.Join(",", Aliases); }
            set { Aliases = value.Split(',').ToList(); }
        }

        public SocialConnection(string name, string skype, List<string> aliases)
        {
            Name = name;
            Aliases = aliases;
            UpdateSkype(skype);
        }

        public void UpdateSkype(string skype)
        {
            Skype = skype;
            AvatarUrl = $"https://api.skype.com/users/{Skype}/profile/avatar";
        }

        // For EF
        private SocialConnection() { }

        public bool HasAlias(string alias)
        {
            return Aliases.Any(a => a.Equals(alias, StringComparison.OrdinalIgnoreCase));
        }
    }
}
