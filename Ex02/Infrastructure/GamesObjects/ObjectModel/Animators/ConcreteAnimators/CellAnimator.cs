////*** Guy Ronen (c) 2008-2011 ***////
using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.GamesObjects.ObjectModel.Animators.ConcreteAnimators
{
    public class CellAnimator : SpriteAnimator
    {
        private readonly int r_NumOfCells = 1;
        private TimeSpan m_CellTime;
        private TimeSpan m_TimeLeftForCell;
        private bool m_Loop = true;
        private int m_CurrentCellIndex = 0;        

        public CellAnimator(ref TimeSpan i_CellTime, int i_NumOfCells, TimeSpan i_AnimationLength, string i_AnimationName) : base(i_AnimationName, i_AnimationLength)
        {
            this.m_CellTime = i_CellTime;
            this.m_TimeLeftForCell = i_CellTime;
            this.r_NumOfCells = i_NumOfCells;
            this.m_Loop = i_AnimationLength == TimeSpan.Zero;
        }

        private void goToNextFrame()
        {
            m_CurrentCellIndex++;
            if (m_CurrentCellIndex >= r_NumOfCells)
            {
                if (m_Loop)
                {
                    m_CurrentCellIndex %= r_NumOfCells;
                }
                else
                {
                    m_CurrentCellIndex = r_NumOfCells - 1; /// lets stop at the last frame
                    this.IsFinished = true;
                }
            }
        }

        public void InitFrame(int i_InitFrameIndex)
        {
            m_CurrentCellIndex = i_InitFrameIndex;
            this.BoundSprite.SourceRectangle = new Rectangle(
               this.BoundSprite.SourceRectangle.X,
               (m_CurrentCellIndex * this.BoundSprite.SourceRectangle.Height) + this.m_OriginalSpriteInfo.SourceRectangle.Y,
               this.BoundSprite.SourceRectangle.Width,
               this.BoundSprite.SourceRectangle.Height);
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.SourceRectangle = m_OriginalSpriteInfo.SourceRectangle;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            if (m_CellTime != TimeSpan.Zero)
            {
                m_TimeLeftForCell -= i_GameTime.ElapsedGameTime;
                if (m_TimeLeftForCell.TotalSeconds <= 0)
                {
                    goToNextFrame();
                    m_TimeLeftForCell = m_CellTime;
                }
            }

            this.BoundSprite.SourceRectangle = new Rectangle(
               this.BoundSprite.SourceRectangle.X,
               (m_CurrentCellIndex * this.BoundSprite.SourceRectangle.Height) + this.m_OriginalSpriteInfo.SourceRectangle.Y,
               this.BoundSprite.SourceRectangle.Width,
               this.BoundSprite.SourceRectangle.Height);
        }
    }
}
