using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using SidiBarrani.Shared.Constants;
using SidiBarrani.Shared.Model.Setup;
using SidiBarrani.Shared.Services;

namespace SidiBarrani.Client.Setup.Services
{
    public class ClientGameSetupService: IGameSetupService
    {
        private readonly HttpClient _httpClient;

        public ClientGameSetupService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GameSetup> OpenGameSetupAsync(string gameName)
        {
            var requestUri = ApiRouteConstants.GameSetupOpenCommand;
            var response = await _httpClient.PostAsJsonAsync(
                requestUri,
                gameName);
            response.EnsureSuccessStatusCode();
            var gameSetup = await response.Content.ReadFromJsonAsync<GameSetup>();

            return gameSetup!;
        }

        public async Task<GameSetup?> GetGameSetupAsync(Guid gameId)
        {
            var requestUri = ApiRouteConstants.GetRequestUriForGameSetupGameQuery(gameId);
            var gameSetup = await _httpClient.GetFromJsonAsync<GameSetup?>(
                requestUri);

            return gameSetup!;
        }

        public async Task<IList<GameSetup>> GetGameSetupListAsync()
        {
            var requestUri = ApiRouteConstants.GameSetupQuery;
            var gameSetupList = await _httpClient.GetFromJsonAsync<IList<GameSetup>>(
                requestUri);

            return gameSetupList!;
        }

        public async Task JoinGameSetupAsync(Guid gameId, PlayerSetup playerSetup, Guid versionId)
        {
            var requestUri = ApiRouteConstants.GetRequestUriForGameSetupGameJoinCommand(gameId, versionId);
            var response = await _httpClient.PostAsJsonAsync(
                requestUri,
                playerSetup);
            response.EnsureSuccessStatusCode();
        }

        public async Task StartGameAsync(Guid gameId, Guid versionId)
        {
            var requestUri = ApiRouteConstants.GetRequestUriForGameSetupGameStartCommand(gameId, versionId);
            var response = await _httpClient.PostAsJsonAsync(
                requestUri,
                "");
            response.EnsureSuccessStatusCode();
        }
    }
}
