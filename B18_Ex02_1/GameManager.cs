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
        public Board m_Board { get; set; }
        public eUserTurn m_CurrentUserTurn { get; set; }
        public eGameStatus m_GameStatus { get; set; }

        public bool m_IsComputerPlaying { get; set; }

        public GameManager(string i_PlayerOneName, string i_PlayerTwoName, int i_SizeOfBoard)
        {
            m_Board = new Board(i_SizeOfBoard);
            m_PlayerOne = new Player(i_PlayerOneName);
            if (i_PlayerTwoName == null)
            {
                m_IsComputerPlaying = true;
                i_PlayerTwoName = "COMPUTER";
            }

            m_PlayerTwo = new Player(i_PlayerTwoName);

            m_CurrentUserTurn = eUserTurn.User1;
            m_GameStatus = eGameStatus.OnPlay;
        }

        public void CalculatePointAndGameStatus(ref int i_PlayerOneScore, ref int i_PlayerTwoScore)
        {
            if(m_GameStatus != eGameStatus.Quit)
            {
                foreach (eCheckerType? currentChecker in m_Board.GetBoard())
                {
                    switch (currentChecker)
                    {
                        case eCheckerType.Team1_Man:
                            i_PlayerOneScore++;
                            break;
                        case eCheckerType.Team1_King:
                            i_PlayerOneScore += 4;
                            break;

                        case eCheckerType.Team2_Man:
                            i_PlayerTwoScore++;
                            break;
                        case eCheckerType.Team2_King:
                            i_PlayerTwoScore += 4;
                            break;
                    }
                }

                if(i_PlayerOneScore == i_PlayerTwoScore)
                {
                    m_GameStatus = eGameStatus.Draw;
                }
                else if(i_PlayerOneScore > i_PlayerTwoScore)
                {
                    m_GameStatus = eGameStatus.PlayerOneWin;
                    m_PlayerOne.m_Score += i_PlayerOneScore - i_PlayerTwoScore;
                }
                else if (i_PlayerOneScore < i_PlayerTwoScore)
                {
                    m_GameStatus = eGameStatus.PlayerTwoWin;
                    m_PlayerTwo.m_Score += i_PlayerTwoScore - i_PlayerOneScore;
                }
            }
            else
            {
                if(m_CurrentUserTurn == eUserTurn.User1)
                {
                    i_PlayerTwoScore++;
                    m_PlayerTwo.m_Score += i_PlayerTwoScore;
                    m_GameStatus = eGameStatus.PlayerTwoWin;
                }
                else
                {
                    i_PlayerOneScore++;
                    m_PlayerOne.m_Score += i_PlayerTwoScore;
                    m_GameStatus = eGameStatus.PlayerOneWin;
                }
            }

            i_PlayerOneScore = m_PlayerOne.m_Score;
            i_PlayerTwoScore = m_PlayerTwo.m_Score;
        }

        public void HandleStatusGame()
        {
            bool isPosibleEat;
            var resultCounterPlayerOne = LegalMovesCalculator.CalculatePosibleMoves(eUserTurn.User1, m_Board, out isPosibleEat).Count;
            var resultCounterPlayerTwo = LegalMovesCalculator.CalculatePosibleMoves(eUserTurn.User2, m_Board, out isPosibleEat).Count;

            if(resultCounterPlayerOne == 0 && resultCounterPlayerTwo > 0)
            {
                m_GameStatus = eGameStatus.PlayerTwoWin;
            }
            else if (resultCounterPlayerTwo == 0 && resultCounterPlayerOne > 0)
            {
                m_GameStatus = eGameStatus.PlayerOneWin;
            }
            else if (resultCounterPlayerOne == 0 && resultCounterPlayerTwo == 0)
            {
                m_GameStatus = eGameStatus.Draw;
            }
        }

        public void NextTurn()
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

        public bool CheckMove(Position i_CurrentPosition, Position i_DesierdPosition)
        {
            bool answer = true;
            bool isMoreEat;

            Dictionary<Position, List<Position>> resultBySourceChecker;

            resultBySourceChecker = LegalMovesCalculator.CalculatePosibleMoves(m_CurrentUserTurn, m_Board, out isMoreEat);

            List<Position> result;
            resultBySourceChecker.TryGetValue(i_CurrentPosition, out result);

            if (result == null || !result.Contains(i_DesierdPosition))
            {
                answer = false;
            }

            return answer;
        }

        public void Move(Position i_CurrentPosition, Position i_DesierdPosition, out bool i_IsMoreEats)
        {
            eCheckerType? value = m_Board.GetCellValue((int)i_CurrentPosition.m_Row,(int)i_CurrentPosition.m_Col);
            i_IsMoreEats = false;
            m_Board.SetBoard((int)i_CurrentPosition.m_Row, (int)i_CurrentPosition.m_Col, null);
            m_Board.SetBoard((int)i_DesierdPosition.m_Row, (int)i_DesierdPosition.m_Col, value);
            ChangeToKingIfNeed(i_DesierdPosition);
            if ((i_CurrentPosition.m_Row - i_DesierdPosition.m_Row > 1) || (i_CurrentPosition.m_Row - i_DesierdPosition.m_Row < -1)) // check for eat 
            {
                m_Board.SetBoard((int)(i_CurrentPosition.m_Row + i_DesierdPosition.m_Row) / 2, (int)(i_CurrentPosition.m_Col + i_DesierdPosition.m_Col) / 2, null);

                //check if exist more eats
                Dictionary<Position, List<Position>> moreLegalEat = LegalMovesCalculator.CalculatePosibleMoves(m_CurrentUserTurn, m_Board, out i_IsMoreEats);
                if(i_IsMoreEats)
                {
                    if(moreLegalEat[i_DesierdPosition].Count != 0)
                    {
                        i_IsMoreEats = true;
                    }
                    else
                    {
                        i_IsMoreEats = false;
                    }
                    
                }
            }
        }

        public void ChangeToKingIfNeed(Position i_CurrentPosition)
        {
            eCheckerType? value = m_Board.GetCellValue((int)i_CurrentPosition.m_Row, (int)i_CurrentPosition.m_Col);

            if(value == eCheckerType.Team1_Man && i_CurrentPosition.m_Row == 0)
            {
                m_Board.SetBoard((int)i_CurrentPosition.m_Row, (int)i_CurrentPosition.m_Col, eCheckerType.Team1_King);
            }
            else
            {
                if (value == eCheckerType.Team2_Man && i_CurrentPosition.m_Row == m_Board.GetBoard().GetLength(1) - 1)
                {
                    m_Board.SetBoard((int)i_CurrentPosition.m_Row, (int)i_CurrentPosition.m_Col, eCheckerType.Team2_King);
                }
            }
        }

        public void GetCurrentTurnFromComputer(out Position i_SourcePosition, out Position i_TargerPosition, out bool o_IsQuit)
        {
            Dictionary<Position, List<Position>> i_PosibleMoves = LegalMovesCalculator.CalculatePosibleMoves(m_CurrentUserTurn, m_Board, out o_IsQuit);

            Random randKey = new Random();
            Random randValue = new Random();

            List<Position> notEmptyKeys = new List<Position>();
            foreach (Position key in i_PosibleMoves.Keys)
            {
                if (i_PosibleMoves[key].Count != 0)
                {
                    notEmptyKeys.Add(key);
                }
            }

            Position currentRandomKey = notEmptyKeys[(int)randKey.Next(0, notEmptyKeys.Count - 1)];
            List<Position> valuesOfRandKey = i_PosibleMoves[currentRandomKey];
            Position currentRandomValue = valuesOfRandKey[randValue.Next(0, valuesOfRandKey.Count - 1)];
            i_SourcePosition = new Position(currentRandomKey.m_Row, currentRandomKey.m_Col);
            i_TargerPosition = new Position(currentRandomValue.m_Row, currentRandomValue.m_Col);
            o_IsQuit = false;
        }

        public string GetNameOfPlayer(eUserTurn i_Player)
        {
            string nameOfPlayer;

            if (i_Player == eUserTurn.User1)
            {
                nameOfPlayer = m_PlayerOne.m_Name;
            }
            else
            {
                nameOfPlayer = m_PlayerTwo.m_Name;
            }

            return nameOfPlayer;
        }

        public char GetSignOfUser(eUserTurn i_User)
        {
            char answer;
            if (i_User == eUserTurn.User1)
            {
                answer = (char)eCheckerType.Team1_Man;
            }
            else
            {
                answer = (char)eCheckerType.Team2_Man;
            }

            return answer;
        }
    }
}
