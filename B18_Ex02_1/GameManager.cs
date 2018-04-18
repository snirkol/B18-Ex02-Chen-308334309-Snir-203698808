using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02_1
{
    class GameManager
    {
        Player m_PlayerOne;
        Player m_PlayerTwo;
        Board m_Board;
        eUserTurn m_CurrentUserTurn;
        eGameStatus m_GameStatus;
        int m_playerOneScore = 0;
        int m_playerTwoScore = 0;

        int[] m_PrevSourcePosition = new int[2];
        int[] m_PrevTargetPosition = new int[2];
        eUserTurn m_PrevUser;

        public GameManager()
        {
            int SizeOfBoard;
            string PlayerOneName, PlayerTwoName;

            UserInterface.StartGame(out PlayerOneName, out PlayerTwoName, out SizeOfBoard);
            Console.WriteLine($"Name of player one: {m_PlayerOne} , Name of player tow: {m_PlayerTwo}\nSize of board:{SizeOfBoard} ");
            m_Board = new Board(SizeOfBoard);
            m_PlayerOne = new Player(PlayerOneName, 'X');
            m_PlayerTwo = new Player(PlayerTwoName, 'O');
            
            
            play();
        }

        void play()
        {
            bool isFirstTurn = true;
            BoardView.PrintBoard(m_Board.GetBoard());
            m_CurrentUserTurn = eUserTurn.User1;
            m_PrevUser = eUserTurn.User2;
            m_GameStatus = eGameStatus.OnPlay;

            while (m_GameStatus == eGameStatus.OnPlay)
            {
                if (!isFirstTurn)
                {
                    string playerName = getPlayer(m_PrevUser).GetName();

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
                    UserInterface.PrintWinner(getPlayer(m_PrevUser).GetName());
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
            int currentRow, currentCol, desierdRow, desierdCol;
            bool isQuit;
            char signOfPlayer = getSignOfUser(m_CurrentUserTurn);
            string currentPlayerName = getPlayer(m_CurrentUserTurn).GetName();

            UserInterface.GetParametersOfCurrentTurn(currentPlayerName, signOfPlayer, out currentRow, out currentCol,
                out desierdRow, out desierdCol,out isQuit);

            if (isQuit == true)
            {
                m_GameStatus = eGameStatus.Quit;
            }
            else
            {
                CheckMove(currentRow, currentCol, desierdRow, desierdCol);
                storePrevTurn(currentRow, currentCol, desierdRow, desierdCol, m_CurrentUserTurn, signOfPlayer);
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

        public static  bool CheckMove(int i_currenPositionX, int i_currenPositiony,
            int i_desierdMoveX, int i_desierdMoveY)
        {
            Console.WriteLine($"currentX: {i_currenPositionX}, currentY: {i_currenPositiony}\nDesieredX: {i_desierdMoveX}, DesieredY: {i_desierdMoveY}");
            bool answer = false;
            return answer;
        }
    }
}
