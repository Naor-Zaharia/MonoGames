//*** Guy Ronen © 2008-2011 ***//
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.ServiceInterfaces;
using Infrastructure.ObjectModel.Animators;
using Infrastructure.GamesObjects.ObjectModel.Animators;
using System.Reflection.Metadata.Ecma335;
using System;

namespace Infrastructure.ObjectModel
{
    public class Sprite : LoadableDrawableComponent
    {
        protected CompositeAnimator m_Animations;
        public Color[] m_Pixels;

        public CompositeAnimator Animations
        {
            get { return m_Animations; }
            set { m_Animations = value; }
        }

        private Texture2D m_Texture;
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

        protected float m_WidthBeforeScale;
        public float WidthBeforeScale
        {
            get { return m_WidthBeforeScale; }
            set { m_WidthBeforeScale = value; }
        }

        protected float m_HeightBeforeScale;
        public float HeightBeforeScale
        {
            get { return m_HeightBeforeScale; }
            set { m_HeightBeforeScale = value; }
        }

        protected Vector2 m_Position = Vector2.Zero;
        /// <summary>
        /// Represents the location of the sprite's origin point in screen coorinates
        /// </summary>
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

        public Vector2 m_PositionOrigin;
        public Vector2 PositionOrigin
        {
            get { return m_PositionOrigin; }
            set { m_PositionOrigin = value; }
        }

        public Vector2 m_RotationOrigin = Vector2.Zero;
        public Vector2 RotationOrigin
        {
            get { return m_RotationOrigin; }// r_SpriteParameters.RotationOrigin; }
            set { m_RotationOrigin = value; }
        }

        protected Vector2 PositionForDraw
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

        protected Rectangle m_SourceRectangle;
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

        protected float m_Rotation = 0;
        public float Rotation
        {
            get { return m_Rotation; }
            set { m_Rotation = value; }
        }

        protected Vector2 m_Scales = Vector2.One;
        public Vector2 Scales
        {
            get { return m_Scales; }
            set
            {
                if (m_Scales != value)
                {
                    m_Scales = value;
                    // Notify the Collision Detection mechanism:
                    OnPositionChanged();
                }
            }
        }

        protected Color m_TintColor = Color.White;
        public Color TintColor
        {
            get { return m_TintColor; }
            set { m_TintColor = value; }
        }

        public float Opacity
        {
            get { return (float)m_TintColor.A / (float)byte.MaxValue; }
            set { m_TintColor.A = (byte)(value * (float)byte.MaxValue); }
        }

        protected float m_LayerDepth;
        public float LayerDepth
        {
            get { return m_LayerDepth; }
            set { m_LayerDepth = value; }
        }

        protected SpriteEffects m_SpriteEffects = SpriteEffects.None;
        public SpriteEffects SpriteEffects
        {
            get { return m_SpriteEffects; }
            set { m_SpriteEffects = value; }
        }

        protected SpriteSortMode m_SortMode = SpriteSortMode.Deferred;
        public SpriteSortMode SortMode
        {
            get { return m_SortMode; }
            set { m_SortMode = value; }
        }

        protected BlendState m_BlendState = BlendState.AlphaBlend;
        public BlendState BlendState
        {
            get { return m_BlendState; }
            set { m_BlendState = value; }
        }

        protected SamplerState m_SamplerState = null;
        public SamplerState SamplerState
        {
            get { return m_SamplerState; }
            set { m_SamplerState = value; }
        }

        protected DepthStencilState m_DepthStencilState = null;
        public DepthStencilState DepthStencilState
        {
            get { return m_DepthStencilState; }
            set { m_DepthStencilState = value; }
        }

        protected RasterizerState m_RasterizerState = null;
        public RasterizerState RasterizerState
        {
            get { return m_RasterizerState; }
            set { m_RasterizerState = value; }
        }

        protected Effect m_Shader = null;
        public Effect Shader
        {
            get { return m_Shader; }
            set { m_Shader = value; }
        }

        protected Matrix m_TransformMatrix = Matrix.Identity;
        public Matrix TransformMatrix
        {
            get { return m_TransformMatrix; }
            set { m_TransformMatrix = value; }
        }

