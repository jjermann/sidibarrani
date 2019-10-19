using System;
using System.Collections.Generic;
using System.Linq;
using SidiBarraniCommon.Action;
using SidiBarraniCommon.Model;
using SidiBarraniCommon.Result;

namespace SidiBarraniCommon.Info
{
    public class GameStageInfo : ICloneable
    {
        public PlayerInfo CurrentPlayer {get;set;}
        public ActionType? ExpectedActionType {get;set;}
        public IList<BetAction> CurrentBetActionList {get;set;}
        public BetResult CurrentBetResult {get;set;}
        public IList<StickRoundInfo> StickRoundInfoList {get;set;}
        public PlayResult CurrentPlayResult {get;set;}
        public IList<RoundResult> RoundResultList {get;set;}
        public GameResult GameResult {get;set;}

        public Bet CurrentActiveBet => CurrentBetResult?.Bet;
        public PlayType? CurrentPlayType => CurrentActiveBet?.PlayType;
        public StickRoundInfo CurrentStickRoundInfo => StickRoundInfoList?.LastOrDefault();
        public IList<PlayAction> CurrentStickPlayActionList => CurrentStickRoundInfo?.PlayActionList;
        public StickResult CurrentStickResult => CurrentStickRoundInfo?.StickResult;
        public RoundResult CurrentRoundResult => RoundResultList?.LastOrDefault();

        public object Clone()
        {
            return new GameStageInfo
            {
                CurrentPlayer = (PlayerInfo)CurrentPlayer?.Clone(),
                ExpectedActionType = ExpectedActionType,
                CurrentBetActionList = CurrentBetActionList
                    ?.Select(a => (BetAction)a?.Clone()).ToList(),
                CurrentBetResult = (BetResult)CurrentBetResult?.Clone(),
                StickRoundInfoList = StickRoundInfoList
                    ?.Select(s => (StickRoundInfo)s?.Clone()).ToList(),
                CurrentPlayResult = (PlayResult)CurrentPlayResult?.Clone(),
                RoundResultList = RoundResultList
                    ?.Select(r => (RoundResult)r?.Clone()).ToList(),
                GameResult = (GameResult)GameResult?.Clone()
            };
        }
    }
}
