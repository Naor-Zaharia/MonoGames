using Infrastructure.GamesObjects.ObjectModel.Animators;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infrastructure.GamesObjects.ServiceInterfaces
{
    public interface ISprite
    {        
        void Update(GameTime i_GameTime);
        void Draw(GameTime i_GameTime);
        void Initialize();
        SpriteBatch SpriteBatch { set; }
        Vector2 Velocity { get; set; }
        float AngularVelocity { get; set; }
        SpriteEffects SpriteEffects { get; set; }
        float LayerDepth { get; set; }
        float Opacity { get; set; }
        Color TintColor { get; set; }
        Vector2 Scales { get; set; }
        float Rotation { get; set; }
        Vector2 SourceRectangleCenter { get; }
        Vector2 TextureCenter { get; }
        Rectangle SourceRectangle { get; set; }
        Rectangle BoundsBeforeScale { get; }
        Rectangle Bounds { get; }
        Vector2 TopLeftPosition { get; set; }
        Vector2 RotationOrigin { get; set; }
        Vector2 PositionOrigin { get; set; }
        Vector2 Position { get; set; }
        float HeightBeforeScale { get; set; }
        float WidthBeforeScale { get; set; }
        float Height { get; set; }
        float Width { get; set; }
        Texture2D Texture { get; set; }
        Color GetPixelAt(int i_Row, int i_Col);
        CompositeAnimator Animations { get; set; }
    }
}
