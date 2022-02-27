using Microsoft.Xna.Framework;
using GamesObjects;

namespace SpaceInvadersGame
{
    public abstract class Bullet : Sprite
    {
        protected const float k_BulletVelocity = 140;

        public Bullet(Game i_Game, string i_AssetName, int i_DrawOrder, int i_UpdateOrder)
            : base(i_Game, i_AssetName, i_DrawOrder, i_UpdateOrder)
        {
            Initialize();
        }

        public abstract override void Update(GameTime i_GameTime);

        internal override void UpdateYPosition(float i_UpdateYValue)
        {
            this.Position = new Vector2(Position.X, Position.Y + i_UpdateYValue);
        }
    }
}
