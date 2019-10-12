﻿using System;
using System.Collections.Generic;
using System.Linq;
using SidiBarraniCommon;
using SidiBarraniCommon.Cache;
using SidiBarraniCommon.Info;
using SidiBarraniCommon.Model;
using SidiBarraniServer.Game;

namespace SidiBarraniServer
{
    public class SidiBarraniServerImplementation : ISidiBarraniServerApi
    {
        private IDictionary<string, GameService> _gameServiceDictionary = new Dictionary<string, GameService>();
        private ActionCache _actionCache;
        private Random _random;

        public SidiBarraniServerImplementation()
        {
            var rules = new Rules();
            _actionCache = new ActionCache(rules);
            _random = new Random();
        }

        public GameInfo OpenGame(string gameName = "Game", string team1Name = "Team1", string team2Name = "Team2")
        {
            Console.WriteLine($"{this}: OpenGame({gameName},{team1Name},{team2Name})");
            if (gameName == null || team1Name == null || team2Name == null)
            {
                return null;
            }
            var playerGroupInfo = new PlayerGroupInfo
            {
                Team1 = new TeamInfo
                {
                    TeamName = team1Name,
                    TeamId = Guid.NewGuid().ToString()
                },
                Team2 = new TeamInfo
                {
                    TeamName = team2Name,
                    TeamId = Guid.NewGuid().ToString()
                }
            };
            var gameInfo = new GameInfo
            {
                GameName = gameName,
                GameId = Guid.NewGuid().ToString(),
                PlayerGroupInfo = playerGroupInfo
            };
            _gameServiceDictionary[gameInfo.GameId] = new GameService
            {
                GameInfo = gameInfo,
                PlayerGroupInfo = playerGroupInfo
            };
            return (GameInfo)gameInfo.Clone();
        }

        public IList<GameInfo> ListOpenGames()
        {
            Console.WriteLine($"{this}: ListOpenGames()");
            return _gameServiceDictionary.Values
                .Where(s => !s.IsActive)
                .Select(g => (GameInfo)(g.GameInfo.Clone()))
                .ToList();
        }

        public PlayerInfo ConnectToGame(string gameId, string playerName, ISidiBarraniClientApi clientApi)
        {
            Console.WriteLine($"{this}: ConnectToGame({gameId},{playerName},{clientApi})");
            if (!_gameServiceDictionary.ContainsKey(gameId))
            {
                return null;
            }
            var gameService = _gameServiceDictionary[gameId];
            if (gameService.PlayerGroupInfo.GetPlayerList().Count >= 4)
            {
                return null;
            }
            var team1 = gameService.PlayerGroupInfo.Team1;
            var team2 = gameService.PlayerGroupInfo.Team2;
            if (team1.GetPlayerList().Count == 2)
            {
                return ConnectToTeam(gameId, team2.TeamId, playerName, clientApi);
            }
            if (team2.GetPlayerList().Count == 2)
            {
                return ConnectToTeam(gameId, team1.TeamId, playerName, clientApi);
            }
            var randomTeam = _random.Next(1) == 0 ? team1 : team2;
            return ConnectToTeam(gameId, randomTeam.TeamId, playerName, clientApi);
        }

        public PlayerInfo ConnectToTeam(string gameId, string teamId, string playerName, ISidiBarraniClientApi clientApi)
        {
            Console.WriteLine($"{this}: ConnectToTeam({gameId},{teamId},{playerName},{clientApi})");
            if (!_gameServiceDictionary.ContainsKey(gameId))
            {
                return null;
            }
            var gameService = _gameServiceDictionary[gameId];
            var teamInfo = gameService.PlayerGroupInfo.GetTeamInfo(teamId);
            if (teamInfo == null || teamInfo.GetPlayerList().Count >= 2)
            {
                return null;
            }
            var playerInfo = new PlayerInfo
            {
                PlayerName = playerName,
                PlayerId = Guid.NewGuid().ToString()
            };
            if (teamInfo.Player1 == null)
            {
                teamInfo.Player1 = playerInfo;
            }
            else
            {
                teamInfo.Player2 = playerInfo;
            }
            gameService.ClientApiDictionary[playerInfo.PlayerId] = clientApi;
            return (PlayerInfo)playerInfo.Clone();
        }

        public bool StartGame(string gameId)
        {
            Console.WriteLine($"{this}: StartGame({gameId})");
            if (!_gameServiceDictionary.ContainsKey(gameId))
            {
                return false;
            }
            var gameService = _gameServiceDictionary[gameId];
            return gameService.StartGame();
        }

        public bool ProcessAction(ActionInfo actionInfo)
        {
            Console.WriteLine($"{this}: ProcessAction({actionInfo})");
            if (!_gameServiceDictionary.ContainsKey(actionInfo.GameId))
            {
                return false;
            }
            var gameService = _gameServiceDictionary[actionInfo.GameId];
            var playerInfo = gameService.PlayerGroupInfo.GetPlayerInfo(actionInfo.PlayerId);
            if (playerInfo == null)
            {
                return false;
            }
            var action = _actionCache.ConstructAction(
                (GameInfo)gameService.GameInfo.Clone(),
                (PlayerInfo)playerInfo.Clone(),
                actionInfo.ActionId);
            if (action == null)
            {
                return false;
            }

            return gameService.ProcessAction(action);
        }

        public override string ToString()
        {
            return $"SidiBarraniServer";
        }
    }
}
