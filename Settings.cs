using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
    public static class Settings //static means non-instantiable and always available
    {
        public static Point Resolution;
        public enum GameState
        {
            Menu,
            Loading,
            Playing,
            Error,
            Exit
        }
    }
}
