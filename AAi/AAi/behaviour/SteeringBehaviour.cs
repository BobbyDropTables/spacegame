using Game.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game.entity.MovingEntities;
using Microsoft.Xna.Framework;

namespace Game
{
    abstract class SteeringBehaviour
    {
        public MovingEntity ME { get; set; }
        public abstract Vector2 Calculate();

        public SteeringBehaviour(MovingEntity me)
        {
            ME = me;
        }
    }

    
}
