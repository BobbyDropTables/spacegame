using System.Collections.Generic;
using AAI.behaviour;
using AAI.Controller;
using AAI.States;
using AAI.world;
using Microsoft.Xna.Framework;

namespace AAI.Entity.MovingEntities
{
    public abstract class MovingEntity : BaseGameEntity
    {
        public Vector2 Velocity;
        public Vector2 Heading;
        public int energy;
        public float Mass;
        public float MaxSpeed;
        public float Maxturn;
        public float MaxForce;
        public List<SteeringBehaviour> Behaviours;
        public Vector2 Side = new Vector2(1, 0);
        public Vector2 OldPosition;
        public StateMachine<MovingEntity> StateMachine; 


        public MovingEntity(Vector2 pos, World w) : base(pos, w)
        {
            energy = 100;
            Mass     = 4f;
            MaxSpeed = 2f;
            Maxturn  = 0.00001f;
            Velocity = new Vector2();
            MaxForce = 0.0005f;
            
        }

        public override void Update()
        {
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
                Side    = new Vector2(-Heading.Y, Heading.X);
            }


            if (Velocity.Length() > MaxSpeed)
            {
                Velocity = Heading * MaxSpeed;
            }

            Pos += Velocity;
        }

        public override string ToString()
        {
            return string.Format("{0}", Velocity);
        }
    }
}