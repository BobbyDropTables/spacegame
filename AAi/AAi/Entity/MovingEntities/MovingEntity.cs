using AAI.behaviour;
using AAI.world;
using Microsoft.Xna.Framework;

namespace AAI.Entity.MovingEntities
{
    internal abstract class MovingEntity : BaseGameEntity
    {
        public Vector2 Velocity { get; set; }
        public float    Mass     { get; set; }
        public float    MaxSpeed { get; set; }
        public float    Maxturn  { get; set; }

        public SteeringBehaviour SB { get; set; }

        public MovingEntity(Vector2 pos, World w) : base(pos, w)
        {
            Mass     = 4f;
            MaxSpeed = 10;
            Maxturn  = 2;
            Velocity = new Vector2();
        }

        public override void Update()
        {
            var steeringforce = SB.Calculate();
            var acceleration = steeringforce/Mass;
            Velocity += (acceleration);
            if (Velocity.Length() > MaxSpeed)
            {
                Velocity.Normalize();
                Velocity *= MaxSpeed;
            }
            this.Pos += Velocity;
        }

        public override string ToString()
        {
            return string.Format("{0}", Velocity);
        }
    }
}