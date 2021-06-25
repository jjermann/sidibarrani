using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SidiBarrani.Shared.Exceptions;
using SidiBarrani.Shared.Model.Connection;
using SidiBarrani.Shared.Model.Setup;
using SidiBarrani.Shared.Services;

namespace SidiBarrani.Application.Services
{
    public class GameSetupService : IGameSetupService
    {
        private readonly IServerConnectionService _serverConnectionService;
        private readonly ConcurrentDictionary<Guid, GameSetup> _gameSetupDictionary = new ();

        public GameSetupService(
            IServerConnectionService serverConnectionService)
        {
            _serverConnectionService = serverConnectionService;
        }

        public async Task<GameSetup> OpenGameSetupAsync(string gameName)
        {
            var gameSetup = GameSetup.Construct(gameName);
            _gameSetupDictionary[gameSetup.GameId] = gameSetup;
            await _serverConnectionService.SendToGameGroupAsync(ConnectionEvent.GameStageListChanged, gameSetup.GameId);
            await _serverConnectionService.SendToAllAsync(ConnectionEvent.GameStageListChanged);

            return gameSetup;

            //(Rules ? rulesParam = null, string gameName = "Game", string team1Name = "Team1", string team2Name = "Team2"

            //var rules = rulesParam ?? new Rules();
            //Log.Verbose($"{this}: OpenGameSetup({rules}, {gameName}, {team1Name}, {team2Name})");
            //if (gameName == null || team1Name == null || team2Name == null)
            //{
            //    return null;
            //}
            //var playerGroupInfo = new PlayerGroupSetup
            //{
            //    Team1 = new TeamSetup
            //    {
            //        TeamName = team1Name,
            //        TeamId = Guid.NewGuid().ToString()
            //    },
            //    Team2 = new TeamSetup
            //    {
            //        TeamName = team2Name,
            //        TeamId = Guid.NewGuid().ToString()
            //    }
            //};
            //var gameInfo = new GameInfo
            //{
            //    GameName = gameName,
            //    GameId = Guid.NewGuid().ToString(),
            //    Rules = rules,
            //    PlayerGroupInfo = playerGroupInfo
            //};
            //_gameServiceDictionary[gameInfo.GameId] = new GameService(gameInfo);
            //return (GameInfo)gameInfo?.Clone();
        }

            public async Task<GameSetup?> GetGameSetupAsync(Guid gameId)
        {
            return await Task.Run(() =>
            {
                return _gameSetupDictionary.Values
                    .SingleOrDefault(gs => gs.GameId == gameId);
            });
        }

        public async Task<IList<GameSetup>> GetGameSetupListAsync()
        {
            return await Task.Run(() =>
            {
                return _gameSetupDictionary.Values
                    .Where(gs => gs.HasOpenSpots())
                    .OrderByDescending(gs => gs.CreationDate)
                    .ToList();
            });
        }

        public async Task JoinGameSetupAsync(Guid gameId, PlayerSetup playerSetup, Guid versionId)
        {
            if (!_gameSetupDictionary.ContainsKey(gameId))
            {
                throw new InvalidOperationException();
            }

            var gameSetup = _gameSetupDictionary[gameId];
            if (gameSetup.VersionId != versionId)
            {
                throw new OptimisticLockingException(versionId, gameSetup.VersionId);
            }
            if (!gameSetup.HasOpenSpots())
            {
                throw new InvalidOperationException();
            }

            gameSetup.AddPlayer(playerSetup);

            await _serverConnectionService.SendToGameGroupAsync(ConnectionEvent.GameStageChanged, gameId);
        }

        public async Task StartGameAsync(Guid gameId, Guid versionId)
        {
            if (!_gameSetupDictionary.ContainsKey(gameId))
            {
                throw new InvalidOperationException();
            }

            var gameSetup = _gameSetupDictionary[gameId];
            if (gameSetup.VersionId != versionId)
            {
                throw new OptimisticLockingException(versionId, gameSetup.VersionId);
            }
            if (!gameSetup.IsReady())
            {
                throw new InvalidOperationException();
            }

            await _serverConnectionService.SendToGameGroupAsync(ConnectionEvent.GameStarted, gameId);
            await _serverConnectionService.SendToAllAsync(ConnectionEvent.GameStageListChanged);

            //TODO
        }

