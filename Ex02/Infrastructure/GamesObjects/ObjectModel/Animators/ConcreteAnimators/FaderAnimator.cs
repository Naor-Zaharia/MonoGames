using System;
using Microsoft.Xna.Framework;

namespace Infrastructure.GamesObjects.ObjectModel.Animators.ConcreteAnimators
{
    public class FaderAnimator : SpriteAnimator
    {
        public FaderAnimator(TimeSpan i_AnimationLength) : base("Fader", i_AnimationLength)
        { 
        }

        public FaderAnimator(TimeSpan i_AnimationLength, string i_AnimationName) : base(i_AnimationName, i_AnimationLength)
        { 
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Opacity = m_OriginalSpriteInfo.Opacity;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            float currentFade = this.BoundSprite.Opacity - ((float)(m_OriginalSpriteInfo.Opacity / AnimationLength.TotalSeconds) * (float)i_GameTime.ElapsedGameTime.TotalSeconds);
            this.BoundSprite.Opacity = MathHelper.Clamp(currentFade, 0, this.BoundSprite.Opacity);
        }
    }
}
