using System;
using System.Collections.Generic;
using System.Text;

namespace AlmutalCore.Models
{
    public class Project
    {
        //TODO private Optional<OptimizationResult> optimizationResult;

        //public List<Panel> Panels { get; set; } = new List<Panel>();
        public double BladeWidth { get; set; } = 3;

        //public List<PanelInstance> getPanelInstances()
        //{
        //    var pis = new List<PanelInstance>();
        //    foreach (var p in Panels)
        //    {
        //        for (int i = 0; i < p.Count; i++)
        //        {
        //            pis.Add(new PanelInstance(p.Title, p.Sheet, p.Dimension,  p.CanRotate, i));
        //        }
        //    }
        //    return pis;
        //}

    }
}
