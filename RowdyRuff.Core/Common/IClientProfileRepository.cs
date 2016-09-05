using System.Collections.Generic;

namespace RowdyRuff.Core.Common
{
    public interface IClientProfileRepository
    {
        ClientProfile FindProfileBy(string profileId);
    }
}
