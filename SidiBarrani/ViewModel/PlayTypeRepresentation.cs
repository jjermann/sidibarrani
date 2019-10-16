using System.IO;
using SidiBarraniCommon.Model;

namespace SidiBarrani.ViewModel
{
    public class PlayTypeRepresentation
    {
        public string ImageSource {get;}
        public double Size {get;}
        public PlayType PlayType {get;}
        private PlayTypeRepresentation() { }
        public PlayTypeRepresentation(PlayType playType, double size)
        {
            PlayType = playType;
            Size = size;
            var playTypeStr = playType.GetStringRepresentation().ToLowerInvariant();
            ImageSource = Path.Combine("Assets", $"{playTypeStr}.png");
        }
    }
}