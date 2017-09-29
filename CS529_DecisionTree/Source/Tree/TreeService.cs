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

        /// <summary>
        /// This method builds a decision tree using the
        /// ID3 method. 
        /// </summary>
        /// <param name="set">The training data to build the tree.</param>
        /// <param name="treeType">The splitting algorithm to use. Can be "gini-index", "information-gain", or "both",</param>
        /// <returns></returns>
        public Node BuildTree(List<DNARecord> set, string treeType)
        {
            Node node = new Node();
            if (DecisionMath.IsPure(set, constants.PurityRatio))
            {
                node.leafClass = DecisionMath.GetClass(set);
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
            else //inforamtion gain
            {
                var gains = DecisionMath.InformationGains(set);
                splitIndex = gains.IndexOf(gains.Max());
            }
            
            //if we should split based on statistical significance.
            //is it significant, or just by chance?
            if (DecisionMath.ShouldSplitChiSquared(set, splitIndex, constants.Alpha))
            {
                node.label = splitIndex;
                foreach (var i in AttributeValues.values)
                {
                    var subset = set.Where(e => e.sequence[splitIndex] == i).ToList();
                    if (subset.Count > 0)
                    {
                        node.children.Add(BuildTree(subset, treeType));
                    }
                }
            }
            node.leafClass = DecisionMath.GetClass(set);
            return node;
        }


        /// <summary>
        /// Thi function traverses the tree recursively. 
        /// It uses a DFS style search
        /// </summary>
        /// <param name="item">The item you use to search the tree</param>
        /// <param name="tree">The tree to traverse</param>
        /// <returns></returns>
        public string TraverseTree(DNARecord item, Node tree)
        {
            if (tree.children.Count == 0)
            {
                //we hit a leaf, so return the class of the leaf.
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
                //take a guess :)
                return "N";
            }
        }

        /// <summary>
        /// This function determines the accuracy of your tree
        /// against training data.
        /// </summary>
        /// <param name="data">The training data with classifiers</param>
        /// <param name="tree">The tree you want to see the accuracy of</param>
        /// <returns></returns>
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

        public static void DrawTree(Node t, string indent, char attribute)
        {
                Console.Write(indent);
                Console.Write("|-");
                indent += "| ";
                if (t.children.Count == 0)
                {
                    Console.WriteLine(attribute + "->(" + t.leafClass + ")");
                }
                else
                {
                    if (t.children.Any(i => i.children.Count > 0))
                    {
                        Console.WriteLine(t.label + " " + attribute);
                    }
                    else
                    {
                        Console.WriteLine(t.label);
                    }
                }
                for (int i = 0; i < t.children.Count; i++)
                {
                    DrawTree(t.children[i], indent, AttributeValues.values[i]);
                }
        }

    }

    
}
