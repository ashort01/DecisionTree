using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree.Models
{
    public class Constants
    {
        private double alpha { get; set; }
        private double purityRatio { get; set; }
        private string decisionAlgorithm { get; set; }

        [Option('a', "alpha", HelpText = "Alpha confidence value for chi squared pruning", DefaultValue = .05)]
        public double Alpha
        {
            get
            {
                return alpha;
            }
            set
            {
                if (ChiSquaredData.lookup_table.Select(i => i.value).Contains(value))
                {
                    alpha = value;
                }
                else
                {
                    Console.WriteLine("Invalid alpha, using default alpha of .05. Please use one of the following alphas: " + ChiSquaredData.lookup_table.Select(i => i.value).ToString());
                    alpha = .05;
                }
            }

        }


        [Option('p', "purity ratio", HelpText = "Purity ratio acceptance value", DefaultValue = .9)]
        public double PurityRatio
        {
            get
            {
                return purityRatio;
            }
            set
            {
                if (value > 0 && value < 1)
                {
                    purityRatio = value;
                }
                else
                {
                    Console.WriteLine("Purity Ratio must be between 0 and 1. Using .9 as the default purity ratio.");
                    purityRatio = .9;
                }
            }
        }

        [Option('d', "decision algorithm", Required = true, HelpText = "Decision algorithm for splitting. Accepted values are: gini-index or information-gain")]
        public string DecisionAlgorithm
        {
            get
            {
                return decisionAlgorithm;
            }
            set
            {
                if(value == "gini-index" || value == "information-gain" || value == "both")
                {
                    decisionAlgorithm = value;
                }
                else
                {
                    Console.WriteLine("Invalid decision algorithm. accepted values are gini-index or information-gain. Using information-gain as default.");
                    decisionAlgorithm = "information-gain";
                }
            }
        }

    }
}

