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
            var betStage = new BetStage(Rules, PlayerGroup, InitialPlayer);
            var betResult = betStage.GetBetResult();
            while (betResult == null) {
                var validActions = betStage.GetValidBetActions();
                //TODO: Don't choose actions randomly :)
                var randomAction = validActions.OrderBy(a => Guid.NewGuid()).First();
                betStage.AddBetActionAndProgress(randomAction);
                betResult = betStage.GetBetResult();
            }

            Console.WriteLine(betResult);
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
    }
}