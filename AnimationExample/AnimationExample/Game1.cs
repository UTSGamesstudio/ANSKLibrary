using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ProjectSneakyGame;
using ModelAnimationLibrary;

namespace AnimationExample
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private Input _inputManager;
        private InputContainer _inputs;
        private Camera _camera;
        private ANSKTest _test;
        private ANSKModel _model;
        private ANSKModelContainer _modelCont;
        private Matrix world, view, proj, _modelTrans;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            LocalPlayerRegistry.InitialisePlayer(PlayerIndex.One);
            // TODO: Add your initialization logic here
            _inputManager = new Input(this);
            _inputManager.AddCommandToCheckKeyboard(Keys.W, Keys.A, Keys.S, Keys.D, Keys.Q, Keys.E, Keys.Z, Keys.C);
            _inputs = _inputManager.RetrieveInputContainer;

            _camera = new Camera(this, Vector3.Backward * 20, Vector3.Forward, Vector3.Up);
            _camera.Target = new GameObject(this, Matrix.Identity.Translation);
            _camera.TargetSpecified = true;

            _model = new ANSKModel(Content.Load<ANSKModelContent>("CubeTest3"));
            _model.ManualInitialise(GraphicsDevice, Content.Load<Effect>("Effects/AnimatableModel"), this);
            _model.MeshRenderer.CenterModelToOrigin();
            _model.MeshRenderer.SetTexture(Content.Load<Texture2D>("Psyduck"));
            _modelCont = new ANSKModelContainer(_model, this, Vector3.Zero);
            _model.PlayAnimation("One");

            world = Matrix.CreateTranslation(0, 0, 0);
            view = Matrix.CreateLookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
            proj = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.01f, 100f);

            _modelTrans = Matrix.CreateTranslation(Vector3.Zero);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            _inputManager.Update(gameTime);

            _camera.Update(gameTime);

            // Please use _inputs for checking inputs.

            if (_inputs[Keys.W])
                _modelCont.Translate(0, 0, 0.5f);
            else if (_inputs[Keys.S])
                _modelCont.Translate(0, 0, -0.5f);
            if (_inputs[Keys.A])
                _modelCont.RotateY(MathHelper.ToRadians(2));
            else if (_inputs[Keys.D])
                _modelCont.RotateY(MathHelper.ToRadians(-2));
            if (_inputs.IsFirstPressed(Keys.E))
                _model.AAC.BlendShapesControl.ChangeBlendValue("InnerBlend", 1);
            else if (_inputs.IsFirstPressed(Keys.Q))
                _model.AAC.BlendShapesControl.ChangeBlendValue("InnerBlend", 0);

            if (_inputs.IsFirstPressed(Keys.Z))
                _model.PlayAnimation("One");
            else if (_inputs.IsFirstPressed(Keys.C))
                _model.PlayAnimation("Two");

            _modelCont.Update(gameTime);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _modelCont.Draw(gameTime, _camera);
            DebugShapeRenderer.Draw(gameTime, _camera.View, _camera.Projection);

            base.Draw(gameTime);
        }
    }
}
