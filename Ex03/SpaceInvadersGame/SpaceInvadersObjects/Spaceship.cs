using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure.ObjectModel;
using SpaceInvadersGame.SpaceInvadersObjects;
using Infrastructure.GamesObjects.ObjectModel.Animators;
using Infrastructure.GamesObjects.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ServiceInterfaces;
using Infrastructure.Managers;
using Infrastructure.GamesObjects.ObjectModel;
using SpaceInvadersGame.SpaceInvadersUtils;
using Infrastructure.ObjectModel.Screens;
using Infrastructure.GamesObjects.Managers;
using Infrastructure.GamesObjects.ServiceInterfaces;

namespace SpaceInvadersGame
{
    internal class Spaceship : Sprite, ICollidable2D
    {
        public event GameEvents.GameEnd.GameEndEventHandler GameEnded = null;

        public event GameEvents.ScoreUpdate.ScoreUpdateEventHandler ScoreUpdate = null;

        public event GameEvents.SpaceshipGotHit.SpaceshipGotHitEventHandler SpaceshipGotHit = null;

        // Consts
        private const string k_FirstSpaceShipAssetName = @"Sprites\Ship01_32x32";
        private const string k_SecondSpaceShipAssetName = @"Sprites\Ship02_32x32";
        private const string k_FirstPlayerName = "Player 1";
        private const string k_SecondPlayerName = "Player 2";        
        private const float k_SpaceShipVelocity = 140;
        private const float k_AmountOfBlinkPerSec = 0.125f;
        private const float k_AnimationLength = 2f;
        private const int k_MaximalAmountOfSpaceShipBulletsAtOnce = 2;
        private const int k_GapSpaceShipFromBottom = 15;
        private const int k_PenaltyOnSpaceShipHit = -600;
        private const int k_FirstPlayerId = 1;
        private const int k_SecondPlayerId = 2;
        private const byte k_InitSpaceShipLife = 3;
        private readonly Gun r_MachineGun;
        private readonly InputManager r_InputManager;
        private readonly SoundManager r_SoundManager;
        private Player m_Player;
        private Keys m_MoveRightButton;
        private Keys m_MoveLeftButton;
        private Keys m_ShootingButton;
        private bool m_IsMouseMovment;

        internal Spaceship(GameScreen i_GameScreen, Player i_Player, SpaceInvadersEnums.ePlayerType i_PlayerType)
            : base(null, i_GameScreen.Game)
        {
            this.r_MachineGun = new Gun(i_GameScreen, k_MaximalAmountOfSpaceShipBulletsAtOnce, this);
            this.r_MachineGun.OwnerOfShootMachine = this;
            this.m_Player = i_Player;
            this.r_InputManager = (InputManager)Game.Services.GetService(typeof(IInputManager));
            this.r_SoundManager = (SoundManager)Game.Services.GetService(typeof(ISoundManager));

            switch (i_PlayerType)
            {
                case SpaceInvadersEnums.ePlayerType.FirstPlayer:
                    this.AssetName = k_FirstSpaceShipAssetName;
                    assignSpaceShipControls(SpaceInvadersControlKeys.sr_FirstPlayerRightKey, SpaceInvadersControlKeys.sr_FirstPlayerLeftKey, SpaceInvadersControlKeys.sr_FirstPlayerShootingKey, true);
                    base.Initialize();
                    break;

                case SpaceInvadersEnums.ePlayerType.SecondPlayer:
                    this.AssetName = k_SecondSpaceShipAssetName;
                    assignSpaceShipControls(SpaceInvadersControlKeys.sr_SecondPlayerRightKey, SpaceInvadersControlKeys.sr_SecondPlayerLeftKey, SpaceInvadersControlKeys.sr_SecondPlayerShootingKey, false);
                    base.Initialize();
                    break;
            }

            i_GameScreen.Add(this);
        }

        private void assignSpaceShipControls(Keys i_RightKey, Keys i_LeftKey, Keys i_ShootingKey, bool i_IsMouseMovment)
        {
            this.m_MoveRightButton = i_RightKey;
            this.m_MoveLeftButton = i_LeftKey;
            this.m_ShootingButton = i_ShootingKey;
            this.m_IsMouseMovment = i_IsMouseMovment;
        }

        public override void Initialize()
        {
            base.Initialize();
            CompositeAnimator compositeAnimator = new CompositeAnimator(this);
            BlinkAnimator blinkAnimator = new BlinkAnimator(TimeSpan.FromSeconds(k_AmountOfBlinkPerSec), TimeSpan.FromSeconds(k_AnimationLength));
            compositeAnimator.Add(blinkAnimator);
            blinkAnimator.Finished += new EventHandler(compositeAnimatorOfSpaceShip_Finished);
            this.Animations.Add(compositeAnimator);
        }

