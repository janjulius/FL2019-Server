using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Encoder
{
    public static class EFEncoder
    {
        public static string EncodeBoolListToString(List<bool> arr)
        {
            StringBuilder result = new StringBuilder();
            arr.ForEach(ele => result.Append(ele ? "1" : "0"));
            return result.ToString();
        }

        public static List<bool> DecodeStringToBoolList(string input)
        {
            List<bool> output = new List<bool>();
            output = input.Select(c => c == '1').ToList();
            return output.ToList();
        }
    }
}
