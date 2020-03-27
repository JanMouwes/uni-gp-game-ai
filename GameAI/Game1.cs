using System;
using System.Collections.Generic;
using System.Linq;
using GameAI.entity;
using System.Collections.Generic;
using System.Linq;
using GameAI.behaviour;
using GameAI.entity;
using GameAI.Util;
using Graph;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using Microsoft.Xna.Framework.Input;
using GameAI.Input;

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

        public Graph<Vector2> NavGraph;

        private LinkedList<Vehicle> selectedEntities = new LinkedList<Vehicle>();

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

            (float, float) dimensions = (WORLD_WIDTH, WORLD_HEIGHT);
            (int, int) vertexCounts = (16, 12);
            (float, float) offset = (10, 10);
            this.NavGraph = GraphGenerator.GenerateGraphWithPadding(
                dimensions, vertexCounts, offset,
                GraphGenerator.AxisAndDiagonalIndices
            );

            this.graphRenderer = new GraphRenderer(this.NavGraph);

            this.keyboardInput = new KeyboardInput();
            this.mouseInput = new MouseInput();

            this.keyboardInput.OnKeyPress(Keys.G, (key, state) => { this.graphRenderer.ToggleEnabled(); });

            this.mouseInput.OnKeyPress(MouseButtons.Left, (input, state) =>
            {
                IEnumerable<Vehicle> entitiesNearMouse = this.world.FindEntitiesNear(this.mouseInput.MouseState.Position.ToVector2(), 3).OfType<Vehicle>();

                if (!this.keyboardInput.KeyboardState.IsKeyDown(Keys.LeftShift)) { ClearSelected(); }

                foreach (Vehicle baseGameEntity in entitiesNearMouse)
                {
                    this.selectedEntities.AddLast(baseGameEntity);
                    baseGameEntity.Color = Color.Red;
                }
            });

            this.mouseInput.OnKeyPress(MouseButtons.Right, (input, state) =>
            {
                foreach (Vehicle selectedEntity in this.selectedEntities) { selectedEntity.Steering = new SeekBehaviour(selectedEntity, this.mouseInput.MouseState.Position.ToVector2()); }
            });

            base.LoadContent();
        }

        private void ClearSelected()
        {
            foreach (Vehicle selectedEntity in this.selectedEntities) { selectedEntity.Color = Color.Blue; }
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState current = Keyboard.GetState();
            if (current.IsKeyDown(Keys.Space) && previous.IsKeyUp(Keys.Space)) { this.paused = !this.paused; }

            if (current.IsKeyDown(Keys.N) && previous.IsKeyUp(Keys.N))
            {
                this.currentVehicleIndex++;

                if (this.currentVehicleIndex > this.world.entities.Count - 1) { this.currentVehicleIndex = 0; }
            }

            previous = Keyboard.GetState();
            if (this.paused) { return; }

            this.world.Update(gameTime);

            this.keyboardInput.Update(Keyboard.GetState());
            this.mouseInput.Update(Mouse.GetState());

            base.Update(gameTime);
        }

        private int currentVehicleIndex = 0;

        private void DebugDraw(SpriteBatch spriteBatch)
        {
            Rock theRock = this.world.obstacles.OfType<Rock>().First();

            Vehicle vehicle = this.world.FindEntitiesNear(theRock.Pos, 1000).OfType<Vehicle>().OrderBy(v => Vector2.DistanceSquared(v.Pos, theRock.Pos)).FirstOrDefault();


            if (vehicle != null)
            {
                // The detection box is the current velocity divided by the max velocity of the entity
                // range is the maximum size of the box
                Vector2 viewBox = vehicle.Velocity / vehicle.MaxSpeed * 100;
                // Add the box in front of the entity
                IEnumerable<Vector2> checkpoints = new[]
                {
                    vehicle.Pos,
                    vehicle.Pos + viewBox / 2f, // Halfway
                    vehicle.Pos + viewBox,      // At the end
                    vehicle.Pos + viewBox * 2   // Square
                };

                foreach (Vector2 checkpoint in checkpoints) { spriteBatch.DrawPoint(checkpoint, Color.Black, 5f); }

                CircleF notAllowedZone = new CircleF(theRock.Pos, theRock.Scale + vehicle.Scale * 2);
                spriteBatch.DrawCircle(notAllowedZone, 360, Color.Orange);

                Vector2 dist = new Vector2(theRock.Pos.X - vehicle.Pos.X, theRock.Pos.X - vehicle.Pos.Y);
                Vector2 perpendicular = new Vector2(-dist.Y, dist.X);

                Vector2 realDist = vehicle.Pos     + dist;
                Vector2 realHaaks = theRock.Pos    + perpendicular;
                Vector2 realMinHaaks = theRock.Pos - perpendicular;

                spriteBatch.DrawLine(vehicle.Pos, realDist, Color.Purple);
                spriteBatch.DrawLine(theRock.Pos, realHaaks, Color.Aqua);
                spriteBatch.DrawLine(theRock.Pos, realMinHaaks, Color.Green);

                Vector2 vehicleVelocityPos = vehicle.Pos + vehicle.Velocity;

                float haaksDistPlus = Vector2.DistanceSquared(realHaaks, vehicleVelocityPos);
                float haaksDistMin = Vector2.DistanceSquared(realMinHaaks, vehicleVelocityPos);

                Vector2 target = haaksDistPlus > haaksDistMin ? realMinHaaks : realHaaks;

                spriteBatch.DrawLine(vehicle.Pos, target, Color.DarkRed);
                spriteBatch.DrawLine(vehicleVelocityPos, target, Color.Chocolate);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            this.graphics.GraphicsDevice.Clear(Color.LightBlue);

            this.spriteBatch.Begin();

            this.graphRenderer.Render(this.spriteBatch);

            this.world.Render(this.spriteBatch);

            DebugDraw(this.spriteBatch);

            base.Draw(gameTime);

            this.spriteBatch.End();
        }
    }
}