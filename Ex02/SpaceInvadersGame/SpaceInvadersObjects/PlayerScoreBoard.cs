using Infrastructure.GamesObjects.ObjectModel;
using Microsoft.Xna.Framework;

namespace SpaceInvadersGame.SpaceInvadersObjects
{
    internal class PlayerScoreBoard : TextBlock
    {
        private const string k_FontName = @"Fonts\Consolas";
        private const string k_ScoreMessage = "Score";
        private const int k_GapFromLeftBound = 10;
        private Player m_Player;

        internal PlayerScoreBoard(Game i_Game, Player i_Player) : base(k_FontName, i_Game)
        {
            this.m_Player = i_Player;
            this.TintColor = i_Player.PlayerColor;
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
    }
}
