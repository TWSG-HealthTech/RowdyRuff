using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RowdyRuff.Areas.Video.DTO;
using RowdyRuff.Core.Common;

namespace RowdyRuff.Areas.Video.Controllers.Api
{
    [Area("Video")]
    public class SettingsController : Controller
    {
        private readonly IClientProfileRepository _clientProfileRepository;

        public SettingsController(IClientProfileRepository clientProfileRepository)
        {
            _clientProfileRepository = clientProfileRepository;
        }

        public IActionResult Index(string profileId)
        {
            var connections = _clientProfileRepository.FindSocialConnectionsFor(profileId);
            return Json(connections.Select(c => new GetSocialConnectionsDTO
            {
                Id = c.Id,
                Name = c.Name,
                Skype = c.Skype
            }));
        }
    }
}