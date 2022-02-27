using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GamesObjects;
using MessageBox = System.Windows.Forms.MessageBox;

namespace SpaceInvadersGame
{
    public class SpaceInvaders : GameBase
    {
        private const string k_MessageBoxTitle = "Game Over: Summary - Score Performance";
        private const string k_GameTitle = "Space Invaders";
        private const string k_RootDir = "Content";
        private GraphicsDeviceManager m_GraphicsDeviceManager;       
        private SpaceInvadersGameManager m_GameManager;

        public SpaceInvaders()
        {
            m_GraphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = k_RootDir;
            IsMouseVisible = false;
        }

        protected override void Initialize()
        {
            base.Initialize();
            this.Window.Title = k_GameTitle;
            m_GameManager = new SpaceInvadersGameManager(this, m_GraphicsDeviceManager);            
        }

        protected override void LoadContent()
        {
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            m_GameManager.Update(i_GameTime);         
        }
        
        protected override void Draw(GameTime i_GameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            m_SpriteBatch.Begin();
            base.Draw(i_GameTime);            
            m_SpriteBatch.End();

            if (m_GameManager.IsGameEnd())
            {
                generateSummaryMessageBox();
            }
        }
        
        private void generateSummaryMessageBox()
        {
            if (MessageBox.Show(m_GameManager.GetEndGameString(), k_MessageBoxTitle, MessageBoxButtons.OK) == DialogResult.OK)
            {
                this.Exit();
            }
        }
    }
}
