using System.Collections.Generic;
using System.Linq;
using GameAI.behaviour;
using GameAI.entity;
using GameAI.Util;
using Graph;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameAI.Input;

namespace GameAI
{
    public class Game1 : Game
    {
        private World world;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

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
            this.world.Update(gameTime);

            this.keyboardInput.Update(Keyboard.GetState());
            this.mouseInput.Update(Mouse.GetState());

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            this.graphics.GraphicsDevice.Clear(Color.White);

            this.spriteBatch.Begin();

            this.graphRenderer.Render(this.spriteBatch);

            this.world.Render(this.spriteBatch);

            base.Draw(gameTime);

            this.spriteBatch.End();
        }
    }
}