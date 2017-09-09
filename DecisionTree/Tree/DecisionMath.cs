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


        public static bool IsPure(List<DNARecord> set)
        {
            foreach (var i in Classifiers.values)
            {
                var count = set.Where(e => e.classifier == i).Count();
                if (((double)count / (double)set.Count) >= .9) return true;
            }
            return false;
        }

        public static string GetClass(List<DNARecord> set)
        {
            foreach (var i in Classifiers.values)
            {
                var count = set.Where(e => e.classifier == i).Count();
                if (((double)count / (double)set.Count) >= .9) return i;
            }
            return "No Class";
        }
    }
}
