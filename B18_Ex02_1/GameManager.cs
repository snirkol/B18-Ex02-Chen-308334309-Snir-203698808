using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ex02.ConsoleUtils;

namespace B18_Ex02_1
{
    class GameManager
    {
        Player m_PlayerOne;
        Player m_PlayerTwo;
        Board m_Board;
        eUserTurn m_CurrentUserTurn;
        eGameStatus m_GameStatus;

        int[] m_PrevSourcePosition = new int[2];
        int[] m_PrevTargetPosition = new int[2];
        eUserTurn m_PrevUser;

        public GameManager()
        {
            int SizeOfBoard;
            string PlayerOneName, PlayerTwoName;

            UserInterface.StartGame(out PlayerOneName, out PlayerTwoName, out SizeOfBoard);
            Console.WriteLine($"Name of player one: {m_PlayerOne} , Name of player two: {m_PlayerTwo}\nSize of board:{SizeOfBoard} ");
            m_Board = new Board(SizeOfBoard);

            m_PlayerOne = new Player(PlayerOneName);
            if (PlayerTwoName != null)
            {
                m_PlayerTwo = new Player(PlayerTwoName);
            }
            //todo - add else for playing vs the computer

            Screen.Clear();
            play();
        }

        void play()
        {
            BoardView.PrintBoard(m_Board.GetBoard());
            m_CurrentUserTurn = eUserTurn.User1;
            m_PrevUser = eUserTurn.User2;
            m_GameStatus = eGameStatus.OnPlay;
            bool isFirstTurn = true;

            while (m_GameStatus == eGameStatus.OnPlay)
            {
                if (!isFirstTurn)
                {
                    string playerName = getPlayer(m_PrevUser).m_Name;

                    UserInterface.PrintParametersOfPrevTurn(playerName,
                        getSignOfUser(m_PrevUser), m_PrevSourcePosition, m_PrevTargetPosition);
                }
                isFirstTurn = false;

                currenTurnLogic();
            }

            finishGame();
        }

        private void finishGame()
        {
            switch (m_GameStatus)
            {
                case eGameStatus.Quit :
                    UserInterface.PrintWinner(getPlayer(m_PrevUser).m_Name);
                        break;

                case eGameStatus.Draw:

                    break;

                case eGameStatus.Win:

                    break;
            }

            if (UserInterface.IsContinueGame())
            {
                m_Board.InitBoard();
                play();
            }

        }

        private void currenTurnLogic()
        {
            int? currentRow, currentCol, desierdRow, desierdCol;
            bool isQuit, is_valid_parameters;
            bool isFirstTurn = true;
            char signOfPlayer = getSignOfUser(m_CurrentUserTurn);
            string currentPlayerName = getPlayer(m_CurrentUserTurn).m_Name;

            is_valid_parameters = false;
            do
            {
                if (!isFirstTurn)
                {
                    UserInterface.PrintErrorMessageInvalidMove();
                }
                UserInterface.GetParametersOfCurrentTurn(currentPlayerName, signOfPlayer,
                    out currentRow, out currentCol, out desierdRow, out desierdCol, out isQuit);
                if(isQuit == true)
                {
                    m_GameStatus = eGameStatus.Quit;
                    break;
                }
                if (CheckMove(currentRow, currentCol, desierdRow, desierdCol))
                {
                    is_valid_parameters = true;
                    Move((int)currentRow, (int)currentCol, (int)desierdRow, (int)desierdCol);
                }
                isFirstTurn = false;
            }
            while (!is_valid_parameters);

            if(m_GameStatus != eGameStatus.Quit)
            {
                storePrevTurn((int)currentCol, (int)currentRow, (int)desierdCol,
                (int)desierdRow, m_CurrentUserTurn, signOfPlayer);
                //TODO check game status (check if win/draw)

                Screen.Clear();
                BoardView.PrintBoard(m_Board.GetBoard());
                nextTurn();
            }
        }

        private void nextTurn()
        {
            if (m_CurrentUserTurn == eUserTurn.User1)
            {
                m_CurrentUserTurn = eUserTurn.User2;
            }
            else
            {
                m_CurrentUserTurn = eUserTurn.User1;
            }
        }

