using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree.Models
{
    public class ChiSquaredRecord
    {
        public double PValue { get; set; }

        public int DegreesOfFreedom { get; set; }

        public double value { get; set; }
    }
    public class ChiSquaredData
    {
        public static List<ChiSquaredRecord> lookup_table =
            new List<ChiSquaredRecord>{
                new ChiSquaredRecord() { value = 0.995, DegreesOfFreedom = 8, PValue = 1.344 },
                new ChiSquaredRecord() { value = 0.975, DegreesOfFreedom = 8, PValue = 2.180 },
                new ChiSquaredRecord() { value = 0.20, DegreesOfFreedom = 8, PValue = 11.030 },
                new ChiSquaredRecord() { value = 0.10, DegreesOfFreedom = 8, PValue = 13.362 },
                new ChiSquaredRecord() { value = 0.05, DegreesOfFreedom = 8, PValue = 15.507 },
                new ChiSquaredRecord() { value = 0.025, DegreesOfFreedom = 8, PValue = 17.535 },
                new ChiSquaredRecord() { value = 0.02, DegreesOfFreedom = 8, PValue = 18.168 },
                new ChiSquaredRecord() { value = 0.01, DegreesOfFreedom = 8, PValue = 20.090 },
                new ChiSquaredRecord() { value = 0.005, DegreesOfFreedom = 8, PValue = 21.955 },
                new ChiSquaredRecord() { value = 0.002, DegreesOfFreedom = 8, PValue = 24.352 },
                new ChiSquaredRecord() { value = 0.001, DegreesOfFreedom = 8, PValue = 26.124 }
                };
    }
}
