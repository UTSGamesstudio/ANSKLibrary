using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using AnimationExample;

namespace ProjectSneakyGame
{
    /// <summary>
    /// The input manager class.
    /// Retrieves the inputs from the computer and processes useful data about them.
    /// Is not involved with user checking or retrieval of input data.
    /// </summary>
    public sealed class Input : Microsoft.Xna.Framework.GameComponent
    {
        private bool _padEnable, _keyEnable, _mouseEnable;                                          // Toggles for whether the Xbox controller/s, Keyboard or Mouse should be checked for input.
        private InputContainer _container;                                                          // The Input Container in which the inputs are organised and preapred for the user.
		private List<Buttons> _gamePadToCheck;                                                      // List of Xbox controller inputs to check for.
		private List<Keys> _keyboardToCheck;                                                        // List of Keyboard inputs to check for.
        private List<Player> _players;                                                              // List of players that are registered in the game. Used for Xbox controller input organisation.
        private List<Dictionary<Buttons, InputTypeComplex>> _gamePadCommandsC;                      // List of Dictionaries of Xbox controller InputTypeComplex data. 1 Dictionary is assigned to 1 player. Max of 4.
        private Dictionary<Keys, InputTypeComplex> _keyboardCommandsC;                              // Dictionary of Keyboard InputTypeComplex data.
        private Dictionary<MouseInput.MouseButtonsXNA, InputTypeComplex> _mouseCommandsC;           // Dictionary of Mouse InputTypeComplex data.
        private MouseInput _mouse;                                                                  // Data for the Mouse input.

        /// <summary>
        /// Collect the Input Container which will interperet the input data to users. Necessary call in order for input checking to work.
        /// </summary>
        public InputContainer RetrieveInputContainer { get { return _container; } }
        public bool CheckInputController { get { return _padEnable; } set { _padEnable = value; } }
        public bool CheckInputKeyboard { get { return _keyEnable; } set { _keyEnable = value; } }
        public bool CheckInputMouse { get { return _mouseEnable; } set { _mouseEnable = value; } }
        /// <summary>
        /// Toggle for whether the mouse cursor is locked to the center of the screen.
        /// </summary>
        public bool MouseForcedCenter { get { return _mouse.MouseForcedCenter; } set { _mouse.MouseForcedCenter = value; } }

        public Input(Game1 game) : base(game)
        {
            _gamePadToCheck = new List<Buttons>();
            _keyboardToCheck = new List<Keys>();

			_gamePadCommandsC = new List<Dictionary<Buttons, InputTypeComplex>> ();
            _keyboardCommandsC = new Dictionary<Keys, InputTypeComplex>();
            _mouseCommandsC = new Dictionary<MouseInput.MouseButtonsXNA, InputTypeComplex>();

            _mouseCommandsC.Add(MouseInput.MouseButtonsXNA.LeftButton, new InputTypeComplex());
            _mouseCommandsC.Add(MouseInput.MouseButtonsXNA.RightButton, new InputTypeComplex());
            _mouseCommandsC.Add(MouseInput.MouseButtonsXNA.MiddleButton, new InputTypeComplex());
            _mouseCommandsC.Add(MouseInput.MouseButtonsXNA.XButton1, new InputTypeComplex());
            _mouseCommandsC.Add(MouseInput.MouseButtonsXNA.XButton2, new InputTypeComplex());

            _padEnable = _keyEnable = _mouseEnable = true;

            for (int i = 0; i < LocalPlayerRegistry.MAX_LOCAL_MACHINE_PLAYERS; i++)
            {
				_gamePadCommandsC.Add(new Dictionary<Buttons, InputTypeComplex>());
            }

            _mouse = new MouseInput(game);
            _mouse.CenterScreen = new Microsoft.Xna.Framework.Vector2(game.GraphicsDevice.DisplayMode.Width / 2, game.GraphicsDevice.DisplayMode.Height / 2);

            _container = new InputContainer(_gamePadCommandsC, _keyboardCommandsC, _mouseCommandsC, _mouse, this);

            ReloadPlayerList();
        }
		
		~Input()
		{
			_gamePadToCheck = null;
			_keyboardToCheck = null;
			_container = null;

            _gamePadCommandsC = null;
            _keyboardCommandsC = null;
            _mouseCommandsC = null;
		}

        public void ReloadPlayerList()
        {
            _players = LocalPlayerRegistry.PlayerList;
        }

		public void LoadDefaultCheckableCommands()
		{
			LoadDefaultCheckableCommandsGamePad();
			LoadDefaultCheckableComandsKeyboard();
		}
		