        private void storePrevTurn(int currentCol, int currentRow, int desidesierdCol, int desidesierdRow,eUserTurn userTurn,
            char UserSign)
        {
            m_PrevUser = userTurn;
            m_PrevSourcePosition[0] = currentCol;
            m_PrevSourcePosition[1] = currentRow;
            m_PrevTargetPosition[0] = desidesierdCol;
            m_PrevTargetPosition[1] = desidesierdRow;
            
        }

        private char getSignOfUser(eUserTurn i_User)
        {
            char answer;
            if(i_User == eUserTurn.User1)
            {
                answer = 'X';
            }
            else
            {
                answer = 'O';
            }

            return answer;
        }

        private Player getPlayer(eUserTurn i_Player)
        {
            Player player;

            if(i_Player == eUserTurn.User1)
            {
                player = m_PlayerOne;
            }
            else
            {
                player = m_PlayerTwo;
            }

            return player;

        }

        public bool CheckMove(int? i_currentPositionRow, int? i_currentPositionCol,
            int? i_desierdMoveRow, int? i_desierdMoveCol)
        {
            bool answer = true;

            Player currenPlayer = getPlayer(m_CurrentUserTurn);
            eCheckerType? sourceChecker = m_Board.GetCellValue((int)i_currentPositionRow, (int)i_currentPositionCol);
            //CheckerBelongsToTheRightTeam(ref answer, sourceChecker);
            //CheckTargetIsEmpty(ref answer, (int)i_desierdMoveRow, (int)i_desierdMoveCol);
            //CheckMoveToRightDirection(ref answer, (int)i_currentPositionRow, (int)i_currentPositionCol,
            //    i_desierdMoveRow, i_desierdMoveCol);

            Dictionary<Position,List<Position>> resultBySourceChecker
                = LegalMovesCalculator.calculatePosibleMoves(m_CurrentUserTurn, m_Board);
            //TODO: check for null
            Position sourcePosition = new Position((int)i_currentPositionRow, (int)i_currentPositionCol);
            Position desierdPosition = new Position((int)i_desierdMoveRow, (int)i_desierdMoveCol);
            List<Position> result = resultBySourceChecker[sourcePosition];
            if(!result.Contains(desierdPosition))
            {
                answer = false;
            }

            return answer;
        }

        private void CheckMoveToRightDirection(ref bool answer, int i_currentPositionRow, int i_currentPositionCol, int? i_desierdMoveRow, int? i_desierdMoveCol)
        { 
            //check if checker type of man go reverse
            if(m_CurrentUserTurn == eUserTurn.User1 &&
                m_Board.GetCellValue(i_currentPositionRow,i_currentPositionCol) == eCheckerType.Team1_Man &&
                    i_currentPositionRow <= i_desierdMoveRow)
            {
                answer = false;
            }
            //check if checker type of man go reverse
            else if (m_CurrentUserTurn == eUserTurn.User2 &&
                m_Board.GetCellValue(i_currentPositionRow, i_currentPositionCol) == eCheckerType.Team2_Man &&
                    i_currentPositionRow >= i_desierdMoveRow) 
            {
                answer = false;
            }
        }

        private void CheckTargetIsEmpty(ref bool answer, int i_desierdMoveRow, int i_desierdMoveCol)
        {
            eCheckerType? targetCell = m_Board.GetCellValue(i_desierdMoveRow, i_desierdMoveCol);
            if (m_Board.GetCellValue(i_desierdMoveRow, i_desierdMoveCol)!= null)
            {
                answer = false;
            }
        }

        private void CheckerBelongsToTheRightTeam(ref bool answer, eCheckerType? sourceChecker)
        {
            if (sourceChecker == null)
                answer = false;

            if (m_CurrentUserTurn == eUserTurn.User1)
            {
                if (sourceChecker == eCheckerType.Team2_King || sourceChecker == eCheckerType.Team2_Man)
                {
                    answer = false;
                }
            }
            else if (m_CurrentUserTurn == eUserTurn.User2)
            {
                if (sourceChecker == eCheckerType.Team1_King || sourceChecker == eCheckerType.Team1_Man)
                {
                    answer = false;
                }
            }
        }

        public void Move(int i_currenPositionRow, int i_currenPositionCol,
            int i_desierdMoveRow, int i_desierdMoveCol)
        {
            eCheckerType? value = m_Board.GetCellValue(i_currenPositionRow, i_currenPositionCol);
            m_Board.SetBoard(i_currenPositionRow, i_currenPositionCol, null);
            m_Board.SetBoard(i_desierdMoveRow, i_desierdMoveCol, value);
        }
    }
}
