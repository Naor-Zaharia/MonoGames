using Microsoft.Xna.Framework;
using System;

namespace Infrastructure.GamesObjects.ObjectModel.Animators.ConcreteAnimators
{
    public class RotatorAnimator : SpriteAnimator
    {
        private readonly TimeSpan r_RotateTime;        

        public RotatorAnimator(TimeSpan i_AnimationLength, TimeSpan i_RotateTime) : base("Rotator", i_AnimationLength)
        {
            this.r_RotateTime = i_RotateTime;
        }

        public RotatorAnimator(TimeSpan i_AnimationLength, TimeSpan i_RotateTime, string i_AnimationName) : base(i_AnimationName, i_AnimationLength)
        {
            this.r_RotateTime = i_RotateTime;
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Rotation = m_OriginalSpriteInfo.Rotation;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            this.BoundSprite.Rotation += (float)((r_RotateTime.TotalSeconds / MathHelper.TwoPi) * i_GameTime.TotalGameTime.TotalSeconds);            
        }
    }
}
