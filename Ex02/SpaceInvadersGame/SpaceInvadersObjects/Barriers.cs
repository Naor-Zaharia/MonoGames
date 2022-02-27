using System.Collections.Generic;
using Infrastructure.GamesObjects.ObjectModel;
using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;

namespace SpaceInvadersGame.SpaceInvadersObjects
{
    internal class Barriers : GroupOfSpriteMovement
    {
        private const float k_XVelocityOfGroup = 35f;
        private const float k_GapPercentage = 1.3f;
        private const float k_Half = 0.5f;
        private const int k_AmountOfBarriers = 4;     
        private float m_YPosition;
        private float m_BarrierWidth;

        internal Barriers(Game i_Game, float i_YPosition) : base(i_Game)
        {
            this.m_YPosition = i_YPosition;
            createBarriers();
            initialize();
            i_Game.Components.Add(this);
        }

        private void initialize()
        {
            UpdateHighestXSpriteOnGroup();
            UpdateLowestXSpriteOnGroup();
        }

        private void createBarriers()
        {
            Barrier currentBarrier;
            float xInitPosition;

            m_GroupOfSprite = new List<Sprite>();
            for (int i = 0; i < k_AmountOfBarriers; i++)
            {
                currentBarrier = new Barrier(Game, 0, 0, Vector2.Zero);
                currentBarrier.Initialize();
                m_BarrierWidth = currentBarrier.Width;
                xInitPosition = getXInitPositionForBarriers();
                currentBarrier.Position = new Vector2((i * m_BarrierWidth * k_GapPercentage) + (i * m_BarrierWidth) + xInitPosition, m_YPosition - m_BarrierWidth);
                m_GroupOfSprite.Add(currentBarrier);
            }
        }

        public override void Update(GameTime i_GameTime)
        {
            float leftBound = getXInitPositionForBarriers() - (m_BarrierWidth * k_Half);
            float rightBound = Game.GraphicsDevice.Viewport.Width - getXInitPositionForBarriers() + (m_BarrierWidth * k_Half);
            UpdateXGroupPosition(i_GameTime, k_XVelocityOfGroup * (float)i_GameTime.ElapsedGameTime.TotalSeconds, rightBound, leftBound);
        }

        private float getXInitPositionForBarriers()
        {
            return (Game.GraphicsDevice.Viewport.Width * k_Half) - (((k_AmountOfBarriers * m_BarrierWidth) + ((k_AmountOfBarriers - 1) * m_BarrierWidth * k_GapPercentage)) * k_Half);
        }
    }
}