        protected Vector2 m_Velocity = Vector2.Zero;
        /// <summary>
        /// Pixels per Second on 2 axis
        /// </summary>
        public Vector2 Velocity
        {
            get { return m_Velocity; }
            set { m_Velocity = value; }
        }

        private float m_AngularVelocity = 0;
        /// <summary>
        /// Radians per Second on X Axis
        /// </summary>
        public float AngularVelocity
        {
            get { return m_AngularVelocity; }
            set { m_AngularVelocity = value; }
        }

        public Sprite(string i_AssetName, Game i_Game, int i_UpdateOrder, int i_DrawOrder)
            : base(i_AssetName, i_Game, i_UpdateOrder, i_DrawOrder)
        { }

        public Sprite(string i_AssetName, Game i_Game, int i_CallsOrder)
            : base(i_AssetName, i_Game, i_CallsOrder)
        { }

        public Sprite(string i_AssetName, Game i_Game)
            : base(i_AssetName, i_Game, int.MaxValue)
        { }

        /// <summary>
        /// Default initialization of bounds
        /// </summary>
        /// <remarks>
        /// Derived classes are welcome to override this to implement their specific boudns initialization
        /// </remarks>
        protected override void InitBounds()
        {
            m_WidthBeforeScale = m_Texture.Width;
            m_HeightBeforeScale = m_Texture.Height;
            m_Position = Vector2.Zero;

            InitSourceRectangle();

            InitOrigins();
        }

        protected virtual void InitOrigins()
        {
        }

        protected virtual void InitSourceRectangle()
        {
            m_SourceRectangle = new Rectangle(0, 0, (int)m_WidthBeforeScale, (int)m_HeightBeforeScale);
        }


        protected bool m_UseSharedBatch = false;
        protected SpriteBatch m_SpriteBatch;
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
            base.Initialize();

            m_Animations = new CompositeAnimator(this);
        }

        protected override void LoadContent()
        {
            m_Texture = Game.Content.Load<Texture2D>(m_AssetName);
            m_Pixels = new Color[m_Texture.Height * m_Texture.Width];
            m_Texture.GetData<Color>(m_Pixels);

            if (m_SpriteBatch == null)
            {
                m_SpriteBatch =
                    Game.Services.GetService(typeof(SpriteBatch)) as SpriteBatch;

                if (m_SpriteBatch == null)
                {
                    m_SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
                    m_UseSharedBatch = false;
                }
            }

            base.LoadContent();
        }

        /// <summary>
        /// Basic movement logic (position += velocity * totalSeconds)
        /// </summary>
        /// <param name="gameTime"></param>
        /// <remarks>
        /// Derived classes are welcome to extend this logic.
        /// </remarks>
        public override void Update(GameTime gameTime)
        {
            float totalSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.Position += this.Velocity * totalSeconds;
            this.Rotation += this.AngularVelocity * totalSeconds;

            base.Update(gameTime);

            this.Animations.Update(gameTime);
        }

        class DeviceStates
        {
            public BlendState BlendState;
            public SamplerState SamplerState;
            public DepthStencilState DepthStencilState;
            public RasterizerState RasterizerState;
        }

        DeviceStates m_SavedDeviceStates = new DeviceStates();
        protected void saveDeviceStates()
        {
            m_SavedDeviceStates.BlendState = GraphicsDevice.BlendState;
            m_SavedDeviceStates.SamplerState = GraphicsDevice.SamplerStates[0];
            m_SavedDeviceStates.DepthStencilState = GraphicsDevice.DepthStencilState;
            m_SavedDeviceStates.RasterizerState = GraphicsDevice.RasterizerState;
        }

        private void restoreDeviceStates()
        {
            GraphicsDevice.BlendState = m_SavedDeviceStates.BlendState;
            GraphicsDevice.SamplerStates[0] = m_SavedDeviceStates.SamplerState;
            GraphicsDevice.DepthStencilState = m_SavedDeviceStates.DepthStencilState;
            GraphicsDevice.RasterizerState = m_SavedDeviceStates.RasterizerState;
        }

