using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using ReactiveUI;
using SidiBarraniCommon.Action;
using SidiBarraniCommon.Info;
using SidiBarraniCommon.Model;

namespace SidiBarrani.ViewModel
{
    public class PlayerRepresentation : ReactiveObject
    {
        private IDictionary<int, BetAction> BetActionDictionary {get;}
        public string PlayerName {get;}
        public IList<BetActionRepresentation> BetActionRepresentationList {get;}
        public bool ShowBetActionList => BetActionRepresentationList?.Any() ?? false;
        public StickPilesRepresentation StickPilesRepresentation {get;}
        public int CardsInHand {get;}
        public bool IsCurrentPlayer {get;}

        public PlayerRepresentation(
            PlayerInfo playerInfo,
            IDictionary<int, BetAction> betActionDictionary,
            IList<IList<Card>> stickPileList,
            int cardsInHand,
            bool isCurrentPlayer)
        {
            BetActionDictionary = betActionDictionary;
            PlayerName = playerInfo.PlayerName;
            CardsInHand = cardsInHand;
            IsCurrentPlayer = isCurrentPlayer;
            StickPilesRepresentation = new StickPilesRepresentation(stickPileList);
            var maxIndex = BetActionDictionary?.Keys?.DefaultIfEmpty()?.Max();
            BetActionRepresentationList = BetActionDictionary
                ?.OrderBy(p => p.Key)
                ?.Select(p => new BetActionRepresentation(
                    p.Value,
                    index: p.Key + 1,
                    isCurrentBetAction: IsCurrentPlayer && p.Key == maxIndex))
                ?.ToList();
        }
    }
}