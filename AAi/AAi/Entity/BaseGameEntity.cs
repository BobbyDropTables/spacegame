using AAI.world;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

internal abstract class BaseGameEntity
{
    public           Vector2     Pos     { get; set; }
    public           float       Scale   { get; set; }
    public           World MyWorld { get; set; }
    private readonly Texture2D   texture;


    public BaseGameEntity(Vector2 pos, World w, Texture2D texture)
    {
        Pos          = pos;
        MyWorld      = w;
        this.texture = texture;
    }

    public abstract void Update(GameTime delta);

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        var size = (int) Scale * 2;
        spriteBatch.Draw(texture, new Vector2(0, 0),
                         new Rectangle(0, 0, size, size),
                         Color.Aqua);
    }
}