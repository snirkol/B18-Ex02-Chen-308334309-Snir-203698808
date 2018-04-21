﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02_1
{
    static class Computer
    {
        public static void GetParametersOfCurrentTurn(string i_CurrentPlayerName, char i_SignOfCurrentPlayer, out int? o_IndexOfCurrentRow,
              out int? o_IndexOfCurrentCol, out int? o_IndexOfNewRow, out int? o_IndexOfNewCol, out bool o_IsQuit, Dictionary<Position, List<Position>> i_PosibleMoves)
        {
            
            Random randKey = new Random();
            Random randValue = new Random();

            List<Position> notEmptyKeys = new List<Position>();
            foreach (Position key in i_PosibleMoves.Keys)
            {
                if(i_PosibleMoves[key].Count != 0)
                {
                    notEmptyKeys.Add(key);
                }
            }

            Position currentRandomKey = notEmptyKeys[(int)randKey.Next(0, notEmptyKeys.Count - 1)];
            List<Position> valuesOfRandKey = i_PosibleMoves[currentRandomKey];
            Position currentRandomValue = valuesOfRandKey[randValue.Next(0, valuesOfRandKey.Count - 1)];

            o_IndexOfCurrentRow = currentRandomKey.m_Row;
            o_IndexOfCurrentCol = currentRandomKey.m_Col;
            o_IndexOfNewRow = currentRandomValue.m_Row;
            o_IndexOfNewCol = currentRandomValue.m_Col;
            o_IsQuit = false;

        }
    }
}
