using System;
using Infrastructure.GamesObjects.ObjectModel;
using Infrastructure.GamesObjects.ObjectModel.Animators;
using Infrastructure.GamesObjects.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Microsoft.Xna.Framework;

namespace SpaceInvadersGame.SpaceInvadersGameScreens
{
   public class LevelTransitionScreen : SpaceInvadersGameScreen
    {
        private const string k_FontName = @"Fonts\ConsolasLargeBold";
        private const string k_CountingDownMessageAssetName = @"Sprites\Counting";
        private const string k_AnimatorPulseName = "Pulse";
        private const string k_AnimatorCellsName = "CountingCellAnimator";
        private const float k_PulseTargetScale = 1.5f;
        private const float k_PulsePerSec = 1.1f;
        private const float k_LevelMsgScales = 1.3f;
        private const float k_ElapsedTimeForNextCountUpdate = 0.8f;
        private const float k_TimeSpanUntilStart = 2.5f;
        private const int k_AmountOfCountingNumbers = 3;
        private readonly TextBlock r_LevelMessage;
        private readonly Sprite r_CountingDownMessage;
        private TimeSpan m_ElapsedTimeForNextCountUpdate;
        private TimeSpan m_TimeSpanUntilStart;

        public LevelTransitionScreen(Game i_Game, int i_Level)
            : base(i_Game)
        {
            this.m_ElapsedTimeForNextCountUpdate = TimeSpan.FromSeconds(k_ElapsedTimeForNextCountUpdate);
            this.m_TimeSpanUntilStart = TimeSpan.FromSeconds(k_TimeSpanUntilStart);
            this.r_LevelMessage = new TextBlock(k_FontName, i_Game);
            this.r_LevelMessage.Text = string.Format(
                @"
Level {0}",
                i_Level);
            this.r_LevelMessage.TextColor = Color.BlueViolet;
            this.Add(r_LevelMessage);
            this.r_CountingDownMessage = new Sprite(k_CountingDownMessageAssetName, i_Game);
            this.Add(r_CountingDownMessage);
            this.BlendState = Microsoft.Xna.Framework.Graphics.BlendState.NonPremultiplied;
        }

        public override void Initialize()
        {
            base.Initialize();
            r_LevelMessage.PositionOrigin = r_LevelMessage.SourceRectangleCenter;
            r_LevelMessage.RotationOrigin = r_LevelMessage.SourceRectangleCenter;
            r_LevelMessage.Position = new Vector2(CenterOfViewPort.X, CenterOfViewPort.Y);
            r_CountingDownMessage.SourceRectangle = new Rectangle(0, 0, (int)r_CountingDownMessage.Width, (int)r_CountingDownMessage.Height / k_AmountOfCountingNumbers);
            CompositeAnimator compositeAnimator = new CompositeAnimator(r_CountingDownMessage);
            PulseAnimator pulseAnimator = new PulseAnimator(k_AnimatorPulseName, TimeSpan.FromSeconds(k_TimeSpanUntilStart), k_PulseTargetScale, k_PulsePerSec);
            CellAnimator cellAnimator = new CellAnimator(ref m_ElapsedTimeForNextCountUpdate, k_AmountOfCountingNumbers, TimeSpan.FromSeconds(k_TimeSpanUntilStart), k_AnimatorCellsName);
            compositeAnimator.Add(pulseAnimator);
            compositeAnimator.Add(cellAnimator);
            cellAnimator.Finished += animationsFinished;
            r_CountingDownMessage.Animations.Add(compositeAnimator);
            r_CountingDownMessage.Animations.Resume();
            r_CountingDownMessage.PositionOrigin = r_CountingDownMessage.SourceRectangleCenter;
            r_CountingDownMessage.RotationOrigin = r_CountingDownMessage.SourceRectangleCenter;
            r_CountingDownMessage.Position = new Vector2(CenterOfViewPort.X, r_LevelMessage.Position.Y + r_LevelMessage.Height);
            r_LevelMessage.Scales = new Vector2(k_LevelMsgScales);
            r_CountingDownMessage.Scales = Vector2.Zero;
        }

        private void animationsFinished(object i_Sender, EventArgs i_E)
        {
            ExitScreen();
            ScreensManager.SetCurrentScreen(r_GameManager.PlayScreen);
        }
    }
}
