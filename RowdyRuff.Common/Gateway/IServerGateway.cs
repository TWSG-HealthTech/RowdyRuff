using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RowdyRuff.Common.Gateway
{
    public interface IServerGateway
    {
        Task<T> GetAsyncWithBasicAuth<T>(string path, string username, string password);
        Task<T> GetAsync<T>(string path, string auth);
    }
}
