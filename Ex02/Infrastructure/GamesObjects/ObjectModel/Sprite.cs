////*** Guy Ronen (c) 2008-2011 ***////
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.ServiceInterfaces;
using Infrastructure.GamesObjects.ObjectModel.Animators;
using Infrastructure.GamesObjects.ServiceInterfaces;

namespace Infrastructure.ObjectModel
{
    public class Sprite : LoadableDrawableComponent, ISprite
    {
        protected Texture2D m_Texture;
        protected float m_WidthBeforeScale;
        protected float m_HeightBeforeScale;
        protected Vector2 m_Scales = Vector2.One;
        public Vector2 m_PositionOrigin;
        public Vector2 m_RotationOrigin = Vector2.Zero;
        protected Rectangle m_SourceRectangle;
        protected Vector2 m_Position = Vector2.Zero;
        private float m_AngularVelocity = 0;
        protected Vector2 m_Velocity = Vector2.Zero;
        protected SpriteEffects m_SpriteEffects = SpriteEffects.None;
        protected float m_LayerDepth;
        protected float m_Rotation = 0;
        protected Color m_TintColor = Color.White;
        private bool m_UseSharedBatch = true;
        protected SpriteBatch m_SpriteBatch;
        protected CompositeAnimator m_Animations;
        public Color[] m_Pixels;

        public Sprite(string i_AssetName, Game i_Game, int i_UpdateOrder, int i_DrawOrder)
       : base(i_AssetName, i_Game, i_UpdateOrder, i_DrawOrder)
        { 
        }

        public Sprite(string i_AssetName, Game i_Game, int i_CallsOrder)
            : base(i_AssetName, i_Game, i_CallsOrder)
        {
        }

        public Sprite(string i_AssetName, Game i_Game)
            : base(i_AssetName, i_Game, int.MaxValue)
        { 
        }

        public CompositeAnimator Animations
        {
            get { return m_Animations; }
            set { m_Animations = value; }
        }

        public Color GetPixelAt(int i_Row, int i_Col)
        {
            return m_Pixels[(i_Row * (int)this.Width) + i_Col];
        }

        public Texture2D Texture
        {
            get { return m_Texture; }
            set { m_Texture = value; }
        }

        public float Width
        {
            get { return m_WidthBeforeScale * m_Scales.X; }
            set { m_WidthBeforeScale = value / m_Scales.X; }
        }

        public float Height
        {
            get { return m_HeightBeforeScale * m_Scales.Y; }
            set { m_HeightBeforeScale = value / m_Scales.Y; }
        }

        public float WidthBeforeScale
        {
            get { return m_WidthBeforeScale; }
            set { m_WidthBeforeScale = value; }
        }

        public float HeightBeforeScale
        {
            get { return m_HeightBeforeScale; }
            set { m_HeightBeforeScale = value; }
        }

        public Vector2 Position
        {
            get { return m_Position; }
            set
            {
                if (m_Position != value)
                {
                    m_Position = value;
                    OnPositionChanged();
                }
            }
        }

        public Vector2 PositionOrigin
        {
            get { return m_PositionOrigin; }
            set { m_PositionOrigin = value; }
        }

        public Vector2 RotationOrigin
        {
            get { return m_RotationOrigin; }
            set { m_RotationOrigin = value; }
        }

        protected virtual Vector2 PositionForDraw
        {
            get { return this.Position - this.PositionOrigin + this.RotationOrigin; }
        }

