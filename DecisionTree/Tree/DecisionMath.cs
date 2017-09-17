using DecisionTree.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree.Tree
{
    public class DecisionMath
    {
        public static double Entropy(List<DNARecord> set)
        {
            if (set.Count > 0)
            {
                double entropy = 0;
                foreach (var i in Classifiers.values)
                {
                    var subset = set.Where(e => e.classifier == i).ToList();
                    var ratio = (double)subset.Count / (double)set.Count;
                    if (ratio != 0)
                    {
                        entropy -= ratio * Math.Log(ratio);
                    }
                }
                return entropy;
            }
            return 0;
        }

        public static double InformationGain(List<DNARecord> set, int attribute)
        {
            if (set.Count > 0)
            {
                double gain = Entropy(set);
                foreach (var i in AttributeValues.values)
                {
                    var subset = set.Where(e => e.sequence[attribute] == i).ToList();
                    var ratio = (double)subset.Count / (double)set.Count;
                    gain -= ratio * Entropy(subset);
                }
                return gain;
            }
            return 0;
        }

        public static List<double> InformationGains(List<DNARecord> set)
        {
            var sequenceLength = 60;
            List<double> gains = new List<double>();
            for (var i = 0; i < sequenceLength; i++)
            {
                gains.Add(InformationGain(set, i));
            }
            return gains;
        }


        public static bool IsPure(List<DNARecord> set, double acceptanceRatio)
        {
            foreach (var i in Classifiers.values)
            {
                var count = set.Where(e => e.classifier == i).Count();
                var ratio = ((double)count / (double)set.Count);
                if (ratio >= acceptanceRatio)
                {
                    return true;
                }
            }
            return false;
        }

        public static string GetClass(List<DNARecord> set, double acceptanceRatio)
        {
            List<double> ratios = new List<double>();
            for(int i = 0; i < Classifiers.values.Count(); i++)
            {
                var count = set.Where(e => e.classifier == Classifiers.values[i]).Count();
                ratios.Add((double)count / (double)set.Count);
            }
            //return maximum ratio 
            return Classifiers.values[ratios.IndexOf(ratios.Max())];
        }

        public static bool ShouldSplitChiSquared(List<DNARecord> set, int splitIndex, double alpha)
        {
            var degreesOfFreedom = (Classifiers.values.Count() - 1) * (AttributeValues.values.Count() - 1);
            var subsets = new List<List<DNARecord>>();
            double criticalValue = 0;
            foreach (var classifier in Classifiers.values)
            {
                var count = (double)set.Where(i => i.classifier == classifier).Count();
                var expected_ratio = count / (double)set.Count();
                foreach (var i in AttributeValues.values)
                {
                    var attribute_set = set.Where(e => e.sequence[splitIndex] == i).ToList();
                    var expected_number = attribute_set.Count() * expected_ratio;
                    var actual_number = (double) attribute_set.Where(e => e.classifier == classifier).Count();
                    if(expected_number != 0)
                        criticalValue += (Math.Pow(actual_number - expected_number, 2)) / expected_number;
                }
            }
            return RejectNull(criticalValue, degreesOfFreedom, alpha);
        }

        private static bool RejectNull(double criticalValue, int degreesOfFreedom, double alpha)
        {
            var tableRow = ChiSquaredData.lookup_table.Where(i => i.DegreesOfFreedom == degreesOfFreedom && i.value == alpha).FirstOrDefault();
            if (tableRow != null)
            {
                var pvalue = tableRow.PValue;
                if (Math.Abs(criticalValue) > pvalue)
                {
                    //reject the null hypothesis, the split is significant
                    return true;
                }
                else
                {
                    //accept the null hypoth, the change in data is just by chance.
                    return false;
                }
            }
            else
            {
                var ex = new Exception("Could not find the Chi Squared Table entry");
                throw ex;
            }
        }
    }
}
