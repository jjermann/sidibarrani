using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using SidiBarraniCommon.Action;
using SidiBarraniCommon.Info;
using SidiBarraniCommon.Model;
using SidiBarraniCommon.Result;

namespace SidiBarraniServer.Game
{
    public class PlayStage
    {
        private Rules Rules { get; }
        private PlayerGroupInfo PlayerGroupInfo { get; }
        private Action ConfirmAction {get;}
        private IDictionary<string,CardPile> PlayerHandDictionary {get;}
        private PlayType PlayType {get;}

        public IList<StickRound> StickRoundList {get;} = new List<StickRound>();
        public PlayResult PlayResult {get;set;}
        public StickRound CurrentStickRound => StickRoundList?.LastOrDefault();

        public PlayStage(
            Rules rules,
            PlayerGroupInfo playerGroupInfo,
            Action confirmAction,
            IDictionary<string,CardPile> playerHandDictionary,
            PlayerInfo initialPlayer,
            PlayType playType)
        {
            Rules = rules;
            PlayerGroupInfo = playerGroupInfo;
            ConfirmAction = confirmAction;
            PlayerHandDictionary = playerHandDictionary;
            PlayType = playType;
            var stickRound = new StickRound(PlayerGroupInfo, PlayerHandDictionary, initialPlayer, PlayType);
            StickRoundList.Add(stickRound);
        }

        public IList<int> GetValidActionIdList(string playerId)
        {
            if (CurrentStickRound == null)
            {
                return new List<int>();
            }
            return CurrentStickRound.GetValidActionIdList(playerId);
        }

        public void ProcessPlayAction(PlayAction playAction)
        {
            CurrentStickRound.ProcessPlayAction(playAction);
            if (CurrentStickRound.StickResult != null)
            {
                Log.Information(CurrentStickRound.StickResult.ToString());
                ConfirmAction?.Invoke();
                PlayResult = GetPlayResult();
                if (PlayResult == null)
                {
                    var currentPlayer = CurrentStickRound.StickResult.Winner;
                    var stickRound = new StickRound(PlayerGroupInfo, PlayerHandDictionary, currentPlayer, PlayType);
                    StickRoundList.Add(stickRound);
                }
            }
            return;
        }

        public PlayResult GetPlayResult()
        {
            var isOver = StickRoundList.Count == 9 && CurrentStickRound.StickResult != null;
            if (!isOver)
            {
                return null;
            }
            var playerResultDictionary = PlayerGroupInfo
                .GetPlayerList()
                .ToDictionary(p => p, p =>
                {
                    var wonCards = StickRoundList
                        .Select(r => r.StickResult)
                        .Where(r => r.Winner == p)
                        .SelectMany(r => r.StickPile.Cards)
                        .ToList();
                    return wonCards;
                });
            var playersWithSticks = playerResultDictionary
                .Where(p => p.Value.Any())
                .Select(p => p.Key)
                .ToList();
            var generalPlayer = playersWithSticks.Count == 1
                ? playersWithSticks.Single()
                : null;
            if (generalPlayer != null)
            {
                //Case General
                var generalAmount = new ScoreAmount(isMatch: true, isGeneral: true);
                var zeroAmount = new ScoreAmount();
                return new PlayResult
                {
                    PlayerGroupInfo = PlayerGroupInfo,
                    Team1Score = PlayerGroupInfo.GetTeamOfPlayer(generalPlayer.PlayerId) == PlayerGroupInfo.Team1
                        ? generalAmount
                        : zeroAmount,
                    Team2Score = PlayerGroupInfo.GetTeamOfPlayer(generalPlayer.PlayerId) == PlayerGroupInfo.Team2
                        ? generalAmount
                        : zeroAmount
                };
            }
            var teamResultDictionary = playerResultDictionary
                .GroupBy(p => PlayerGroupInfo.GetTeamOfPlayer(p.Key.PlayerId))
                .ToDictionary(g => g.Key, g => g.ToList().SelectMany(p => p.Value).ToList());
            var teamsWithSticks = teamResultDictionary
                .Where(p => p.Value.Any())
                .Select(p => p.Key)
                .ToList();
            var matchTeam = teamsWithSticks.Count == 1
                ? teamsWithSticks.Single()
                : null;
            if (matchTeam != null)
            {
                //Case Match
                var matchAmount = new ScoreAmount(isMatch: true);
                var zeroAmount = new ScoreAmount();
                return new PlayResult
                {
                    PlayerGroupInfo = PlayerGroupInfo,
                    Team1Score = PlayerGroupInfo.GetTeamOfPlayer(generalPlayer.PlayerId) == PlayerGroupInfo.Team1
                        ? matchAmount
                        : zeroAmount,
                    Team2Score = PlayerGroupInfo.GetTeamOfPlayer(generalPlayer.PlayerId) == PlayerGroupInfo.Team2
                        ? matchAmount
                        : zeroAmount
                };
            }
            //Case no General/Match
            var team1Amount = teamResultDictionary[PlayerGroupInfo.Team1]
                .Select(c => c.GetValue(PlayType))
                .Sum();
            var team2Amount = teamResultDictionary[PlayerGroupInfo.Team2]
                .Select(c => c.GetValue(PlayType))
                .Sum();
            var lastStickTeam = PlayerGroupInfo.GetTeamOfPlayer(StickRoundList.Last().StickResult.Winner.PlayerId);
            if (lastStickTeam == PlayerGroupInfo.Team1)
            {
                team1Amount += 5;
            }
            if (lastStickTeam == PlayerGroupInfo.Team2)
            {
                team2Amount += 5;
            }
            return new PlayResult
            {
                PlayerGroupInfo = PlayerGroupInfo,
                Team1Score = new ScoreAmount(team1Amount),
                Team2Score = new ScoreAmount(team2Amount)
            };
        }
    }
}