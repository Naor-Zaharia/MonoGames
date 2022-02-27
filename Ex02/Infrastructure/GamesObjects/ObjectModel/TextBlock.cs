using Infrastructure.GamesObjects.ServiceInterfaces;
using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infrastructure.GamesObjects.ObjectModel
{
    public class TextBlock : Sprite, ITextBlock
    {
        private string m_Text = string.Empty;
        private SpriteFont m_FontName;
        private bool m_IsRightToLeft = false;
        private bool m_UseSharedBatch = true;

        public TextBlock(string i_AssetName, Game i_Game) : base(i_AssetName, i_Game)
        { 
        }

        protected override void LoadContent()
        {
            m_FontName = Game.Content.Load<SpriteFont>(AssetName);
            if (m_SpriteBatch == null)
            {
                m_SpriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

                if (m_SpriteBatch == null)
                {
                    m_SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
                    m_UseSharedBatch = false;
                }
            }
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
        }

        public override void Draw(GameTime i_GameTime)
        {
            if (!m_UseSharedBatch)
            {
                m_SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
            }

            m_SpriteBatch.DrawString(m_FontName, m_Text, m_Position, m_TintColor);

            if (!m_UseSharedBatch)
            {
                m_SpriteBatch.End();
            }
        }

        protected override void InitBounds()
        {
            m_Position = Vector2.Zero;
            InitSourceRectangle();
        }

        public string Text
        {
            get
            {
                return m_Text;
            }

            set
            {
                m_Text = value;
            }
        }

        public SpriteFont FontName
        {
            get
            {
                return m_FontName;
            }

            set
            {
                m_FontName = value;
            }
        }

        public bool IsRightToLeft
        {
            get
            {
                return m_IsRightToLeft;
            }

            set
            {
                m_IsRightToLeft = value;
            }
        }

        public Vector2 MeasureString
        {
            get
            {
                return m_FontName.MeasureString(m_Text);
            }
        }
    }
}
