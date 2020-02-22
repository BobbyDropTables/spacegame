using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace entity
{
    internal abstract class MovingEntity : BaseGameEntity
    {
        public Vector2 Velocity { get; set; }
        public float    Mass     { get; set; }
        public float    MaxSpeed { get; set; }
        public float    Maxturn  { get; set; }

        public SteeringBehaviour SB { get; set; }

        public MovingEntity(Vector2 pos, World.World w , Texture2D texture) : base(pos, w, texture)
        {
            Mass     = 30;
            MaxSpeed = 100;
            Maxturn  = 30;
            Velocity = new Vector2();
        }

        public override void Update(GameTime timeElapsed)
        {
            var steeringforce = SB.Calculate();
            var acceleration = Vector2.Divide(steeringforce, Mass);
            Velocity = Vector2.Add(Velocity, Vector2.Multiply(acceleration, timeElapsed.TotalGameTime.Milliseconds));
            if (Velocity.Length() > MaxSpeed)
            {
                Velocity.Normalize();
                Velocity = Vector2.Multiply(Velocity , MaxSpeed);
            }
            this.Pos = Vector2.Add(this.Pos,(Vector2.Multiply(Velocity,timeElapsed.TotalGameTime.Milliseconds)));
        }

        public override string ToString()
        {
            return string.Format("{0}", Velocity);
        }
    }
}