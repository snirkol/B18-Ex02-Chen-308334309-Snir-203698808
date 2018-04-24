using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ex02.ConsoleUtils;

namespace B18_Ex02_1
{
    class UserInterface
    {
        string m_NameOfPlayer1, m_NameOfPlayer2;
        int m_SizeOfBoard;
        GameManager m_GameManager;

        bool m_IsFirstTurn;

        Position m_PrevSourcePosition;
        Position m_PrevTargetPosition;
        eUserTurn m_PrevUser;

        public UserInterface()
        {
            getDetailsOfPlay();
            m_GameManager = new GameManager(m_NameOfPlayer1, m_NameOfPlayer2, m_SizeOfBoard);
            Screen.Clear();
            BoardView.PrintBoard(m_GameManager.m_Board.GetBoard());
            startGame();

        }

        private void startGame()
        {
            m_IsFirstTurn = true;
            while (m_GameManager.m_GameStatus == eGameStatus.OnPlay)
            {
                play();
            }

            finishGame();
        }

        private void play()
        {
            bool isFirstTry = true;
            if (!m_IsFirstTurn)
            {
                PrintParametersOfPrevTurn(m_GameManager.GetNameOfPlayer(m_PrevUser), m_GameManager.GetSignOfUser(m_PrevUser));
            }

            bool isQuit, IsDesiredMoveValid;
            bool isMoreEats = false;

            IsDesiredMoveValid = false;

            Position sourcePosition, targetPosition;

            do
            {
                if (!isFirstTry)
                {
                    printErrorMessageInvalidMove();
                }
                //check if computer playing
                if (m_GameManager.m_CurrentUserTurn == eUserTurn.User2 && m_GameManager.m_IsComputerPlaying == true)
                {
                    m_GameManager.GetCurrentTurnFromComputer(out sourcePosition, out targetPosition, out isQuit);
                }
                else
                {
                    getParametersOfCurrentTurn(out sourcePosition, out targetPosition, out isQuit);
                }

                if (isQuit == true)
                {
                    m_GameManager.m_GameStatus = eGameStatus.Quit;
                    break;
                }

                if (m_GameManager.CheckMove(sourcePosition, targetPosition))
                {
                    IsDesiredMoveValid = true;
                    m_GameManager.Move(sourcePosition, targetPosition, out isMoreEats);
                }
                m_IsFirstTurn = false;
                isFirstTry = false;
            }
            while (!IsDesiredMoveValid);

            if (m_GameManager.m_GameStatus != eGameStatus.Quit)
            {
                storePrevTurn(sourcePosition, targetPosition, m_GameManager.m_CurrentUserTurn, m_GameManager.GetSignOfUser(m_GameManager.m_CurrentUserTurn));
                m_GameManager.HandleStatusGame();
                Screen.Clear();
                BoardView.PrintBoard(m_GameManager.m_Board.GetBoard());
                if (!isMoreEats)
                {
                    m_GameManager.NextTurn();
                }
            }
        }

