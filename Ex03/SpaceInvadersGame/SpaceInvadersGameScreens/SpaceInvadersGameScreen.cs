using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure.GamesObjects.Managers;
using Infrastructure.GamesObjects.ServiceInterfaces;
using Infrastructure.ObjectGeneralGame;
using Infrastructure.ObjectModel.Screens;
using SpaceInvadersGame.SpaceInvadersGameServices;
using SpaceInvadersGame.SpaceInvadersGameServices.SpaceInvadersGameInterfaces;

namespace SpaceInvadersGame.SpaceInvadersGameScreens
{
    public class SpaceInvadersGameScreen : GameScreen
    {
        private const string k_BackgroundAssetName = @"Sprites\BG_Space01_1024x768";
        private const float k_BackgroundPercentage = 0.8f;
        private const int k_TintRate = 200;
        protected readonly SoundManager r_SoundManager;
        protected readonly SpaceInvaderGameManagerService r_GameManager;
        protected Background m_Background;

        public SpaceInvadersGameScreen(Game i_Game) : base(i_Game)
        {
            this.r_GameManager = (SpaceInvaderGameManagerService)Game.Services.GetService(typeof(ISpaceInvaderGameManagerService));
            this.r_SoundManager = (SoundManager)Game.Services.GetService(typeof(ISoundManager));
            this.m_Background = new Background(i_Game, k_BackgroundAssetName, 0, 0, k_BackgroundPercentage);
            this.m_Background.TintColor = new Color(k_TintRate, k_TintRate, k_TintRate, 255);
            this.Add(m_Background);
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            if (InputManager.PrevKeyboardState.IsKeyDown(Keys.M) && InputManager.KeyboardState.IsKeyUp(Keys.M))
            {
                r_SoundManager.ToggleSound();
            }
        }
    }
}
