using System;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using SpaceInvadersGame.SpaceInvadersUtils;
using Infrastructure.ServiceInterfaces;
using Infrastructure.GamesObjects.ObjectModel.Animators;
using Infrastructure.GamesObjects.ObjectModel.Animators.ConcreteAnimators;
using SpaceInvadersGame.SpaceInvadersObjects;
using Infrastructure.ObjectModel.Screens;
using Infrastructure.GamesObjects.Managers;
using Infrastructure.GamesObjects.ServiceInterfaces;
using SpaceInvadersGame.SpaceInvadersGameServices;
using SpaceInvadersGame.SpaceInvadersGameServices.SpaceInvadersGameInterfaces;

namespace SpaceInvadersGame
{
    internal class Enemy : Sprite, ICollidable2D
    {
        public event GameEvents.EnemyDied.EnemyDiedEventHandler EnemyDied = null;
                
        private const int k_MaximalAmountOfEnemyBulletsAtOnce = 1;
        private const string k_EnemiesAssetName = @"Sprites\Enemies_32x192";
        private const string k_MotherShipAssetName = @"Sprites\MotherShip_32x120";        
        private const string k_EnemyKillSoundName = "EnemyKill";
        private const int k_InitTopEnemyValue = 300;
        private const int k_InitMiddleEnemyValue = 200;
        private const int k_InitLowEnemyValue = 70;
        private const int k_Width = 32;
        private const int k_Height = 32;
        private const float k_AmoutOfSpinPerSec = 5f;
        private const float k_AnimationLength = 1.7f;
        protected const int k_InitMotherShipValue = 600;
        private static readonly Random sr_Random = new Random();
        private readonly Color r_MotherShipColor = Color.Red;
        private readonly Gun r_Gun;
        private readonly SpaceInvaderGameManagerService r_GameManager;
        protected readonly SoundManager r_SoundManager;
        protected GameScreen m_GameScreen;
        private SpaceInvadersEnums.eEnemyType m_EnemyType;
        private CompositeAnimator m_CompositeAnimator;
        protected int m_EnemyValue;
        protected float m_DrawElapsedTime;
        private float m_ElapsedTimeForShot;
        private float m_SecondsForNextShot;
        protected int m_RandomIntervalForEnemyShots;

        internal Enemy(GameScreen i_GameScreen, int i_DrawOrder, int i_UpdateOrder, Vector2 i_EnemyPosition, SpaceInvadersEnums.eEnemyType i_EnemyType, ref int io_RandomIntervalForEnemyShots) : base(null, i_GameScreen.Game, i_UpdateOrder, i_DrawOrder)
        {
            this.r_SoundManager = (SoundManager)Game.Services.GetService(typeof(ISoundManager));
            this.r_GameManager = (SpaceInvaderGameManagerService)Game.Services.GetService(typeof(ISpaceInvaderGameManagerService));

            switch (i_EnemyType)
            {
                case SpaceInvadersEnums.eEnemyType.TopEnemy:
                    this.m_AssetName = k_EnemiesAssetName;
                    this.Initialize();
                    this.m_TintColor = Color.Pink;
                    this.m_EnemyValue = k_InitTopEnemyValue + r_GameManager.ExtraValueForEnemiesLevel;
                    break;

                case SpaceInvadersEnums.eEnemyType.MiddleEnemy:
                    this.m_AssetName = k_EnemiesAssetName;
                    this.Initialize();
                    this.m_TintColor = Color.Azure;
                    this.m_EnemyValue = k_InitMiddleEnemyValue + r_GameManager.ExtraValueForEnemiesLevel;
                    break;

                case SpaceInvadersEnums.eEnemyType.LowEnemy:
                    this.m_AssetName = k_EnemiesAssetName;
                    this.Initialize();
                    this.m_TintColor = Color.LightYellow;
                    this.m_EnemyValue = k_InitLowEnemyValue + r_GameManager.ExtraValueForEnemiesLevel;
                    break;
            }

            i_GameScreen.Add(this);
            this.m_EnemyType = i_EnemyType;
            this.m_RandomIntervalForEnemyShots = io_RandomIntervalForEnemyShots;
            this.RotationOrigin = this.SourceRectangleCenter;
            this.m_DrawElapsedTime = 0;
            this.m_ElapsedTimeForShot = 0;
            this.r_Gun = new Gun(i_GameScreen, k_MaximalAmountOfEnemyBulletsAtOnce, this);
            this.m_GameScreen = i_GameScreen;
            generateNewTimeForNextShot();
            InitSourceRectangle();
        }

