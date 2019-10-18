
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;
using SidiBarraniCommon.Info;

namespace SidiBarrani.ViewModel
{
    public class PlayerRepresentation : ReactiveObject
    {
        public string PlayerName {get;}
        public IList<BetRepresentation> BetRepresentationList => null;
        public BetRepresentation BetRepresentation => null;
        public StickPilesRepresentation StickPilesRepresentation => null;
        public int CardsInHand => 0;
        public bool IsCurrentPlayer => false;

        //TODO
        public PlayerRepresentation(PlayerInfo playerInfo)
        {
            PlayerName = playerInfo.PlayerName;
        }
    }
}