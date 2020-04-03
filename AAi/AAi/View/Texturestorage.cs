using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AAI.View
{
    class TextureStorage
    {
        private readonly ContentManager _content;

        // Textures.
        public static Dictionary<string, Texture2D>  Textures;
        public static Dictionary<string, SpriteFont> Fonts;

        public TextureStorage(ContentManager contentManager)
        {
            _content = contentManager;
            Textures = new Dictionary<string, Texture2D>();
            Fonts    = new Dictionary<string, SpriteFont>();
        }

        public void LoadTextures()
        {
            Fonts.Add("Font", _content.Load<SpriteFont>("Font"));
            Textures.Add("Arrow", _content.Load<Texture2D>("Entities/Green_Arrow"));
            Textures.Add("Vacuum", _content.Load<Texture2D>("Entities/vacuum"));
            Textures.Add("Food", _content.Load<Texture2D>("Entities/Cookie"));
            Textures.Add("Bed", _content.Load<Texture2D>("Entities/bed"));
            Textures.Add("Whiskey", _content.Load<Texture2D>("Entities/Whiskey"));
            Textures.Add("Line", _content.Load<Texture2D>("Entities/Line"));
            Textures.Add("Vertex", _content.Load<Texture2D>("Entities/vertex"));
            Textures.Add("Pixel", _content.Load<Texture2D>("Entities/pixel"));
        }
    }
}
