using System.Runtime.Serialization;
using KrossKlient.Common;
using ReactiveUI;

namespace KrossKlient.ViewModels
{
    public interface ILoginMethods
    {
        void SaveCredentials(string userName = null);
    }


    [DataContract]
    public class CellViewModel : EmptyCellViewModel
    {
        public CellViewModel(int col, int row, string value, WordViewModel wordViewModel, string wordPosition)
            : base(col, row, value)
        {
            _wordViewModel = wordViewModel;
            base.WordPosition = wordPosition;
            IsVisible = CellState.IsUsed;
        }

        [DataMember]private WordViewModel _wordViewModel;
        public WordViewModel Word
        {
            get { return _wordViewModel; }
            set { this.RaiseAndSetIfChanged(ref _wordViewModel, value); }
        }
    }
}