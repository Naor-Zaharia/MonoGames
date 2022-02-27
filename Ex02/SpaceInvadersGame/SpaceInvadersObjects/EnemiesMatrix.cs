using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SpaceInvadersGame.SpaceInvadersUtils;
using Infrastructure.GamesObjects.ObjectModel.Animators;
using Infrastructure.GamesObjects.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.GamesObjects.ObjectModel;
using Infrastructure.ObjectModel;

namespace SpaceInvadersGame
{
    internal class EnemiesMatrix : GroupOfSpriteMovement
    {
        public event GameEvents.GameEnd.GameEndEventHandler GameEnded = null;

        public event GameEvents.EnemiesYUpdate.EnemiesYUpdateEventHandler EnemiesYUpdate = null;

        private const string k_CellsAnimationName = "CellAnimator";
        private const float k_KillAcceleratePercentage = 0.03f;
        private const float k_AcceleratePercentage = 0.05f;
        private const float k_GapEnemyFromEnemy = 0.6f;
        private const float k_InitEnemiesVelocity = 60;
        private const int k_InitRandomIntervakForEnemyShots = 35;
        private const int k_AmountOfEnemyTypes = 6;
        private const int k_NumOfRowsTopEnemy = 1;
        private const int k_NumOfRowsMiddleEnemy = 2;
        private const int k_NumOfRowsLowEnemy = 2;
        private const int k_MatrixWidth = 9;
        private const int k_InitAmountOfKillsForNextAccelerate = 5;
        private const int k_GapEnemyFromTop = 3;
        private TimeSpan m_ElapsedTimeForNextEnemiesUpdate;
        private float m_ElapsedTimeForEnemiesUpdate;
        private float m_EnemyHeight;
        private float m_EnemyWidth;
        private int m_RandomIntervalForEnemyShots;
        private byte m_AmountOfKillsForNextAccelerate;

        internal EnemiesMatrix(Game i_Game)
            : base(i_Game)
        {
            this.m_GroupOfSprite = new List<Sprite>();
            this.m_AmountOfKillsForNextAccelerate = k_InitAmountOfKillsForNextAccelerate;
            this.m_Direction = 1;
            this.m_RandomIntervalForEnemyShots = k_InitRandomIntervakForEnemyShots + m_GroupOfSprite.Count; // Random for shooting releated to amount of eneimes
            this.m_ElapsedTimeForNextEnemiesUpdate = TimeSpan.FromSeconds(0.5f);
            initializePosition();
            i_Game.Components.Add(this);
        }

        private void initializePosition()
        {
            createEnemiesMatrix(0, k_NumOfRowsTopEnemy, SpaceInvadersEnums.eEnemyType.TopEnemy);
            createEnemiesMatrix(k_NumOfRowsTopEnemy, k_NumOfRowsMiddleEnemy, SpaceInvadersEnums.eEnemyType.MiddleEnemy);
            createEnemiesMatrix(k_NumOfRowsTopEnemy + k_NumOfRowsMiddleEnemy, k_NumOfRowsLowEnemy, SpaceInvadersEnums.eEnemyType.LowEnemy);
            UpdateHighestXSpriteOnGroup();
            UpdateLowestXSpriteOnGroup();
            UpdateHighestYSpriteOnGroup();
        }

