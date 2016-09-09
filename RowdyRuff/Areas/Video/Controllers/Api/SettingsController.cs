using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RowdyRuff.Areas.Video.DTO;
using RowdyRuff.Core.Common;

namespace RowdyRuff.Areas.Video.Controllers.Api
{
    [Area("Video")]
    [Route("/video/api/{profileId}/[controller]")]
    [Produces("application/json")]
    public class SettingsController : Controller
    {
        private readonly IClientProfileRepository _clientProfileRepository;

        public SettingsController(IClientProfileRepository clientProfileRepository)
        {
            _clientProfileRepository = clientProfileRepository;
        }

        [HttpGet]
        [Produces(typeof(IEnumerable<GetSocialConnectionsDTO>))]
        public IActionResult Index(string profileId)
        {
            var connections = _clientProfileRepository.FindSocialConnectionsFor(profileId);
            return Json(connections.Select(c => new GetSocialConnectionsDTO
            {
                Id = c.Id,
                Name = c.Name,
                Skype = c.Skype,
                AvatarUrl = c.AvatarUrl
            }));
        }

        [HttpPut("{connectionId:int}")]
        public IActionResult Update(int connectionId, [FromBody] UpdateSocialConnectionInput input)
        {
            var connection = _clientProfileRepository.FindConnectionById(connectionId);

            connection.UpdateSkype(input.Skype);

            _clientProfileRepository.UpdateConnection(connection);

            return Ok();
        }
    }
}