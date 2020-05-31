using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameAI.Entity;
using GameAI.Entity.Components;
using GameAI.Entity.Navigation;
using GameAI.Entity.Steering;
using GameAI.Entity.Steering.Complex;
using GameAI.Entity.Steering.Simple;
using GameAI.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameAI.Input;
using GameAI.Navigation;
using GameAI.world;
using MonoGame.Extended;

namespace GameAI
{
    public class Game1 : Game
    {
        private World world;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private bool paused;

        private GraphRenderer graphRenderer;

        private KeyboardInput keyboardInput;
        private MouseInput mouseInput;

        private const int WORLD_WIDTH = 800;
        private const int WORLD_HEIGHT = 600;

        private LinkedList<Ship> selectedEntities = new LinkedList<Ship>();
        private SpriteFont mainFont;

        private bool drawAgentGoals;

        public Game1()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            this.world = new World(WORLD_WIDTH, WORLD_HEIGHT);

            this.graphics.PreferredBackBufferWidth = WORLD_WIDTH;
            this.graphics.PreferredBackBufferHeight = WORLD_HEIGHT;
            this.graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            Content.RootDirectory = "Content";

            this.mainFont = Content.Load<SpriteFont>("opensans");

            Texture2D rock = Content.Load<Texture2D>("rock");

            Texture2D boat = Content.Load<Texture2D>("ship1");
            Texture2D pirateBoat = Content.Load<Texture2D>("ship-pirate");

            Texture2D birdTexture = Content.Load<Texture2D>("bird-white");

            Texture2D[] teamTextures =
            {
                boat, pirateBoat
            };

            Populate(5, 10, teamTextures, rock, birdTexture);

            IPathSmoother smoother = new CustomizablePathSmoother(this.world, 5);

            (float, float) dimensions = (WORLD_WIDTH, WORLD_HEIGHT);
            (int, int) vertexCounts = (24, 18);
            (float, float) offset = (10, 10);
            this.world.NavigationGraph = GraphGenerator.GenerateGraphWithObstacles(
                dimensions, vertexCounts, offset, this.world.Entities.OfType<Rock>(),
                GraphGenerator.AxisAndDiagonalIndices
            );

            this.world.PathFinder = new PathFinder(this.world.NavigationGraph, smoother);

            this.graphRenderer = new GraphRenderer(this.world.NavigationGraph, this.mainFont, Color.White);

            this.keyboardInput = new KeyboardInput();
            this.mouseInput = new MouseInput();

            this.mouseInput.OnKeyPress(MouseButtons.Left, (input, state) =>
            {
                IEnumerable<Ship> entitiesNearMouse = this.world.FindEntitiesNear(this.mouseInput.MouseState.Position.ToVector2(), 3).OfType<Ship>();

                if (!this.keyboardInput.KeyboardState.IsKeyDown(Keys.LeftShift)) { ClearSelected(); }

                foreach (Ship baseGameEntity in entitiesNearMouse)
                {
                    this.selectedEntities.AddLast(baseGameEntity);
                    baseGameEntity.Color = Color.Red;
                }
            });

            this.keyboardInput.OnKeyPress(Keys.G, (key, state) => { this.graphRenderer.ToggleEnabled(); });
            this.keyboardInput.OnKeyPress(Keys.Space, (input, state) => { this.paused = !this.paused; });
            this.keyboardInput.OnKeyPress(Keys.O, (input, state) => { this.drawAgentGoals = !this.drawAgentGoals; });

            base.LoadContent();
        }

