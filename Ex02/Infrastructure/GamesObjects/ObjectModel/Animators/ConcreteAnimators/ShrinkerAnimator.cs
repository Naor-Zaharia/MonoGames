using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.GamesObjects.ObjectModel.Animators.ConcreteAnimators
{
    public class ShrinkerAnimator : SpriteAnimator
    {
        public ShrinkerAnimator(TimeSpan i_AnimationLength) : base("Shrinker", i_AnimationLength) 
        {
        }

        public ShrinkerAnimator(TimeSpan i_AnimationLength, string i_AnimationName) : base(i_AnimationName, i_AnimationLength)
        { 
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            this.BoundSprite.Scales -= (m_OriginalSpriteInfo.Scales / (float)AnimationLength.TotalSeconds) * (float)i_GameTime.ElapsedGameTime.TotalSeconds;
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Scales = this.m_OriginalSpriteInfo.Scales;
        }
    }
}
