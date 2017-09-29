using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionTree.Models
{
    public class AttributeValues
    {
        public static readonly char[] values = { 'A', 'C','G','T','N' };

        public enum valueIndex {A = 0, C = 1, G = 2, T = 3, N = 4};
    }
}
