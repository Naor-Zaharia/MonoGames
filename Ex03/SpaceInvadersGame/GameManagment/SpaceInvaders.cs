using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.GamesObjects.Managers;
using Infrastructure.Managers;
using Infrastructure.ObjectGeneralGame;
using Infrastructure.ServiceInterfaces;
using SpaceInvadersGame.SpaceInvadersGameScreens;
using SpaceInvadersGame.SpaceInvadersGameServices;

namespace SpaceInvadersGame
{
    public class SpaceInvaders : Game
    {
        private const string k_GameTitle = "Space Invaders";
        private const string k_RootDir = "Content";
        private const string k_BackgroundAssetName = @"Sprites\BG_Space01_1024x768";
        private const float k_BackgroundPercentage = 0.8f;
        private readonly SpaceInvaderGameManagerService r_SpacecInvadersGameManager;
        private readonly SoundManager r_SoundManager;
        private readonly ScreensMananger r_ScreensMananger;
        private SpriteBatch m_SpriteBatch;
        private GraphicsDeviceManager m_GraphicsDeviceManager;        
        private Background m_Background;

        public SpaceInvaders()
        {
            Content.RootDirectory = k_RootDir;
            CollisionsManager collisionsManager = new CollisionsManager(this);
            IInputManager inputManager = new InputManager(this);
            this.m_GraphicsDeviceManager = new GraphicsDeviceManager(this);
            this.Services.AddService(typeof(GraphicsDeviceManager), m_GraphicsDeviceManager);
            this.r_SpacecInvadersGameManager = new SpaceInvaderGameManagerService(this);
            this.r_SoundManager = new SoundManager(this);
            this.r_ScreensMananger = new ScreensMananger(this);
            this.IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            this.Window.Title = k_GameTitle;
            r_ScreensMananger.SetCurrentScreen(new WelcomeScreen(this));
        }

        protected override void LoadContent()
        {
            this.m_SpriteBatch = new SpriteBatch(GraphicsDevice);
            this.m_Background = new Background(this, k_BackgroundAssetName, 1, 1, k_BackgroundPercentage);
            this.m_GraphicsDeviceManager.PreferredBackBufferWidth = (int)(m_Background.Width * m_Background.BackgroundPercentage);
            this.m_GraphicsDeviceManager.PreferredBackBufferHeight = (int)(m_Background.Height * m_Background.BackgroundPercentage);
            this.m_GraphicsDeviceManager.ApplyChanges();
            GameSoundManager.LoadGameSounds(this);
        }

        protected override void Draw(GameTime i_GameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(i_GameTime);
        }
    }
}
