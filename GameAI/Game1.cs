using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameAI.entity;
using GameAI.GoalBehaviour.Composite;
using GameAI.Steering;
using GameAI.Util;
using Graph;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using GameAI.Input;
using GameAI.Navigation;

namespace GameAI
{
    public class Game1 : Game
    {
        private World world;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private bool paused;

        private GraphRenderer graphRenderer;

        private PathFinder pathFinder;
        private CustomizablePathSmoother pathSmoother;

        private KeyboardInput keyboardInput;
        private MouseInput mouseInput;

        private const int WORLD_WIDTH = 800;
        private const int WORLD_HEIGHT = 600;

        public Graph<Vector2> NavGraph;

        private LinkedList<Vehicle> selectedEntities = new LinkedList<Vehicle>();
        private SpriteFont mainFont;

        public Game1()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            this.world = new World(WORLD_WIDTH, WORLD_HEIGHT);
            this.world.Populate(10);

            this.graphics.PreferredBackBufferWidth = WORLD_WIDTH;
            this.graphics.PreferredBackBufferHeight = WORLD_HEIGHT;
            this.graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            this.spriteBatch = new SpriteBatch(this.GraphicsDevice);

            this.mainFont = Content.Load<SpriteFont>("Content/opensans");

            (float, float) dimensions = (WORLD_WIDTH, WORLD_HEIGHT);
            (int, int) vertexCounts = (24, 18);
            (float, float) offset = (10, 10);
            this.NavGraph = GraphGenerator.GenerateGraphWithObstacles(
                dimensions, vertexCounts, offset, 
                world,
                GraphGenerator.AxisAndDiagonalIndices
            );

            this.graphRenderer = new GraphRenderer(this.NavGraph, this.mainFont, Color.White);

            this.pathSmoother = new CustomizablePathSmoother(world);
            this.pathFinder = new PathFinder(this.NavGraph, pathSmoother);

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
                Vector2 target = Mouse.GetState().Position.ToVector2();

                bool shouldClear = !Keyboard.GetState().IsKeyDown(Keys.LeftShift);

                foreach (Vehicle selectedEntity in this.selectedEntities)
                {
                    if (shouldClear) { selectedEntity.Brain.ClearGoals(); }

                    selectedEntity.Brain.AddSubgoal(new MoveTo<Vehicle>(selectedEntity, target, this.pathFinder));
                }
            });

            this.keyboardInput.OnKeyPress(Keys.Space, (input, state) => { this.paused = !this.paused; });

            base.LoadContent();
        }

        private void ClearSelected()
        {
            foreach (Vehicle selectedEntity in this.selectedEntities) { selectedEntity.Color = selectedEntity.Team.Colour; }

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

        private int currentVehicleIndex = 0;

        private void DebugDraw(SpriteBatch spriteBatch)
        {
            // Rock theRock = this.world.obstacles.OfType<Rock>().First();
            Vehicle vehicle = this.selectedEntities.FirstOrDefault();

            if (vehicle != null)
            {
                StringBuilder text = new StringBuilder();

                text.Append($"Position: {vehicle.Pos.ToPoint()}\n");
                text.Append($"Steering: {vehicle.Steering.Calculate().ToPoint()}\n");
                text.Append($"Velocity: {vehicle.Velocity.ToPoint()}\n");

                spriteBatch.DrawString(this.mainFont, text.ToString(), Vector2.Zero, Color.Black);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            this.graphics.GraphicsDevice.Clear(Color.LightBlue);

            this.spriteBatch.Begin();

            this.graphRenderer.Render(this.spriteBatch);

            foreach (Vehicle selectedEntity in this.selectedEntities)
            {
                if (selectedEntity.Brain.CurrentGoal is MoveTo<Vehicle> followPath) { PathRenderer.RenderPath(this.spriteBatch, this.mainFont, followPath.Path, Color.Green); }
            }

            this.world.Render(this.spriteBatch);

            DebugDraw(this.spriteBatch);

            base.Draw(gameTime);

            this.spriteBatch.End();
        }
    }
}