using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.GamesObjects.Managers;
using Infrastructure.GamesObjects.ServiceInterfaces;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Infrastructure.ServiceInterfaces;

namespace SpaceInvadersGame.SpaceInvadersObjects
{
    internal class Barrier : Sprite, ICollidable2D
    {
        private const string k_BarrierAssetName = @"Sprites\Barrier_44x32";
        private const string k_BarrierHitSoundName = "BarrierHit";
        private const float k_PrecentageOfHit = 0.35f;
        private const float k_BarrierVelocity = 35;
        private readonly SoundManager r_SoundManager;

        internal Barrier(GameScreen i_GameScreen, int i_DrawOrder, int i_UpdateOrder, Vector2 i_BarrierPosition)
            : base(k_BarrierAssetName, i_GameScreen.Game, i_DrawOrder, i_UpdateOrder)
        {
            this.Position = i_BarrierPosition;
            this.Velocity = new Vector2(k_BarrierVelocity, 0);
            this.Enabled = false;
            this.r_SoundManager = (SoundManager)Game.Services.GetService(typeof(ISoundManager));
            i_GameScreen.Add(this);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Texture = new Texture2D(Game.GraphicsDevice, Texture.Width, Texture.Height);
            Texture.SetData<Color>(Pixels);
            Pixels = new Color[Texture.Width * Texture.Height];
            Texture.GetData<Color>(Pixels);
        }

        public override void Collided(ICollidable i_Collidable)
        {
            bool isPerPixelCollided = false;
            Rectangle pointOfCollision = this.CheckPerPixelCollision(i_Collidable, ref isPerPixelCollided);
            Bullet bullet = i_Collidable as Bullet;

            if (isPerPixelCollided)
            {
                int x = (int)pointOfCollision.X;
                int y = (int)pointOfCollision.Y;

                if (bullet != null && !(i_Collidable as Bullet).IsBottomUp())
                {
                    bullet.ReleaseBusyBullet();
                    this.SetTransparentPixelsOnSprite(new Rectangle(x, y, (int)bullet.Width, (int)Math.Floor(bullet.Height * k_PrecentageOfHit)));
                }

                if (bullet != null && (i_Collidable as Bullet).IsBottomUp())
                {
                    bullet.ReleaseBusyBullet();
                    this.SetTransparentPixelsOnSprite(new Rectangle(x, y - (int)Math.Floor(bullet.Height * k_PrecentageOfHit), (int)bullet.Width, (int)Math.Ceiling(bullet.Height * k_PrecentageOfHit)));
                }

                if (i_Collidable is Bullet)
                {
                    r_SoundManager.PlaySound(k_BarrierHitSoundName);
                }

                if (i_Collidable is Enemy)
                {
                    this.SetTransparentPixelsOnSprite(this.GetIntersectionRectangle(i_Collidable));
                }
            }
        }
    }
}