using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RowdyRuff.Areas.Main.DTO;
using RowdyRuff.Core.Common;

namespace RowdyRuff.Areas.Main.Controllers.Api
{
    [Area("Main")]
    public class ProfileController : Controller
    {
        private readonly IClientProfileRepository _clientProfileRepository;

        public ProfileController(IClientProfileRepository clientProfileRepository)
        {
            _clientProfileRepository = clientProfileRepository;
        }

        public IActionResult Index(string profileId)
        {
            var profile = _clientProfileRepository.FindProfileBy(profileId);
            return Json(new GetProfileByIdDTO
            {
                Id = profile.Id,
                Email = profile.Email,
                Connections = profile.Connections.Select(c => new GetProfileByIdDTO.SocialConnectionDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Aliases = c.Aliases
                })
            });
        }
    }
}