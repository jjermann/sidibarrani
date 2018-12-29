using System;
using System.IO;
using SidiBarrani.Model;

namespace SidiBarrani.ViewModel
{
    public class PlayTypeRepresentation
    {
        public string ImageSource {get;}
        private PlayType PlayType {get;}
        public PlayTypeRepresentation(PlayType playType)
        {
            PlayType = playType;
            var playTypeStr = playType.ToString().ToLowerInvariant();
            ImageSource = Path.Combine(@"Images", $"{playTypeStr}.png");
        }
    }
}