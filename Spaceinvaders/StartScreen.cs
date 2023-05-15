using System.Numerics;
using Raylib_cs;

namespace Spaceinvaders
{
    class StartScreen
    {
        public StartScreen()
        {
            Raylib.SetTargetFPS(60);
        }

        public void Draw()
        {
            Raylib.ClearBackground(Color.BLACK);

            // Calculate the width of the first text
            int titleWidth = Raylib.MeasureText("Space Invaders", 80);
            // Calculate the position to center the first text
            Vector2 titlePosition = new Vector2((Raylib.GetScreenWidth() - titleWidth) / 2, 350);

            // Draw the first text centered
            Raylib.DrawText("Space Invaders", (int)titlePosition.X, (int)titlePosition.Y, 80, Color.YELLOW);

            // Calculate the width of the second text
            int startWidth = Raylib.MeasureText("Press SPACE to start", 40);
            // Calculate the position to center the second text
            Vector2 startPosition = new Vector2((Raylib.GetScreenWidth() - startWidth) / 2, 550);

            // Draw the second text centered
            Raylib.DrawText("Press SPACE to start", (int)startPosition.X, (int)startPosition.Y, 40, Color.GRAY);
        }

        public bool Update()
        {
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_SPACE))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
