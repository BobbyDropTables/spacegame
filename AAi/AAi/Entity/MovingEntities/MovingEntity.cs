using System.Collections.Generic;
using AAI.behaviour;
using AAI.world;
using Microsoft.Xna.Framework;

namespace AAI.Entity.MovingEntities
{
    internal abstract class MovingEntity : BaseGameEntity
    {
        public Vector2                 Velocity { get; set; }
        public float                   Mass     { get; set; }
        public float                   MaxSpeed { get; set; }
        public float                   Maxturn  { get; set; }
        public SteeringBehaviour       SB       { get; set; }
        public List<SteeringBehaviour> Behaviours;


        public MovingEntity(Vector2 pos, World w) : base(pos, w)
        {
            Mass     = 4f;
            MaxSpeed = 10;
            Maxturn  = 2;
            Velocity = new Vector2();
        }

        public override void Update()
        {
            Vector2 steeringForce = SB.Calculate();
            foreach (SteeringBehaviour behaviour in Behaviours)
                steeringForce += behaviour.Calculate();
            Vector2 acceleration  = steeringForce / Mass;
            Velocity += acceleration;
            if (Velocity.Length() > MaxSpeed)
            {
                Velocity.Normalize();
                Velocity *= MaxSpeed;
            }

            Pos += Velocity;
        }

        public override string ToString()
        {
            return string.Format("{0}", Velocity);
        }
    }
}