using System;
using Raylib_cs;
using System.Collections.Generic;
using System.Numerics;
using Spaceinvaders;

namespace SpaceInvaders
{
    class Program
    {
        static void Main(string[] args)
        {
            invaders game = new invaders();

            game.GameLoop();
        }   
    }
}
