using System;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using SpaceInvadersGame.SpaceInvadersUtils;
using Infrastructure.ServiceInterfaces;
using Infrastructure.GamesObjects.ObjectModel.Animators;
using Infrastructure.GamesObjects.ObjectModel.Animators.ConcreteAnimators;
using SpaceInvadersGame.SpaceInvadersObjects;

namespace SpaceInvadersGame
{
    internal class Enemy : Sprite, ICollidable2D
    {
        public event GameEvents.EnemyDied.EnemyDiedEventHandler EnemyDied = null;

        private const string k_EnemiesAssetName = @"Sprites\Enemies_32x192";
        private const string k_MotherShipAssetName = @"Sprites\MotherShip_32x120";
        private const float k_AmoutOfSpinPerSec = 5f;
        private const float k_AnimationLength = 1.7f;
        private const int k_MaximalAmountOfEnemyBulletsAtOnce = 1;
        private const int k_MotherShipValue = 600;
        private const int k_TopEnemyValue = 300;
        private const int k_MiddleEnemyValue = 200;
        private const int k_LowEnemyValue = 70;
        private const int k_Width = 32;
        private const int k_Height = 32;
        private static readonly Random sr_Random = new Random();       
        private readonly Color r_MotherShipColor = Color.Red;
        private Gun r_Gun;
        private CompositeAnimator m_CompositeAnimator;
        protected int m_RandomIntervalForEnemyShots;
        protected float m_DrawElapsedTime;
        private float m_ElapsedTimeForShot;
        private float m_SecondsForNextShot;
        private int m_EnemyValue;

        internal Enemy(Game i_Game, int i_DrawOrder, int i_UpdateOrder, Vector2 i_EnemyPosition, SpaceInvadersEnums.eEnemyType i_EnemyType, ref int io_RandomIntervalForEnemyShots) : base(null, i_Game)
        {
            switch (i_EnemyType)
            {
                case SpaceInvadersEnums.eEnemyType.TopEnemy:
                    this.m_AssetName = k_EnemiesAssetName;
                    this.Initialize();
                    this.SourceRectangle = new Rectangle(0, 0, k_Width, k_Height);
                    this.m_TintColor = Color.Pink;
                    this.m_EnemyValue = k_TopEnemyValue;
                    break;

                case SpaceInvadersEnums.eEnemyType.MiddleEnemy:
                    this.m_AssetName = k_EnemiesAssetName;
                    this.Initialize();
                    this.SourceRectangle = new Rectangle(0, k_Height * 2, k_Width, k_Height);
                    this.m_TintColor = Color.Azure;
                    this.m_EnemyValue = k_MiddleEnemyValue;
                    break;

                case SpaceInvadersEnums.eEnemyType.LowEnemy:
                    this.m_AssetName = k_EnemiesAssetName;
                    this.Initialize();
                    this.SourceRectangle = new Rectangle(0, k_Height * 4, k_Width, k_Height);
                    this.m_TintColor = Color.LightYellow;
                    this.m_EnemyValue = k_LowEnemyValue;
                    break;
            }

            this.m_RandomIntervalForEnemyShots = io_RandomIntervalForEnemyShots;
            this.RotationOrigin = this.SourceRectangleCenter;
            this.m_DrawElapsedTime = 0;
            this.m_ElapsedTimeForShot = 0;
            this.r_Gun = new Gun(i_Game, k_MaximalAmountOfEnemyBulletsAtOnce, this);
            generateNewTimeForNextShot();
        }

        internal Enemy(Game i_Game, int i_DrawOrder, int i_UpdateOrder, Vector2 i_EnemyPosition, SpaceInvadersEnums.eEnemyType i_EnemyType) : base(null, i_Game)
        {
            this.m_AssetName = k_MotherShipAssetName;
            this.Initialize();
            this.m_TintColor = r_MotherShipColor;
            this.m_EnemyValue = k_MotherShipValue;
            this.RotationOrigin = this.SourceRectangleCenter;
            this.Position = i_EnemyPosition;
        }

        private void generateNewTimeForNextShot()
        {
            this.m_SecondsForNextShot = sr_Random.Next(0, m_RandomIntervalForEnemyShots);
            m_ElapsedTimeForShot = 0;
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            m_ElapsedTimeForShot += (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            // Check if needed that enemy shoot
            if (m_SecondsForNextShot <= m_ElapsedTimeForShot)
            {
                r_Gun.ShootBullet();
                generateNewTimeForNextShot();
            }
        }

        internal int EnemyValue
        {
            get
            {
                return m_EnemyValue;
            }

            set
            {
                this.m_EnemyValue = value;
            }
        }

        internal Gun Gun
        {
            get
            {
                return r_Gun;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            m_CompositeAnimator = new CompositeAnimator(this);
            ShrinkerAnimator shrinkerAnimator = new ShrinkerAnimator(TimeSpan.FromSeconds(k_AnimationLength));
            RotatorAnimator rotatorAnimator = new RotatorAnimator(TimeSpan.FromSeconds(k_AnimationLength), TimeSpan.FromSeconds(k_AmoutOfSpinPerSec));
            m_CompositeAnimator.Add(shrinkerAnimator);
            m_CompositeAnimator.Add(rotatorAnimator);
            shrinkerAnimator.Finished += new EventHandler(compositeAnimatorOfEnemy_Finished);
        }

        public override void Collided(ICollidable i_Collidable)
        {
            isCollidedWithSpaceShipBullet(i_Collidable);
        }

        private void compositeAnimatorOfEnemy_Finished(object i_Sender, EventArgs i_EventArgs)
        {
            onEnemyDied();
            this.Visible = false;
            Game.Components.Remove(this);
        }

        private void onEnemyDied()
        {
            if (EnemyDied != null)
            {
                EnemyDied.Invoke(this);
            }
        }

        private void isCollidedWithSpaceShipBullet(ICollidable i_Collidable)
        {
            Bullet spaceShipBullet = i_Collidable as Bullet;

            if (spaceShipBullet != null && spaceShipBullet.IsBottomUp())
            {
                base.Collided(i_Collidable);
                this.Dispose();
                spaceShipBullet.Visible = false;
                this.Animations.Add(m_CompositeAnimator);
                this.Animations.Resume();
                updateSpaceShipScore(spaceShipBullet);
            }
        }

        protected void updateSpaceShipScore(Bullet i_Bullet)
        {
            (i_Bullet.BulletOwner as Spaceship).PlayerScoreUpdate(this.EnemyValue);
        }
    }
}
