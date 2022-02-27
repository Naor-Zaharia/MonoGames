using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SpaceInvadersGame.SpaceInvadersObjects;

namespace SpaceInvadersGame
{
    internal class EnemiesMatrix : GameComponent
    {
        private const int k_NumOfRowsTopEnemy = 1;
        private const int k_NumOfRowsMiddleEnemy = 2;
        private const int k_NumOfRowsLowEnemy = 2;        
        private const int k_MatrixWidth = 9;        
        private const int k_TopEnemyValue = 300;
        private const int k_MiddleEnemyValue = 200;
        private const int k_LowEnemyValue = 70;
        private const int k_RandomIntervalForEnemyShots = 3;
        private const int k_InitAmountOfKillsForNextAccelerate = 5;
        private const int k_MaximalAmountOfEnemyBulletsAtOnce = 5;
        private const int k_GapEnemyFromTop = 3;
        private const float k_KillAcceleratePercentage = 0.03f;
        private const float k_AcceleratePercentage = 0.06f;        
        private const float k_GapEnemyFromEnemy = 0.6f;
        private const float k_InitEnemiesVelocity = 60;
        private const string k_TopEnemyAssetName = @"Sprites\Enemy0101_32x32";
        private const string k_MiddleEnemyAssetName = @"Sprites\Enemy0201_32x32";
        private const string k_LowEnemyAssetName = @"Sprites\Enemy0301_32x32";
        private const string k_BulletAssetName = @"Sprites\Bullet";

        private static readonly Random sr_Random = new Random();
        private byte m_AmountOfKillsForNextAccelerate;
        private float m_SecondsForNextShot;
        private float m_ElapsedTime;
        private List<Enemy> m_LiveEnemiesList;
        private List<EnemyBullet> m_EnemiesBulletList;
        private float m_EnemiesVelocity;

        public EnemiesMatrix(Game i_Game)
            : base(i_Game)
        {
            this.m_LiveEnemiesList = new List<Enemy>();
            this.m_EnemiesBulletList = new List<EnemyBullet>();
            this.m_EnemiesVelocity = k_InitEnemiesVelocity;
            this.m_AmountOfKillsForNextAccelerate = k_InitAmountOfKillsForNextAccelerate;
            this.m_ElapsedTime = 0;
            generateNewTimeForNextShot();
            initializePosition();
            i_Game.Components.Add(this);
        }

        private void initializePosition()
        {
            addEnemies(0, k_NumOfRowsTopEnemy, k_TopEnemyAssetName, Color.Pink, k_TopEnemyValue);
            addEnemies(k_NumOfRowsTopEnemy, k_NumOfRowsMiddleEnemy, k_MiddleEnemyAssetName, Color.Azure, k_MiddleEnemyValue);
            addEnemies(k_NumOfRowsTopEnemy + k_NumOfRowsMiddleEnemy, k_NumOfRowsLowEnemy, k_LowEnemyAssetName, Color.LightYellow, k_LowEnemyValue);
        }

