using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GamesObjects
{
    public abstract class Sprite : DrawableGameComponent
    {
        
        protected Color m_Color;
        protected string m_AssetName;
        protected int m_Height;
        protected int m_Width;        
        protected Vector2 m_Velocity = Vector2.Zero;
        protected Rectangle m_Rectangle;
        private Texture2D m_Texture;
        private Vector2 m_Position;
        private bool m_IsInitialize;        

        public Sprite(Game i_Game, string i_AssetName, int i_DrawOrder, int i_UpdateOrder) : base(i_Game)
        {
            this.m_AssetName = i_AssetName;
            this.UpdateOrder = i_UpdateOrder;
            this.DrawOrder = i_DrawOrder;
            this.m_IsInitialize = false;          
        }

        public Texture2D Texture
        {
            get
            {
                return m_Texture;
            }

            set
            {
                m_Texture = value;
            }
        }

        public Color Color
        {
            get
            {
                return m_Color;
            }

            set
            {
                m_Color = value;
            }
        }

        public string AssetName
        {
            get
            {
                return m_AssetName;
            }

            set
            {
                m_AssetName = value;
            }
        }

        public int Height
        {
            get
            {
                return m_Height;
            }

            set
            {
                m_Height = value;
            }
        }

        public int Width
        {
            get
            {
                return m_Width;
            }

            set
            {
                m_Width = value;
            }
        }

        public Vector2 Position
        {
            get
            {
                return m_Position;
            }

            set
            {
                m_Position = value;
                updateSpriteRectangle();
            }
        }

        private void updateSpriteRectangle()
        {
            this.m_Rectangle = new Rectangle((int)m_Position.X, (int)m_Position.Y, m_Width, m_Height);
        }

        public Vector2 Velocity
        {
            get
            {
                return m_Velocity;
            }

            set
            {
                m_Velocity = value;
            }
        }

        public Rectangle Rectangle
        {
            get
            {
                return m_Rectangle;
            }

            set
            {
                this.m_Rectangle = value;
            }
        }

        public override void Initialize()
        {
            if (!m_IsInitialize)
            {
                base.Initialize();
                InitBounds();
                m_IsInitialize = true;
            }
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_Texture = Game.Content.Load<Texture2D>(m_AssetName);
        }

        protected virtual void InitBounds()
        {
            m_Width = m_Texture.Width;
            m_Height = m_Texture.Height;
            m_Position = Vector2.Zero;
        }

        public override void Update(GameTime gameTime)
        {            
            if (!m_IsInitialize)
            {
                Initialize();
            }

            m_Position += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            (Game as GameBase).SpriteBatch.Draw(m_Texture, m_Position, m_Color);
        }

        internal virtual void UpdateXPosition(float i_UpdateXValue)
        {
            this.m_Position.X += i_UpdateXValue;
            this.m_Position.X = MathHelper.Clamp(this.m_Position.X, 0, Game.GraphicsDevice.Viewport.Width - m_Width);
            this.m_Rectangle.X = (int)this.m_Position.X;
        }

        internal virtual void UpdateYPosition(float i_UpdateYValue)
        {
            this.m_Position.Y += i_UpdateYValue;
            this.m_Position.Y = MathHelper.Clamp(this.m_Position.Y, 0, Game.GraphicsDevice.Viewport.Height - m_Height);
            this.m_Rectangle.Y = (int)this.m_Position.Y;
        }
    }
}
