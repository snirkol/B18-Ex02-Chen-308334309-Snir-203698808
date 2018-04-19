using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02_1
{
    static class LegalMovesCalculator
    {
        public static Dictionary<int[],List<int[]>> calculatePosibleMoves(eUserTurn i_CurrentTurn, Board i_Board)
        {
            Dictionary<int[], List<int[]>> posibleMoves = new Dictionary<int[], List<int[]>>();

            
            eCheckerType?[,] currentBoard = i_Board.GetBoard();
            eCheckerType? currentChecker;

            for (int row = 0; row < currentBoard.GetLength(0); row++)
            {
                for (int col = 0; col < currentBoard.GetLength(1); col++)
                {
                    currentChecker = currentBoard[row, col];

                    if (CheckerBelongsToTheRightTeam(i_CurrentTurn,currentChecker))
                    {
                        posibleMoves.Add(new int[] {row,col}, new List<int[]>()); //Store the currentChecker.
                    }
                }
            }



            return posibleMoves;
        }

        private static bool CheckerBelongsToTheRightTeam(eUserTurn i_CurrentTurn, eCheckerType? sourceChecker)
        {
            bool answer = true;

            if (sourceChecker == null)
                answer = false;

            if (i_CurrentTurn == eUserTurn.User1)
            {
                if (sourceChecker == eCheckerType.Team2_King || sourceChecker == eCheckerType.Team2_Man)
                {
                    answer = false;
                }
            }
            else if (i_CurrentTurn == eUserTurn.User2)
            {
                if (sourceChecker == eCheckerType.Team1_King || sourceChecker == eCheckerType.Team1_Man)
                {
                    answer = false;
                }
            }

            return answer;
        }
    }
}
