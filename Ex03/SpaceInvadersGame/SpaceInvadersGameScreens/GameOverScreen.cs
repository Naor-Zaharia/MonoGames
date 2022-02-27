using System;
using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.GamesObjects.ObjectModel;
using SpaceInvadersGame.SpaceInvadersGameScreens;

namespace GameScreens.Screens
{
    public class GameOverScreen : SpaceInvadersGameScreen
    {  
        private const string k_FontName = @"Fonts\Consolas";
        private const string k_GameOverMsgAssetName = @"Sprites\GameOverMsg";
        private const string k_GameOverInstructionMsgAssetName = @"Sprites\GameOverInstructionMsg";
        private const string k_AnimatorPulseName = "Pulse";
        private const float k_PulseTargetScale = 1.05f;
        private const float k_PulsePerSec = 0.7f;
        private const float k_GapFromTitle = 0.8f;
        private const float k_GapFromWinnerMsg = 4f;
        private const float k_TextBlockScales = 1.5f;
        private readonly Sprite r_GameOverMessage;
        private readonly TextBlock r_GameScoreMsg;
        private readonly TextBlock r_GameResultMsg;
        private readonly Sprite r_GameOverInstructions;

        public GameOverScreen(Game i_Game)
            : base(i_Game)
        {
            m_Background.TintColor = Color.Red;
            r_GameOverMessage = new Sprite(k_GameOverMsgAssetName, this.Game);
            this.Add(r_GameOverMessage);
            r_GameOverInstructions = new Sprite(k_GameOverInstructionMsgAssetName, this.Game);
            this.Add(r_GameOverInstructions);
            r_GameScoreMsg = new TextBlock(k_FontName, Game);
            this.Add(r_GameScoreMsg);
            r_GameResultMsg = new TextBlock(k_FontName, Game);
            this.Add(r_GameResultMsg);
        }

        public override void Initialize()
        {
            base.Initialize();
            r_GameOverMessage.Animations.Add(new PulseAnimator(k_AnimatorPulseName, TimeSpan.Zero, k_PulseTargetScale, k_PulsePerSec));
            r_GameOverMessage.Animations.Enabled = true;
            r_GameOverMessage.PositionOrigin = r_GameOverMessage.SourceRectangleCenter;
            r_GameOverMessage.RotationOrigin = r_GameOverMessage.SourceRectangleCenter;
            r_GameOverMessage.Position = new Vector2(CenterOfViewPort.X, CenterOfViewPort.Y / 2);
            r_GameScoreMsg.Text = r_GameManager.GameOverScoreString;
            r_GameScoreMsg.Scales = new Vector2(k_TextBlockScales);
            r_GameScoreMsg.TextColor = Color.MediumVioletRed;
            r_GameScoreMsg.PositionOrigin = r_GameScoreMsg.SourceRectangleCenter;
            r_GameScoreMsg.RotationOrigin = r_GameScoreMsg.SourceRectangleCenter;
            r_GameScoreMsg.Position = new Vector2(CenterOfViewPort.X, r_GameOverMessage.Position.Y + (r_GameOverMessage.Height * k_GapFromTitle));
            r_GameResultMsg.Text = r_GameManager.GameOverWinnerString;
            r_GameResultMsg.Scales = new Vector2(k_TextBlockScales);
            r_GameResultMsg.TextColor = Color.Green;
            r_GameResultMsg.PositionOrigin = r_GameResultMsg.SourceRectangleCenter;
            r_GameResultMsg.RotationOrigin = r_GameResultMsg.SourceRectangleCenter;
            r_GameResultMsg.Position = new Vector2(CenterOfViewPort.X, r_GameScoreMsg.Position.Y + r_GameScoreMsg.Height);
            r_GameOverInstructions.PositionOrigin = r_GameOverInstructions.SourceRectangleCenter;
            r_GameOverInstructions.RotationOrigin = r_GameOverInstructions.SourceRectangleCenter;
            r_GameOverInstructions.Position = new Vector2(CenterOfViewPort.X, r_GameResultMsg.Position.Y + (k_GapFromWinnerMsg * r_GameResultMsg.Height));
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);

            if (InputManager.KeyPressed(Keys.Escape))
            {
                Game.Exit();
            }

            if (InputManager.KeyPressed(Keys.Home))
            {
                this.r_GameManager.GameManagerRematch();
                r_GameManager.PlayScreen.InitGame();
                m_ScreensManager.SetCurrentScreen(new LevelTransitionScreen(Game, 1));
            }

            if (InputManager.KeyPressed(Keys.M))
            {
                ExitScreen();
                r_GameManager.MainMenuScreen.Initialize();
                m_ScreensManager.SetCurrentScreen(r_GameManager.MainMenuScreen);
            }
        }
    }
}
