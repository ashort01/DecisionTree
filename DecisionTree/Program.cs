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
            if (CommandLine.Parser.Default.ParseArguments(args, constants))
            {
                var data = ReadData();
                var treeService = new TreeService(constants);
                Console.WriteLine("Building Tree...");
                var t = treeService.BuildTree(data);
                Console.WriteLine("Traversing Tree...");
                var str = treeService.TraverseTree(data[0], t);
                var i = treeService.DetermineAccuracy(data, t);
                Console.WriteLine("Our tree was " + i + "% accurate!");
            }
            else
            {
                Console.WriteLine("Arguments failed to parse. Program exiting.");
            }

            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

 

        public static List<DNARecord> ReadData()
        {
            string projectDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string fileName = @"Data\training.csv";
            string path = Path.Combine(projectDirectory, fileName);
            using (var reader = new StreamReader(path))
            {
                List<DNARecord> dna = new List<DNARecord>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    //parse lines of comma delimited data into DNARecord Objects.
                    dna.Add(new DNARecord { id = values[0], sequence = values[1].ToArray(), classifier = values[2] });
                }
                return dna;
            }
        }
    }
}
