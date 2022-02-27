using System;
using Microsoft.Xna.Framework;
using SpaceInvadersGame.SpaceInvadersUtils;
using Infrastructure.GamesObjects.ObjectModel.Animators;
using Infrastructure.GamesObjects.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ServiceInterfaces;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.ObjectModel.Screens;

namespace SpaceInvadersGame
{
    internal class MotherShip : Enemy, ICollidable2D
    {        
        private const string k_MothershipKillSoundName = "MotherShipKill";
        private const float k_MotherShipVelocity = 95;
        private const int k_RandomIntervalForMotherShipAppearance = 20;
        private const float k_AnimatorLength = 3f;
        private const float k_AnimatorAmountOfBlinkPerSec = 0.1f;
        private static readonly Random sr_Random = new Random();
        private Vector2 m_InitPosition;
        private float m_MotherShipSecondsForNextAppearance;
        private float m_ElapsedTime;
        private bool m_IsCurrentlySweep;
        private bool m_GotHit;

        internal MotherShip(GameScreen i_GameScreen, int i_DrawOrder, int i_UpdateOrder, Vector2 i_EnemyPosition, SpaceInvadersEnums.eEnemyType i_EnemyType)
            : base(i_GameScreen, i_DrawOrder, i_UpdateOrder, i_EnemyPosition, i_EnemyType)
        {
            this.m_IsCurrentlySweep = false;
            this.Visible = true;
            this.m_GotHit = false;
        }

        public override void Update(GameTime i_GameTime)
        {
            m_ElapsedTime += (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            // Check if it is first generate or need new generate
            if (Position.X >= Game.GraphicsDevice.Viewport.Width + Width || (!m_IsCurrentlySweep && Visible))
            {
                GenerateMotherShipNewNextAppearance();
            }

            if (m_IsCurrentlySweep)
            {
                UpdateXPosition(k_MotherShipVelocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds);
            }

            if (!m_IsCurrentlySweep && MotherShipSecondsForNextAppearance <= m_ElapsedTime)
            {
                Visible = true;
                m_IsCurrentlySweep = true;
            }

            this.Animations.Update(i_GameTime);
        }

        public override void Initialize()
        {
            base.Initialize();
            this.m_InitPosition = new Vector2(-Width, Height);
            CompositeAnimator compositeAnimator = new CompositeAnimator(this);
            FaderAnimator faderAnimator = new FaderAnimator(TimeSpan.FromSeconds(k_AnimatorLength));
            ShrinkerAnimator shrinkerAnimator = new ShrinkerAnimator(TimeSpan.FromSeconds(k_AnimatorLength));
            BlinkAnimator blinkAnimator = new BlinkAnimator(TimeSpan.FromSeconds(k_AnimatorAmountOfBlinkPerSec), TimeSpan.FromSeconds(k_AnimatorLength));
            compositeAnimator.Add(faderAnimator);
            compositeAnimator.Add(shrinkerAnimator);
            compositeAnimator.Add(blinkAnimator);
            this.Animations.Add(compositeAnimator);
            faderAnimator.Finished += new EventHandler(compositeAnimatorOfMotherShip_Finished);
            this.BlendState = BlendState.NonPremultiplied;
        }

        protected override void InitBounds()
        {
            base.InitBounds();
            Position = m_InitPosition;
        }

        private void compositeAnimatorOfMotherShip_Finished(object i_Sender, EventArgs i_EventArgs)
        {
            GenerateMotherShipNewNextAppearance();
        }

        internal float MotherShipSecondsForNextAppearance
        {
            get
            {
                return m_MotherShipSecondsForNextAppearance;
            }
        }

        internal void GenerateMotherShipNewNextAppearance()
        {
            this.InitBounds();
            this.m_MotherShipSecondsForNextAppearance = sr_Random.Next(0, k_RandomIntervalForMotherShipAppearance);
            this.m_ElapsedTime = 0;
            this.Visible = false;
            this.m_IsCurrentlySweep = false;
            this.m_GotHit = false;
            this.EnemyValue = k_InitMotherShipValue;
        }

        public override void UpdateXPosition(float i_UpdateXValue)
        {
            this.Position = new Vector2(Position.X + i_UpdateXValue, Position.Y);
        }

        public override void Collided(ICollidable i_Collidable)
        {
            Bullet spaceShipBullet = i_Collidable as Bullet;
            if (spaceShipBullet != null && spaceShipBullet.BulletType == SpaceInvadersEnums.eBulletType.SpaceshipBullet && !m_GotHit)
            {
                spaceShipBullet.Visible = false;
                UpdateSpaceShipScore(spaceShipBullet);
                this.EnemyValue = 0;
                r_SoundManager.PlaySound(k_MothershipKillSoundName);
                this.Animations.Restart();
                this.m_GotHit = true;
            }
        }
    }
}