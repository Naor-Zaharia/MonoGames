using Infrastructure.GamesObjects.Managers;
using Infrastructure.GamesObjects.ServiceInterfaces;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvadersGame.SpaceInvadersGameScreens
{
    public class PauseScreen : GameScreen
    {
        private const string k_FontName = @"Fonts\Consolas";
        private const string k_PauseMessageAssetName = @"Sprites\Pause";
        private const string k_ResumeMessageAssetName = @"Sprites\PauseMsg";
        private const float k_BlackTintAlpha = 0.4f;
        private readonly Sprite r_PauseMessage;
        private readonly Sprite r_ResumeMsg;
        private readonly SoundManager r_SoundManager;

        public PauseScreen(Game i_Game)
            : base(i_Game)
        {
            this.IsModal = true;
            this.IsOverlayed = true;
            this.BlackTintAlpha = k_BlackTintAlpha;
            this.UseGradientBackground = false;
            this.r_PauseMessage = new Sprite(k_PauseMessageAssetName, i_Game, 1, 1);
            this.Add(r_PauseMessage);
            this.r_ResumeMsg = new Sprite(k_ResumeMessageAssetName, i_Game, 1, 1);
            this.Add(r_ResumeMsg);
            this.r_SoundManager = (SoundManager)Game.Services.GetService(typeof(ISoundManager));
            this.BlendState = Microsoft.Xna.Framework.Graphics.BlendState.NonPremultiplied;
        }

        public override void Initialize()
        {
            base.Initialize();
            r_PauseMessage.PositionOrigin = r_PauseMessage.SourceRectangleCenter;
            r_PauseMessage.RotationOrigin = r_PauseMessage.SourceRectangleCenter;
            r_PauseMessage.Position = CenterOfViewPort;
            r_ResumeMsg.PositionOrigin = r_ResumeMsg.SourceRectangleCenter;
            r_ResumeMsg.Position = new Vector2(CenterOfViewPort.X, CenterOfViewPort.Y + r_PauseMessage.Height);
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (InputManager.KeyPressed(Keys.R))
            {
                ExitScreen();
            }

            if (InputManager.PrevKeyboardState.IsKeyDown(Keys.M) && InputManager.KeyboardState.IsKeyUp(Keys.M))
            {
                r_SoundManager.ToggleSound();
            }
        }
    }
}
