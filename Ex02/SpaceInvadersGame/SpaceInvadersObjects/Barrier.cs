using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvadersGame.SpaceInvadersObjects
{
    internal class Barrier : Sprite, ICollidable2D
    {
        private const string k_BarrierAssetName = @"Sprites\Barrier_44x32";
        private const float k_BarrierVelocity = 35;
        private const float k_BulletInsertToBarrierPowerPrecentage = 0.35f;

        internal Barrier(Game i_Game, int i_DrawOrder, int i_UpdateOrder, Vector2 i_BarrierPosition)
            : base(k_BarrierAssetName, i_Game, i_DrawOrder, i_UpdateOrder)
        {
            this.Position = i_BarrierPosition;
            this.Velocity = new Vector2(k_BarrierVelocity, 0);
            this.Enabled = false;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_Texture = new Texture2D(Game.GraphicsDevice, Texture.Width, Texture.Height);
            Texture.SetData<Color>(Pixels);
            Pixels = new Color[Texture.Width * Texture.Height];
            Texture.GetData<Color>(Pixels);
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is Bullet && !(i_Collidable as Bullet).IsBottomUp())
            {
                this.SetTransparentPixelsOnSprite(this.GetPartialIntersectRectanglePerPixelCollision(i_Collidable, false, k_BulletInsertToBarrierPowerPrecentage));
            }

            if (i_Collidable is Bullet && (i_Collidable as Bullet).IsBottomUp())
            {
                this.SetTransparentPixelsOnSprite(this.GetPartialIntersectRectanglePerPixelCollision(i_Collidable, true, k_BulletInsertToBarrierPowerPrecentage));
            }

            if (i_Collidable is Enemy)
            {
                bool isPerPixelCollided = false;
                Rectangle eraseRectangle = this.CheckPerPixelCollision(i_Collidable, ref isPerPixelCollided);
                if (isPerPixelCollided)
                {
                    this.SetTransparentPixelsOnSprite(eraseRectangle);
                }
            }
        }
    }
}