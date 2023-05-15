using Raylib_cs;
using SpaceInvaders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static SpaceInvaders.Program;

namespace Spaceinvaders
{
    internal class invaders
    {
        public static int numEnemies = 25;

        public int screenWidth = 800;
        public int screenHeight = 1000;

        Player player;
        List<Bullet> bullets;
        List<Enemy> enemies;
        private Vector2 position;

        public bool moveRight = true;
        public static bool shouldChangeDirection = false;
        public bool moveDown = false;
        public float speed = 0.02f;

        private Texture2D enemyImage;
        private Texture2D playerImage;

        bool gameOver = false;
        bool gameStarted = false;

        StartScreen startScreen;

        enum GameState { Playing, Win, Lose };
        GameState gameState = GameState.Playing;

        void init()
        {
            startScreen = new StartScreen();
    
            Raylib.InitAudioDevice();

            Raylib.InitWindow(screenWidth, screenHeight, "Space Invaders");

            float playerSpeed = 120;
            int playerSize = 40;
            Vector2 playerStart = new Vector2(screenWidth / 2, screenHeight - playerSize * 2);

            bullets = new List<Bullet>();

            enemies = new List<Enemy>();

            enemyImage = Raylib.LoadTexture("Images/enemy.png");
            playerImage = Raylib.LoadTexture("Images/player.png");

            player = new Player(playerStart, new Vector2(0, 0), playerSpeed, playerSize, playerImage);

            for (int i = 0; i < numEnemies; i++)
            {
                int row = i / 5; // row number
                int col = i % 5; // column number

                Vector2 position = new Vector2(col * 100 + 100, row * 100 + 100);
                Enemy enemy = new Enemy(position, new Vector2(1, 0), 50, enemyImage, new List<Player> { player });

                enemies.Add(enemy);
            }

            Raylib.SetTargetFPS(2500);
        }

        public void GameLoop()
        {
            init();

            // Game loop
            while (!Raylib.WindowShouldClose())
            {
                // Draw start screen and wait for player to start game
                if (!gameStarted)
                {   
                    startScreen.Draw();
                    if (startScreen.Update())
                    {
                        gameStarted = true;
                    }
                }

                // Update player and enemies if the game is still running
                if (!gameOver && gameStarted)
                {
                    UpdateEnemies();
                    player.Update(enemies);
                    foreach (Enemy enemy in enemies.ToList())
                    {
                        enemy.Update(player);
                    }
                }

                // Draw player and enemies
                Raylib.BeginDrawing();

                // Draw win/lose screen if the game is over
                if (!gameStarted)
                {
                    startScreen.Draw();
                }
                else if (gameOver)
                {
                    drawGameOver();
                }
                else
                {
                    drawGame();
                }

                // Check for win/lose conditions
                if (player.health <= 0)
                {
                    gameState = GameState.Lose;
                    gameOver = true;
                }
                else if (enemies.Count == 0)
                {
                    gameState = GameState.Win;
                    gameOver = true;
                }

                Raylib.EndDrawing();
            }
            Raylib.CloseWindow();
            
            Raylib.UnloadTexture(enemyImage);
            Raylib.UnloadTexture(playerImage);
        }

        void drawGameOver()
        {
            if (gameState == GameState.Lose)
            {
                Raylib.ClearBackground(Color.RED);
                Raylib.DrawText("Game Over!", 250, 400, 50, Color.BLACK);
                Raylib.DrawText("You got:" + player.score + " score", 225, 500, 40, Color.BLACK);
            }
            else if (gameState == GameState.Win)
            {
                Raylib.ClearBackground(Color.LIME);
                Raylib.DrawText("You Win!", 250, 400, 50, Color.BLACK);
                Raylib.DrawText("You got:" + player.score + " score", 225, 500, 40, Color.BLACK);
            }
            Raylib.DrawText("Press ENTER to start again", 175, 600, 30, Color.BLACK);

            // Check for enter key press to restart the game
            if (Raylib.IsKeyPressed(KeyboardKey.KEY_ENTER) && gameOver)
            {
                // Reset game state
                gameState = GameState.Playing;
                player.health = 100;
                player.score = 0;
                bullets.Clear();
                enemies.Clear();
                shouldChangeDirection = false;
                moveRight = true;
                moveDown = false;

                // Reinitialize player, bullets, and enemies
                float playerSpeed = 120;
                int playerSize = 40;
                Vector2 playerStart = new Vector2(screenWidth / 2, screenHeight - playerSize * 2);
                player = new Player(playerStart, new Vector2(0, 0), playerSpeed, playerSize, playerImage);
                for (int i = 0; i < numEnemies; i++)
                {
                    int row = i / 5; // row number
                    int col = i % 5; // column number
                    Vector2 position = new Vector2(col * 100 + 100, row * 100 + 100);
                    Enemy enemy = new Enemy(position, new Vector2(1, 0), 50, enemyImage, new List<Player> { player });
                    enemies.Add(enemy);
                }

                // Set game flags
                gameStarted = true;
                gameOver = false;
            }
        }

        void drawGame()
        {
            Raylib.ClearBackground(Color.WHITE);
            player.Draw();
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw();
            }
            player.DrawScore();

            // Draw player health
            string healthText = "Health: " + player.health.ToString();
            Raylib.DrawText(healthText, 10, 10, 20, Color.BLACK);
        }

        void UpdateEnemies()
        {
            bool wallHit = false;
            float enemySpeed = speed;

            // Move the enemies 
            foreach (Enemy enemy in enemies)
            {
                enemy.position.X += enemySpeed * enemy.transform.direction.X;

                // Check if the enemy hits the left or right wall
                if (enemy.position.X - enemy.transform.size / 2 <= 0 || enemy.position.X + enemy.transform.size / 2 >= screenWidth)
                {
                    wallHit = true;
                }
            }

            // If any enemy hits the wall, change their direction and move them down
            if (wallHit)
            {
                foreach (Enemy enemy in enemies)
                {
                    enemy.transform.direction.X *= -1.0f;
                    enemy.position.Y += 10;
                }
            }

            // Check if the enemies should change direction 
            if (enemies.Count > 0 && moveRight && enemies.Last().position.X >= screenWidth - 20)
            {
                moveRight = false;
                shouldChangeDirection = true;
                position.Y += 10; // add a small vertical offset
            }

            else if (enemies.Count > 0 && !moveRight && enemies.First().position.X <= 20)
            {
                moveRight = true;
                shouldChangeDirection = true;
                position.Y += 10; // add a small vertical offset
            }

            // If the enemies should change direction, change their direction and move them down
            if (shouldChangeDirection)
            {
                speed *= -1;
                shouldChangeDirection = false;
                moveDown = true;
                foreach (Enemy enemy in enemies)
                {
                    enemy.transform.direction.X *= -1.0f;
                    enemy.position.Y += 10;
                }
            }
        }
    }
}
