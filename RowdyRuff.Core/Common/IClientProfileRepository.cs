using System.Collections.Generic;

namespace RowdyRuff.Core.Common
{
    public interface IClientProfileRepository
    {
        ClientProfile FindProfileBy(string profileId);
        List<SocialConnection> FindSocialConnectionsFor(string profileId);
        SocialConnection FindConnectionById(int id);
        void UpdateConnection(SocialConnection connection);
    }
}
