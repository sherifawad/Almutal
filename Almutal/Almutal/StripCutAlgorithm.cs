using Almutal.Helpers;
using DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Almutal
{
    public class StripCutAlgorithm
    {
        #region Public Properties

        public Strip[] CutList { get; private set; }
        public double BarLength { get; private set; }
        public double CutterEndWidth { get; private set; } // نسوية الرايش بالمبرد حياكل اد ايه
        public double BladeWidth { get; private set; }
        public int[] Bins { get; private set; }

        #endregion

        #region Constructor

        public StripCutAlgorithm(Strip[] cutList, double barLength, double cutterEndWidth = 0, double bladeWidth = 0, int[] bins = null)
        {
            CutList = cutList;
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
            var list = CutList.OrderByDescending(x => x.Length).ToList();
            list.ForEach(x => {
                x.Length = x.Length + CutterEndWidth + BladeWidth;
            });
            // نزود الرايش  لكل قطعة عشان لما يتشال مينقص طول القطعة اللي احنا عايزينها
            // نزود علي طوول كل قطعة ضخة المنشار فاللي حياكله المنشار  مينقص طول القطعة اللي احنا عايزينها

        }

        #endregion

        #region Public Methods

        public List<StockStrip> best()
        {
            Dictionary<int, List<double>> ts = new Dictionary<int, List<double>>();
            var slist = new List<StockStrip>();

            var bins_used = 1;
            double[] binarray = new double[CutList.Length];
            binarray[0] = BarLength;
            var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(15)).Token;

            for (var item = 0; item < CutList.Length; item++)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;
                var candidate = -1;
                double capacity = 0;

                for (var bin = 0; bin < bins_used; bin++)
                {
                    if (binarray[bin] >= CutList[item].Length)
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
                    binarray[candidate] -= CutList[item].Length;

                    Bins[item] = candidate;
                }
                else
                {
                    /* Create a new bin and add to it */
                    binarray[bins_used] = BarLength - CutList[item].Length;

                    Bins[item] = bins_used;

                    bins_used++;

                }

                var cc = slist.FirstOrDefault(x => x.Id == Bins[item]);
                if (cc == null)
                {
                    slist.Add(new StockStrip {Id = Bins[item],
                        Length = BarLength,
                    CuttedStrips = new List<Strip> {CutList[item] } 
                    });
                }
                else
                    cc.CuttedStrips.Add(CutList[item]);
            }

            return slist;

        }

        #endregion
    }
}
