namespace SidiBarrani.Shared.Model.Setup
{
    public class PlayerSetup
    {
        public PlayerSetup(string playerConnectionId, string playerName)
        {
            PlayerConnectionId = playerConnectionId;
            PlayerName = playerName;
        }

        public string PlayerConnectionId { get; }
        public string PlayerName { get; set; }

        public override string ToString() => PlayerName;
    }
}