        public Vector2 TopLeftPosition
        {
            get { return this.Position - this.PositionOrigin; }
            set { this.Position = value + this.PositionOrigin; }
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)TopLeftPosition.X,
                    (int)TopLeftPosition.Y,
                    (int)this.Width,
                    (int)this.Height);
            }
        }

        public Rectangle BoundsBeforeScale
        {
            get
            {
                return new Rectangle(
                    (int)TopLeftPosition.X,
                    (int)TopLeftPosition.Y,
                    (int)this.WidthBeforeScale,
                    (int)this.HeightBeforeScale);
            }
        }

        public Rectangle SourceRectangle
        {
            get { return m_SourceRectangle; }
            set { m_SourceRectangle = value; }
        }

        public Vector2 TextureCenter
        {
            get
            {
                return new Vector2((float)(m_Texture.Width / 2), (float)(m_Texture.Height / 2));
            }
        }

        public Vector2 SourceRectangleCenter
        {
            get { return new Vector2((float)(m_SourceRectangle.Width / 2), (float)(m_SourceRectangle.Height / 2)); }
        }

        public float Rotation
        {
            get { return m_Rotation; }
            set { m_Rotation = value; }
        }

        public Vector2 Scales
        {
            get { return m_Scales; }
            set
            {
                if (m_Scales != value)
                {
                    m_Scales = value;
                    OnPositionChanged();
                }
            }
        }

        public Color TintColor
        {
            get { return m_TintColor; }
            set { m_TintColor = value; }
        }

        public Color[] Pixels
        {
            get
            {
                return m_Pixels;
            }

            set
            {
                m_Pixels = value;
            }
        }

        public float Opacity
        {
            get
            {
                return (float)m_TintColor.A / (float)byte.MaxValue;
            }

            set
            {
                m_TintColor.A = (byte)(value * (float)byte.MaxValue);
            }
        }

        public float LayerDepth
        {
            get { return m_LayerDepth; }
            set { m_LayerDepth = value; }
        }

        public SpriteEffects SpriteEffects
        {
            get { return m_SpriteEffects; }
            set { m_SpriteEffects = value; }
        }

        public Vector2 Velocity
        {
            get { return m_Velocity; }
            set { m_Velocity = value; }
        }

        public float AngularVelocity
        {
            get { return m_AngularVelocity; }
            set { m_AngularVelocity = value; }
        }

        protected override void InitBounds()
        {
            m_WidthBeforeScale = m_Texture.Width;
            m_HeightBeforeScale = m_Texture.Height;
            m_Position = Vector2.Zero;
            InitSourceRectangle();
        }

        protected virtual void InitSourceRectangle()
        {
            m_SourceRectangle = new Rectangle(0, 0, (int)m_WidthBeforeScale, (int)m_HeightBeforeScale);
        }

        public SpriteBatch SpriteBatch
        {
            set
            {
                m_SpriteBatch = value;
                m_UseSharedBatch = true;
            }
        }

        public override void Initialize()
        {
            m_Animations = new CompositeAnimator(this);
            base.Initialize();
            InitBounds();
        }

        protected override void LoadContent()
        {
            m_Texture = Game.Content.Load<Texture2D>(m_AssetName);
            m_Pixels = new Color[m_Texture.Height * m_Texture.Width];
            m_Texture.GetData<Color>(m_Pixels);

            if (m_SpriteBatch == null)
            {
                SpriteBatch = Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

                if (m_SpriteBatch == null)
                {
                    m_SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
                    m_UseSharedBatch = false;
                }
            }

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            float totalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.Position += this.Velocity * totalSeconds;
            this.Rotation += this.AngularVelocity * totalSeconds;

            base.Update(gameTime);

            this.Animations.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (!m_UseSharedBatch)
            {
                m_SpriteBatch.Begin();
            }

            m_SpriteBatch.Draw(
                m_Texture,
                this.PositionForDraw,
                this.SourceRectangle,
                this.TintColor,
                this.Rotation,
                this.RotationOrigin,
                this.Scales,
                SpriteEffects.None,
                this.LayerDepth);

            if (!m_UseSharedBatch)
            {
                m_SpriteBatch.End();
            }

            base.Draw(gameTime);
        }

        public virtual bool CheckCollision(ICollidable i_Source)
        {
            bool collided = false;
            ICollidable2D source = i_Source as ICollidable2D;
            if (source != null)
            {
                collided = source.Bounds.Intersects(this.Bounds);
            }

            return collided;
        }

        public virtual void Collided(ICollidable i_Collidable)
        {
            // Defualt Behavior
            this.Velocity *= -1;
        }

        public Sprite ShallowClone()
        {
            return this.MemberwiseClone() as Sprite;
        }

        public virtual void UpdateXPosition(float i_UpdateXValue)
        {
            float currentXPosition = this.m_Position.X + i_UpdateXValue;
            currentXPosition = MathHelper.Clamp(currentXPosition, 0, Game.GraphicsDevice.Viewport.Width - Width);
            this.Position = new Vector2(currentXPosition, this.Position.Y);
        }

        public virtual void UpdateYPosition(float i_UpdateYValue)
        {
            float currentYPosition = this.m_Position.Y + i_UpdateYValue;
            currentYPosition = MathHelper.Clamp(currentYPosition, 0, Game.GraphicsDevice.Viewport.Height - Height);
            this.Position = new Vector2(this.Position.X, currentYPosition);
        }

        public void SetTransparentPixelsOnSprite(Rectangle i_Rectangle)
        {
            int rectangleBottom = i_Rectangle.Bottom - Bounds.Y;
            int rectangleTop = i_Rectangle.Top - Bounds.Y;
            int rectangleRight = i_Rectangle.Right - Bounds.X;
            int rectangleLeft = i_Rectangle.Left - Bounds.X;

            for (int i = rectangleLeft; i <= rectangleRight && rectangleRight < Bounds.Width; i++)
            {
                for (int j = rectangleTop; j <= rectangleBottom && rectangleBottom < Bounds.Height; j++)
                {
                    Pixels[(j * Bounds.Width) + i] = Color.FromNonPremultiplied(0, 0, 0, 0);
                }
            }

            Texture.SetData<Color>(Pixels);
        }

        public Rectangle GetPartialIntersectRectanglePerPixelCollision(ICollidable i_TargetCollidable, bool i_IsCollissionFromBottom, float i_PrecentageOfHit)
        {            
            Rectangle resultRectangle = new Rectangle();
            Sprite i_TargetSprite = i_TargetCollidable as Sprite;
            int topLeftCornerX = 0;
            int topLeftCornerY = 0;
            int bottomRightCornerY = 0;
            int bottomRightCornerX = 0;

            if (i_TargetSprite != null)
            {
                topLeftCornerX = MathHelper.Max((int)i_TargetSprite.Position.X, (int)Position.X);
                bottomRightCornerX = MathHelper.Min((int)i_TargetSprite.Position.X + (int)i_TargetSprite.Width, (int)Position.X + (int)Width);
                if (!i_IsCollissionFromBottom)
                {
                    // Collided from top
                    topLeftCornerY = MathHelper.Max((int)i_TargetSprite.Position.Y + (int)i_TargetSprite.Height - 2, (int)Position.Y); //// Offsetting bounds
                    bottomRightCornerY = MathHelper.Min(topLeftCornerY + (int)(i_TargetSprite.Height * i_PrecentageOfHit), (int)Position.Y + (int)Height);
                }
                else
                {
                    // Collided from bottom
                    bottomRightCornerY = MathHelper.Min((int)i_TargetSprite.Position.Y + 1, (int)Position.Y + (int)Height - 1); //// Offsetting bounds
                    topLeftCornerY = MathHelper.Max(bottomRightCornerY - (int)(i_TargetSprite.Height * i_PrecentageOfHit), (int)Position.Y);
                }
            }

            resultRectangle = new Rectangle(topLeftCornerX, topLeftCornerY, bottomRightCornerX - topLeftCornerX - 1, bottomRightCornerY - topLeftCornerY);
            return resultRectangle;
        }

        public Rectangle CheckPerPixelCollision(ICollidable i_TargetCollidable, ref bool o_IsPerPixelCollided)
        {
            o_IsPerPixelCollided = false;
            Sprite i_TargetSprite = i_TargetCollidable as Sprite;
            int topLeftCornerX = MathHelper.Max(Bounds.X, i_TargetSprite.Bounds.X);
            int bottomRightCornerX = MathHelper.Min(Bounds.X + Bounds.Width, i_TargetSprite.Bounds.X + i_TargetSprite.Bounds.Width);
            int topLeftCornerY = MathHelper.Max(Bounds.Y, i_TargetSprite.Bounds.Y);
            int bottomRightCornerY = MathHelper.Min(Bounds.Y + Bounds.Height, i_TargetSprite.Bounds.Y + i_TargetSprite.Bounds.Height);
            Rectangle intersectionRectangle = new Rectangle(topLeftCornerX, topLeftCornerY, bottomRightCornerX - topLeftCornerX - 1, bottomRightCornerY - topLeftCornerY - 1);

            for (int x = intersectionRectangle.Left; x < intersectionRectangle.Right && !o_IsPerPixelCollided; x++)
            {
                for (int y = intersectionRectangle.Top; y < intersectionRectangle.Bottom && !o_IsPerPixelCollided; y++)
                {
                    Color targetColor = m_Pixels[(x - this.Bounds.Left) + ((y - this.Bounds.Top) * this.Bounds.Width)];
                    Color sourceColor = i_TargetSprite.m_Pixels[(x - i_TargetSprite.Bounds.Left) + ((y - i_TargetSprite.Bounds.Top) * i_TargetSprite.Bounds.Width)];

                    if (targetColor.A != 0 && sourceColor.A != 0)
                    {
                        o_IsPerPixelCollided = true;
                    }
                }
            }

            return intersectionRectangle;
        }
    }
}