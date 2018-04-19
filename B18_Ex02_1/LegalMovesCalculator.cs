using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02_1
{
    static class LegalMovesCalculator
    {
        public static Dictionary<Position,List<Position>> calculatePosibleMoves(eUserTurn i_CurrentTurn, Board i_Board)
        {
            Dictionary<Position, List<Position>> posibleMovesPerPosition = new Dictionary<Position, List<Position>>();
            
            eCheckerType?[,] currentBoard = i_Board.GetBoard();
            eCheckerType? currentChecker;

            for (int row = 0; row < currentBoard.GetLength(0); row++)
            {
                for (int col = 0; col < currentBoard.GetLength(1); col++)
                {
                    currentChecker = currentBoard[row, col];

                    if (CheckerBelongsToTheRightTeam(i_CurrentTurn,currentChecker))
                    {
                        List<Position> ListOfPosibleMoves = getPosibleMoves(row, col, i_CurrentTurn, currentBoard);
                        Position currentPosition = new Position(row, col);

                        posibleMovesPerPosition.Add(currentPosition, ListOfPosibleMoves);
                    }
                }
            }


            return posibleMovesPerPosition;
        }

        private static List<Position> getPosibleMoves(int row, int col, eUserTurn i_CurrentTurn, eCheckerType?[,] i_Board)
        {
            List<Position> PosibleMoves = new List<Position>();

            eCheckerType currentChecker =(eCheckerType) i_Board[row, col];
            
            if(currentChecker == eCheckerType.Team1_Man || currentChecker == eCheckerType.Team1_King)
            {
                if(CheckDiagonallyUpLeft(row, col, i_Board))
                {
                    PosibleMoves.Add(new Position(row - 1, col - 1));
                }
                if (CheckDiagonallyUpRight(row, col, i_Board))
                {
                    PosibleMoves.Add(new Position(row - 1, col - 1));
                }
            }

            return PosibleMoves;

        }

        private static bool CheckDiagonallyUpRight(int i_Row, int i_Col, eCheckerType?[,] i_Board)
        {
            bool answer = false;
            //check board Limit
            if (i_Row < i_Board.GetLength(0) - 1 && i_Col > 0)
            {
                //check cell content
                eCheckerType? upLeftCell = i_Board[i_Row + 1, i_Col - 1];

                if (i_Board[i_Row + 1, i_Col - 1] == null)
                {
                    answer = true;
                }
                else
                {
                    //TODO: Need to check for reqursive
                    answer = false;
                }
            }

            return answer;
        }

        private static bool CheckDiagonallyUpLeft(int i_Row, int i_Col, eCheckerType?[,] i_Board)
        {
            bool answer = false;
            //check board Limit
            if (i_Row > 0  && i_Col > 0)
            {
                //check cell content
                eCheckerType? upLeftCell = i_Board[i_Row - 1, i_Col - 1];

                if (i_Board[i_Row - 1, i_Col - 1] == null)
                {
                    answer = true;
                }
                else
                {
                    //TODO: Need to check for reqursive
                    answer = false;
                }
            }

            return answer;
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
