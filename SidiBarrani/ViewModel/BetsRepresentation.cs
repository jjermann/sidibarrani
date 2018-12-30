using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Linq;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using SidiBarrani.Model;

namespace SidiBarrani.ViewModel
{
    public class BetsRepresentation : ReactiveObject
    {
        public PlayTypeRepresentation PlayTypeRepresentation {get;}
        public IList<ScoreAmount> BetAmountList {get;}
        private string _logOutput;
        public string LogOutput
        {
            get { return _logOutput; }
            set { this.RaiseAndSetIfChanged(ref _logOutput, value); }
        }

        private BetsRepresentation() { }
        public BetsRepresentation(PlayType playType, IList<ScoreAmount> betAmountList)
        {
            PlayTypeRepresentation = new PlayTypeRepresentation(playType, 20);
            BetAmountList = betAmountList;
        }
    }
}