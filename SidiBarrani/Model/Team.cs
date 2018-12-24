namespace SidiBarrani.Model
{
    public class Team
    {
        public string Name {get;set;}
        public Player Player1 {get; set;}
        public Player Player2 {get; set;}
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Name))
            {
                return Name;
            }
            return $"({Player1}, {Player2})";
        }
    }
}