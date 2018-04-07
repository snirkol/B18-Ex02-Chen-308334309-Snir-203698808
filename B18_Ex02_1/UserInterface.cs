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
            int modeOfGame; // 1-two players 2-computer
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

            Console.WriteLine("For game with two players press 1\nFor game with the computer press 2");
            resultOfTryParse = int.TryParse(Console.ReadLine(), out modeOfGame);
            while((!resultOfTryParse) || (!validateModeOfGame(modeOfGame)))
            {
                Console.WriteLine("The input is not valid\nFor game with two players press 1\nFor game with the computer press 2");
                resultOfTryParse = int.TryParse(Console.ReadLine(), out modeOfGame);
            }
            if(modeOfGame == 1) //game with two players
            {
                Console.WriteLine("Enter the name of player 2 and then press 'Enter'");
                o_NameOfPlayer2 = Console.ReadLine();
                while (!validateNameOfPlayer(o_NameOfPlayer2))
                {
                    Console.WriteLine("The name is not valid, please enter your name and then press 'Enter'");
                    o_NameOfPlayer2 = Console.ReadLine();
                }
            }
            else //game with the computer 
            {
                o_NameOfPlayer2 = "";
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
    }
}
