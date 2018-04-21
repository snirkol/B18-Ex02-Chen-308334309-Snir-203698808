using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02_1
{
    static class Computer
    {
        public static void GetParametersOfCurrentTurn(string i_CurrentPlayerName, char i_SignOfCurrentPlayer, out int? o_IndexOfCurrentRow,
              out int? o_IndexOfCurrentCol, out int? o_IndexOfNewRow, out int? o_IndexOfNewCol, Dictionary<Position, List<Position>> i_PossibleMoves)
        {
            o_IndexOfCurrentRow = 1;
            o_IndexOfCurrentCol = 1;
            o_IndexOfNewRow = 1;
            o_IndexOfNewCol = 1;
        }
    }
}
