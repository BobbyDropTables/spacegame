using System.Collections.Generic;
using AAI.behaviour;
using AAI.Entity.staticEntities;
using AAI.Pathing;
using AAI.Goals;
using AAI.world;
using Microsoft.Xna.Framework;

namespace AAI.Entity.MovingEntities
{
    public abstract class MovingEntity : BaseGameEntity
    {
        public Vector2 Velocity;
        public Vector2 Heading;
        public float MaxSpeed;
        public List<SteeringBehaviour> Behaviours;
        public Vector2 OldPosition;
        

        public MovingEntity(Vector2 pos, World w) : base(pos, w)
        {
            MaxSpeed = 2f;
            Velocity = new Vector2();
        }

        public override void Update()
        {
            OldPosition = Pos;
            //StateMachine.Update();
            Vector2 steeringForce = new Vector2();
            // Apply all behaviours
            foreach (var behaviour in Behaviours)
            {
                steeringForce += behaviour.Calculate();
            }

            Velocity += steeringForce;

            // Apply change heading and side when velocity is big enough
            if (Velocity.Length() > 0.0000001)
            {
                Heading = Vector2.Normalize(Velocity);
            }


            if (Velocity.Length() > MaxSpeed)
            {
                Velocity = Heading * MaxSpeed;
            }

            Pos += Velocity;

            //check if pos is in wall if so go back to old pos
            foreach (Wall wall in MyWorld.walls)
            {
                if (wall.IsWithin(Pos))
                {
                    Pos = OldPosition;
                } 
            }
        }

        public override string ToString()
        {
            return string.Format("{0}", Velocity);
        }
    }
}