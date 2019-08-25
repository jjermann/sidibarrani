using System;
using System.IO;
using SidiBarrani.Model;

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
            ImageSource = Path.Combine(Constants.BaseUri, @"Images", $"{playTypeStr}.png");
        }
    }
}