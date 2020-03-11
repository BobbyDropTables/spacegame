using Game.view;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.entity
{
    public abstract class BaseGameEntity
    {
        public Vector2 Pos { get; set; }
        public float Scale { get; set; }
        public World MyWorld { get; set; }
        public Texture2D Texture;

        protected BaseGameEntity(Vector2 pos, World w)
        {
            Pos = pos;
            MyWorld = w;
            Scale = 20;
        }

        public virtual void Update()
        {
        }

        public virtual void Render(SpriteBatch spriteBatch)
        {

            Texture = TextureStorage.Textures["Target"];
            spriteBatch.Draw(Texture,
                             new Rectangle((int)Pos.X, (int)Pos.Y, 100, 100),
                             null,
                             Color.White);
        }
        

    }
    


    

    
}
