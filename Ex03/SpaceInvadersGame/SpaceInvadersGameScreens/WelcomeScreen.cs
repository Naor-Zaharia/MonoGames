using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure.ObjectGeneralGame;
using Infrastructure.ObjectModel.Screens;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ObjectModel;
using Infrastructure.GamesObjects.Managers;
using Infrastructure.GamesObjects.ServiceInterfaces;
using GameScreens.Screens;
using SpaceInvadersGame.SpaceInvadersGameScreens.Menus;
using SpaceInvadersGame.SpaceInvadersGameServices;
using SpaceInvadersGame.SpaceInvadersGameServices.SpaceInvadersGameInterfaces;

namespace SpaceInvadersGame.SpaceInvadersGameScreens
{
    public class WelcomeScreen : GameScreen
    {
        private const string k_FontName = @"Fonts\Consolas";
        private const string k_BackgroundAssetName = @"Sprites\BG_Space01_1024x768";
        private const float k_BackgroundPercentage = 0.8f;
        private const string k_WelcomeMsgAssetName = @"Sprites\WelcomeMsg";
        private const string k_InstructionMsgAssetName = @"Sprites\WelcomeInstructionMsg";
        private const float k_m_WelcomeMsgeScale = 1.4f;
        private const string k_AnimatorPulseName = "Pulse";
        private const float k_PulseTargetScale = 1.6f;
        private const float k_PulsePerSec = 0.6f;
        private const float k_GapFromWelcomeMsg = 1.2f;
        private readonly PlayScreen r_PlayScreen;
        private readonly MainMenuScreen r_MainMenuScreen;
        private readonly SoundManager r_SoundManager;
        private readonly SpaceInvaderGameManagerService r_GameManager;
        private readonly Sprite r_WelcomeMessage;
        private readonly Sprite r_InstructionMsg;
        protected Background m_Background;

        public WelcomeScreen(Game i_Game)
            : base(i_Game)
        {
            this.r_GameManager = (SpaceInvaderGameManagerService)Game.Services.GetService(typeof(ISpaceInvaderGameManagerService));
            this.r_SoundManager = (SoundManager)Game.Services.GetService(typeof(ISoundManager));
            this.r_PlayScreen = new PlayScreen(Game);
            this.r_GameManager.PlayScreen = r_PlayScreen;
            this.r_SoundManager.PlayBackgoundMusicOnLoop();
            this.m_Background = new Background(i_Game, k_BackgroundAssetName, 0, 0, k_BackgroundPercentage);
            this.Add(m_Background);
            this.r_WelcomeMessage = new Sprite(k_WelcomeMsgAssetName, i_Game, 1, 1);
            this.r_WelcomeMessage.Scales = new Vector2(k_m_WelcomeMsgeScale);
            this.Add(r_WelcomeMessage);
            this.r_InstructionMsg = new Sprite(k_InstructionMsgAssetName, i_Game, 1, 1);
            this.Add(r_InstructionMsg);
            this.r_MainMenuScreen = new MainMenuScreen(Game);
            this.r_GameManager.MainMenuScreen = r_MainMenuScreen;
            this.BlendState = Microsoft.Xna.Framework.Graphics.BlendState.NonPremultiplied;
        }

        public override void Initialize()
        {
            base.Initialize();
            r_WelcomeMessage.Animations.Add(new PulseAnimator(k_AnimatorPulseName, TimeSpan.Zero, k_PulseTargetScale, k_PulsePerSec));
            r_WelcomeMessage.Animations.Enabled = true;
            r_WelcomeMessage.PositionOrigin = r_WelcomeMessage.SourceRectangleCenter;
            r_WelcomeMessage.RotationOrigin = r_WelcomeMessage.SourceRectangleCenter;
            r_WelcomeMessage.Position = new Vector2(CenterOfViewPort.X, CenterOfViewPort.Y);
            r_InstructionMsg.PositionOrigin = r_InstructionMsg.SourceRectangleCenter;
            r_InstructionMsg.Position = new Vector2(CenterOfViewPort.X, r_WelcomeMessage.Position.Y + (k_GapFromWelcomeMsg * r_WelcomeMessage.Height));
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (InputManager.KeyPressed(Keys.Escape))
            {
                Game.Exit();
            }

            if (InputManager.KeyPressed(Keys.Enter))
            {
                m_ScreensManager.SetCurrentScreen(new LevelTransitionScreen(Game, 1));
            }

            if (InputManager.KeyPressed(Keys.M))
            {
                ScreensManager.SetCurrentScreen(r_GameManager.MainMenuScreen);
            }
        }
    }
}
