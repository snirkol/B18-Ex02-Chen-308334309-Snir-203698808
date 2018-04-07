using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02_1
{
    class Program
    {
        static void Main(string[] args)
        {
            Board board = new Board(10);
            BoardView.PrintBoard(board.GetBoard());
        }
    }
}
