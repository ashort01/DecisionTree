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

            //get the gini index
            double max = gini_gain.Max(); //first find the largest gini attribute value
            int max_index = gini_gain.IndexOf(max); //the index of the max value represents the split index
            return max_index;
        }

        //calculates the gini value of the entire system
        public static double calc_gini_system(List<DNARecord> set)
        {
            if (set.Count > 0)
            {
                double gini_system = 1;
                //first calculate the gini value for our system using the classifying attribute
                foreach (var i in Classifiers.values)
                {
                    //calculate the probability of each classifier with respect to the entire set
                    var classifier_count = set.Where(e => e.classifier == i).Count();
                    var classifier_probability = (double)classifier_count / (double)set.Count;
                    //the gini system value is the multiplication of all of these probabilities
                    gini_system *= classifier_probability;
                }
                return gini_system;
            }
            return 0;
        }

        //calculates the gini value for each attribute in the set
        public static List<double> calc_gini_attributes(List<DNARecord> set)
        {
            List<double> gini_values = new List<double>();
            if (set.Count > 0)
            {
                int sequence_length = set[0].sequence.Length; //this only works if all sequences are the same length
                for (int i = 0; i < sequence_length; i++)
                {
                    //will hold the gini value for this attribute
                    double gini_attr = 0;
                    //for each possible attribute value for this attribute...
                    foreach (var a in AttributeValues.values)
                    {
                        //calculate the probability of getting this attribute value for this attribute with respect to the entire set
                        int attribute_count = set.Where(e => e.sequence[i] == a).Count();

                        if (attribute_count < 1) continue; //skip if there's none

                        double attribute_prob = (double)attribute_count / (double)set.Count();

                        //this will store the multiplication of the probabilities of this attribute with each classifier
                        double attr_class_multiplier = 1;
                        foreach (var c in Classifiers.values)
                        {
                            //calculate the probability of this attribute appearing with this classifier
                            int attr_class_count = set.Where(e => e.sequence[i] == a && e.classifier == c).Count();
                            double attr_class_prob = 0;
                            attr_class_prob = (double)attr_class_count / (double)attribute_count;
                            //multiply it with the prior probrabilities
                            attr_class_multiplier *= attr_class_prob;
                        }
                        //the gini value for the attribute is equal to the sum of the probability of each attribute multipled with the probabilities of each attribute/class
                        gini_attr += (attribute_prob * attr_class_multiplier);
                    }
                    gini_values.Add(gini_attr);
                }
            }
            return gini_values;
        }

        //calcuates the gini gain for each attibute
        //gini gain for an attribute is the gini system value minus the gini value for the attribute
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
