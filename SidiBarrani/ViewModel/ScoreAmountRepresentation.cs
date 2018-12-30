using SidiBarrani.Model;

namespace SidiBarrani.ViewModel
{
    public class ScoreAmountRepresentation
    {
        private ScoreAmount ScoreAmount {get;}
        public string AmountString {get;}
        public string AmountColor {get;}

        private ScoreAmountRepresentation() { }
        public ScoreAmountRepresentation(ScoreAmount scoreAmount)
        {
            ScoreAmount = scoreAmount;
            AmountString = scoreAmount.ToString();
            if (scoreAmount.IsGeneral)
            {
                AmountString = scoreAmount.Amount.ToString();
            }
            else if (scoreAmount.IsMatch)
            {
                AmountString = scoreAmount.Amount.ToString();
            }
            AmountColor = scoreAmount.IsGeneral
                ? "DarkRed"
                : scoreAmount.IsMatch
                    ? "Orange"
                    : "Black";
        }
    }
}