		public void LoadDefaultCheckableCommandsGamePad()
		{
			_gamePadToCheck.Add(Buttons.A);
			_gamePadToCheck.Add(Buttons.B);
			_gamePadToCheck.Add(Buttons.X);
			_gamePadToCheck.Add(Buttons.Y);
			_gamePadToCheck.Add(Buttons.Back);
			_gamePadToCheck.Add(Buttons.Start);
			_gamePadToCheck.Add(Buttons.LeftThumbstickDown);
			_gamePadToCheck.Add(Buttons.LeftThumbstickUp);
			_gamePadToCheck.Add(Buttons.LeftThumbstickLeft);
			_gamePadToCheck.Add(Buttons.LeftThumbstickRight);
			_gamePadToCheck.Add(Buttons.RightThumbstickDown);
			_gamePadToCheck.Add(Buttons.RightThumbstickUp);
			_gamePadToCheck.Add(Buttons.RightThumbstickLeft);
			_gamePadToCheck.Add(Buttons.RightThumbstickRight);

			for (int i = 0; i < _gamePadCommandsC.Count; i++)
			{
	            _gamePadCommandsC[i].Add(Buttons.A, new InputTypeComplex());
	            _gamePadCommandsC[i].Add(Buttons.B, new InputTypeComplex());
	            _gamePadCommandsC[i].Add(Buttons.X, new InputTypeComplex());
	            _gamePadCommandsC[i].Add(Buttons.Y, new InputTypeComplex());
	            _gamePadCommandsC[i].Add(Buttons.Back, new InputTypeComplex());
	            _gamePadCommandsC[i].Add(Buttons.Start, new InputTypeComplex());
	            _gamePadCommandsC[i].Add(Buttons.LeftThumbstickDown, new InputTypeComplex());
	            _gamePadCommandsC[i].Add(Buttons.LeftThumbstickUp, new InputTypeComplex());
	            _gamePadCommandsC[i].Add(Buttons.LeftThumbstickLeft, new InputTypeComplex());
	            _gamePadCommandsC[i].Add(Buttons.LeftThumbstickRight, new InputTypeComplex());
	            _gamePadCommandsC[i].Add(Buttons.RightThumbstickDown, new InputTypeComplex());
	            _gamePadCommandsC[i].Add(Buttons.RightThumbstickUp, new InputTypeComplex());
	            _gamePadCommandsC[i].Add(Buttons.RightThumbstickLeft, new InputTypeComplex());
	            _gamePadCommandsC[i].Add(Buttons.RightThumbstickRight, new InputTypeComplex());
			}
		}
		
		public void LoadDefaultCheckableComandsKeyboard()
		{
			_keyboardToCheck.Add(Keys.W);
			_keyboardToCheck.Add(Keys.A);
			_keyboardToCheck.Add(Keys.S);
			_keyboardToCheck.Add(Keys.D);
			_keyboardToCheck.Add(Keys.Escape);
			_keyboardToCheck.Add(Keys.LeftShift);
			_keyboardToCheck.Add(Keys.Space);

            _keyboardCommandsC.Add(Keys.W, new InputTypeComplex());
            _keyboardCommandsC.Add(Keys.A, new InputTypeComplex());
            _keyboardCommandsC.Add(Keys.S, new InputTypeComplex());
            _keyboardCommandsC.Add(Keys.D, new InputTypeComplex());
            _keyboardCommandsC.Add(Keys.Escape, new InputTypeComplex());
            _keyboardCommandsC.Add(Keys.LeftShift, new InputTypeComplex());
            _keyboardCommandsC.Add(Keys.Space, new InputTypeComplex());
		}
		
		public void AddCommandToCheckGamePad(Buttons button)
		{
			_gamePadToCheck.Add(button);

            for (int i = 0; i < _gamePadCommandsC.Count; i++)
                _gamePadCommandsC[i].Add(button, new InputTypeComplex());
		}
		
		public void AddCommandToCheckGamePad(params Buttons[] button)
		{
            for (int i = 0; i < button.Length; i++)
            {
                _gamePadToCheck.Add(button[i]);

				for (int q = 0; q < _gamePadCommandsC.Count; q++) 
				{
					_gamePadCommandsC[q].Add(button[i], new InputTypeComplex());
				}
            }
		}
		
		public void AddCommandToCheckKeyboard(Keys key)
		{
			_keyboardToCheck.Add(key);

            _keyboardCommandsC.Add(key, new InputTypeComplex());
		}
		
		public void AddCommandToCheckKeyboard(params Keys[] key)
		{
            for (int i = 0; i < key.Length; i++)
            {
                _keyboardToCheck.Add(key[i]);

                _keyboardCommandsC.Add(key[i], new InputTypeComplex());
            }
		}
		
