using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RowdyRuff.Areas.Main.DTO;
using RowdyRuff.Core.Common;

namespace RowdyRuff.Areas.Main.Controllers.Api
{
    [Area("Main")]
    [Route("/main/api/[controller]/{profileId}")]
    [Produces("application/json")]
    public class ProfileController : Controller
    {
        private readonly IClientProfileRepository _clientProfileRepository;

        public ProfileController(IClientProfileRepository clientProfileRepository)
        {
            _clientProfileRepository = clientProfileRepository;
        }

        [HttpGet]
        [Produces(typeof(GetProfileByIdDTO))]
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

        [HttpPut("connection/{connectionId:int}")]
        public IActionResult Update(int connectionId, [FromBody] UpdateProfileConnectionInput input)
        {
            var connection = _clientProfileRepository.FindConnectionById(connectionId);

            connection.UpdateProfile(input.Name, input.Aliases);

            _clientProfileRepository.UpdateConnection(connection);

            return Ok();
        }
    }
}