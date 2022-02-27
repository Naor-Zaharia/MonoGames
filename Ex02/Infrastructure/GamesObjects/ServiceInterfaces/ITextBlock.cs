using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Infrastructure.GamesObjects.ServiceInterfaces
{
    public interface ITextBlock : ISprite
    {
        string Text { get; set; }

        SpriteFont FontName { get; set; }

        bool IsRightToLeft { get; set; }

        Vector2 MeasureString { get; }
    }
}
