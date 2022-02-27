using System;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using Infrastructure.ServiceInterfaces;
using Infrastructure.ObjectModel.Screens;
using SpaceInvadersGame.SpaceInvadersUtils;

namespace SpaceInvadersGame
{
    internal class Bullet : Sprite, ICollidable2D
    {
        private static readonly Random sr_Random = new Random();
        protected const float k_BulletVelocity = 140;
        private GameScreen m_GameScreen;
        private Sprite m_BulletOwner;
        private SpaceInvadersEnums.eBulletType m_BulletType;
        private int m_BulletDirection = 1;

        internal Bullet(GameScreen i_GameScreen, string i_AssetName, int i_DrawOrder, int i_UpdateOrder, SpaceInvadersEnums.eBulletType i_BulletType)
            : base(i_AssetName, i_GameScreen.Game, i_DrawOrder, i_UpdateOrder)
        {
            if (i_BulletType == SpaceInvadersEnums.eBulletType.SpaceshipBullet)
            {
                this.m_BulletDirection = -1;
                this.m_TintColor = Color.Red;
            }
            else
            {
                this.m_TintColor = Color.Blue;
            }

            this.Velocity = new Vector2(0, k_BulletVelocity * m_BulletDirection);
            this.m_GameScreen = i_GameScreen;
            this.m_BulletType = i_BulletType;

            Initialize();
            i_GameScreen.Add(this);
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
            if ((i_Collidable is Enemy && IsBottomUp()) || (i_Collidable is Spaceship && !IsBottomUp()))
            {
                ReleaseBusyBullet();
            }
        }

        internal bool IsBottomUp()
        {
            return m_BulletDirection == -1;
        }

        internal SpaceInvadersEnums.eBulletType BulletType
        {
            get
            {
                return m_BulletType;
            }
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
            if ((bullet != null) && this.BulletType == SpaceInvadersEnums.eBulletType.SpaceshipBullet && bullet.BulletType == SpaceInvadersEnums.eBulletType.EnemyBullet)
            {
                int RandomCoin = sr_Random.Next(0, 2);
                if (RandomCoin == 1)
                {
                    bullet.ReleaseBusyBullet();
                }

                this.ReleaseBusyBullet();
            }
        }

        private void releaseBulletsOutOfFrame(GameTime i_GameTime)
        {
            bool isEnemyBulletOutOfFrame = (this.BulletType == SpaceInvadersEnums.eBulletType.EnemyBullet) && this.Position.Y > Game.GraphicsDevice.Viewport.Height;
            bool isSpaceshipBulletOutOfFrame = (this.BulletType == SpaceInvadersEnums.eBulletType.SpaceshipBullet) && this.Position.Y + Height < 0;

            if (isEnemyBulletOutOfFrame || isSpaceshipBulletOutOfFrame)
            {
                ReleaseBusyBullet();
            }
        }

        public void ReleaseBusyBullet()
        {
            this.Visible = false;
            this.Enabled = false;
        }

        public void CleanBullet()
        {
            m_GameScreen.Remove(this);
            this.Dispose();
        }
    }
}
