using System;
using System.Diagnostics;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using KrossKlient.ViewModels;

namespace KrossKlient.Common
{
    public class BoardCanvas :  Panel
    {
        //internal static int BubbleSize
        //{
        //    get { return 32; }
        //}

        //internal int ColumnCount
        //{
        //    get { return (int)Math.Floor(base.ActualWidth / BubbleSize); }
        //}

        //internal int RowCount
        //{
        //    get { return (int)Math.Floor(base.ActualHeight / BubbleSize); }
        //}


        //internal double CalculateLeft(FrameworkElement bubbleContainer)
        //{
        //    if (bubbleContainer == null)
        //        throw new ArgumentNullException("cellContainer");

        //    var bubble = bubbleContainer.DataContext as CellViewModel;
        //    if (bubble == null)
        //        throw new ArgumentException("Element does not have a CellViewModel as its DataContext.", "bubbleContainer");

        //    return CalculateLeft(bubble.Col);
        //}

        //internal double CalculateTop(FrameworkElement bubbleContainer)
        //{
        //    if (bubbleContainer == null)
        //        throw new ArgumentNullException("cellContainer");

        //    var bubble = bubbleContainer.DataContext as CellViewModel;
        //    if (bubble == null)
        //        throw new ArgumentException("Element does not have a CellViewModel as its DataContext.", "bubbleContainer");

        //    return CalculateTop(bubble.Row);
        //}

        //private double CalculateLeft(int column)
        //{
        //    double bubblesWidth = BubbleSize * ColumnCount;
        //    double horizOffset = (base.ActualWidth - bubblesWidth) / 2;
        //    return column * BubbleSize + horizOffset;
        //}

        //private double CalculateTop(int row)
        //{
        //    double bubblesHeight = BubbleSize * RowCount;
        //    double vertOffset = (base.ActualHeight - bubblesHeight) / 2;
        //    return row * BubbleSize + vertOffset;
        //}

        protected override Size MeasureOverride(Size availableSize)
        {
            // Get the collection of children
            UIElementCollection mychildren = Children;

            // Get total number of children
            int count = mychildren.Count;

            var size = (int) Math.Floor((decimal) (availableSize.Width/12));

            foreach (var uiElement in Children)
            {
                var child = (FrameworkElement) uiElement;
                child.Measure(new Size(32, 32));
            }

            var width = 30*12;
            // return the size available to the whole panel
            return new Size(width, width);
        }


        protected override Size ArrangeOverride(Size finalSize)
        {
            // Get the collection of children
            UIElementCollection mychildren = Children;
            // Get total number of children
            int count = mychildren.Count;
            // Arrange children
            int i;
            for (i = 0; i < count; i++)
            {
                // Get (left, top) origin point for the element in the 3x3 block
                Point cellOrigin = GetOrigin(i, 12, new Size(32, 32));

                // Arrange child
                // Get desired height and width. This will not be larger than 100x100 as set in MeasureOverride.
               // var contentPresenter = mychildren[i] as ContentPresenter;
               // var cellViewModel = contentPresenter.DataContext as CellViewModel;
                double dw = mychildren[i].DesiredSize.Width;
                double dh = mychildren[i].DesiredSize.Height;

                mychildren[i].Arrange(new Rect(cellOrigin.X, cellOrigin.Y, dw, dh));
                //Canvas.SetLeft(mychildren[i], CalculateLeft(cellViewModel.Col));
                //Canvas.SetTop(mychildren[i], CalculateLeft(cellViewModel.Row));
            }

            // Return final size of the panel
            return finalSize;
        }

        protected Point GetOrigin(int blockNum, int blocksPerRow, Size itemSize)
        {
            // Get row number (zero-based)
            var row = (int) Math.Floor((decimal) (blockNum/blocksPerRow));

            // Get column number (zero-based)
            int column = blockNum - blocksPerRow*row;

            // Calculate origin
            var origin = new Point(itemSize.Width*column, itemSize.Height*row);
            return origin;
        }
    }
}