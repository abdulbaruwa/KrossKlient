using System.Runtime.Serialization;
using ReactiveUI;

namespace KrossKlient.ViewModels
{
    [DataContract]
    public class SelectedWordViewModel : WordViewModel
    {
        [DataMember]private int _cursor;
        public int Cursor
        {
            get { return _cursor; }
            set { this.RaiseAndSetIfChanged(ref _cursor, value); }
        }
    }
}