using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02_1
{
    static class BoardView
    {
        public static void PrintBoard(string[,] BoardModel)
        {
            int arrayLength = BoardModel.GetLength(0);
            int arrayWidth = BoardModel.GetLength(1);

            Console.Write(" ");
            PrintLineOfUpperCaseLetter(arrayWidth);
            PrintLineOfEqualsLetters(arrayWidth);

            for (int i = 0; i < BoardModel.GetLength(0); i++)
            {
                Console.Write((char)(97 + i));
                Console.Write("| ");

                for (int j = 0; j < BoardModel.GetLength(1); j++)
                {
                    if(BoardModel[i, j] == null)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write(BoardModel[i, j]);
                    }
                    Console.Write(" ");
                    Console.Write("| ");
                }
                Console.WriteLine();
                PrintLineOfEqualsLetters(arrayWidth);
            }
        }

        private static void PrintLineOfEqualsLetters(int Length)
        {
            Console.Write("=");
            for (int k = 0; k < Length; k++)
            {
                Console.Write("====");
            }

            Console.Write("=");
            Console.WriteLine();
        }

        private static void PrintLineOfUpperCaseLetter(int Length)
        {
            Console.Write(" ");
            for (int k = 0; k < Length; k++)
            {
                Console.Write(" {0}  ", (char)(k + 65));
            }
            Console.WriteLine();
        }
    }
}
