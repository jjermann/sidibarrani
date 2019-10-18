using ReactiveUI;
using SidiBarraniCommon.Info;
using SidiBarraniCommon.Model;

namespace SidiBarrani.ViewModel
{
    public class BoardRepresentation : ReactiveObject
    {
        private GameStageInfo GameStageInfo {get;}
        public CardRepresentation TopCardRepresentation {get;}
        public CardRepresentation LeftCardRepresentation {get;}
        public CardRepresentation RightCardRepresentation {get;}
        public CardRepresentation BottomCardRepresentation {get;}

        public BoardRepresentation(GameStageInfo gameStageInfo)
        {
            GameStageInfo = gameStageInfo;
            //TODO
            var someCard = new Card();
            TopCardRepresentation = new CardRepresentation(someCard);
            LeftCardRepresentation = new CardRepresentation(someCard);
            RightCardRepresentation = new CardRepresentation(someCard);
            BottomCardRepresentation = new CardRepresentation(someCard);
        }
    }
}