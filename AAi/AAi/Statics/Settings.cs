using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAI.Statics
{
	public static class Settings
    {
        public static class General
        {
            public static int WindowWidth  = 1366;
            public static int WindowHeight = 768;
        }

        public static class GameWorld
        {
            public static int   Width                = 2000;
            public static int   Height               = 2000;
            public static float NodeSeperation       = 100;
            public static float NodeBorderSeperation = 100;
            public static float NodeStaticSeperation = 20;
        }

        public static class Player
        {
            public static Vector2 Size = new Vector2(32, 32);
        }

        public static class Fighter
        {
            public static Vector2 Size = new Vector2(32, 32);
        }

        public static class Hunter
        {
            public static Vector2 Size             = new Vector2(32, 32);
            public static float   PlayerHuntEnergy = -0.4f;
            public static float   FlockHuntEnergy  = -0.1f;
            public static float   RestEnergy       = 0.2f;
        }

        public static class MainCamera
        {
            public static Vector2 StartPosition         = Vector2.Zero;
            public static int     HorizontalScrollSpeed = 2;
            public static int     VerticalScrollSpeed   = 2;
            public static float   StartZoom             = 0.6f;
            public static float   ZoomSpeed             = 0.01f;
            public static float   ZoomMax               = 80f;
            public static float   ZoomMin               = 0.1f;
            public static int     StartRotation         = 0;
            public static float   RotationSpeed         = 1f;
        }

        public class GuiCamera
        {
            public static Vector2 StartPosition = new Vector2(0, 0);
            public static float   StartZoom     = 1.0f;
        }
    }
}
