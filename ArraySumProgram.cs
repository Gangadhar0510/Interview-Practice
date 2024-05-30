using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Array_Pratices
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int  k = 1;
           
            int[] lst_Array = { -1,1,0,-3,3};
            int[] answer = new int[lst_Array.Length];
            for (int i = 0; i < lst_Array.Length; i++)
            {
                for (int j = 0; j < lst_Array.Length; j++)
                {
                    if (i != j)
                    {
                        k= k * lst_Array[j];
                        answer[i] = k;
                    }
                }
                k = 1;
            }
            foreach(var item in answer)
            {
                Console.Write(item + ",");
            }
               
            
            Console.ReadLine();
        }
    }
}
