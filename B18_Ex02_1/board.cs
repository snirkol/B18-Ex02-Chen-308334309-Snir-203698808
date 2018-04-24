using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02_1
{
    class Board
    {
        private eCheckerType?[,] m_Matrix;
        private int m_Size;

        public Board(int i_Size)
        {
            m_Size = i_Size;
            m_Matrix = new eCheckerType?[m_Size, m_Size];
            InitBoard();
        }

        public int GetSizeOfBoard()
        {
            return m_Size;
        }

        public eCheckerType?[,] GetBoard()
        {
            return m_Matrix;
        }

        public eCheckerType? GetCellValue(int i_Row, int i_Col)
        {
            return m_Matrix[i_Row, i_Col];
        }

        public void SetBoard(int i_Row, int i_Col, eCheckerType? i_Value)
        {
            m_Matrix[i_Row, i_Col] = i_Value;
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

            for(int i=0; i<m_Size; i++)
            {
                for(int j=0;j<m_Size; j++)
                {
                    m_Matrix[i, j] = null;
                }
            }

            FillBoard(numOfRowsForEachPlayer + 2, m_Size, eCheckerType.Team1_Man);
            FillBoard(0, numOfRowsForEachPlayer, eCheckerType.Team2_Man);
            
        }

        public void FillBoard(int i_StartRow, int i_EndRow, eCheckerType i_Value)
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
    }
}
