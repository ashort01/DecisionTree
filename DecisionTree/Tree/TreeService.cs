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
                return node;
            }
            var gains = DecisionMath.InformationGains(set);
            var splitIndex = gains.IndexOf(gains.Max());
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

    }
}
