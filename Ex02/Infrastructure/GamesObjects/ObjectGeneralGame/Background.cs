using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;

namespace Infrastructure.ObjectGeneralGame
{
    public class Background : Sprite
    {
        private readonly float r_BackgroundPercentage;

        public Background(Game i_Game, string i_AssetName, int i_DrawOrder, int i_UpdateOrder, float i_BackgroundPercentage)
            : base(i_AssetName, i_Game, i_DrawOrder, i_UpdateOrder)
        {
            this.m_AssetName = i_AssetName;
            this.TintColor = Color.White;
            this.r_BackgroundPercentage = i_BackgroundPercentage;
            this.Initialize();
        }

        public float BackgroundPercentage
        {
            get
            {
                return r_BackgroundPercentage;
            }
        }
    }
}
