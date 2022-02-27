using System;
using Microsoft.Xna.Framework;

namespace SpaceInvadersGame
{
    internal class MotherShip : Enemy
    {
        private const float k_MotherShipVelocity = 95;
        private const int k_RandomIntervalForMotherShipAppearance = 20;

        private static readonly Random sr_Random = new Random();
        private readonly Color r_MotherShipColor = Color.Red;
        private float m_MotherShipSecondsForNextAppearance;
        private float m_ElapsedTime;
        private bool m_IsCurrentlySweep;

        public MotherShip(Game i_Game, string i_AssetName, int i_DrawOrder, int i_UpdateOrder, Vector2 i_EnemyPosition, Color i_EnemyColor, int i_EnemyValue)
            : base(i_Game, i_AssetName, i_DrawOrder, i_UpdateOrder, i_EnemyPosition, i_EnemyColor, i_EnemyValue)
        {
            this.m_Color = r_MotherShipColor;
            this.m_IsCurrentlySweep = false;
            this.Visible = true;
            i_Game.Components.Add(this);
        }

        public override void Update(GameTime i_GameTime)
        {
            m_ElapsedTime += (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            // Check if it is first generate or need new generate
            if (Position.X >= Game.GraphicsDevice.Viewport.Width + m_Width || (!m_IsCurrentlySweep && Visible))
            {
                GenerateMotherShipNewNextAppearance();
            }

            if (m_IsCurrentlySweep)
            {
                UpdateXPosition(k_MotherShipVelocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds);
            }

            if (!m_IsCurrentlySweep && MotherShipSecondsForNextAppearance <= m_ElapsedTime)
            {
                Visible = true;
                m_IsCurrentlySweep = true;
            }
        }

        protected override void InitBounds()
        {
            base.InitBounds();
            Position = new Vector2(-m_Width, m_Height);
        }

        internal float MotherShipSecondsForNextAppearance
        {
            get
            {
                return m_MotherShipSecondsForNextAppearance;
            }
        }

        internal void GenerateMotherShipNewNextAppearance()
        {
            this.m_MotherShipSecondsForNextAppearance = sr_Random.Next(0, k_RandomIntervalForMotherShipAppearance);
            InitBounds();
            this.m_ElapsedTime = 0;
            this.Visible = false;
            this.m_IsCurrentlySweep = false;
        }

        internal override void UpdateXPosition(float i_UpdateXValue)
        {
            this.Position = new Vector2(Position.X + i_UpdateXValue, Position.Y);
        }
    }
}