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

        bool m_isMoreEats = false;

        int[] m_PrevSourcePosition = new int[2];
        int[] m_PrevTargetPosition = new int[2];
        eUserTurn m_PrevUser;

        public GameManager()
        {
            int SizeOfBoard;
            string PlayerOneName, PlayerTwoName;

            UserInterface.StartGame(out PlayerOneName, out PlayerTwoName, out SizeOfBoard);
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

                currentTurnLogic();
            }

            finishGame();
        }

        private void finishGame()
        {
            CalculatePointAndGameStatus();

            switch (m_GameStatus)
            {
                case eGameStatus.Quit :

                    if (m_CurrentUserTurn == eUserTurn.User1)
                    {
                        m_PlayerTwo.m_Score++;
                    }
                    else
                    {
                        m_PlayerOne.m_Score++;
                    }

                    UserInterface.PrintWinner(getPlayer(m_PrevUser).m_Name);
                    break;

                case eGameStatus.Draw:
                    UserInterface.PrintDraw();
                    break;

                case eGameStatus.PlayerOneWin:
                    UserInterface.PrintWinner(m_PlayerOne.m_Name);
                    break;
                case eGameStatus.PlayerTwoWin:
                    UserInterface.PrintWinner(m_PlayerTwo.m_Name);
                    break;
            }

            if (UserInterface.IsContinueGame()) //check if user want to continue
            {
                m_Board.InitBoard();
                Screen.Clear();
                play();
            }
        }

        private void CalculatePointAndGameStatus()
        {

            int PlayerOnePoint = 0;
            int PlayerTwoPoint = 0;

            if(m_GameStatus != eGameStatus.Quit)
            {
                foreach (eCheckerType currentChecker in m_Board.GetBoard())
                {
                    switch (currentChecker)
                    {
                        case eCheckerType.Team1_Man:
                            PlayerOnePoint++;
                            break;
                        case eCheckerType.Team1_King:
                            PlayerOnePoint += 4;
                            break;

                        case eCheckerType.Team2_Man:
                            PlayerTwoPoint++;
                            break;
                        case eCheckerType.Team2_King:
                            PlayerTwoPoint += 4;
                            break;
                    }
                }

                if(PlayerOnePoint == PlayerTwoPoint)
                {
                    m_GameStatus = eGameStatus.Draw;
                }
                else if(PlayerOnePoint >PlayerTwoPoint)
                {
                    m_GameStatus = eGameStatus.PlayerOneWin;
                    m_PlayerOne.m_Score += PlayerOnePoint - PlayerTwoPoint;
                }
                else if (PlayerOnePoint < PlayerTwoPoint)
                {
                    m_GameStatus = eGameStatus.PlayerTwoWin;
                    m_PlayerTwo.m_Score += PlayerTwoPoint - PlayerOnePoint;
                }
            }
        }

        private void currentTurnLogic()
        {
            int? currentRow, currentCol, desierdRow, desierdCol;
            bool isQuit, IsDesiredMoveValid;
            bool isFirstTurn = true;
            char signOfPlayer = getSignOfUser(m_CurrentUserTurn);
            string currentPlayerName = getPlayer(m_CurrentUserTurn).m_Name;

            IsDesiredMoveValid = false;
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
                Position currentPosition = new Position((int)currentRow, (int)currentCol);
                Position desierdPosition = new Position((int)desierdRow, (int)desierdCol);
                if (CheckMove(currentPosition, desierdPosition))
                {
                    IsDesiredMoveValid = true;
                    Move(currentPosition, desierdPosition);
                    ChangeToKingIfNeed(desierdPosition);
                    if ((currentRow - desierdRow > 1) || (currentRow - desierdRow  < -1)) // check for eat 
                    {
                        m_Board.SetBoard((int)(currentRow + desierdRow) / 2, (int)(currentCol + desierdCol) / 2, null);
                        
                        //check if exist more eats
                        Dictionary<Position, List<Position>> moreLegalEat = LegalMovesCalculator.CalculatePosibleEats(m_CurrentUserTurn, m_Board);
                        if(moreLegalEat[desierdPosition].Count != 0)
                        {
                            m_isMoreEats = true;
                        }
                        else
                        {
                            m_isMoreEats = false;
                        }
                    }
                    else
                    {
                        m_isMoreEats = false;
                    }
                }

                isFirstTurn = false;
            }
            while (!IsDesiredMoveValid);

            if(m_GameStatus != eGameStatus.Quit)
            {
                storePrevTurn((int)currentCol, (int)currentRow, (int)desierdCol,
                (int)desierdRow, m_CurrentUserTurn, signOfPlayer);
                //TODO check game status (check if win/draw)

                Screen.Clear();
                BoardView.PrintBoard(m_Board.GetBoard());

                if(!m_isMoreEats)
                {
                    nextTurn();
                }
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
                answer = (char)eCheckerType.Team1_Man;
            }
            else
            {
                answer = (char)eCheckerType.Team2_Man;
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

        public bool CheckMove(Position i_CurrentPosition, Position i_DesierdPosition)
        {
            bool answer = true;
            Player currenPlayer = getPlayer(m_CurrentUserTurn);

            Dictionary<Position, List<Position>> resultBySourceChecker;

            if (!m_isMoreEats)
            {
                resultBySourceChecker = LegalMovesCalculator.CalculatePosibleMoves(m_CurrentUserTurn, m_Board);
            }
            else
            {
                resultBySourceChecker = LegalMovesCalculator.CalculatePosibleEats(m_CurrentUserTurn, m_Board);
            }

            List<Position> result;
            resultBySourceChecker.TryGetValue(i_CurrentPosition, out result);

            if (result == null || !result.Contains(i_DesierdPosition))
            {
                answer = false;
            }

            return answer;
        }

        public void Move(Position i_CurrentPosition, Position i_DesierdPosition)
        {
            eCheckerType? value = m_Board.GetCellValue(i_CurrentPosition.m_Row,i_CurrentPosition.m_Col);
            m_Board.SetBoard(i_CurrentPosition.m_Row, i_CurrentPosition.m_Col, null);
            m_Board.SetBoard(i_DesierdPosition.m_Row, i_DesierdPosition.m_Col, value);
        }

        public void ChangeToKingIfNeed(Position i_CurrentPosition)
        {
            eCheckerType? value = m_Board.GetCellValue(i_CurrentPosition.m_Row, i_CurrentPosition.m_Col);

            if(value == eCheckerType.Team1_Man && i_CurrentPosition.m_Row == 0)
            {
                m_Board.SetBoard(i_CurrentPosition.m_Row, i_CurrentPosition.m_Col, eCheckerType.Team1_King);
            }
            else
            {
                if (value == eCheckerType.Team2_Man && i_CurrentPosition.m_Row == m_Board.GetBoard().GetLength(1) - 1)
                {
                    m_Board.SetBoard(i_CurrentPosition.m_Row, i_CurrentPosition.m_Col, eCheckerType.Team2_King);
                }
            }
        }
    }
}