        protected override void InitBounds()
        {
            base.InitBounds();
            InitSpaceshipPosition();
        }

        public void InitSpaceshipPosition()
        {
            this.Position = new Vector2((Width * m_Player.PlayerId) - Width, Game.GraphicsDevice.Viewport.Height - Height - k_GapSpaceShipFromBottom);
            this.RotationOrigin = this.SourceRectangleCenter;
        }

        private void compositeAnimatorOfSpaceShip_Finished(object i_Sender, EventArgs i_EventArgs)
        {
            this.Animations.Pause();
        }

        public override void Update(GameTime i_GameTime)
        {
            moveSpaceShip(i_GameTime);
            shootBullet(i_GameTime);
            base.Update(i_GameTime);
        }

        internal void SpaceShipGetHit()
        {
            if (m_Player.AmountOfLives != 1)
            {
                this.Position = new Vector2((Width * m_Player.PlayerId) - Width, Game.GraphicsDevice.Viewport.Height - Height - k_GapSpaceShipFromBottom);
                this.Animations.Restart();
            }

            onSpaceshipHit();
        }

        internal List<Bullet> SpaceShipBulletList
        {
            get
            {
                return r_MachineGun.BulletsList;
            }
        }

        private void moveSpaceShip(GameTime i_GameTime)
        {
            if (r_InputManager.KeyHeld(m_MoveLeftButton))
            {
                UpdateXPosition((float)i_GameTime.ElapsedGameTime.TotalSeconds * k_SpaceShipVelocity * -1);
            }

            if (r_InputManager.KeyHeld(m_MoveRightButton))
            {
                UpdateXPosition((float)i_GameTime.ElapsedGameTime.TotalSeconds * k_SpaceShipVelocity);
            }

            if (m_IsMouseMovment)
            {
                // Check if there is a .PrevMouseState.X, (Update delta mouse position, only if prev mouse state known)
                if (r_InputManager.PrevMouseState.X != 0)
                {
                    UpdateXPosition(r_InputManager.MousePositionDelta.X);
                }
            }
        }

        private void shootBullet(GameTime i_GameTime)
        {
            if (m_Player.AmountOfLives != 0)
            {
                if (r_InputManager.KeyPressed(m_ShootingButton) ||
                    (r_InputManager.ButtonPressed(eInputButtons.Left) && m_IsMouseMovment))
                {
                    r_MachineGun.ShootBullet();                    
                }
            }
        }

        internal int PenaltyOnSpaceShipHit
        {
            get
            {
                return k_PenaltyOnSpaceShipHit;
            }
        }

        internal Gun Gun
        {
            get
            {
                return r_MachineGun;
            }
        }

        internal float GetYPositionOfSpaceShipFromBottom
        {
            get
            {
                return Height + k_GapSpaceShipFromBottom;
            }
        }

        public override void Collided(ICollidable i_Collidable)
        {
            isCollidedWithEnemyBullet(i_Collidable);
            isCollidedWithEnemy(i_Collidable);
        }

        private void isCollidedWithEnemyBullet(ICollidable i_Collidable)
        {
            Bullet enemyBullet = i_Collidable as Bullet;

            if (enemyBullet != null && !enemyBullet.IsBottomUp())
            {
                SpaceShipGetHit();
                onScoreUpdate(k_PenaltyOnSpaceShipHit);
            }
        }

        private void isCollidedWithEnemy(ICollidable i_Collidable)
        {
            Enemy enemy = i_Collidable as Enemy;
            if (enemy != null)
            {
                onGameEnd();
            }
        }

        private void onGameEnd()
        {
            if (GameEnded != null)
            {
                GameEnded.Invoke();
            }
        }

        internal void PlayerScoreUpdate(int i_ScoreUpdate)
        {
            onScoreUpdate(i_ScoreUpdate);
        }

        private void onScoreUpdate(int i_ScoreUpdate)
        {
            if (ScoreUpdate != null)
            {
                ScoreUpdate.Invoke(i_ScoreUpdate);
            }
        }

        private void onSpaceshipHit()
        {
            if (SpaceshipGotHit != null)
            {
                SpaceshipGotHit.Invoke();
            }
        }

        internal float GapSpaceShipFromBottom
        {
            get
            {
                return k_GapSpaceShipFromBottom;
            }
        }

        public void CleanSpaceShipComponents()
        {
            this.Visible = false;
            r_MachineGun.CleanAllBullets();
        }
    }
}
