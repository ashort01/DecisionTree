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
        public TreeService(Constants c)
        {
            constants = c;
        }
        public Constants constants { get; set; }


        public Node BuildTree(List<DNARecord> set, string treeType)
        {
            Node node = new Node();
            if (DecisionMath.IsPure(set, constants.PurityRatio))
            {
                node.leafClass = DecisionMath.GetClass(set, constants.PurityRatio);
                return node;
            }

            int splitIndex;
            if (treeType.Equals("both"))
            {
                //try to calucate the split index using gini
                //if gini returns 0, use information gain instead
                splitIndex = Gini.gini_index(set);
                if (splitIndex < 1)
                {
                    var gains = DecisionMath.InformationGains(set);
                    splitIndex = gains.IndexOf(gains.Max());
                }
            }
            else if (treeType.Equals("gini-index"))
            {
                splitIndex = Gini.gini_index(set);
            }
            else
            {
                var gains = DecisionMath.InformationGains(set);
                splitIndex = gains.IndexOf(gains.Max());
            }
            
            node.label = splitIndex;
            if (DecisionMath.ShouldSplitChiSquared(set, splitIndex, constants.Alpha))
            {
                foreach (var i in AttributeValues.values)
                {
                    var subset = set.Where(e => e.sequence[splitIndex] == i).ToList();
                    if (subset.Count > 0)
                    {
                        node.children.Add(BuildTree(subset, treeType));
                    }
                }
            }
            node.leafClass = DecisionMath.GetClass(set, constants.PurityRatio);
            return node;
        }

        public string TraverseTree(DNARecord item, Node tree)
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

        public decimal DetermineAccuracy(List<DNARecord> data, Node tree)
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

        public bool IsCorrectClassification(DNARecord item, string prediction)
        {
            return item.classifier == prediction;
        }

    }
}
