using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree.Models
{
    public class Node
    {
        public Node()
        {
            children = new List<Node>();
        }

        public int label { get; set; }

        public string leafClass { get; set; }

        public List<Node> children { get; set; }
    }
}
