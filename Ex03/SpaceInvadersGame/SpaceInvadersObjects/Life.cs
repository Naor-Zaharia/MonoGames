using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;

namespace SpaceInvadersGame.SpaceInvadersObjects
{
    internal class Life : Sprite
    {
        private const float k_LifeOpacity = 0.5f;
        private const float k_LifeScale = 0.5f;

        internal Life(GameScreen i_GameScreen, string i_AssetName) : base(i_AssetName, i_GameScreen.Game)
        {
            this.m_AssetName = i_AssetName;
            i_GameScreen.Add(this);
        }

        public override void Initialize()
        {
            base.Initialize();
            this.Scales = new Vector2(k_LifeScale);
            this.Opacity = k_LifeOpacity;
            this.BlendState = BlendState.NonPremultiplied;
        }
    }
}
