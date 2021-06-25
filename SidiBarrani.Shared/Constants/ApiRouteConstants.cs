using System;

namespace SidiBarrani.Shared.Constants
{
    public static class ApiRouteConstants
    {
        private const string ApiPrefix = "/api";
        private const string GameSetupPrefix = ApiPrefix + "/gameSetup";

        public const string NotificationHub = ApiPrefix + "/notificationhub";
        public const string GameSetupQuery = GameSetupPrefix;
        public const string GameSetupGameQuery = GameSetupPrefix + "/{gameId}";
        public const string GameSetupOpenCommand = GameSetupPrefix + "/open";
        public const string GameSetupGameJoinCommand = GameSetupPrefix + "/{gameId}/join";
        public const string GameSetupGameStartCommand = GameSetupPrefix + "/{gameId}/start";

        public static string GetRequestUriForGameSetupGameQuery(Guid gameId) =>
            GameSetupGameQuery.Replace("{gameId}", gameId.ToString());
        public static string GetRequestUriForGameSetupGameJoinCommand(Guid gameId, Guid versionId)
        {
            var routeString = GameSetupGameJoinCommand.Replace("{gameId}", gameId.ToString());
            var queryString = $"?versionId={versionId}";

            return routeString + queryString;
        }

        public static string GetRequestUriForGameSetupGameStartCommand(Guid gameId, Guid versionId)
        {
            var routeString = GameSetupGameStartCommand.Replace("{gameId}", gameId.ToString());
            var queryString = $"?versionId={versionId}";

            return routeString + queryString;
        }
    }
}
