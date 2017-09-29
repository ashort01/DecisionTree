using DecisionTree.Models;
using DecisionTree.Tree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    class Program
    {
        static void Main(string[] args)
        {
            var constants = new Constants();
            //parse arguments
            if (CommandLine.Parser.Default.ParseArguments(args, constants))
            {
                var data = ReadData("training.csv");
                var treeService = new TreeService(constants);
                var t = new Node();
                if (constants.DecisionAlgorithm == "information-gain") Console.WriteLine("Building tree using information gain...");
                else if (constants.DecisionAlgorithm == "gini-index") Console.WriteLine("Building tree using gini index...");
                else Console.WriteLine("Building tree with gini and information gain");
                //build tree
                t = treeService.BuildTree(data, constants.DecisionAlgorithm);

                Console.WriteLine("Traversing Tree...");
                //traverse tree
                var i = treeService.DetermineAccuracy(data, t);
                Console.WriteLine("Our tree was " + i + "% accurate!");


                //run on testing data
                data = ReadData("testing.csv");
                int length = data.Count;
                string[] lines = new string[length+1];
                lines[0] = "id,class";
                for (int k = 0; k < length; k++)
                {
                    var c = treeService.TraverseTree(data[k], t);
                    lines[k+1] = data[k].id + "," + c;
                }
                System.IO.File.WriteAllLines(@"Data\results.csv", lines);

            }
            else
            {
                Console.WriteLine("Arguments failed to parse. Program exiting.");
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

 

        public static List<DNARecord> ReadData(string csvName)
        {
            string projectDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string fileName = @"Data\" + csvName;
            string path = Path.Combine(projectDirectory, fileName);
            using (var reader = new StreamReader(path))
            {
                List<DNARecord> dna = new List<DNARecord>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    //parse lines of comma delimited data into DNARecord Objects.
                    if (values.Length > 2){
                        dna.Add(new DNARecord { id = values[0], sequence = values[1].ToArray(), classifier = values[2] });
                    }
                    else
                    {
                        dna.Add(new DNARecord { id = values[0], sequence = values[1].ToArray()});
                    }
                }
                return dna;
            }
        }
    }
}
