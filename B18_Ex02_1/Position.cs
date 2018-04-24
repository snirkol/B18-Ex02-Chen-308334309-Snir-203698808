using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02_1
{
    struct Position
    {
        public int? m_Row { get; set; }
        public int? m_Col { get; set; }

        public Position(int? i_Row, int? i_Col)
        {
            m_Row = i_Row;
            m_Col = i_Col;
        }
    }
}
