using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;

namespace SidiBarrani.Model
{
    public class PlayStage : ReactiveObject
    {
        private PlayStage() { }
        public PlayStage(Rules rules, PlayerGroup playerGroup, Player initialPlayer, PlayType playType) {
            Rules = rules;
            PlayerGroup = playerGroup;
            PlayType = playType;
            StickResultSourceList = new SourceList<StickResult>();
            StickResultSourceList
                .Connect()
                .ToCollection()
                .ToProperty(this, x => x.StickResultList, out _stickResultList, new ReadOnlyCollection<StickResult>(new List<StickResult>()));

            CurrentStickRound = new StickRound(Rules, PlayerGroup, initialPlayer, PlayType);
        }

        private Rules Rules {get;set;}
        private PlayerGroup PlayerGroup {get;}
        private PlayType PlayType {get;}
        private SourceList<StickResult> StickResultSourceList {get;}
        private ObservableAsPropertyHelper<IReadOnlyCollection<StickResult>> _stickResultList;
        public IReadOnlyCollection<StickResult> StickResultList
        {
            get { return _stickResultList.Value; }
        }

        private StickRound _stickRound;
        public StickRound CurrentStickRound
        {
            get { return _stickRound; }
            private set { this.RaiseAndSetIfChanged(ref _stickRound, value); }
        }
        private StickResult _stickResult;
        public StickResult StickResult
        {
            get { return _stickResult; }
            private set { this.RaiseAndSetIfChanged(ref _stickResult, value); }
        }
        //TODO: Make this readonly reactive
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
            return StickResultSourceList.Count == 9;
        }

        public IList<PlayAction> GetValidPlayActions()
        {
            var validPlayActions = IsOver()
                ? new List<PlayAction>()
                : CurrentStickRound.GetValidPlayActions();
            return validPlayActions;
        }

        public StickResult AddPlayActionAndProceed(PlayAction playAction)
        {
            var validPlayActions = GetValidPlayActions();
            if (!validPlayActions.Contains(playAction)) 
            {
                throw new InvalidOperationException();
            }
            CurrentStickRound.AddPlayActionAndProceed(playAction);
            var stickResult = CurrentStickRound.GetStickResult();
            return stickResult;
        }

        public void ProcessStickResult(StickResult stickResult)
        {
            StickResult = stickResult;
            StickResultSourceList.Add(stickResult);
            stickResult.Winner.Context.WonSticks.Add(stickResult.StickPile);
        }

        public void UpdateStickRound(StickResult stickResult)
        {
            CurrentStickRound.ResetCardContext();
            CurrentStickRound = IsOver()
                ? null
                : new StickRound(Rules, PlayerGroup, stickResult.Winner, PlayType);
        }
    }
}