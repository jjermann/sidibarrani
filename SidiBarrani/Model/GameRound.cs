using System;
using System.Linq;

namespace SidiBarrani.Model
{
    public class GameRound
    {
        private Rules Rules {get;set;}
        private PlayerGroup PlayerGroup {get;set;}
        private Player InitialPlayer {get;set;}
        public GameRound(Rules rules, PlayerGroup playerGroup, Player initialPlayer) {
            Rules = rules;
            PlayerGroup = playerGroup;
            InitialPlayer = initialPlayer;
        }
        public RoundResult PlayRound()
        {
            var betResult = ProcessBetStage(Rules, PlayerGroup, InitialPlayer);
            //TODO: All the rest!
            var playStage = new PlayStage();
            var playResult = playStage.GetPlayResult();
            while (playResult != null)
            {
                // get next PlayAction
                playResult = playStage.GetPlayResult();
            }
            // determine RoundResult
            var roundResult = new RoundResult();
            return roundResult;
        }

        private static BetResult ProcessBetStage(Rules rules, PlayerGroup playerGroup, Player initialPlayer)
        {
            var betStage = new BetStage(rules, playerGroup, initialPlayer);
            var betResult = betStage.GetBetResult();
            while (betResult == null) {
                var validActionDictionary = betStage
                    .GetValidBetActions()
                    .GroupBy(a => a.Player)
                    .ToDictionary(g => g.Key, g => g.ToList());
                // foreach (var player in validActionDictionary.Keys) {
                //     var validPlayerActions = validActionDictionary[player];
                //     //Create a task for player to return an action with cancelation token in case an action occurs somewhere else (or something of that sort)
                //     //-> Figure out how to handle time for Sidi/Barrani!
                // }
                var randomAction = validActionDictionary.Values
                    .SelectMany(l => l)
                    .OrderBy(a => Guid.NewGuid())
                    .FirstOrDefault();
                betStage.AddBetActionAndProgress(randomAction);
                betResult = betStage.GetBetResult();
            }
            return betResult;
        }
    }
}