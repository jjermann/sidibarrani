using System.Collections.Generic;
using System.Linq;

namespace SidiBarrani.Shared.Model.Game
{
    public record Rules(int MinBet = 40, bool AllowUpDown = true, int EndScore = 200)
    {
        public IList<Bet> GetValidBets()
        {
            var amountList = new List<int>
            {
                40,
                50,
                60,
                70,
                80,
                90,
                100,
                110,
                120,
                130,
                140,
                150
            };
            var playTypeList = new List<PlayType>
            {
                PlayType.TrumpDiamonds,
                PlayType.TrumpHearts,
                PlayType.TrumpClubs,
                PlayType.TrumpSpades,
                PlayType.UpDown,
                PlayType.DownUp
            };
            var betList = playTypeList
                .SelectMany(t =>
                {
                    var betsForType = amountList
                        .Select(a => new Bet(t, a))
                        .ToList();
                    return betsForType;
                })
                .ToList();
            var specialBetList = playTypeList
                .SelectMany(t =>
                {
                    var specialListForType = new List<Bet> {
                        // ReSharper disable once RedundantArgumentDefaultValue
                        new(t, false),
                        new(t, true)
                    };
                    return specialListForType;
                })
                .ToList();
            betList.AddRange(specialBetList);

            var result = betList
                .Where(b => b.BetAmount.Amount >= MinBet)
                .Where(b => AllowUpDown || b.PlayType != PlayType.DownUp && b.PlayType != PlayType.UpDown)
                .ToList();
            return result;
        }
    }
}