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
            Textures.Add("Arrow", _content.Load<Texture2D>("Entities/Green_Arrow"));
            Textures.Add("Target", _content.Load<Texture2D>("Entities/X"));
            Textures.Add("Line", _content.Load<Texture2D>("Entities/Line"));
            Textures.Add("Vertex", _content.Load<Texture2D>("Entities/vertex"));
            Textures.Add("Pixel", _content.Load<Texture2D>("Entities/pixel"));
        }
    }
}
