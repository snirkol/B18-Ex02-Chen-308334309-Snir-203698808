using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02_1
{
    class Player
    {
        public string m_Name { get; set; }
        public int m_Score { get; set; }
        public char m_Sign { get; set; }

        public Player(string i_Name, char i_Sign)
        {
            m_Name = i_Name;
            m_Sign = i_Sign;
            m_Score = 0;
        }

    }
}
