namespace SidiBarrani.Model
{
    public class Player
    {
        public string Name {get;set;}
        public Team Team {get; set;}
        public PlayerContext Context {get;set;}
        public override string ToString() {
            return Name;
        }
    }
}