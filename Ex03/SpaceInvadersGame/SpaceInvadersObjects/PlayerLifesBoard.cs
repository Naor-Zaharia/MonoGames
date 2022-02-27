using System.Collections.Generic;
using Infrastructure.GamesObjects.Managers;
using Infrastructure.GamesObjects.ObjectModel;
using Infrastructure.GamesObjects.ServiceInterfaces;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;

namespace SpaceInvadersGame.SpaceInvadersObjects
{
    internal class PlayerLifesBoard
    {
        private const int k_GapFromRightBound = 10;
        private const string k_LifDieSoundName = "LifeDie";
        private readonly SoundManager r_SoundManager;
        private List<Sprite> m_LivesTextureList;
        private Player m_Player;
        private GameScreen m_GameScreen;        
        private string m_AssetName;

        internal PlayerLifesBoard(GameScreen i_GameScreen, string i_AssetName, Player i_Player)
        {
            this.m_GameScreen = i_GameScreen;
            this.m_AssetName = i_AssetName;
            this.m_Player = i_Player;
            this.m_LivesTextureList = new List<Sprite>();
            this.r_SoundManager = (SoundManager)i_GameScreen.Game.Services.GetService(typeof(ISoundManager));
            createSpaceShipLives();
        }

        private void createSpaceShipLives()
        {
            for (int i = 0; i < m_Player.AmountOfLives; i++)
            {
                Sprite currentLife = new Life(m_GameScreen, m_AssetName);
                currentLife.Initialize();
                m_LivesTextureList.Add(currentLife);
                currentLife.Position = new Vector2(m_GameScreen.GraphicsDevice.Viewport.Width - k_GapFromRightBound - (currentLife.Width * m_Player.AmountOfLives) + (i * currentLife.Width), currentLife.Width * m_Player.PlayerId);
            }
        }

        internal void RemoveLife()
        {
            if (m_LivesTextureList.Count != 0)
            {
                r_SoundManager.PlaySound(k_LifDieSoundName);
                m_GameScreen.Remove(m_LivesTextureList[0]);
                m_LivesTextureList.RemoveAt(0);
            }
        }

        internal List<Sprite> LivesTextureList
        {
            get
            {
                return m_LivesTextureList;
            }
        }

        public void CleanLifeBorad()
        {
            foreach(Sprite currentLife in m_LivesTextureList)
            {
                currentLife.Visible = false;
                currentLife.Dispose();
            }
        }
    }
}
