using Infrastructure.GamesObjects.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;

namespace SpaceInvadersGame.SpaceInvadersObjects
{
    internal class PlayerScoreBoard : TextBlock
    {
        private const string k_FontName = @"Fonts\Consolas";
        private const string k_ScoreMessage = "Score";
        private int k_GapFromLeftBound = 10;        
        private Player m_Player;

        internal PlayerScoreBoard(GameScreen i_GameScreen, Player i_Player) : base(k_FontName, i_GameScreen.Game)
        {
            this.m_Player = i_Player;
            this.TextColor = i_Player.PlayerColor;
            i_GameScreen.Add(this);
        }

        public override void Initialize()
        {
            base.Initialize();
            BoardScoreChange();
            this.Position = new Vector2(k_GapFromLeftBound, this.MeasureString.Y * m_Player.PlayerId);
        }

        internal void BoardScoreChange()
        {
            this.Text = string.Format("{0} {1} : {2}", m_Player.PlayerName, k_ScoreMessage, m_Player.PlayerScore);
        }

        internal string BoardScoreString()
        {
            return string.Format("{0} {1} : {2}", m_Player.PlayerName, k_ScoreMessage, m_Player.PlayerScore);
        }

        protected override void Dispose(bool i_Disposing)
        {
            base.Dispose(i_Disposing);
            this.Visible = false;
        }

        public void CleanScore()
        {
            this.Dispose();
        }
    }
}
