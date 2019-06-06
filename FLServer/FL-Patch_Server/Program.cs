using System;
using System.Collections.Generic;

namespace FL_Patch_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> intList = new List<int>();
            int size = 10000;
            
            for(int i = 0; i < size; i++)
            {
                if (i != 0)
                {
                    if (intList[i - 1] > int.MaxValue / 2)
                        break;
                    intList.Add(intList[i - 1] + intList[i - 1]);
                }
                else
                {
                    intList.Add(1);
                }
            }

            intList.ForEach(item => Console.WriteLine($"{item} {Convert.ToString(item, 2)}"));

            //bijv ik own character 1, 5, 16

            int value = intList[5] + intList[16] + intList[1];
            Console.WriteLine($"{Convert.ToString(value, 2)}");

           // PatchServer server = new PatchServer();
           // server.Run();
           Console.ReadKey();
        }
    }
}
