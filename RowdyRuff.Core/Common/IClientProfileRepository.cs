using System.Collections.Generic;

namespace RowdyRuff.Core.Common
{
    public interface IClientProfileRepository
    {
        ClientProfile FindProfileBy(string profileId);
        List<SocialConnection> FindSocialConnectionsFor(string profileId);
        void UpdateConnectionVideoSetting(int connectionId, string skype);
    }
}
