using System;
using System.Collections.Generic;
using System.Text;

namespace R_matrix_visualization
{
    public class CCBoxItem {
        private double min;
        public double Min {
            get { return min; }
            set { min = value; }
        }

        private bool add;
        public bool Add
        {
            get { return add; }
            set { add = value; }
        }

        private double max;
        public double Max {
            get { return max; }
            set { max = value; }
        }

        public CCBoxItem() {
        }

        public CCBoxItem(double min, double max, bool add) {
            this.max = max;
            this.min = min;
            this.add = add;
        }

        public override string ToString() {
            if (add)
                return string.Format("+ {0:F2} - {1:F2}", min, max);
            else
                return string.Format("-  {0:F2} - {1:F2}", min, max);
        }
    }
}
