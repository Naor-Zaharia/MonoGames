using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Infrastructure.GamesObjects.ObjectModel
{
    public abstract class GroupOfSpriteMovement : CompositeDrawableComponent<Sprite>
    {
        protected List<Sprite> m_GroupOfSprite;
        protected int m_Direction;
        protected Sprite m_HighestXSprite;
        protected Sprite m_LowestXSprite;
        protected Sprite m_HighestYSprite;        

        public GroupOfSpriteMovement(Game i_Game)
            : base(i_Game)
        {
            this.m_Direction = 1;
        }

        protected virtual void UpdateXGroupOfSpritePosition(GameTime i_GameTime,float i_JumpSize)
        {          
                foreach (Sprite sprite in m_GroupOfSprite)
                {
                    sprite.UpdateXPosition(i_JumpSize * m_Direction);
                }            
        }

        protected virtual void UpdateYPositionOfSpritePosition(float i_JumpSize)
        {                  
            for (int i = 0; i < m_GroupOfSprite.Count; i++)
            {                
                m_GroupOfSprite[i].UpdateYPosition(i_JumpSize);
            }
        }


        protected virtual void UpdateHighestXSpriteOnGroup()
        {
            Sprite rightestSprite = getFirstVisibleSpriteFromGroup();
            for (int i = 0; i < m_GroupOfSprite.Count && rightestSprite!=null; i++)
            {
                Sprite currentSprite = m_GroupOfSprite[i];
                if (currentSprite.Visible && currentSprite.Position.X > rightestSprite.Position.X)
                {   // Find the rightest sprite X
                    rightestSprite = currentSprite;
                }
            }

            this.m_HighestXSprite = rightestSprite;
        }

        private Sprite getFirstVisibleSpriteFromGroup()
        {
            Sprite visibleSprite = null;
            for (int i = 0; i < m_GroupOfSprite.Count; i++)
            {
                if (m_GroupOfSprite[i].Visible)
                {
                    visibleSprite = m_GroupOfSprite[i];
                    break;
                }
            }

            return visibleSprite;
        }

        protected virtual void UpdateLowestXSpriteOnGroup()
        {
            Sprite leftestSprite = getFirstVisibleSpriteFromGroup();
            for (int i = 0; i < m_GroupOfSprite.Count && leftestSprite != null; i++)
            {
                Sprite currentSprite = m_GroupOfSprite[i];
                if (currentSprite.Visible && currentSprite.Position.X < leftestSprite.Position.X)
                {   // Find the leftest sprite X
                    leftestSprite = currentSprite;
                }
            }

            this.m_LowestXSprite = leftestSprite;
        }

        protected virtual void UpdateHighestYSpriteOnGroup()
        {
            Sprite lowestSpritePosition = getFirstVisibleSpriteFromGroup();
            for (int i = 0; i < m_GroupOfSprite.Count && lowestSpritePosition!=null; i++)
            {
                Sprite currentSprite = m_GroupOfSprite[i];
                if (currentSprite.Visible && currentSprite.Position.Y > lowestSpritePosition.Position.Y)
                {
                    // Find the bottom Y of sprite
                    lowestSpritePosition = currentSprite;
                }
            }

            this.m_HighestYSprite = lowestSpritePosition;
        }

        protected virtual float CalculateJumpSize(float i_JumpSize,float i_RightBound,float i_LeftBound)
        {
            // Check if the sprites group touch the bounds
            if (m_Direction == 1 && m_HighestXSprite.Position.X + i_JumpSize > i_RightBound - m_HighestXSprite.Width)
            {
                i_JumpSize = (i_RightBound - m_HighestXSprite.Width) - (this.m_HighestXSprite.Position.X);
            }

            if (m_Direction == -1 && m_LowestXSprite.Position.X - i_JumpSize < i_LeftBound)
            {
                i_JumpSize = m_LowestXSprite.Position.X- i_LeftBound;
            }

            return i_JumpSize;
        }

        protected virtual void UpdateXGroupPosition(GameTime i_GameTime, float i_JumpSize, float i_RightBound, float i_LeftBound)
        {
            // Check if the sprites group touch the bounds            
            if ((m_Direction == 1 && m_HighestXSprite.Position.X + m_HighestXSprite.Width >= i_RightBound)
                   || (m_Direction == -1 && m_LowestXSprite.Position.X <= i_LeftBound))
            {
                m_Direction *= -1;                
            }

            UpdateXGroupOfSpritePosition(i_GameTime, CalculateJumpSize(i_JumpSize,i_RightBound,i_LeftBound));
        }

        protected virtual bool IsUpdateYGroupPosition(GameTime i_GameTime,float i_JumpSize)
        {
            bool isUpdatedYPosition = false;
            if ((m_Direction == 1 && m_HighestXSprite.Position.X + m_HighestXSprite.Width >= Game.GraphicsDevice.Viewport.Width)
                  || (m_Direction == -1 && m_LowestXSprite.Position.X <= 0))
            {                
                UpdateYPositionOfSpritePosition(i_JumpSize);
                isUpdatedYPosition = true;
            }

            return isUpdatedYPosition;
        }
    }
}
