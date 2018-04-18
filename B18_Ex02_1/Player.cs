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

        public Player(string i_Name)
        {
            m_Name = i_Name;
            m_Score = 0;
        }

    }
}
