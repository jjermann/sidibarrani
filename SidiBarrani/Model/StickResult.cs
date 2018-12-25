namespace SidiBarrani.Model
{
    public class StickResult
    {
        public Player Winner {get;set;}
        public StickPile StickPile {get;set;}

        public override string ToString()
        {
            var str = $"{Winner} won the stick.";
            return str;
        }
    }
}