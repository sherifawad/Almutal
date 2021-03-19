using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Almutal
{
    public class StripCutAlgorithm
    {
        #region Public Properties

        public double[] CutList { get; private set; }
        public double BarLength1 { get; }
        public double BarLength { get; private set; }
        public double CutterEndWidth { get; private set; } // نسوية الرايش بالمبرد حياكل اد ايه
        public double BladeWidth { get; private set; }
        public int[] Bins { get; private set; }

        #endregion

        #region Constructor

        public StripCutAlgorithm(double[] cutList, double barLength, double cutterEndWidth = 0, double bladeWidth = 0, int[] bins = null)
        {
            CutList = cutList;
            BarLength1 = barLength;
            CutterEndWidth = cutterEndWidth;
            BarLength = barLength - CutterEndWidth;  // ازالة جزء الرايش مت اخر قطعة باقية من اول طرفها عشان ممكن نستخدمعا و نقطع منها تاني
            BladeWidth = bladeWidth;
            Bins = bins ?? new int[CutList.Length];
            ListConstruction();
        }
        #endregion

        #region PrivateMethods

        private void ListConstruction()
        {
            CutList = CutList.OrderByDescending(x => x).ToArray();

            // نزود الرايش  لكل قطعة عشان لما يتشال مينقص طول القطعة اللي احنا عايزينها
            // نزود علي طوول كل قطعة ضخة المنشار فاللي حياكله المنشار  مينقص طول القطعة اللي احنا عايزينها
            for (int i = 0; i < CutList.Length; i++)
            {
                CutList[i] = CutList[i] + CutterEndWidth + BladeWidth;
            }

        }

        #endregion

        #region Public Methods

        public Dictionary<int, List<double>> best()
        {
            Dictionary<int, List<double>> ts = new Dictionary<int, List<double>>();

            var bins_used = 1;
            double[] binarray = new double[CutList.Length];
            binarray[0] = BarLength;
            for (var item = 0; item < CutList.Length; item++)
            {
                var candidate = -1;
                double capacity = 0;

                for (var bin = 0; bin < bins_used; bin++)
                {
                    if (binarray[bin] >= CutList[item])
                    {

                        if (candidate == -1 || binarray[bin] < capacity)
                        {
                            candidate = bin;
                            capacity = binarray[bin];

                        }
                    }
                }

                if (candidate != -1)
                {
                    /* Add to candidate bin */
                    binarray[candidate] -= CutList[item];

                    Bins[item] = candidate;
                }
                else
                {
                    /* Create a new bin and add to it */
                    binarray[bins_used] = BarLength - CutList[item];

                    Bins[item] = bins_used;

                    bins_used++;

                }

                var cc = ts.FirstOrDefault(x => x.Key == Bins[item]);
                if (cc.Value == null)
                    ts.Add(Bins[item], new List<double> { CutList[item] });
                else
                    cc.Value.Add(CutList[item]);
            }

            return ts;

        }

        #endregion
    }
}
