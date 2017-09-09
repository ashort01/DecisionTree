using DecisionTree.Models;
using DecisionTree.Tree;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree
{
    class Program
    {
        static void Main(string[] args)
        {
            var data = ReadData();
            var t = TreeService.BuildTree(data);
        }

        public static List<DNARecord> ReadData()
        {
            using (var reader = new StreamReader("c:\\users\\austin\\documents\\visual studio 2015\\Projects\\DecisionTree\\DecisionTree\\Data\\training.csv"))
            {
                List<DNARecord> dna = new List<DNARecord>();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    dna.Add(new DNARecord { id = values[0], sequence = values[1].ToArray(), classifier = values[2] });
                }
                return dna;
            }
        }

    }
}
