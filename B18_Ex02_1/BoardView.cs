using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02_1
{
    static class BoardView
    {
        public static void PrintBoard(eCheckerType?[,] i_BoardModel)
        {
            int arrayLength = i_BoardModel.GetLength(0);
            int arrayWidth = i_BoardModel.GetLength(1);

            Console.Write(" ");
            PrintLineOfUpperCaseLetter(arrayWidth);
            PrintLineOfEqualsLetters(arrayWidth);

            for (int i = 0; i < i_BoardModel.GetLength(0); i++)
            {
                Console.Write((char)(97 + i));
                Console.Write("| ");

                for (int j = 0; j < i_BoardModel.GetLength(1); j++)
                {
                    if(i_BoardModel[i, j] == null)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write((char)i_BoardModel[i, j]);
                    }
                    Console.Write(" ");
                    Console.Write("| ");
                }
                Console.WriteLine();
                PrintLineOfEqualsLetters(arrayWidth);
            }
        }

        private static void PrintLineOfEqualsLetters(int i_Length)
        {
            Console.Write("=");
            for (int k = 0; k < i_Length; k++)
            {
                Console.Write("====");
            }

            Console.Write("=");
            Console.WriteLine();
        }

        private static void PrintLineOfUpperCaseLetter(int i_Length)
        {
            Console.Write(" ");
            for (int k = 0; k < i_Length; k++)
            {
                Console.Write(" {0}  ", (char)(k + 65));
            }
            Console.WriteLine();
        }
    }
}
