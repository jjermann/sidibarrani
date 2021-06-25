namespace SidiBarrani.Shared.Model.Game
{
    public record Card(CardSuit CardSuit, CardRank CardRank)
    {
        public override string ToString()
        {
            var str = $"{CardRank} of {CardSuit}";
            return str;
        }
    }
}