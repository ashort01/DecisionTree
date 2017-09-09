using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree.Models
{
    public class DNARecord
    {
        public string id { get; set; }

        public char[] sequence { get; set; }

        public string classifier { get; set; } 
    }
}
