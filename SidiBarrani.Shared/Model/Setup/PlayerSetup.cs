namespace SidiBarrani.Shared.Model.Setup
{
    public class PlayerSetup
    {
        public PlayerSetup(string playerId, string playerName)
        {
            PlayerId = playerId;
            PlayerName = playerName;
        }

        public string PlayerId { get; }
        public string PlayerName { get; set; }

        public override string ToString() => PlayerName;
    }
}
