using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02_1
{
    class Board
    {
        private string[,] m_Matrix;
        private int m_Size;

        public Board(int i_Size)
        {
            m_Size = i_Size;
            m_Matrix = new string[m_Size, m_Size];
            InitBoard();
        }

        public int GetSizeOfBoard()
        {
            return m_Size;
        }

        public string[,] GetBoard()
        {
            return m_Matrix;
        }

        public void InitBoard()
        {
            int numOfRowsForEachPlayer = 0; //need to validate this value before this method
            switch (m_Size)
            {
                case 6:
                    numOfRowsForEachPlayer = 2;
                    break;
                case 8:
                    numOfRowsForEachPlayer = 3;
                    break;
                case 10:
                    numOfRowsForEachPlayer = 4;
                    break;
            }

            FillBoard(0, numOfRowsForEachPlayer, "O");
            FillBoard(numOfRowsForEachPlayer + 2, m_Size, "X");
        }

        public void FillBoard(int i_StartRow, int i_EndRow, string i_Value)
        {
            int j;
            for (int i = i_StartRow; i < i_EndRow; i++)
            {
                if (i % 2 == 0)
                {
                    j = 1;
                }
                else
                {
                    j = 0;
                }

                for (; j < m_Size; j += 2)
                {
                    m_Matrix[i, j] = i_Value;
                }
            }
        }

        public void PrintBoard()
        {
            for(int i = 0; i < m_Size; i++)
            {
                for (int j = 0; j < m_Size; j++)
                {
                    Console.Write("{0}  ", m_Matrix[i, j]);
                }
                Console.WriteLine("\n");
            }
        }
    }
}