        internal Enemy(GameScreen i_GameScreen, int i_DrawOrder, int i_UpdateOrder, Vector2 i_EnemyPosition, SpaceInvadersEnums.eEnemyType i_EnemyType) : base(null, i_GameScreen.Game)
        {
            this.m_AssetName = k_MotherShipAssetName;
            this.Initialize();
            this.m_GameScreen = i_GameScreen;
            this.m_TintColor = r_MotherShipColor;
            this.m_EnemyValue = k_InitMotherShipValue;
            this.RotationOrigin = this.SourceRectangleCenter;
            this.Position = i_EnemyPosition;
            this.r_SoundManager = (SoundManager)Game.Services.GetService(typeof(ISoundManager));
        }

        private void generateNewTimeForNextShot()
        {
            this.m_SecondsForNextShot = sr_Random.Next(0, m_RandomIntervalForEnemyShots);
            this.m_ElapsedTimeForShot = 0;
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            m_ElapsedTimeForShot += (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            // Check if needed that enemy shoot
            if (m_SecondsForNextShot <= m_ElapsedTimeForShot)
            {
                this.r_Gun.ShootBullet();                
                generateNewTimeForNextShot();
            }
        }

        internal virtual int EnemyValue
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
            this.m_CompositeAnimator = new CompositeAnimator(this);
            ShrinkerAnimator shrinkerAnimator = new ShrinkerAnimator(TimeSpan.FromSeconds(k_AnimationLength));
            RotatorAnimator rotatorAnimator = new RotatorAnimator(TimeSpan.FromSeconds(k_AnimationLength), TimeSpan.FromSeconds(k_AmoutOfSpinPerSec));
            this.m_CompositeAnimator.Add(shrinkerAnimator);
            this.m_CompositeAnimator.Add(rotatorAnimator);
            shrinkerAnimator.Finished += new EventHandler(compositeAnimatorOfEnemy_Finished);
        }

        protected override void InitBounds()
        {
            if (!(this is MotherShip))
            {
                Width = k_Width;
                Height = k_Height;
                InitSourceRectangle();
                RotationOrigin = this.SourceRectangleCenter;
            }
            else
            {
                base.InitBounds();
            }
        }

        protected override void InitSourceRectangle()
        {
            switch (this.m_EnemyType)
            {
                case SpaceInvadersEnums.eEnemyType.TopEnemy:
                    this.SourceRectangle = new Rectangle(0, 0, k_Width, k_Height);
                    break;

                case SpaceInvadersEnums.eEnemyType.MiddleEnemy:
                    this.SourceRectangle = new Rectangle(0, k_Height * 2, k_Width, k_Height);
                    break;

                case SpaceInvadersEnums.eEnemyType.LowEnemy:
                    this.SourceRectangle = new Rectangle(0, k_Height * 4, k_Width, k_Height);
                    break;
                default:
                    base.InitSourceRectangle();
                    break;
            }
        }

        public override void Collided(ICollidable i_Collidable)
        {
            isCollidedWithSpaceShipBullet(i_Collidable);
        }

        private void compositeAnimatorOfEnemy_Finished(object i_Sender, EventArgs i_EventArgs)
        {
            this.Visible = false;
            onEnemyDied();
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

            if (spaceShipBullet != null && spaceShipBullet.BulletType == SpaceInvadersEnums.eBulletType.SpaceshipBullet)
            {
                base.Collided(i_Collidable);
                UpdateSpaceShipScore(spaceShipBullet);
                this.EnemyValue = 0;
                this.Dispose();
                spaceShipBullet.Visible = false;
                this.Animations.Add(m_CompositeAnimator);
                this.Animations.Resume();
                this.r_SoundManager.PlaySound(k_EnemyKillSoundName);
            }
        }

        protected void UpdateSpaceShipScore(Bullet i_Bullet)
        {
            (i_Bullet.BulletOwner as Spaceship).PlayerScoreUpdate(this.EnemyValue);
        }

        public void CleanEnemy()
        {
            this.m_GameScreen.Remove(this);
            this.Dispose();
        }
    }
}
