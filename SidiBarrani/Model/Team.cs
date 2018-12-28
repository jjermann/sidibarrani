namespace SidiBarrani.Model
{
    public class Team
    {
        public string Name {get;set;}
        public Player Player1 {get; set;}
        public Player Player2 {get; set;}

        public static Team CreateTeam(Player player1, Player player2, string name = null)
        {
            var team = new Team
            {
                Player1 = player1,
                Player2 = player2,
                Name = name
            };
            player1.Team = team;
            player2.Team = team;
            return team;
        }

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