using System.Collections.Generic;
using Infrastructure.GamesObjects.ObjectModel;
using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;

namespace SpaceInvadersGame.SpaceInvadersObjects
{
    internal class PlayerLifesBoard
    {
        private const int k_GapFromRightBound = 10;
        private List<Sprite> m_LivesTextureList;
        private Player m_Player;
        private Game m_Game;
        private string m_AssetName;

        internal PlayerLifesBoard(Game i_Game, string i_AssetName, Player i_Player)
        {
            this.m_Game = i_Game;
            this.m_AssetName = i_AssetName;
            this.m_Player = i_Player;
            this.m_LivesTextureList = new List<Sprite>();
            createSpaceShipLives();
        }

        private void createSpaceShipLives()
        {
            for (int i = 0; i < m_Player.AmountOfLives; i++)
            {
                Sprite currentLife = new Life(m_Game, m_AssetName);
                currentLife.Initialize();
                m_LivesTextureList.Add(currentLife);
                currentLife.Position = new Vector2(m_Game.GraphicsDevice.Viewport.Width - k_GapFromRightBound - (currentLife.Width * m_Player.AmountOfLives) + (i * currentLife.Width), currentLife.Width * m_Player.PlayerId);
            }
        }

        internal void RemoveLife()
        {
            m_Game.Components.Remove(m_LivesTextureList[0]);
            m_LivesTextureList.RemoveAt(0);
        }

        internal List<Sprite> LivesTextureList
        {
            get
            {
                return m_LivesTextureList;
            }
        }
    }
}
