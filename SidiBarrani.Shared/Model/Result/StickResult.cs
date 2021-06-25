using System.Collections.Generic;
using SidiBarrani.Shared.Model.Game;
using SidiBarrani.Shared.Model.Setup;

namespace SidiBarrani.Shared.Model.Result
{
    public record StickResult(PlayerSetup Winner, IList<Card> StickPile)
    {
        public override string ToString()
        {
            var str = $"{Winner} won the stick.";
            return str;
        }
    }
}