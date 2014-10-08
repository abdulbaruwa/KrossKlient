using KrossKlient.Common;
using ReactiveUI;

namespace KrossKlient.ViewModels.DesignTime
{
    public class DesignCellViewModel : ReactiveObject
    {
        private int _col;
        private int _row;
        private string _value = string.Empty;
        private string _startPosition;
        private bool _isFirtCharacter;
        private Direction _direction;
        public DesignCellViewModel()
        {
            _value = DesignTimeHelper.GetRandomChar();
            _col = 2;
            _row = 0;
            _isFirtCharacter = true;
            _direction = Direction.Down;
            _startPosition = 3.ToString();
        }

        public string StartPosition
        {
            get
            {
                return _isFirtCharacter ? _startPosition : string.Empty;
            }
        }

        public bool ShowVertical
        {
            get { return _isFirtCharacter && (Direction == Direction.Down || Direction == Direction.Both); }
        }

        public bool ShowHorizontal
        {
            get { return _isFirtCharacter && (Direction == Direction.Across || Direction == Direction.Both); }
        }

        public int Row
        {
            get { return _row; }
            set { this.RaiseAndSetIfChanged(ref _row, value); }
        }

        public int Col
        {
            get { return _col; }
            set { this.RaiseAndSetIfChanged(ref _col, value); }
        }

        public string Value
        {
            get { return _value; }
            set { this.RaiseAndSetIfChanged(ref _value, value); }
        }

        public bool IsFirtCharacter
        {
            get { return _isFirtCharacter; }
        }

        public Direction Direction
        {
            get { return _direction; }
        }
    }
}