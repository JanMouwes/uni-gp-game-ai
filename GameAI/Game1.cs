using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameAI.Entity;
using GameAI.Entity.GoalBehaviour;
using GameAI.Entity.GoalBehaviour.Composite;
using GameAI.GoalBehaviour;
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

        private KeyboardInput keyboardInput;
        private MouseInput mouseInput;

        private const int WORLD_WIDTH = 400;
        private const int WORLD_HEIGHT = 300;

        public Graph<Vector2> NavGraph;

        private LinkedList<Vehicle> selectedEntities = new LinkedList<Vehicle>();
        private SpriteFont mainFont;
        private Texture2D boat;

        public Game1()
        {
            this.graphics = new GraphicsDeviceManager(this);
            this.IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            (float, float) dimensions = (WORLD_WIDTH, WORLD_HEIGHT);
            (int, int) vertexCounts = (24, 18);
            (float, float) offset = (10, 10);
            this.NavGraph = GraphGenerator.GenerateGraphWithPadding(
                dimensions, vertexCounts, offset,
                GraphGenerator.AxisAndDiagonalIndices
            );

            this.pathFinder = new PathFinder(this.NavGraph);

            this.world = new World(WORLD_WIDTH, WORLD_HEIGHT, this.pathFinder);
            

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

            boat = Content.Load<Texture2D>("ship1");
            
            this.world.Populate(5, this.boat);
            
            this.graphRenderer = new GraphRenderer(this.NavGraph, this.mainFont, Color.White);

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

        private void DebugDraw(SpriteBatch spriteBatch)
        {
            // Rock theRock = this.world.Entities.OfType<Rock>().First();
            Vehicle vehicle = this.selectedEntities.FirstOrDefault();

            if (vehicle != null)
            {
                string GetGoalText(Goal<Vehicle> goal, int inset)
                {
                    StringBuilder stringBuilder = new StringBuilder();

                    for (int i = 0; i < inset; i++)
                    {
                        stringBuilder.Append(' ');
                        stringBuilder.Append(' ');
                    }

                    stringBuilder.Append(goal);
                    stringBuilder.Append('\n');

                    if (goal is GoalComposite<Vehicle> composite)
                    {
                        foreach (Goal<Vehicle> compositeGoal in composite.Goals) { stringBuilder.Append(GetGoalText(compositeGoal, inset + 1)); }
                    }

                    return stringBuilder.ToString();
                }

                StringBuilder text = new StringBuilder();

                text.Append($"Goals:\n");
                text.Append(GetGoalText(vehicle.Brain, 1));

                // text.Append($"Position: {vehicle.Position.ToPoint()}\n");
                // text.Append($"Steering: {vehicle.Steering.Calculate().ToPoint()}\n");
                // text.Append($"Velocity: {vehicle.Velocity.ToPoint()}\n");

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