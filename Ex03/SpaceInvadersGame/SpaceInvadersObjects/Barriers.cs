using System.Collections.Generic;
using Infrastructure.GamesObjects.ObjectModel;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using SpaceInvadersGame.SpaceInvadersGameServices;
using SpaceInvadersGame.SpaceInvadersGameServices.SpaceInvadersGameInterfaces;

namespace SpaceInvadersGame.SpaceInvadersObjects
{
    internal class Barriers : GroupOfSpriteMovement
    {
        private const int k_AmountOfBarriers = 4;
        private const float k_GapPercentage = 1.3f;
        private const float k_Half = 0.5f;
        private readonly SpaceInvaderGameManagerService r_GameManager;
        private float m_YPosition;
        private float m_BarrierWidth;
        private GameScreen m_GameScreen;        

        internal Barriers(GameScreen i_GameScreen, float i_YPosition) : base(i_GameScreen.Game)
        {
            this.m_GameScreen = i_GameScreen;
            this.m_YPosition = i_YPosition;
            this.r_GameManager = (SpaceInvaderGameManagerService)Game.Services.GetService(typeof(ISpaceInvaderGameManagerService));
            createBarriers();
            initialize();
            this.m_GameScreen.Add(this);
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
                currentBarrier = new Barrier(m_GameScreen, 0, 0, Vector2.Zero);
                currentBarrier.Initialize();
                m_BarrierWidth = currentBarrier.Width;
                xInitPosition = getXInitPositionForBarriers();
                currentBarrier.Position = new Vector2((i * m_BarrierWidth * k_GapPercentage) + (i * m_BarrierWidth) + xInitPosition, m_YPosition - m_BarrierWidth);
                m_GroupOfSprite.Add(currentBarrier);
            }
        }

        public void RebuildBarriers()
        {
            Dispose();
            createBarriers();
            initialize();
        }

        public override void Update(GameTime i_GameTime)
        {
            float leftBound = getXInitPositionForBarriers() - (m_BarrierWidth * k_Half);
            float rightBound = Game.GraphicsDevice.Viewport.Width - getXInitPositionForBarriers() + (m_BarrierWidth * k_Half);
            UpdateXGroupPosition(i_GameTime, r_GameManager.XVelocityOfBarriersLevel * (float)i_GameTime.ElapsedGameTime.TotalSeconds, rightBound, leftBound);
        }

        private float getXInitPositionForBarriers()
        {
            return (Game.GraphicsDevice.Viewport.Width * k_Half) - (((k_AmountOfBarriers * m_BarrierWidth) + ((k_AmountOfBarriers - 1) * m_BarrierWidth * k_GapPercentage)) * k_Half);
        }

        protected override void Dispose(bool i_Disposing)
        {
            base.Dispose(i_Disposing);
            foreach (Barrier currentBarrier in m_GroupOfSprite)
            {
                m_GameScreen.Remove(currentBarrier);
                currentBarrier.Visible = false;
                currentBarrier.Dispose();
            }
        }
    }
}
