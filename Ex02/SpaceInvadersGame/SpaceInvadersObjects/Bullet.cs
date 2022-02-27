using System;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using SpaceInvadersGame.SpaceInvadersObjects;

namespace SpaceInvadersGame
{
    internal class Bullet : Sprite, ICollidable2D
    {
        private static readonly Random sr_Random = new Random();
        protected const float k_BulletVelocity = 140;
        private Sprite m_BulletOwner;
        private int m_BulletDirection = 1;

        internal Bullet(Game i_Game, string i_AssetName, int i_DrawOrder, int i_UpdateOrder, bool i_IsBulletBottomUp)
            : base(i_AssetName, i_Game, i_DrawOrder, i_UpdateOrder)
        {
            if (i_IsBulletBottomUp)
            {
                this.m_BulletDirection = -1;
                this.m_TintColor = Color.Red;
            }
            else
            {
                this.m_TintColor = Color.Blue;
            }

            this.Velocity = new Vector2(0, k_BulletVelocity * m_BulletDirection);
            Initialize();
        }

        public override void Update(GameTime i_GameTime)
        {
            releaseBulletsOutOfFrame(i_GameTime);
            base.Update(i_GameTime);
        }

        public override void Collided(ICollidable i_Collidable)
        {
            isCollidedWithEnemyOrSpaceShip(i_Collidable);
            isCollidedEnemyBulletWithSpaceShipBullet(i_Collidable);
        }

        private void isCollidedWithEnemyOrSpaceShip(ICollidable i_Collidable)
        {
            bool isPerPixelCollided = false;
            this.CheckPerPixelCollision(i_Collidable, ref isPerPixelCollided);
            if ((i_Collidable is Enemy && IsBottomUp()) || (i_Collidable is Spaceship && !IsBottomUp()) || (i_Collidable is Barrier && isPerPixelCollided))
            {
                releaseBusyBullet();
            }
        }

        internal bool IsBottomUp()
        {
            return m_BulletDirection == -1;
        }

        internal Sprite BulletOwner
        {
            get
            {
                return m_BulletOwner;
            }

            set
            {
                m_BulletOwner = value;
            }
        }

        private void isCollidedEnemyBulletWithSpaceShipBullet(ICollidable i_Collidable)
        {
            Bullet bullet = i_Collidable as Bullet;
            if ((bullet != null) && this.IsBottomUp() && !bullet.IsBottomUp())
            {
                int RandomCoin = sr_Random.Next(0, 2);
                if (RandomCoin == 1)
                {
                    bullet.releaseBusyBullet();
                }

                this.releaseBusyBullet();
            }
        }

        private void releaseBulletsOutOfFrame(GameTime i_GameTime)
        {
            bool isEnemyBulletOutOfFrame = !this.IsBottomUp() && this.Position.Y > Game.GraphicsDevice.Viewport.Height;
            bool isSpaceshipBulletOutOfFrame = this.IsBottomUp() && this.Position.Y + Height < 0;

            if (isEnemyBulletOutOfFrame || isSpaceshipBulletOutOfFrame)
            {
                releaseBusyBullet();
            }
        }

        private void releaseBusyBullet()
        {
            this.Visible = false;
            this.Enabled = false;
        }
    }
}
