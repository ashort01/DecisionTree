using DecisionTree.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree.Tree
{
    public class TreeService
    {
        public static Node BuildTree(List<DNARecord> set)
        {
            Node node = new Node();
            if(DecisionMath.IsPure(set))
            {
                node.leafClass = DecisionMath.GetClass(set);
                return node;
            }
            //var gains = DecisionMath.InformationGains(set);
            //Take the maximum information gain
            //var splitIndex = gains.IndexOf(gains.Max());
            var splitIndex = Gini.gini_index(set);
            if (splitIndex < 1)
            {
                var gains = DecisionMath.InformationGains(set);
                splitIndex = gains.IndexOf(gains.Max());
            }
            else
            {
                Console.Write("gini\n");
            }
            node.label = splitIndex;
            foreach (var i in AttributeValues.values)
            {
                var subset = set.Where(e => e.sequence[splitIndex] == i).ToList();
                if (subset.Count > 0)
                {
                    node.children.Add(BuildTree(subset));
                }
            }
            return node;
        }

        public static string TraverseTree(DNARecord item, Node tree)
        {
            if (tree.children.Count == 0)
            {
                return tree.leafClass;
            }
            var attibute = item.sequence[tree.label];
            var attributeIndex = (int)((AttributeValues.valueIndex)Enum.Parse(typeof(AttributeValues.valueIndex), attibute.ToString()));
            try
            {
                return TraverseTree(item, tree.children[attributeIndex]);
            }
            catch (System.ArgumentOutOfRangeException e)
            {
                return "Could not classify";
            }
        }

        public static decimal DetermineAccuracy(List<DNARecord> data, Node tree)
        {
            List<bool> accuracyList = new List<bool>();
            foreach (var i in data)
            {
                var classPrediction = TraverseTree(i, tree);
                accuracyList.Add(IsCorrectClassification(i, classPrediction));
            }
            var falseCount = (decimal)accuracyList.Where(i => i == false).Count();
            var trueCount = (decimal)accuracyList.Where(i => i == true).Count();
            return 1 - (falseCount / trueCount);
        }

        public static bool IsCorrectClassification(DNARecord item, string prediction)
        {
            return item.classifier == prediction;
        }

    }
}
