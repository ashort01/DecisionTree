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
        public static Node BuildTree(List<DNARecord> set, double alpha)
        {
            Node node = new Node();
            if(DecisionMath.IsPure(set))
            {
                node.leafClass = DecisionMath.GetClass(set);
                return node;
            }
            var gains = DecisionMath.InformationGains(set);
            //Take the maximum information gain
            var splitIndex = gains.IndexOf(gains.Max());
            node.label = splitIndex;
            if(DecisionMath.ShouldSplitChiSquared(set, splitIndex, alpha))
            {
                foreach (var i in AttributeValues.values)
                {
                    var subset = set.Where(e => e.sequence[splitIndex] == i).ToList();
                    if (subset.Count > 0)
                    {
                        node.children.Add(BuildTree(subset, alpha));
                    }
                }
            }
            node.leafClass = DecisionMath.GetClass(set);
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
            var ratio = (trueCount / (trueCount + falseCount));
            return ratio * 100;
        }

        public static bool IsCorrectClassification(DNARecord item, string prediction)
        {
            return item.classifier == prediction;
        }

    }
}
