using System;
using KrossKlient.Common;

namespace KrossKlient.Services
{
    public class InsertWordResult
    {
        public Direction Direction { get; set; }
        public Tuple<int, int> StartCell { get; set; }
        public Tuple<int, int> EndCell { get; set; }
        public bool Inserted { get; set; }
        public string Word { get; set; }
    }
}