using System.Linq;
using AAI.Entity.MovingEntities;
using AAI.Entity.staticEntities;
using Microsoft.Xna.Framework;

namespace AAI.behaviour
{
    class ObjectAvoidance : SteeringBehaviour
    {
        private float        _panicDistance;

        public ObjectAvoidance(MovingEntity me, float panicDistance) : base(me)
        {
            _panicDistance = panicDistance;
        }

        public override Vector2 Calculate()
        {
            var output             = new Vector2();
            var entities           = ME.MyWorld.entities;
            var closeStaticEntites = entities.Where(entity => entity is StaticEntity).ToList();

            foreach (var staticEntity in closeStaticEntites)
            {
                if (Vector2.Distance(ME.Pos, staticEntity.Pos) < staticEntity.Scale + _panicDistance + ME.Scale)
                    output += Vector2.Normalize(ME.Pos - staticEntity.Pos) * ME.Velocity;
            }

            return output;
        }
	}
}