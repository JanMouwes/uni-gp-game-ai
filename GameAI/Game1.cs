using GameAI.Util;
using Graph;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameAI
{
    public class Game1 : Game
    {
        private World world;
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private GraphRenderer graphRenderer;

        private const int WORLD_WIDTH = 800;
        private const int WORLD_HEIGHT = 600;

        public Graph<Vector2> NavGraph;

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

            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            this.world.Update(gameTime);

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