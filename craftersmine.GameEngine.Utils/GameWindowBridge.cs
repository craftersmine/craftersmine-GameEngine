using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.GameEngine.Utils
{
    /// <summary>
    /// [Internal Engine Class] Represents Brige of GameWindow data
    /// </summary>
    public sealed class GameWindowBridge
    {
        /// <summary>
        /// [Internal Engine Property] Current game tick
        /// </summary>
        public static int CurrentTick { get; set; }
    }
}
