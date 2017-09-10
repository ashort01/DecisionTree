using DecisionTree.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree.Tree
{
    public class Gini
    {
        public static int gini_index(List<DNARecord> set)
        {
            double gini_system = calc_gini_system(set);
            List<double> gini_attributes = calc_gini_attributes(set);
            List<double> gini_gain = calc_gini_gain(gini_attributes, gini_system);
            double max = gini_gain.Max();
            int max_index = gini_gain.IndexOf(max);
            return max_index;
        }

        public static double calc_gini_system(List<DNARecord> set)
        {
            if (set.Count > 0)
            {
                double gini_system = 1;
                //first calculate the gini value for our system using the classifying attribute
                foreach (var i in Classifiers.values)
                {
                    var subset = set.Where(e => e.classifier == i).ToList();
                    var ratio = (double)subset.Count / (double)set.Count;
                    gini_system *= ratio;
                }
                return gini_system;
            }
            return 0;
        }

        public static List<double> calc_gini_attributes(List<DNARecord> set)
        {
            List<double> gini_values = new List<double>();
            if (set.Count > 0)
            {
                int sequence_length = set[0].sequence.Length;
                for (int i = 0; i < sequence_length; i++)
                {
                    //will hold the gini value for this attribute
                    double gini_attr = 0;
                    foreach (var a in AttributeValues.values)
                    {
                        int attribute_count = set.Where(e => e.sequence[i] == a).Count();
                        //calcualte the probability of getting this attribute
                        double attribute_prob = (double)attribute_count / (double)set.Count();
                        //this will store the multiplication of the probabilities of this attribute with each classifier
                        double attr_class_multiplier = 1;
                        foreach (var c in Classifiers.values)
                        {
                            int attr_class_count = set.Where(e => e.sequence[i] == a && e.classifier == c).Count();
                            double attr_class_prob = 0;
                            if (attribute_count > 0)
                            {
                                attr_class_prob = (double)attr_class_count / (double)attribute_count;
                            }
                            attr_class_multiplier *= attr_class_prob;
                        }
                        gini_attr += (attribute_prob * attr_class_multiplier);
                    }
                    gini_values.Add(gini_attr);
                }
            }
            return gini_values;
        }

        public static List<double> calc_gini_gain(List<double> gini_attributes, double gini_system)
        {
            List<double> gini_gain = new List<double>();
            foreach(var g in gini_attributes)
            {
                double gain = gini_system - g;
                gini_gain.Add(gain);
            }
            return gini_gain;
        }
    }
}
