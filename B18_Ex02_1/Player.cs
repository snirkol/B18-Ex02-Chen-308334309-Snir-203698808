using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B18_Ex02_1
{
    class Player
    {
        string m_Name;
        int m_Score;
        char m_Sign;

        public Player(string i_name, char sign)
        {
            m_Name = i_name;
            m_Sign = sign;
            m_Score = 0;
        }

        public string GetName()
        {
            return m_Name;
        }
    }
}
