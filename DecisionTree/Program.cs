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
            var alpha = ParseArguments(args);
            var data = ReadData();
            Console.WriteLine("Building Tree...");
            var t = TreeService.BuildTree(data, alpha);
            Console.WriteLine("Traversing Tree...");
            var str = TreeService.TraverseTree(data[0], t);
            var i = TreeService.DetermineAccuracy(data, t);
            Console.WriteLine("Our tree was " + i + "% accurate!");
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }

        public static double ParseArguments(string [] args)
        {
            try
            {
                double alpha;
                if (!Double.TryParse(args[0], out alpha))
                {
                    Console.WriteLine("Alpha failed to parse, using alpha = 0.05 as the default.");
                    return 0.05;
                }
                return alpha;
            }
            catch(System.IndexOutOfRangeException e)
            {
                Console.WriteLine("Alpha not passed as argument, using alpha = 0.05 as the default.");
                return 0.05;
            }
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