        public void getDetailsOfPlay()
        {
            int modeOfGame; //1-computer 2-two players
            bool resultOfTryParse;
            Console.WriteLine("Hello!\nEnter your name and then press 'Enter'");
            m_NameOfPlayer1 = Console.ReadLine();
            while(!validateNameOfPlayer(m_NameOfPlayer1))
            {
                Console.WriteLine("The name is not valid, please enter your name and then press 'Enter'");
                m_NameOfPlayer1 = Console.ReadLine();
            }

            Console.WriteLine("Enter the size of board (6 or 8 or 10) and then press 'Enter'");
            resultOfTryParse = int.TryParse(Console.ReadLine(), out m_SizeOfBoard);
            while ((!resultOfTryParse) || (!validateSizeOfBoard(m_SizeOfBoard)))
            {
                Console.WriteLine("The size is not valid, please enter the size of board (6 or 8 or 10) and then press 'Enter'");
                resultOfTryParse = int.TryParse(Console.ReadLine(), out m_SizeOfBoard);
            }

            Console.WriteLine("For game with the computer press 1\nFor game with two players press 2");
            resultOfTryParse = int.TryParse(Console.ReadLine(), out modeOfGame);
            while((!resultOfTryParse) || (!validateModeOfGame(modeOfGame)))
            {
                Console.WriteLine("The input is not valid\nFor game with the computer press 1\nFor game with two players press 2");
                resultOfTryParse = int.TryParse(Console.ReadLine(), out modeOfGame);
            }
            if (modeOfGame == 1) //game with the computer 
            {
                m_NameOfPlayer2 = null;
            }
            else //game with two players
            {
                Console.WriteLine("Enter the name of player 2 and then press 'Enter'");
                m_NameOfPlayer2 = Console.ReadLine();
                while (!validateNameOfPlayer(m_NameOfPlayer2))
                {
                    Console.WriteLine("The name is not valid, please enter your name and then press 'Enter'");
                    m_NameOfPlayer2 = Console.ReadLine();
                }
            }
        }

        private static bool validateNameOfPlayer(string i_Name)
        {
            bool isValidName = true;
            if((i_Name.Length < 1) && (i_Name.Length > 20))
            {
                isValidName = false;
            }

            if((isValidName)&&(i_Name.Contains(' ')))
            {
                isValidName = false;
            }

            return isValidName;
        }

        private static bool validateSizeOfBoard(int i_SizeOfBoard)
        {
            bool isValidSize = true;
            if (((i_SizeOfBoard != 6) && (i_SizeOfBoard != 8) && (i_SizeOfBoard != 10)))
            {
                isValidSize = false;
            }

            return isValidSize;
        }

        private static bool validateModeOfGame(int i_ModeOfGeme)
        {
            bool isValidMode = true;
            if((i_ModeOfGeme != 1) && (i_ModeOfGeme != 2))
            {
                isValidMode = false;
            }

            return isValidMode;
        }

        private void getParametersOfCurrentTurn(out Position o_SourcePosition, out Position o_TargetPosition, out bool o_IsQuit)
        {
            string turnParameters;
            bool isFirstTurn = true;
            bool isValidParameters = false;
            Console.Write($"{m_GameManager.GetNameOfPlayer(m_GameManager.m_CurrentUserTurn)}'s turn ({m_GameManager.GetSignOfUser(m_GameManager.m_CurrentUserTurn)}):");
            turnParameters = Console.ReadLine();

            o_IsQuit = true;

            o_SourcePosition = new Position(null, null);
            o_TargetPosition = new Position(null, null);

            do
            {
                
                if (!isFirstTurn)
                {
                    Console.WriteLine("The input is not valid, please enter input in this format: COLrow>COLrow");
                    turnParameters = Console.ReadLine();
                }
                
                if (turnParameters.Equals("Q"))
                {
                    break;
                }

                if (validateTurnParameters(turnParameters))
                {
                    o_SourcePosition.m_Col = (char)turnParameters[0] - 65;
                    o_SourcePosition.m_Row = (char)turnParameters[1] - 97;
                    o_TargetPosition.m_Col = (char)turnParameters[3] - 65;
                    o_TargetPosition.m_Row = (char)turnParameters[4] - 97;
                    o_IsQuit = false;
                    isValidParameters = true;
                }
                isFirstTurn = false;
          
            } while (!isValidParameters);
        }

        public void PrintParametersOfPrevTurn(string i_PrevPlayerName, char i_SignOfPrevPlayer)
        {
            char indexOfCurrentCol, indexOfCurrentRow, indexOfNewCol, indexOfNewRow;
            indexOfCurrentCol = (char)(m_PrevSourcePosition.m_Col + 65);
            indexOfCurrentRow = (char)(m_PrevSourcePosition.m_Row + 97);
            indexOfNewCol = (char)(m_PrevTargetPosition.m_Col + 65);
            indexOfNewRow = (char)(m_PrevTargetPosition.m_Row + 97);

            Console.WriteLine($"{i_PrevPlayerName}'s move was ({i_SignOfPrevPlayer}): {indexOfCurrentCol}{indexOfCurrentRow}>{indexOfNewCol}{indexOfNewRow}");
        }

