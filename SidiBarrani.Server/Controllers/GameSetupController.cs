using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SidiBarrani.Shared.Constants;
using SidiBarrani.Shared.Model.Setup;
using SidiBarrani.Shared.Services;

namespace SidiBarrani.Server.Controllers
{
    [ApiController]
    public class GameSetupController : ControllerBase
    {
        private readonly IGameSetupService _gameSetupService;

        // ReSharper disable once NotAccessedField.Local
        private readonly ILogger<GameSetupController> _logger;

        public GameSetupController(
            IGameSetupService gameSetupService,
            ILogger<GameSetupController> logger)
        {
            _gameSetupService = gameSetupService;
            _logger = logger;
        }

        [HttpGet(ApiRouteConstants.GameSetupGameQuery)]
        public async Task<GameSetup?> GetGameSetup([FromRoute] string gameId)
        {
            var gameSetup = await _gameSetupService.GetGameSetupAsync(Guid.Parse(gameId));

            return gameSetup;
        }

        [HttpGet(ApiRouteConstants.GameSetupQuery)]
        public async Task<IList<GameSetup>> GetGameSetupList()
        {
            var gameSetupList = await _gameSetupService.GetGameSetupListAsync();

            return gameSetupList;
        }

        [HttpPost(ApiRouteConstants.GameSetupOpenCommand)]
        public async Task<GameSetup> OpenGame([FromBody] string gameName)
        {
            var gameSetup = await _gameSetupService.OpenGameSetupAsync(gameName);

            return gameSetup;
        }

        [HttpPost(ApiRouteConstants.GameSetupGameJoinCommand)]
        public async Task JoinGame([FromRoute] string gameId, [FromBody] PlayerSetup playerSetup, [FromQuery] string versionId)
        {
            await _gameSetupService.JoinGameSetupAsync(Guid.Parse(gameId), playerSetup, Guid.Parse(versionId));
        }

        [HttpPost(ApiRouteConstants.GameSetupGameStartCommand)]
        public async Task StartGame([FromRoute] string gameId, [FromBody] string emptyString, [FromQuery] string versionId)
        {
            await _gameSetupService.StartGameAsync(Guid.Parse(gameId), Guid.Parse(versionId));
        }
    }
}