            //public PlayerInfo ConnectToGame(string gameId, string playerName, ISidiBarraniClientApi clientApi)
            //{
            //    Log.Verbose($"{this}: ConnectToGame({gameId},{playerName},{clientApi})");
            //    if (!_gameServiceDictionary.ContainsKey(gameId))
            //    {
            //        return null;
            //    }
            //    var gameService = _gameServiceDictionary[gameId];
            //    if (gameService.PlayerGroupInfo.GetPlayerList().Count >= 4)
            //    {
            //        return null;
            //    }
            //    var team1 = gameService.PlayerGroupInfo.Team1;
            //    var team2 = gameService.PlayerGroupInfo.Team2;
            //    if (team1.GetPlayerList().Count == 2)
            //    {
            //        return ConnectToTeam(gameId, team2.TeamId, playerName, clientApi);
            //    }
            //    if (team2.GetPlayerList().Count == 2)
            //    {
            //        return ConnectToTeam(gameId, team1.TeamId, playerName, clientApi);
            //    }
            //    var randomTeam = _random.Next(1) == 0 ? team1 : team2;
            //    return ConnectToTeam(gameId, randomTeam.TeamId, playerName, clientApi);
            //}

            //public PlayerInfo? ConnectToTeam(string gameId, string teamId, string playerName, ISidiBarraniClientApi clientApi)
            //{
            //    Log.Verbose($"{this}: ConnectToTeam({gameId},{teamId},{playerName},{clientApi})");
            //    if (!_gameServiceDictionary.ContainsKey(gameId))
            //    {
            //        return null;
            //    }
            //    var gameService = _gameServiceDictionary[gameId];
            //    var teamInfo = gameService.PlayerGroupInfo.GetTeamInfo(teamId);
            //    if (teamInfo == null || teamInfo.GetPlayerList().Count >= 2)
            //    {
            //        return null;
            //    }
            //    var playerInfo = new PlayerInfo
            //    {
            //        PlayerName = playerName,
            //        PlayerConnectionId = Guid.NewGuid().ToString()
            //    };
            //    if (teamInfo.Player1 == null)
            //    {
            //        teamInfo.Player1 = playerInfo;
            //    }
            //    else
            //    {
            //        teamInfo.Player2 = playerInfo;
            //    }
            //    gameService.ClientApiDictionary[playerInfo.PlayerConnectionId] = clientApi;
            //    return (PlayerInfo)playerInfo?.Clone();
            //}

            //public bool StartGame(string gameId)
            //{
            //    Log.Verbose($"{this}: StartGame({gameId})");
            //    if (!_gameServiceDictionary.ContainsKey(gameId))
            //    {
            //        return false;
            //    }
            //    var gameService = _gameServiceDictionary[gameId];
            //    return gameService.StartGame();
            //}

            //public bool ProcessAction(ActionInfo actionInfo)
            //{
            //    Log.Verbose($"{this}: ProcessAction({actionInfo})");
            //    if (!_gameServiceDictionary.ContainsKey(actionInfo.GameId))
            //    {
            //        return false;
            //    }
            //    var gameService = _gameServiceDictionary[actionInfo.GameId];
            //    var playerInfo = gameService.PlayerGroupInfo.GetPlayerInfo(actionInfo.PlayerConnectionId);
            //    if (playerInfo == null)
            //    {
            //        return false;
            //    }
            //    var action = _actionCache.ConstructAction(
            //        (GameInfo)gameService.GameInfo?.Clone(),
            //        (PlayerInfo)playerInfo?.Clone(),
            //        actionInfo.ActionId);
            //    if (action == null)
            //    {
            //        return false;
            //    }

            //    return gameService.ProcessAction(action);
            //}

            //public override string ToString()
            //{
            //    return $"SidiBarraniServer";
            //}
        }
    }