        private static bool validateTurnParameters(string i_TurnParameters)
        {
            bool isValidTurnParameters = true;
            if ((i_TurnParameters.Length != 5) || (!i_TurnParameters[2].Equals('>')) ||
                (!Char.IsLower(i_TurnParameters[1])) || (!Char.IsLower(i_TurnParameters[4])) || 
                (!Char.IsUpper(i_TurnParameters[0])) || (!Char.IsUpper(i_TurnParameters[3])))
            {
                isValidTurnParameters = false;
            }

            return isValidTurnParameters;
        }

        public static void PrintWinner(string i_WinnerName, string i_LoserName, int i_WinnerScore, int i_LoserScore)
        {
            Console.WriteLine($"The winner in this round is: {i_WinnerName}");
            Console.WriteLine($"Score Status: {i_WinnerName}: {i_WinnerScore}, {i_LoserName}: {i_LoserScore}");
        }

        public static void PrintDraw()
        {
            Console.WriteLine($"This round finish in draw");
        }

        private static bool isContinueGame()
        {
            int countinueGame;
            bool result;
            Console.WriteLine("To Continue play press 1\nTo finished current game press 0");
            result = int.TryParse(Console.ReadLine(), out countinueGame);
            while ((!result) && ((countinueGame != 1) || (countinueGame != 0)))
            {
                Console.WriteLine("Invalid input\nTo Continue play press 1\nTo finished current game press 0");
                result = int.TryParse(Console.ReadLine(), out countinueGame);
            }

            if(countinueGame == 1)
            {
                result = true;
            }
            else //result = 0
            {
                result = false;
            }

            return result;
        }

        private static void printErrorMessageInvalidMove()
        {
            Console.WriteLine("invalid move");
        }

        private void finishGame()
        {
            int playerOneScore = 0 , playerTwoScore = 0;
            m_GameManager.CalculatePointAndGameStatus(ref playerOneScore, ref playerTwoScore);

            switch (m_GameManager.m_GameStatus)
            {
                case eGameStatus.Draw:
                    UserInterface.PrintDraw();
                    break;

                case eGameStatus.PlayerOneWin:
                    UserInterface.PrintWinner(m_NameOfPlayer1, m_GameManager.GetNameOfPlayer(eUserTurn.User2), playerOneScore, playerTwoScore);
                    break;
                case eGameStatus.PlayerTwoWin:
                    UserInterface.PrintWinner(m_GameManager.GetNameOfPlayer(eUserTurn.User2), m_NameOfPlayer1, playerTwoScore, playerOneScore);
                    break;
            }

            if (isContinueGame()) //check if user want to continue
            {
                m_GameManager.m_Board.InitBoard();
                m_GameManager.m_GameStatus = eGameStatus.OnPlay;
                m_GameManager.m_CurrentUserTurn = eUserTurn.User1;
                Screen.Clear();
                BoardView.PrintBoard(m_GameManager.m_Board.GetBoard());
                startGame();
            }
        }

        private void storePrevTurn(Position i_SourcePosition, Position i_TargetPosition, eUserTurn userTurn, char UserSign)
        {
            m_PrevUser = userTurn;
            m_PrevSourcePosition.m_Col = (int) i_SourcePosition.m_Col;
            m_PrevSourcePosition.m_Row = (int) i_SourcePosition.m_Row;
            m_PrevTargetPosition.m_Col = (int) i_TargetPosition.m_Col;
            m_PrevTargetPosition.m_Row = (int) i_TargetPosition.m_Row;

        }
    }
}
