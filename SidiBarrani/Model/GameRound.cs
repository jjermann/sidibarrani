namespace SidiBarrani.Model
{
    public class GameRound
    {
        public RoundResult PlayRound()
        {
            var betStage = new BetStage();
            var betResult = betStage.GetBetResult();
            while (betResult == null) {
                // get next BetAction
                betResult = betStage.GetBetResult();
            }
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