        public override void Update(GameTime i_GameTime)
        {
            m_ElapsedTimeForEnemiesUpdate += (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            if (m_ElapsedTimeForEnemiesUpdate > m_ElapsedTimeForNextEnemiesUpdate.TotalSeconds && m_HighestXSprite != null)
            {
                UpdateXGroupPosition(i_GameTime, m_HighestXSprite.Width / 2, Game.GraphicsDevice.Viewport.Width, 0);
                if (IsUpdateYGroupPosition(i_GameTime, m_EnemyHeight / 2))
                {
                    accelerateEnemies(k_AcceleratePercentage);
                    onEnemiesYUpdate();
                }

                m_ElapsedTimeForEnemiesUpdate = (float)-m_ElapsedTimeForNextEnemiesUpdate.TotalSeconds;
            }

            if (m_AmountOfKillsForNextAccelerate == 0)
            {
                accelerateEnemies(k_KillAcceleratePercentage);
                m_AmountOfKillsForNextAccelerate = k_InitAmountOfKillsForNextAccelerate;
            }

            isGameEnded();
        }

        private void onGameEnded()
        {
            if (GameEnded != null)
            {
                GameEnded.Invoke();
            }
        }

        private void onEnemiesYUpdate()
        {
            if (EnemiesYUpdate != null)
            {
                EnemiesYUpdate.Invoke(this.HighestYLiveEnemy);
            }
        }

        private void isGameEnded()
        {
            isGameEndedOnWinning();
        }

        private void isGameEndedOnWinning()
        {
            if (this.m_GroupOfSprite.Count == 0)
            {
                onGameEnded();
            }
        }

        private void createEnemiesMatrix(int i_StartRowIndex, int i_NumOfRows, SpaceInvadersEnums.eEnemyType i_EnemyType)
        {
            Enemy currentEnemy = null;
            for (int i = 0; i < i_NumOfRows; i++)
            {
                for (int j = 0; j < k_MatrixWidth; j++)
                {
                    currentEnemy = new Enemy(this.Game, 0, 0, Vector2.Zero, i_EnemyType, ref m_RandomIntervalForEnemyShots);
                    float enemyWidth = currentEnemy.Width;
                    float enemyHeight = currentEnemy.Height / k_AmountOfEnemyTypes;
                    currentEnemy.Height = currentEnemy.Height / k_AmountOfEnemyTypes;
                    currentEnemy.EnemyDied += updateLiveEnemiesList;

                    // Calculate init position of enemy before indentation
                    float initEnemyXPosition = (enemyWidth * k_GapEnemyFromEnemy) + enemyWidth;
                    float initEnemyYPosition = (enemyHeight * k_GapEnemyFromEnemy) + enemyHeight;
                    Vector2 enemyPosition = new Vector2(j * initEnemyXPosition, ((i + i_StartRowIndex) * initEnemyYPosition) + (k_GapEnemyFromTop * enemyHeight));
                    currentEnemy.Position = enemyPosition;
                    m_GroupOfSprite.Add(currentEnemy);
                    SpriteAnimator cellsAnimation = new CellAnimator(ref m_ElapsedTimeForNextEnemiesUpdate, 2, TimeSpan.Zero, k_CellsAnimationName);
                    currentEnemy.Animations.Add(cellsAnimation);
                    currentEnemy.Animations.Initialize();
                    if (i % 2 != 0)
                    {
                        ((CellAnimator)currentEnemy.Animations[k_CellsAnimationName]).InitFrame(1);
                    }

                    currentEnemy.Animations.Resume();
                }
            }

            m_EnemyWidth = currentEnemy.Width;
            m_EnemyHeight = currentEnemy.Height;
        }

        private void updateLiveEnemiesList(Enemy i_Enemy)
        {
            m_GroupOfSprite.Remove(i_Enemy);
            this.m_RandomIntervalForEnemyShots = k_InitRandomIntervakForEnemyShots + m_GroupOfSprite.Count;
            m_AmountOfKillsForNextAccelerate--;
            if (i_Enemy == m_HighestXSprite)
            {
                UpdateHighestXSpriteOnGroup();
            }

            if (i_Enemy == m_LowestXSprite)
            {
                UpdateLowestXSpriteOnGroup();
            }

            if (i_Enemy == m_HighestYSprite)
            {
                UpdateHighestYSpriteOnGroup();
            }
        }

        private void accelerateEnemies(float i_AcceleratePercentage)
        {
            m_ElapsedTimeForNextEnemiesUpdate = m_ElapsedTimeForNextEnemiesUpdate - (i_AcceleratePercentage * m_ElapsedTimeForNextEnemiesUpdate);
        }

        internal List<Sprite> LiveEnemiesList
        {
            get
            {
                return m_GroupOfSprite;
            }
        }

        internal Enemy HighestYLiveEnemy
        {
            get
            {
                return (Enemy)m_HighestYSprite;
            }
        }
    }
}
