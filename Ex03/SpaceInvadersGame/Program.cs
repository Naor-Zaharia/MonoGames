﻿using System;

namespace SpaceInvadersGame
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            using (var game = new SpaceInvaders())
            {
                game.Run();
            }
        }
    }
}
