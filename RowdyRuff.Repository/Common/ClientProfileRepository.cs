using System.Linq;
using Microsoft.EntityFrameworkCore;
using RowdyRuff.Core.Common;

namespace RowdyRuff.Repository.Common
{
    public class ClientProfileRepository : IClientProfileRepository
    {
        private readonly RowdyRuffContext _context;
        private readonly DbSet<ClientProfile> _set;

        public ClientProfileRepository(RowdyRuffContext context)
        {
            _context = context;
            _set = _context.Set<ClientProfile>();
        }

        public ClientProfile FindProfileBy(string profileId)
        {
            return _set
                .Include(p => p.Connections)
                .FirstOrDefault(s => s.Id == profileId);
        }
    }
}
