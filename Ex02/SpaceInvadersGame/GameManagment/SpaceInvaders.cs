using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvadersGame
{
    public class SpaceInvaders : Game
    {
        private const string k_GameTitle = "Space Invaders";
        private const string k_RootDir = "Content";
        private SpriteBatch m_SpriteBatch;
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
            m_GameManager = new SpaceInvadersGameManager(this, m_SpriteBatch, m_GraphicsDeviceManager);
        }

        protected override void LoadContent()
        {
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
        }

        protected override void Draw(GameTime i_GameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            m_SpriteBatch.Begin();
            base.Draw(i_GameTime);
            m_SpriteBatch.End();
        }
    }
}