        protected bool m_SaveAndRestoreDeviceState = false;
        public bool SaveAndRestoreDeviceState
        {
            get { return m_SaveAndRestoreDeviceState; }
            set { m_SaveAndRestoreDeviceState = value; }
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

        public Color GetPixelAt(int i_Row, int i_Col)
        {
            return m_Pixels[(i_Row * (int)this.Width) + i_Col];
        }

        public void ChangeOpacityPixelAt(int i_Row, int i_Col, byte i_Opacity)
        {
            m_Pixels[(i_Row * (int)this.Width) + i_Col].A = i_Opacity;
        }

        public override void Draw(GameTime gameTime)
        {
            if (!m_UseSharedBatch)
            {
                if (SaveAndRestoreDeviceState)
                {
                    saveDeviceStates();
                }

                m_SpriteBatch.Begin(
                    SortMode, BlendState, SamplerState,
                    DepthStencilState, RasterizerState, Shader, TransformMatrix);
            }

            m_SpriteBatch.Draw(m_Texture, this.PositionForDraw,
                 this.SourceRectangle, this.TintColor,
                this.Rotation, this.RotationOrigin, this.Scales,
                SpriteEffects.None, this.LayerDepth);

            if (!m_UseSharedBatch)
            {
                m_SpriteBatch.End();

                if (SaveAndRestoreDeviceState)
                {
                    restoreDeviceStates();
                }
            }

            base.Draw(gameTime);
        }

        #region Collision Handlers
        protected override void DrawBoundingBox()
        {
            // not implemented yet
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
            // defualt behavior - change direction:
            this.Velocity *= -1;
        }
        #endregion //Collision Handlers

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

            for (int i = rectangleLeft; i <= rectangleRight; i++)
            {
                for (int j = rectangleTop; j <= rectangleBottom; j++)
                {
                    if ((i < Bounds.Width && i >= 0) && (j < Bounds.Height && j >= 0))
                    {
                        Pixels[(j * Bounds.Width) + i] = Color.FromNonPremultiplied(0, 0, 0, 0);
                    }
                }
            }

            Texture.SetData<Color>(Pixels);
        }

        public Rectangle CheckPerPixelCollision(ICollidable i_TargetCollidable, ref bool o_IsPerPixelCollided)
        {
            o_IsPerPixelCollided = false;
            Sprite i_TargetSprite = i_TargetCollidable as Sprite;
            Vector2 pointOfCollision = Vector2.Zero;


            Rectangle intersectionRectangle = GetIntersectionRectangle(i_TargetCollidable);

            for (int x = intersectionRectangle.Left; x < intersectionRectangle.Right && !o_IsPerPixelCollided; x++)
            {
                for (int y = intersectionRectangle.Top; y < intersectionRectangle.Bottom && !o_IsPerPixelCollided; y++)
                {
                    Color targetColor = m_Pixels[(x - this.Bounds.Left) + (y - this.Bounds.Top) * this.Bounds.Width];
                    Color sourceColor = i_TargetSprite.m_Pixels[(x - i_TargetSprite.Bounds.Left) + (y - i_TargetSprite.Bounds.Top) * i_TargetSprite.Bounds.Width];

                    if (targetColor != Color.FromNonPremultiplied(0, 0, 0, 0) && sourceColor != Color.FromNonPremultiplied(0, 0, 0, 0))
                    {
                        o_IsPerPixelCollided = true;
                        pointOfCollision = new Vector2(x, y);
                        intersectionRectangle = new Rectangle(x, y, intersectionRectangle.Width, intersectionRectangle.Height);
                    }
                }
            }

            return intersectionRectangle;
        }

        public Rectangle GetIntersectionRectangle(ICollidable i_TargetCollidable)
        {
            Sprite i_TargetSprite = i_TargetCollidable as Sprite;

            int topLeftCornerX = MathHelper.Max(Bounds.X, i_TargetSprite.Bounds.X);
            int bottomRightCornerX = MathHelper.Min(Bounds.X + Bounds.Width, i_TargetSprite.Bounds.X + i_TargetSprite.Bounds.Width);
            int topLeftCornerY = MathHelper.Max(Bounds.Y, i_TargetSprite.Bounds.Y);
            int bottomRightCornerY = MathHelper.Min(Bounds.Y + Bounds.Height, i_TargetSprite.Bounds.Y + i_TargetSprite.Bounds.Height);

            return new Rectangle(topLeftCornerX, topLeftCornerY, bottomRightCornerX - topLeftCornerX, bottomRightCornerY - topLeftCornerY);
        }
    }
}