		public void ClearCheckableCommands()
		{
			ClearCheckableGamePadCommands();
			ClearCheckableKeyboardCommands();
		}
		
		public void ClearCheckableGamePadCommands()
		{
			_gamePadToCheck.Clear();
		}

        public void ClearCheckableKeyboardCommands()	
		{
			_keyboardToCheck.Clear();
		}

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{
            base.Update(gameTime);

            // Loop through all the Xbox controllers that are plugged in and check for inputs. If Xbox controller checking is enabled.
            for (int i = 0; i < _players.Count; i++)
            {
                if (_players[i] != null)
                {
                    if (_padEnable)
                        PadInputCheck(i, gameTime);
                }
            }

            // If Keyboard input checking is on, then check for Keyboard input.
            if (_keyEnable)
                KeyInputCheck(gameTime);

            // If Mouse input checking is on, then check for Mouse input.
            if (_mouseEnable)
            {
                MouseKeysDown(gameTime);
                _mouse.UpdatePosition();
            }
		}

        private void PadInputCheck(int i, Microsoft.Xna.Framework.GameTime gameTime)
        {
            for (int w = 0; w < _gamePadToCheck.Count; w++)
            {
                Buttons temp = _gamePadToCheck[w];
                if (_players[i].IsInputDownPad(temp))
                {
                    _gamePadCommandsC[i][temp].Pressed = true;
                    _gamePadCommandsC[i][temp].TimePressed += gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    _gamePadCommandsC[i][temp].Pressed = false;
                    _gamePadCommandsC[i][temp].TimePressed = 0;
                }
            }

            return;
        }

        private void KeyInputCheck(Microsoft.Xna.Framework.GameTime gameTime)
        {
            for (int i = 0; i < _keyboardToCheck.Count; i++)
            {
                Keys temp = _keyboardToCheck[i];
                if (_players[0].IsInputDownKeyboard(temp))
                {
                    _keyboardCommandsC[temp].Pressed = true;
                    _keyboardCommandsC[temp].TimePressed += gameTime.ElapsedGameTime.TotalSeconds;
                }
                else
                {
                    _keyboardCommandsC[temp].Pressed = false;
                    _keyboardCommandsC[temp].TimePressed = 0;
                }
            }

            return;
        }

        private void MouseKeysDown(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                _mouseCommandsC[MouseInput.MouseButtonsXNA.LeftButton].Pressed = true;
                _mouseCommandsC[MouseInput.MouseButtonsXNA.LeftButton].TimePressed += gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                _mouseCommandsC[MouseInput.MouseButtonsXNA.LeftButton].Pressed = false;
                _mouseCommandsC[MouseInput.MouseButtonsXNA.LeftButton].TimePressed = 0;
            }

            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                _mouseCommandsC[MouseInput.MouseButtonsXNA.RightButton].Pressed = true;
                _mouseCommandsC[MouseInput.MouseButtonsXNA.RightButton].TimePressed += gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                _mouseCommandsC[MouseInput.MouseButtonsXNA.RightButton].Pressed = false;
                _mouseCommandsC[MouseInput.MouseButtonsXNA.RightButton].TimePressed = 0;
            }

            if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
            {
                _mouseCommandsC[MouseInput.MouseButtonsXNA.MiddleButton].Pressed = true;
                _mouseCommandsC[MouseInput.MouseButtonsXNA.MiddleButton].TimePressed += gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                _mouseCommandsC[MouseInput.MouseButtonsXNA.MiddleButton].Pressed = false;
                _mouseCommandsC[MouseInput.MouseButtonsXNA.MiddleButton].TimePressed = 0;
            }

            if (Mouse.GetState().XButton1 == ButtonState.Pressed)
            {
                _mouseCommandsC[MouseInput.MouseButtonsXNA.XButton1].Pressed = true;
                _mouseCommandsC[MouseInput.MouseButtonsXNA.XButton1].TimePressed += gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                _mouseCommandsC[MouseInput.MouseButtonsXNA.XButton1].Pressed = false;
                _mouseCommandsC[MouseInput.MouseButtonsXNA.XButton1].TimePressed = 0;
            }

            if (Mouse.GetState().XButton2 == ButtonState.Pressed)
            {
                _mouseCommandsC[MouseInput.MouseButtonsXNA.XButton2].Pressed = true;
                _mouseCommandsC[MouseInput.MouseButtonsXNA.XButton2].TimePressed += gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                _mouseCommandsC[MouseInput.MouseButtonsXNA.XButton2].Pressed = false;
                _mouseCommandsC[MouseInput.MouseButtonsXNA.XButton2].TimePressed = 0;
            }
        }
    }
}
