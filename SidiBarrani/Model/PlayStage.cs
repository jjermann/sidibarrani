using System.Collections.Generic;
using System.Linq;

namespace SidiBarrani.Model
{
    public class PlayStage
    {
        private PlayStage() { }
        public PlayStage(Rules rules, PlayerGroup playerGroup, Player initialPlayer, PlayType playType) {
            Rules = rules;
            PlayerGroup = playerGroup;
            PlayType = playType;
            StickResultList = new List<StickResult>();
            CurrentStickRound = new StickRound(Rules, PlayerGroup, initialPlayer, PlayType);
        }

        private Rules Rules {get;set;}
        private PlayerGroup PlayerGroup {get;}
        private PlayType PlayType {get;}
        private IList<StickResult> StickResultList {get;}
        private StickRound CurrentStickRound {get;set;}
        public Player CurrentPlayer => CurrentStickRound.CurrentPlayer;

        public PlayResult GetPlayResult()
        {
            if (!IsOver())
            {
                return null;
            }
            var playerResultDictionary = PlayerGroup
                .GetPlayerList()
                .ToDictionary(p => p, p => {
                    var wonCards = StickResultList
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
                var generalAmount = new ScoreAmount(isMatch:true, isGeneral:true);
                var zeroAmount = new ScoreAmount();
                return new PlayResult
                {
                    PlayerGroup = PlayerGroup,
                    Team1Score = generalPlayer.Team == PlayerGroup.Team1
                        ? generalAmount
                        : zeroAmount,
                    Team2Score = generalPlayer.Team == PlayerGroup.Team2
                        ? generalAmount
                        : zeroAmount,
                };
            }
            var teamResultDictionary = playerResultDictionary
                .GroupBy(p => p.Key.Team)
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
                var matchAmount = new ScoreAmount(isMatch:true);
                var zeroAmount = new ScoreAmount();
                return new PlayResult
                {
                    PlayerGroup = PlayerGroup,
                    Team1Score = generalPlayer.Team == PlayerGroup.Team1
                        ? matchAmount
                        : zeroAmount,
                    Team2Score = generalPlayer.Team == PlayerGroup.Team2
                        ? matchAmount
                        : zeroAmount
                };                
            }
            //Case no General/Match
            var team1Amount = teamResultDictionary[PlayerGroup.Team1]
                .Select(c => c.GetValue(PlayType))
                .Sum();
            var team2Amount = teamResultDictionary[PlayerGroup.Team2]
                .Select(c => c.GetValue(PlayType))
                .Sum();
            var lastStickTeam = StickResultList.Last().Winner.Team;
            if (lastStickTeam == PlayerGroup.Team1)
            {
                team1Amount += 5;
            }
            if (lastStickTeam == PlayerGroup.Team2)
            {
                team2Amount += 5;
            }
            return new PlayResult
            {
                PlayerGroup = PlayerGroup,
                Team1Score = new ScoreAmount(team1Amount),
                Team2Score = new ScoreAmount(team2Amount)
            };  
        }

        private bool IsOver()
        {
            return StickResultList.Count == 9;
        }

        public IList<PlayAction> GetValidPlayActions()
        {
            var validPlayActions = IsOver()
                ? new List<PlayAction>()
                : CurrentStickRound.GetValidPlayActions();
            return validPlayActions;
        }

        public bool AddPlayActionAndProceed(PlayAction playAction)
        {
            var validPlayActions = GetValidPlayActions();
            if (!validPlayActions.Contains(playAction)) 
            {
                return false;
            }
            CurrentStickRound.AddPlayActionAndProceed(playAction);
            var stickResult = CurrentStickRound.GetStickResult();
            if (stickResult != null)
            {
                StickResultList.Add(stickResult);
                stickResult.Winner.Context.WonSticks.Add(stickResult.StickPile);
                CurrentStickRound = IsOver()
                    ? null
                    : new StickRound(Rules, PlayerGroup, stickResult.Winner, PlayType);
            }
            return true;
        }
    }
}