        public override void Update(GameTime i_GameTime)
        {
            updateEnemyMatrixPosition(i_GameTime);
            m_ElapsedTime += (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            // Check if needed that enemy shoot
            if (m_SecondsForNextShot <= m_ElapsedTime)
            {
                bulletShoots(i_GameTime, generateEnemyForShot());
                generateNewTimeForNextShot();
            }

            updateLiveEnemiesList();
            EnemiesBulletsUpdate(i_GameTime);
            if (m_AmountOfKillsForNextAccelerate == 0)
            {
                accelerateEnemies(k_KillAcceleratePercentage);
                m_AmountOfKillsForNextAccelerate = k_InitAmountOfKillsForNextAccelerate;
            }
        }

        internal void EnemiesBulletsUpdate(GameTime i_GameTime)
        {
            for (int i = 0; i < m_EnemiesBulletList.Count; i++)
            {
                EnemyBullet currentBullet = m_EnemiesBulletList[i];
                if (currentBullet.Position.Y > Game.GraphicsDevice.Viewport.Height || !currentBullet.Visible)
                {
                    m_EnemiesBulletList.Remove(currentBullet);
                    Game.Components.Remove(currentBullet);
                    currentBullet.Visible = false;
                }
            }
        }

        private void addEnemies(int i_StartRowIndex, int i_NumOfRows, string i_AssetName, Color i_EnemyColor, int i_EnemyValue)
        {
            for (int i = 0; i < i_NumOfRows; i++)
            {
                for (int j = 0; j < k_MatrixWidth; j++)
                {
                    Enemy currentEnemy = new Enemy(this.Game, i_AssetName, 0, 0, Vector2.Zero, i_EnemyColor, i_EnemyValue);
                    currentEnemy.Initialize();
                    Game.Components.Add(currentEnemy);
                    float enemyWidth = currentEnemy.Width;
                    float enemyHeight = currentEnemy.Height;

                    // Calculate init position of enemy before indentation
                    float initEnemyXPosition = (enemyWidth * k_GapEnemyFromEnemy) + enemyWidth;
                    float initEnemyYPosition = (enemyHeight * k_GapEnemyFromEnemy) + enemyHeight;
                    Vector2 enemyPosition = new Vector2(j * initEnemyXPosition, ((i + i_StartRowIndex) * initEnemyYPosition) + (k_GapEnemyFromTop * enemyHeight));
                    currentEnemy.Position = enemyPosition;
                    m_LiveEnemiesList.Add(currentEnemy);
                }
            }
        }

        private void bulletShoots(GameTime i_GameTime, Enemy i_Enemy)
        {
            if (m_EnemiesBulletList.Count < k_MaximalAmountOfEnemyBulletsAtOnce)
            {
                EnemyBullet currentEnemyBullet = new EnemyBullet(this.Game, k_BulletAssetName, 0, 0);

                // Find bullet position according to the enemy position
                float bulletXIndex = i_Enemy.Position.X + (i_Enemy.Width / 2.0f);
                float bulletYIndex = i_Enemy.Position.Y + i_Enemy.Height;
                currentEnemyBullet.Position = new Vector2(bulletXIndex, bulletYIndex);
                m_EnemiesBulletList.Add(currentEnemyBullet);
                Game.Components.Add(currentEnemyBullet);
            }
        }

        private Enemy generateEnemyForShot()
        {
            int randomEnemyIndex = sr_Random.Next(0, m_LiveEnemiesList.Count);
            return m_LiveEnemiesList[randomEnemyIndex];
        }

        private void generateNewTimeForNextShot()
        {
            this.m_SecondsForNextShot = sr_Random.Next(0, k_RandomIntervalForEnemyShots);
            m_ElapsedTime = 0;
        }

        private void updateEnemyMatrixPosition(GameTime i_GameTime)
        {
            // Check if the enemies matrix touch the bounds
            if (getRightestEnemyXPosition() >= Game.GraphicsDevice.Viewport.Width || getLeftestEnemyXPosition() <= 0)
            {
                updateYPositionOfEnemies();
            }

            updateXPositionOfEnemies((float)i_GameTime.ElapsedGameTime.TotalSeconds);
        }

        private void updateXPositionOfEnemies(float ElapsedGameTime)
        {
            foreach (Enemy currentEnemy in m_LiveEnemiesList)
            {
                currentEnemy.UpdateXPosition(m_EnemiesVelocity * ElapsedGameTime);
            }
        }

        private void updateYPositionOfEnemies()
        {
            m_EnemiesVelocity *= -1;
            accelerateEnemies(k_AcceleratePercentage);
            for (int i = 0; i < m_LiveEnemiesList.Count; i++)
            {
                float enemyHeight = m_LiveEnemiesList[i].Height;
                m_LiveEnemiesList[i].UpdateYPosition(enemyHeight / 2);
            }
        }

        private void updateLiveEnemiesList()
        {
            for (int i = 0; i < m_LiveEnemiesList.Count; i++)
            {
                Enemy currentEnemy = m_LiveEnemiesList[i];

                // Check if enemy is not alive
                if (!currentEnemy.Visible)
                {
                    m_LiveEnemiesList.Remove(currentEnemy);
                    m_AmountOfKillsForNextAccelerate--;
                }
            }
        }

        private void accelerateEnemies(float i_AcceleratePercentage)
        {
            m_EnemiesVelocity = (i_AcceleratePercentage * m_EnemiesVelocity) + m_EnemiesVelocity;
        }

        private float getRightestEnemyXPosition()
        {
            float rightestEnemyPosition = 0;
            for (int i = 0; i < m_LiveEnemiesList.Count; i++)
            {
                Enemy currentEnemy = m_LiveEnemiesList[i];
                if (currentEnemy.Position.X > rightestEnemyPosition)
                {   // Find the rightest enemy X
                    rightestEnemyPosition = currentEnemy.Position.X + currentEnemy.Width;
                }
            }

            return rightestEnemyPosition;
        }

        private float getLeftestEnemyXPosition()
        {
            float leftestEnemyPosition = Game.GraphicsDevice.Viewport.Width;
            for (int i = 0; i < m_LiveEnemiesList.Count; i++)
            {
                Enemy currentEnemy = m_LiveEnemiesList[i];
                if (currentEnemy.Position.X < leftestEnemyPosition)
                {
                    leftestEnemyPosition = currentEnemy.Position.X;
                }
            }

            return leftestEnemyPosition;
        }

        internal float GetHighestEnemyYPosition()
        {
            float highestYEnemyPosition = 0;
            for (int i = 0; i < m_LiveEnemiesList.Count; i++)
            {
                Enemy currentEnemy = m_LiveEnemiesList[i];
                if (currentEnemy.Position.Y > highestYEnemyPosition)
                {
                    // Find the bottom Y of enemy
                    highestYEnemyPosition = currentEnemy.Position.Y + currentEnemy.Height;
                }
            }

            return highestYEnemyPosition;
        }

        internal List<Enemy> LiveEnemiesList
        {
            get
            {
                return m_LiveEnemiesList;
            }
        }

        internal List<EnemyBullet> EnemiesBulletList
        {
            get
            {
                return m_EnemiesBulletList;
            }
        }
    }
}
