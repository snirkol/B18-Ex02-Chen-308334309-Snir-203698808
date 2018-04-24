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
        public static Dictionary<Position, List<Position>> CalculatePosibleMoves(eUserTurn i_CurrentTurn, Board i_Board, out bool i_IsPossibleEat)
        {
            i_IsPossibleEat = false;

            Dictionary<Position, List<Position>> posibleMovesPerPosition = calculatePosibleEats(i_CurrentTurn, i_Board);
            foreach (Position key in posibleMovesPerPosition.Keys)
            {
                if(posibleMovesPerPosition[key].Count != 0)
                {
                    i_IsPossibleEat = true;
                    break;
                }
            }

            if(!i_IsPossibleEat)
            {
                posibleMovesPerPosition = new Dictionary<Position, List<Position>>();

                eCheckerType?[,] currentBoard = i_Board.GetBoard();
                eCheckerType? currentChecker;

                for (int row = 0; row < currentBoard.GetLength(0); row++)
                {
                    for (int col = 0; col < currentBoard.GetLength(1); col++)
                    {
                        currentChecker = currentBoard[row, col];

                        if (checkerBelongsToTheRightTeam(i_CurrentTurn, currentChecker))
                        {
                            List<Position> ListOfPosibleMoves = getPosibleMoves(row, col, i_CurrentTurn, currentBoard);
                            Position currentPosition = new Position(row, col);
                            if (ListOfPosibleMoves.Count > 0)
                            {
                                posibleMovesPerPosition.Add(currentPosition, ListOfPosibleMoves);
                            }
                        }
                    }
                }
            }
     
            return posibleMovesPerPosition;
        }

        private static Dictionary<Position, List<Position>> calculatePosibleEats(eUserTurn i_CurrentTurn, Board i_Board)
        {
            Dictionary<Position, List<Position>> posibleMovesPerPosition = new Dictionary<Position, List<Position>>();

            eCheckerType?[,] currentBoard = i_Board.GetBoard();
            eCheckerType? currentChecker;

            for (int row = 0; row < currentBoard.GetLength(0); row++)
            {
                for (int col = 0; col < currentBoard.GetLength(1); col++)
                {
                    currentChecker = currentBoard[row, col];

                    if (checkerBelongsToTheRightTeam(i_CurrentTurn, currentChecker))
                    {
                        Position currentPosition = new Position(row, col);
                        List<Position> posibleMovesToEat = new List<Position>();

                        if (currentChecker == eCheckerType.Team1_Man || currentChecker == eCheckerType.Team1_King
                            || currentChecker == eCheckerType.Team2_King)
                        {
                            upLeftEat(row, col, currentBoard, ref posibleMovesToEat);
                            upRightEat(row, col, currentBoard, ref posibleMovesToEat);
                        }
                        if (currentChecker == eCheckerType.Team2_Man || currentChecker == eCheckerType.Team2_King
                            || currentChecker == eCheckerType.Team1_King)
                        {
                            downLeftEat(row, col, currentBoard, ref posibleMovesToEat);
                            downRightEat(row, col, currentBoard, ref posibleMovesToEat);
                        }
                        if (posibleMovesToEat.Count > 0)
                        {
                            posibleMovesPerPosition.Add(currentPosition, posibleMovesToEat);
                        }
                    }
                }
            }
            
            return posibleMovesPerPosition;
        }

        private static List<Position> getPosibleMoves(int row, int col, eUserTurn i_CurrentTurn, eCheckerType?[,] i_Board)
        {
            List<Position> posibleMoves = new List<Position>();

            eCheckerType currentChecker =(eCheckerType) i_Board[row, col];
            
            if(currentChecker == eCheckerType.Team1_Man || currentChecker == eCheckerType.Team1_King 
                || currentChecker == eCheckerType.Team2_King)
            {
                diagonallyUpLeft(row, col, i_Board, ref posibleMoves);
                diagonallyUpRight(row, col, i_Board, ref posibleMoves);
            }
            if (currentChecker == eCheckerType.Team2_Man || currentChecker == eCheckerType.Team2_King
                || currentChecker == eCheckerType.Team1_King)
            {
                diagonallyDownLeft(row, col, i_Board, ref posibleMoves);
                diagonallyDownRight(row, col, i_Board, ref posibleMoves);
            }

            return posibleMoves;
        }

        private static void diagonallyUpRight(int i_Row, int i_Col, eCheckerType?[,] i_Board, ref List<Position> i_PosibleMoves)
        {
            //check board Limit
            if (i_Row > 0 && i_Col < i_Board.GetLength(0) - 1)
            {
                eCheckerType? upRightCell = i_Board[i_Row - 1, i_Col + 1];

                //check cell content
                if (upRightCell == null)
                {
                    i_PosibleMoves.Add(new Position(i_Row - 1, i_Col + 1));
                }
            }
        }

        private static void diagonallyDownRight(int i_Row, int i_Col, eCheckerType?[,] i_Board, ref List<Position> i_PosibleMoves)
        {
            //check board Limit
            if (i_Row < i_Board.GetLength(0) - 1 && i_Col > 0)
            {
                eCheckerType? upRightCell = i_Board[i_Row + 1, i_Col - 1];

                //check cell content
                if (upRightCell == null)
                {
                    i_PosibleMoves.Add(new Position(i_Row + 1, i_Col - 1));
                }
            }
        }

        private static void diagonallyUpLeft(int i_Row, int i_Col, eCheckerType?[,] i_Board, ref List<Position> i_PosibleMoves)
        {
            //check board Limit
            if (i_Row > 0 && i_Col > 0)
            {
                eCheckerType? upLeftCell = i_Board[i_Row - 1, i_Col - 1];

                //check cell content
                if (upLeftCell == null)
                {
                    i_PosibleMoves.Add(new Position(i_Row - 1, i_Col - 1));
                }
            }
        }

        private static void diagonallyDownLeft(int i_Row, int i_Col, eCheckerType?[,] i_Board, ref List<Position> i_PosibleMoves)
        {
            //check board Limit
            if (i_Row < i_Board.GetLength(0) - 1 && i_Col < i_Board.GetLength(0) -1)
            {
                eCheckerType? upLeftCell = i_Board[i_Row + 1, i_Col + 1];

                //check cell content
                if (upLeftCell == null)
                {
                    i_PosibleMoves.Add(new Position(i_Row + 1, i_Col + 1));
                }
            }
        }

        private static void upRightEat(int i_Row, int i_Col, eCheckerType?[,] i_Board, ref List<Position> i_PosibleMoves)
        {
            if (i_Row > 0 && i_Col < i_Board.GetLength(0) - 1)
            {
                eCheckerType? currentCell = i_Board[i_Row, i_Col];
                eCheckerType? upRightCell = i_Board[i_Row - 1, i_Col + 1];

                if ((currentCell != eCheckerType.Team2_King &&
                    (upRightCell == eCheckerType.Team2_Man || upRightCell == eCheckerType.Team2_King)) ||
                    (currentCell == eCheckerType.Team2_King &&
                    (upRightCell == eCheckerType.Team1_Man || upRightCell == eCheckerType.Team1_King)))
                {
                    //check board limit
                    if (i_Row - 1 > 0 && i_Col + 1 < i_Board.GetLength(0) - 1)
                    {
                        upRightCell = i_Board[i_Row - 2, i_Col + 2];
                        //check cell content
                        if (upRightCell == null)
                        {
                            i_PosibleMoves.Add(new Position(i_Row - 2, i_Col + 2));
                        }
                    }
                }
            }
        }


        private static void downRightEat(int i_Row, int i_Col, eCheckerType?[,] i_Board, ref List<Position> i_PosibleMoves)
        {
            if (i_Row < i_Board.GetLength(0) - 1 && i_Col > 0)
            {
                eCheckerType? currentCell = i_Board[i_Row, i_Col];
                eCheckerType? upRightCell = i_Board[i_Row + 1, i_Col - 1];

                if ((currentCell != eCheckerType.Team1_King &&
                    (upRightCell == eCheckerType.Team1_Man || upRightCell == eCheckerType.Team1_King)) ||
                    (currentCell == eCheckerType.Team1_King &&
                    (upRightCell == eCheckerType.Team2_Man || upRightCell == eCheckerType.Team2_King)))
                {
                    //check board Limit
                    if (i_Row + 1 < i_Board.GetLength(0) - 1 && i_Col - 1 > 0)
                    {
                        upRightCell = i_Board[i_Row + 2, i_Col - 2];
                        //check cell content
                        if (upRightCell == null)
                        {
                            i_PosibleMoves.Add(new Position(i_Row + 2, i_Col - 2));
                        }
                    }
                }
            }
        }

        private static void upLeftEat(int i_Row, int i_Col, eCheckerType?[,] i_Board, ref List<Position> i_PosibleMoves)
        {
            if (i_Row > 0 && i_Col > 0)
            {
                eCheckerType? currentCell = i_Board[i_Row, i_Col];
                eCheckerType? upLeftCell = i_Board[i_Row - 1, i_Col - 1];

                if ((currentCell != eCheckerType.Team2_King &&
                    (upLeftCell == eCheckerType.Team2_Man || upLeftCell == eCheckerType.Team2_King)) ||
                    (currentCell == eCheckerType.Team2_King &&
                    (upLeftCell == eCheckerType.Team1_Man || upLeftCell == eCheckerType.Team1_King)))
                {
                    //check board Limit
                    if (i_Row -1 > 0 && i_Col -1 > 0)
                    {
                        upLeftCell = i_Board[i_Row - 2, i_Col - 2];
                        //check cell content
                        if (upLeftCell == null)
                        {
                            i_PosibleMoves.Add(new Position(i_Row - 2, i_Col - 2));
                        }
                    }
                }
            }
        }

        private static void downLeftEat(int i_Row, int i_Col, eCheckerType?[,] i_Board, ref List<Position> i_PosibleMoves)
        {
            if (i_Row < i_Board.GetLength(0) - 1 && i_Col < i_Board.GetLength(0) - 1)
            {
                eCheckerType? currentCell = i_Board[i_Row, i_Col];
                eCheckerType? upLeftCell = i_Board[i_Row + 1, i_Col + 1];
                if ((currentCell != eCheckerType.Team1_King &&
                    (upLeftCell == eCheckerType.Team1_Man || upLeftCell == eCheckerType.Team1_King)) ||
                    (currentCell == eCheckerType.Team1_King &&
                    (upLeftCell == eCheckerType.Team2_Man || upLeftCell == eCheckerType.Team2_King)))
                {
                    //check board Limit
                    if (i_Row + 1 < i_Board.GetLength(0) - 1 && i_Col + 1 < i_Board.GetLength(0) - 1)
                    {
                        upLeftCell = i_Board[i_Row + 2, i_Col + 2];
                        //check cell content
                        if (upLeftCell == null)
                        {
                            i_PosibleMoves.Add(new Position(i_Row + 2, i_Col + 2));
                        }
                    }
                }
            }
        }

        private static bool checkerBelongsToTheRightTeam(eUserTurn i_CurrentTurn, eCheckerType? sourceChecker)
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
