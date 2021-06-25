using System.Collections.Generic;
using System.Linq;
using SidiBarraniCommon.ActionOld;
using SidiBarraniCommon.Model;
using SidiBarraniCommon.Result;

namespace SidiBarraniCommon.InfoOld
{
    public class GameStageInfo
    {
        public PlayerInfo? CurrentPlayer {get;set;}
        public ActionType? ExpectedActionType {get;set;}
        public IList<BetAction>? CurrentBetActionList {get;set;}
        public BetResult? CurrentBetResult {get;set;}
        public IList<StickRoundInfo>? StickRoundInfoList {get;set;}
        public PlayResult? CurrentPlayResult {get;set;}
        public IList<RoundResult?>? RoundResultList {get;set;}
        public GameResult? GameResult {get;set;}

        public Bet? CurrentActiveBet => CurrentBetResult?.Bet;
        public PlayType? CurrentPlayType => CurrentActiveBet?.PlayType;
        public StickRoundInfo? CurrentStickRoundInfo => StickRoundInfoList?.LastOrDefault();
        public IList<PlayAction>? CurrentStickPlayActionList => CurrentStickRoundInfo?.PlayActionList;
        public StickResult? CurrentStickResult => CurrentStickRoundInfo?.StickResult;
        public RoundResult? CurrentRoundResult => RoundResultList?.LastOrDefault();
    }
}
