using System;
using Microsoft.Xna.Framework;
using SpaceInvadersGame.SpaceInvadersObjects;

namespace SpaceInvadersGame
{
    internal class SpaceInvadersGameManager
    {
        private const string k_LostMessage = "You Lost !!!";
        private const string k_ScoreMessage = "Your score is:";
        private const string k_WinMessage = "You Won !!!";
        private const string k_MotherShipAssetName = @"Sprites\MotherShip_32x120";
        private const string k_BackgroundAssetName = @"Background\BG_Space01_1024x768";
        private const string k_SpaceShipAssetName = @"Sprites\Ship01_32x32";
        private const int k_MotherShipValue = 600;
        private const float k_BackgroundPercentage = 0.8f;
        private const int k_PenaltyOnSpaceShipHit = 600;

        private readonly Background r_Background;
        private EnemiesMatrix m_EnemiesMatrix;
        private MotherShip m_MotherShip;
        private SpaceShip m_SpaceShip;       
        private GraphicsDeviceManager m_GraphicsDeviceManager;
        private Game m_Game;
        private int m_Score;

        public SpaceInvadersGameManager(Game i_Game, GraphicsDeviceManager i_GraphicsDeviceManager)
        {
            this.m_Game = i_Game;
            this.m_GraphicsDeviceManager = i_GraphicsDeviceManager;
            this.r_Background = new Background(i_Game, k_BackgroundAssetName, 0, 0);
            i_Game.Components.Add(r_Background);
            r_Background.Initialize();
            m_GraphicsDeviceManager.PreferredBackBufferWidth = (int)(r_Background.Width * k_BackgroundPercentage);
            m_GraphicsDeviceManager.PreferredBackBufferHeight = (int)(r_Background.Height * k_BackgroundPercentage);            
            m_GraphicsDeviceManager.ApplyChanges();
            m_EnemiesMatrix = new EnemiesMatrix(i_Game);
            m_EnemiesMatrix.Initialize();
            m_MotherShip = new MotherShip(i_Game, k_MotherShipAssetName, 0, 0, Vector2.Zero, Color.Red, k_MotherShipValue);
            m_MotherShip.Initialize();
            m_SpaceShip = new SpaceShip(i_Game, k_SpaceShipAssetName, 0, 0);
            m_SpaceShip.Initialize();
            this.m_Score = 0;
        }

        internal void Update(GameTime i_GameTime)
        {
            if (!IsGameEnd())
            {               
                checkForEnemiesHit();
                checkForSpaceShipHit();
            }
        }

        internal bool IsGameEnd()
        {
            return isGameEndOnLost() || isGameEndOnWin();
        }

        internal string GetEndGameString()
        {
            string endOfGameString = null;

            if (isGameEndOnLost())
            {
                endOfGameString = string.Format("{0}{1}{2} {3}", k_LostMessage, Environment.NewLine, k_ScoreMessage, m_Score.ToString());
            }
            else
            {
                endOfGameString = string.Format("{0}{1}{2} {3}", k_WinMessage, Environment.NewLine, k_ScoreMessage, m_Score.ToString());
            }

            return endOfGameString;
        }

        private void checkForSpaceShipHit()
        {
            foreach (EnemyBullet currentBullet in m_EnemiesMatrix.EnemiesBulletList)
            {
                if (currentBullet.Rectangle.Intersects(m_SpaceShip.Rectangle))
                {
                    m_SpaceShip.SpaceShipGetHit();
                    currentBullet.Visible = false;
                    if (m_Score - k_PenaltyOnSpaceShipHit < 0)
                    {
                        m_Score = 0;
                    }
                    else
                    {
                        m_Score -= k_PenaltyOnSpaceShipHit;
                    }
                }
            }
        }

        private void checkForEnemiesHit()
        {
            foreach (SpaceShipBullet currentBullet in m_SpaceShip.SpaceShipBulletList)
            {
                foreach (Enemy currentEnemy in m_EnemiesMatrix.LiveEnemiesList)
                {
                    if (currentEnemy.Rectangle.Intersects(currentBullet.Rectangle))
                    {
                        currentEnemy.Visible = false;
                        currentBullet.Visible = false;
                        m_Score += currentEnemy.EnemyValue;
                        m_Game.Components.Remove(currentEnemy);
                        m_Game.Components.Remove(currentBullet);
                    }
                }

                if (m_MotherShip.Rectangle.Intersects(currentBullet.Rectangle) && m_MotherShip.Visible && currentBullet.Visible)
                {
                    m_Score += m_MotherShip.EnemyValue;
                    currentBullet.Visible = false;
                    m_MotherShip.Visible = false;
                    m_Game.Components.Remove(currentBullet);
                }
            }
        }

        private bool isGameEndOnWin()
        {
            return m_EnemiesMatrix.LiveEnemiesList.Count == 0;
        }

        private bool isGameEndOnLost()
        {
            return isSpaceShipDied() || isEnemyTouchSpaceShip() | isEnemiesArriveDown();
        }

        private bool isSpaceShipDied()
        {
            return m_SpaceShip.RemainingLife == 0;
        }

        private bool isEnemyTouchSpaceShip()
        {
            bool isEnemyTouchSpaceShip = false;
            foreach (Enemy currentEnemy in m_EnemiesMatrix.LiveEnemiesList)
            {
                if (currentEnemy.Rectangle.Intersects(m_SpaceShip.Rectangle))
                {
                    isEnemyTouchSpaceShip = true;
                }
            }

            return isEnemyTouchSpaceShip;
        }

        private bool isEnemiesArriveDown()
        {
            return m_EnemiesMatrix.GetHighestEnemyYPosition() >= m_Game.GraphicsDevice.Viewport.Height;
        }
    }
}
