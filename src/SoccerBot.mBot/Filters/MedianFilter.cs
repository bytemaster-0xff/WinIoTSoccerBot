using LagoVista.Core.Models.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoccerBot.mBot.Filters
{
    public class MedianFilter
    {
        int _filterSize = 12;
        int _throwAwaySize = 3;

        public Point2D<int>[] _points;
        int _head;
        int _arraySize;

        public MedianFilter(int size = 12, int throwAway = 3)
        {
            _filterSize = size;
            _throwAwaySize = throwAway;
            _points = new Point2D<int>[size];
        }

        public void Add(int x, int y)
        {
            Add(new Point2D<int>(x, y));
        }

        public void Add(Point2D<int> point)
        {
            _points[_head++] = point;
            if (_head == _filterSize)
            {
                _head = 0;
            }

            _arraySize++;
            _arraySize = Math.Min(_filterSize, _arraySize);
        }

        public Point2D<double> Filtered
        {
            get
            {
                var sortedX = _points.Where(pt => pt != null).Select(pt => pt.X).OrderBy(pt => pt);
                var sortedY = _points.Where(pt => pt != null).Select(pt => pt.Y).OrderBy(pt => pt);

                if (sortedX.Count() == 0)
                    return null;

                if (sortedX.Count() == _filterSize)
                {
                    var subsetX = sortedX.Skip(_throwAwaySize).Take(_filterSize - _throwAwaySize * 2);
                    var subsetY = sortedY.Skip(_throwAwaySize).Take(_filterSize - _throwAwaySize * 2);
                    return new Point2D<double>(subsetX.Average(), subsetY.Average());
                }
                else
                {
                    return new Point2D<double>(sortedX.Average(), sortedY.Average());
                }
            }
        }
    }
}