        public void Populate(int vehicleCount, int flockCount, Texture2D[] teamTextures, Texture2D rockTexture, Texture2D birdTexture)
        {
            // Add obstacles
            // Rock r = new Rock(this, new Vector2(100, 100), 15, Color.Black);
            // r.Graphics = new TextureGraphics(r, rockTexture);
            // this.entities.Add(r);

            const int numberOfTeams = 2;
            Color[] teamColours =
            {
                Color.Blue,
                Color.Red
            };
            Vector2[][] teamSpawns =
            {
                new[]
                {
                    new Vector2(50, 50),
                    new Vector2(100, 50),
                    new Vector2(50, 100),
                },
                new[]
                {
                    new Vector2(this.world.Width - 50, this.world.Height - 50),
                    new Vector2(this.world.Width - 100, this.world.Height - 50),
                    new Vector2(this.world.Width - 50, this.world.Height - 100),
                }
            };

            Vector2[] teamFlagPosition =
            {
                new Vector2(75, 75),
                new Vector2(this.world.Width - 75, this.world.Height - 75)
            };

            for (int teamIndex = 0; teamIndex < numberOfTeams; teamIndex++)
            {
                Team team = new Team(teamColours[teamIndex]);
                Texture2D vehicleTexture = teamTextures[teamIndex];

                team.AddSpawnPoints(teamSpawns[teamIndex]);

                Flag flag = new Flag(team, 5f);

                this.world.SpawnGameEntity(flag, teamFlagPosition[teamIndex]);
                team.Flag = flag;


                team.Base = teamFlagPosition[teamIndex];

                this.world.Teams.Add(team.Colour, team);

                Rectangle[] rectangles =
                {
                    new Rectangle(3, 698, 32, 32),
                    new Rectangle(3, 400, 32, 32)
                };


                // Add Entities
                for (int vehicleIndex = 0; vehicleIndex < vehicleCount; vehicleIndex++)
                {
                    Ship ship = new Ship(this.world, team)
                    {
                        MaxSpeed = 400f,
                        Mass = 2
                    };
                    ship.Graphics = new TextureGraphics(ship, vehicleTexture)
                    {
                        SourceRectangle = rectangles[teamIndex], RotationOffset = (float) Math.PI
                    };
                    ship.Steering = new FlockingBehaviour(ship, this.world, 10, 5);

                    this.world.SpawnVehicle(ship);
                }
            }

            foreach (Team team in this.world.Teams.Values)
            {
                Team otherTeam = this.world.Teams.Values.First(other => other != team);

                team.Flag.Captured += (capturedFlag) =>
                {
                    capturedFlag.Team.RespawnFlag();
                    otherTeam.Points++;
                };
            }

            void SpawnFlock(float size, Vector2 position)
            {
                Rectangle[] birdFrames =
                {
                    new Rectangle(0, 0, 16, 16),
                    new Rectangle(16, 0, 16, 16),
                    new Rectangle(32, 0, 16, 16),
                };

                for (int i = 0; i < size; i++)
                {
                    Vector2 orientation = Vector2.One;

                    if (i % 2 == 0) { orientation *= -1; }

                    Bird bird = new Bird(this.world)
                    {
                        MaxSpeed = 30f,
                        MinSpeed = 3f,
                        Mass = 1,
                        Orientation = orientation
                    };

                    int startingFrame = i % 3;

                    bird.Graphics = new AnimatedTextureGraphics(bird, birdTexture, birdFrames, 100)
                    {
                        RotationOffset = -45f,
                        CurrentFrame = startingFrame
                    };


                    SteeringBehaviour behaviour = new SteeringUnion(bird, new WanderBehaviour(bird, 15), new FlockingBehaviour(bird, this.world, 10, 5))
                    {
                        WeightA = .5f,
                        WeightB = 2f
                    };

                    bird.Steering = behaviour;

                    this.world.SpawnGameEntity(bird, position);
                }
            }

            const int flockSize = 10;

            Random random = new Random();

            for (int i = 0; i < flockCount; i++)
            {
                Vector2 spawnPoint = new Vector2(random.Next(50, this.world.Height - 50));

                SpawnFlock(flockSize, spawnPoint);
            }
        }

        private void ClearSelected()
        {
            foreach (Ship selectedEntity in this.selectedEntities) { selectedEntity.Color = selectedEntity.Team.Colour; }

            this.selectedEntities.Clear();
        }

        protected override void Update(GameTime gameTime)
        {
            this.keyboardInput.Update(Keyboard.GetState());
            this.mouseInput.Update(Mouse.GetState());

            if (this.paused) { return; }

            this.world.Update(gameTime);

            base.Update(gameTime);
        }

        private void DrawControls(SpriteBatch batch)
        {
            IEnumerable<(Keys key, string description)> keyControls = new[]
            {
                (Keys.G, "toggle graph"),
                (Keys.Space, "toggle paused"),
                (Keys.O, "toggle agent goals")
            };
            IEnumerable<(MouseButtons button, string description)> mouseControls = new[]
            {
                (MouseButtons.Left, "select agent")
            };

            StringBuilder stringBuilder = new StringBuilder();

            foreach ((Keys key, string description) in keyControls) { stringBuilder.Append($"{description}: {key}\n"); }

            foreach ((MouseButtons button, string description) in mouseControls) { stringBuilder.Append($"{description}: {button} click\n"); }

            batch.DrawString(this.mainFont, stringBuilder.ToString(), new Vector2(WORLD_WIDTH - 180, 5), Color.Black);
        }

        private void DebugDraw(SpriteBatch batch)
        {
            Ship ship = this.selectedEntities.FirstOrDefault();


            // DebugRendering.DrawWallPanicDistance(this.spriteBatch, 15f, this.world);
            if (ship != null && this.drawAgentGoals) { DebugRendering.DrawAgentGoals(batch, this.mainFont, ship); }
        }

        protected override void Draw(GameTime gameTime)
        {
            this.graphics.GraphicsDevice.Clear(Color.LightBlue);

            this.spriteBatch.Begin();

            this.graphRenderer.Render(this.spriteBatch);


            // Draw spawns (manually for now)
            foreach (Team team in this.world.Teams.Values)
            {
                foreach (Vector2 spawn in team.SpawnPoints) { this.spriteBatch.DrawString(this.mainFont, "S", spawn, team.Colour); }

                this.spriteBatch.DrawString(this.mainFont, team.Points.ToString(), team.Base, team.Colour);
            }

            this.world.Render(this.spriteBatch, gameTime);

            // Special rendering for selected entities
            foreach (Ship selectedEntity in this.selectedEntities)
            {
                this.spriteBatch.DrawCircle(selectedEntity.Position, selectedEntity.Scale, 360, selectedEntity.Team.Colour);
                selectedEntity.Brain.Render(spriteBatch);
            }

            DebugDraw(this.spriteBatch);
            DrawControls(this.spriteBatch);

            base.Draw(gameTime);

            this.spriteBatch.End();
        }
    }
}