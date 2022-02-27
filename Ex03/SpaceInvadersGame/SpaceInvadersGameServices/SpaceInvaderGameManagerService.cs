using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using GameScreens.Screens;
using SpaceInvadersGame.SpaceInvadersGameServices.SpaceInvadersGameInterfaces;
using SpaceInvadersGame.SpaceInvadersGameScreens.Menus;

namespace SpaceInvadersGame.SpaceInvadersGameServices
{
    public class SpaceInvaderGameManagerService : GameService, ISpaceInvaderGameManagerService
    {
        private const string k_WinnerString = "The player with the highest score is:";
        private const string k_TieString = "Both players have same score";
        private const float k_InitXBarriersVelocityOfGroup = 35f;
        private const int k_ExtraValuePerLevel = 100;
        private const int k_InitMatrixWidth = 9;
        private const float k_BarriersAcceleratePercentagePerLevel = 1.06f;
        private const int k_AmountOfLevelForRoutine = 4;
        private int m_AmountOfPlayers;
        private int m_Level;
        private int m_EnemyMatrixWidthLevel;
        private int m_ExtraValueForEnemiesLevel;
        private float m_XVelocityOfBarriersLevel;
        private string m_GameOverScoreString;
        private string m_GameOverWinnerString;
        private PlayScreen m_PlayScreen;
        private MainMenuScreen m_MainMenuScreen;

        public SpaceInvaderGameManagerService(Game i_Game) : base(i_Game)
        {
            InitGameManager();
        }

        protected override void RegisterAsService()
        {
            Game.Services.AddService(typeof(ISpaceInvaderGameManagerService), this);
        }

        public PlayScreen PlayScreen
        {
            get
            {
                return m_PlayScreen;
            }

            set
            {
                m_PlayScreen = value;
            }
        }

        public MainMenuScreen MainMenuScreen
        {
            get
            {
                return m_MainMenuScreen;
            }

            set
            {
                m_MainMenuScreen = value;
            }
        }

        public string GameOverScoreString
        {
            get
            {
                return m_GameOverScoreString;
            }

            set
            {
                m_GameOverScoreString = value;
            }
        }

        public string GameOverWinnerString
        {
            get
            {
                return m_GameOverWinnerString;
            }
        }

        public int Level
        {
            get
            {
                return m_Level;
            }
        }

        public int AmountOfPlayers
        {
            get
            {
                return m_AmountOfPlayers;
            }
        }

        public int EnemyMatrixWidthLevel
        {
            get
            {
                return m_EnemyMatrixWidthLevel;
            }
        }

        public float XVelocityOfBarriersLevel
        {
            get
            {
                return m_XVelocityOfBarriersLevel;
            }
        }

        public int ExtraValueForEnemiesLevel
        {
            get
            {
                return m_ExtraValueForEnemiesLevel;
            }
        }

        public void StepUpLevel()
        {
            m_Level++;
            updateGameParameters();
        }

        public void SetWinnerNameString(string i_WinnerString)
        {
            if (i_WinnerString == string.Empty)
            {
                m_GameOverWinnerString = k_TieString;
            }
            else
            {
                m_GameOverWinnerString = string.Format("{0} {1}", k_WinnerString, i_WinnerString);
            }
        }

        private void updateGameParameters()
        {
            if ((Level - 1) % k_AmountOfLevelForRoutine == 0)
            {
                m_XVelocityOfBarriersLevel = 0;
            }

            if ((Level - 1) % k_AmountOfLevelForRoutine == 1)
            {
                m_XVelocityOfBarriersLevel = k_InitXBarriersVelocityOfGroup;
            }
            else
            {
                m_XVelocityOfBarriersLevel += XVelocityOfBarriersLevel * k_BarriersAcceleratePercentagePerLevel;
            }

            m_EnemyMatrixWidthLevel = ((m_Level - 1) % k_AmountOfLevelForRoutine) + k_InitMatrixWidth;
            m_ExtraValueForEnemiesLevel = ((m_Level - 1) % k_AmountOfLevelForRoutine) * k_ExtraValuePerLevel;
        }

        public void IncreaseAmountOfPlayers()
        {
            m_AmountOfPlayers = (AmountOfPlayers % 2) + 1;
        }

        public void InitGameManager()
        {
            this.m_AmountOfPlayers = 1;
            this.m_GameOverScoreString = string.Empty;
            this.m_GameOverWinnerString = string.Empty;
            InitGameLevel();
        }

        public void InitGameLevel()
        {
            this.m_Level = 1;
            this.m_GameOverScoreString = string.Empty;
            this.m_GameOverWinnerString = string.Empty;
            updateGameParameters();
        }

        public void GameManagerRematch()
        {
            InitGameLevel();
            m_PlayScreen.InitGame();
        }
    }
}
