using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;

namespace SpaceInvadersGame.SpaceInvadersObjects
{
    internal class Life : Sprite
    {
        private const float k_LifeOpacity = 0.5f;
        private const float k_LifeScale = 0.5f;

        internal Life(Game i_Game, string i_AssetName) : base(i_AssetName, i_Game)
        {
            this.m_AssetName = i_AssetName;
        }

        public override void Initialize()
        {
            this.Scales = new Vector2(k_LifeScale);
            this.Opacity = k_LifeOpacity;
            base.Initialize();
        }
    }
}
