using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02_1
{
    static class UserInterface
    {
        public static void StartGame(out string o_NameOfPlayer1, out string o_NameOfPlayer2, out int o_SizeOfBoard)
        {
            int modeOfGame; //1-computer 2-two players
            bool resultOfTryParse;
            Console.WriteLine("Hello!\nEnter your name and then press 'Enter'");
            o_NameOfPlayer1 = Console.ReadLine();
            while(!validateNameOfPlayer(o_NameOfPlayer1))
            {
                Console.WriteLine("The name is not valid, please enter your name and then press 'Enter'");
                o_NameOfPlayer1 = Console.ReadLine();
            }

            Console.WriteLine("Enter the size of board (6 or 8 or 10) and then press 'Enter'");
            resultOfTryParse = int.TryParse(Console.ReadLine(), out o_SizeOfBoard);
            while ((!resultOfTryParse) || (!validateSizeOfBoard(o_SizeOfBoard)))
            {
                Console.WriteLine("The size is not valid, please enter the size of board (6 or 8 or 10) and then press 'Enter'");
                resultOfTryParse = int.TryParse(Console.ReadLine(), out o_SizeOfBoard);
            }

            Console.WriteLine("For game with the computer press 1\nFor game with two players press 2");
            resultOfTryParse = int.TryParse(Console.ReadLine(), out modeOfGame);
            while((!resultOfTryParse) || (!validateModeOfGame(modeOfGame)))
            {
                Console.WriteLine("The input is not valid\nFor game with the computer press 1\nFor game with two players press 2");
                resultOfTryParse = int.TryParse(Console.ReadLine(), out modeOfGame);
            }
            if(modeOfGame == 1) //game with the computer 
            {
                o_NameOfPlayer2 = null;
            }
            else //game with two players
            {
                Console.WriteLine("Enter the name of player 2 and then press 'Enter'");
                o_NameOfPlayer2 = Console.ReadLine();
                while (!validateNameOfPlayer(o_NameOfPlayer2))
                {
                    Console.WriteLine("The name is not valid, please enter your name and then press 'Enter'");
                    o_NameOfPlayer2 = Console.ReadLine();
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

        public static void GetParametersOfCurrentTurn(string i_CurrentPlayerName, char i_SignOfCurrentPlayer, out int? o_IndexOfCurrentRow, 
            out int? o_IndexOfCurrentCol, out int? o_IndexOfNewRow, out int? o_IndexOfNewCol, out bool o_IsQuit)
        {
            string turnParameters;
            bool isFirstTurn = true;
            bool isValidParameters = false;
            Console.Write($"{i_CurrentPlayerName}'s turn ({i_SignOfCurrentPlayer}):");
            turnParameters = Console.ReadLine();

            o_IsQuit = true;
            o_IndexOfCurrentCol = null;
            o_IndexOfCurrentRow = null;
            o_IndexOfNewCol = null;
            o_IndexOfNewRow = null;

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
                    o_IndexOfCurrentCol = (char)turnParameters[0] - 65;
                    o_IndexOfCurrentRow = (char)turnParameters[1] - 97;
                    o_IndexOfNewCol = (char)turnParameters[3] - 65;
                    o_IndexOfNewRow = (char)turnParameters[4] - 97;
                    o_IsQuit = false;
                    isValidParameters = true;
                }
                isFirstTurn = false;
          
            } while (!isValidParameters);
        }

        public static void PrintParametersOfPrevTurn(string i_PrevPlayerName, char i_SignOfPrevPlayer, int[] i_IndexesOfSource, int[] i_IndexesOfTarget)
        {
            char indexOfCurrentCol, indexOfCurrentRow, indexOfNewCol, indexOfNewRow;
            indexOfCurrentCol = (char)(i_IndexesOfSource[0] + 65);
            indexOfCurrentRow = (char)(i_IndexesOfSource[1] + 97);
            indexOfNewCol = (char)(i_IndexesOfTarget[0] + 65);
            indexOfNewRow = (char)(i_IndexesOfTarget[1] + 97);

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

        public static void PrintWinner(string i_Winner)
        {
            Console.WriteLine($"The winner in this round is: {i_Winner}");
        }

        public static void PrintDraw()
        {
            Console.WriteLine($"This round finish in draw");
        }

        public static bool IsContinueGame()
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

        public static void PrintErrorMessageInvalidMove()
        {
            Console.WriteLine("invalid move");
        }
    }
}
