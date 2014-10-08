using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using KrossKlient.Common;
using ReactiveUI;

namespace KrossKlient.ViewModels
{
    [DataContract]
    public class WordViewModel : ReactiveObject
    {
        private bool _enteredValueAddedToBoard;

        public WordViewModel()
        {
            //Messenger.Default.Register<CellValueChangedMessage>(this, m => HandleChangedCellValue(m));
        }

        [DataMember]private ObservableCollection<EmptyCellViewModel> _cells;
        public ObservableCollection<EmptyCellViewModel> Cells
        {
            get { return _cells; }
            set { this.RaiseAndSetIfChanged(ref _cells, value); }
        }

        [DataMember]private string _word;
        public string Word
        {
            get { return _word; }
            set { this.RaiseAndSetIfChanged(ref _word, value); }
        }



        [DataMember]private string _wordHint;
        public string WordHint
        {
            get
            {
                return string.Format("{0} {1}", _wordHint, WordLength);
            }
            set { this.RaiseAndSetIfChanged(ref _wordHint, value); }
        }

        [DataMember]private int _index;
        public int Index
        {
            get { return _index; }
            set { this.RaiseAndSetIfChanged(ref _index, value); }
        }

        [DataMember]private string _wordLength;
        public string WordLength
        {
            get { return _wordLength; }
            set { this.RaiseAndSetIfChanged(ref _wordLength, value); }
        }

        [DataMember]private Direction _direction;
        public Direction Direction
        {
            get { return _direction; }
            set { this.RaiseAndSetIfChanged(ref _direction, value); }
        }

        public bool EnteredValueAddedToBoard
        {
            get { return _enteredValueAddedToBoard; }
            set { this.RaiseAndSetIfChanged(ref _enteredValueAddedToBoard, value); }
        }

        public bool IsWordAnswerCorrect
        {
            get { return GetWordAnswer(); }
        }

        private bool GetWordAnswer()
        {
            return _cells.All(cellEmptyViewModel =>
                            cellEmptyViewModel.EnteredValue != null && cellEmptyViewModel.EnteredValue.Equals(cellEmptyViewModel.Value, StringComparison.OrdinalIgnoreCase));
        }

        public void AcceptCellValueChanges()
        {
            RejectCellValueChanges(); //Unregister first as we may already be registered. (Multiple registration for same object instance seems to fire twice) 
            //Messenger.Default.Register<CellValueChangedMessage>(this, m => HandleChangedCellValue(m));
        }

        public void RejectCellValueChanges()
        {
            //Messenger.Default.Unregister<CellValueChangedMessage>(this);
        }

        //private void HandleChangedCellValue(CellValueChangedMessage cellValueChangedMessage)
        //{
        //    //If this word has the changed cell passed in, modify the instance of the cell to reflect it's new value
        //    EmptyCellViewModel cell =
        //        Cells.FirstOrDefault(x => x.Col == cellValueChangedMessage.Col && x.Row == cellValueChangedMessage.Row);
        //    if (cell != null)
        //    {
        //        cell.EnteredValue = cellValueChangedMessage.Character;
        //    }
        //}
    }
}