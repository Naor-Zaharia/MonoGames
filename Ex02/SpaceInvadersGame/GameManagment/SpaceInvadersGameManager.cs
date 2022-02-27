using System;
using System.Windows.Forms;
using Infrastructure.ObjectGeneralGame;
using Microsoft.Xna.Framework;
using SpaceInvadersGame.SpaceInvadersUtils;
using SpaceInvadersGame.SpaceInvadersObjects;
using Infrastructure.Managers;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvadersGame
{
    internal class SpaceInvadersGameManager
    {
        private const string k_LostMessage = "You Lost !!!";
        private const string k_WinMessage = "You Won !!!";
        private const string k_WinnerString = "The player with the highest score is:";
        private const string k_TieString = "Both players have same score";
        private const string k_BackgroundAssetName = @"Background\BG_Space01_1024x768";
        private const string k_MessageBoxTitle = "Game Over: Summary - Score Performance";
        private const float k_BackgroundPercentage = 0.8f;

        private readonly Background r_Background;
        private CollisionsManager m_CollisionsManager;
        private GraphicsDeviceManager m_GraphicsDeviceManager;
        private Game m_Game;        
        private EnemiesMatrix m_EnemiesMatrix;
        private MotherShip m_MotherShip;
        private SpaceInvaderPlayer m_FirstPlayer;
        private SpaceInvaderPlayer m_SecondPlayer;
        private Barriers m_Barriers;
        private int m_CurrentLivePlayer;

        public SpaceInvadersGameManager(Game i_Game, SpriteBatch i_SpriteBatch, GraphicsDeviceManager i_GraphicsDeviceManager)
        {
            this.m_Game = i_Game;
            this.m_GraphicsDeviceManager = i_GraphicsDeviceManager;
            this.m_CollisionsManager = new CollisionsManager(i_Game);
            this.m_CollisionsManager.Initialize();
            InputManager inputManager = new InputManager(i_Game);
            inputManager.Initialize();
            m_Game.Services.AddService(typeof(SpriteBatch), i_SpriteBatch);

            this.r_Background = new Background(i_Game, k_BackgroundAssetName, 0, 0, k_BackgroundPercentage);
            this.m_GraphicsDeviceManager.PreferredBackBufferWidth = (int)(r_Background.Width * r_Background.BackgroundPercentage);
            this.m_GraphicsDeviceManager.PreferredBackBufferHeight = (int)(r_Background.Height * r_Background.BackgroundPercentage);
            this.m_GraphicsDeviceManager.ApplyChanges();
            this.m_EnemiesMatrix = new EnemiesMatrix(i_Game);
            this.m_MotherShip = new MotherShip(i_Game, 0, 0, Vector2.Zero, SpaceInvadersEnums.eEnemyType.MotherShipEnemy);
            this.m_FirstPlayer = new SpaceInvaderPlayer(i_Game, SpaceInvadersEnums.ePlayerType.FirstPlayer);
            this.m_SecondPlayer = new SpaceInvaderPlayer(i_Game, SpaceInvadersEnums.ePlayerType.SecondPlayer);
            this.m_CurrentLivePlayer = 2;
            initialize();
        }

        private void initialize()
        {
            this.m_EnemiesMatrix.Initialize();
            this.m_FirstPlayer.Initialize();
            this.m_SecondPlayer.Initialize();
            this.m_Barriers = new Barriers(m_Game, m_FirstPlayer.Spaceship.Position.Y - m_FirstPlayer.Spaceship.GapSpaceShipFromBottom);
            this.m_FirstPlayer.GameEnded += playerOutOfGame;
            this.m_SecondPlayer.GameEnded += playerOutOfGame;
            this.m_EnemiesMatrix.GameEnded += playerOutOfGame;
            this.m_EnemiesMatrix.EnemiesYUpdate += m_FirstPlayer.isEnemiesArriveToSpaceship;
            this.m_EnemiesMatrix.EnemiesYUpdate += m_SecondPlayer.isEnemiesArriveToSpaceship;
        }

        private string getEndGameString()
        {
            string endOfGameString = null;

            if (!isGameEndOnWin())
            {
                endOfGameString = string.Format("{0}{1}{2}{3}{4}{5}{6}", k_LostMessage, Environment.NewLine, m_FirstPlayer.PlayerScoreString(), Environment.NewLine, m_SecondPlayer.PlayerScoreString(), Environment.NewLine, generateWinnerString());
            }
            else
            {
                endOfGameString = string.Format("{0}{1}{2}{3}{4}{5}{6}", k_WinMessage, Environment.NewLine, m_FirstPlayer.PlayerScoreString(), Environment.NewLine, m_SecondPlayer.PlayerScoreString(), Environment.NewLine, generateWinnerString());
            }

            return endOfGameString;
        }

        private string generateWinnerString()
        {
            string resultString=null;
            if (m_FirstPlayer.SpaceShipScore > m_SecondPlayer.SpaceShipScore)
            {
                resultString = string.Format("{0} {1}", k_WinnerString, m_FirstPlayer.Player.PlayerName);
            }

            if (m_FirstPlayer.SpaceShipScore < m_SecondPlayer.SpaceShipScore)
            {
                resultString = string.Format("{0} {1}", k_WinnerString, m_SecondPlayer.Player.PlayerName);
            }

            if (m_FirstPlayer.SpaceShipScore == m_SecondPlayer.SpaceShipScore)
            {
                resultString = string.Format("{0}", k_TieString);
            }

            return resultString;
        }

        private bool isGameEndOnWin()
        {
            return m_EnemiesMatrix.LiveEnemiesList.Count == 0;
        }

        private void playerOutOfGame()
        {
            this.m_CurrentLivePlayer--;
            if (this.m_CurrentLivePlayer == 0)
            {
                gameEndedGenerateMessageBox();
            }
        }

        private void gameEndedGenerateMessageBox()
        {
            if (MessageBox.Show(getEndGameString(), k_MessageBoxTitle, MessageBoxButtons.OK) == DialogResult.OK)
            {
                m_Game.Exit();
            }
        }
    }
}
