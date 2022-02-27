using Microsoft.Xna.Framework;

namespace Infrastructure.GamesObjects.ObjectModel
{
    public class Player
    {
        private const string k_ScoreMessage = "scroe is:";
        private readonly int r_InitAmoutOfLives;
        private Color m_PlayerColor;
        private string m_PlayerName;
        private int m_PlayerId;
        private int m_PlayerScore;
        private int m_AmountOfLives;

        public Player(int i_PlayerId, string i_PlayerName, Color i_PlayerColor, int i_AmountOfLives)
        {
            this.m_PlayerId = i_PlayerId;
            this.m_PlayerName = i_PlayerName;
            this.m_PlayerColor = i_PlayerColor;
            this.m_AmountOfLives = i_AmountOfLives;
            this.r_InitAmoutOfLives = i_AmountOfLives;
            this.m_PlayerScore = 0;
        }

        public Player(int i_AmountOfLives)
        {
            this.m_AmountOfLives = i_AmountOfLives;
            this.r_InitAmoutOfLives = i_AmountOfLives;
            this.m_PlayerScore = 0;
        }

        public int PlayerId
        {
            get
            {
                return m_PlayerId;
            }

            set
            {
                m_PlayerId = value;
            }
        }

        public string PlayerName
        {
            get
            {
                return m_PlayerName;
            }

            set
            {
                m_PlayerName = value;
            }
        }

        public int PlayerScore
        {
            get
            {
                return m_PlayerScore;
            }

            set
            {
                m_PlayerScore = value;
            }
        }

        public Color PlayerColor
        {
            get
            {
                return m_PlayerColor;
            }

            set
            {
                m_PlayerColor = value;
            }
        }

        public int AmountOfLives
        {
            get
            {
                return m_AmountOfLives;
            }

            set
            {
                m_AmountOfLives = value;
            }
        }

        public int InitAmoutOfLives
        {
            get
            {
                return r_InitAmoutOfLives;
            }
        }

        public string ScoreString()
        {
            return string.Format("{0} {1} {2}", m_PlayerName, k_ScoreMessage, m_PlayerScore);
        }
    }
}
