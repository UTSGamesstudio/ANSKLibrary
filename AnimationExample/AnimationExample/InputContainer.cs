using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace ProjectSneakyGame
{
    /// <summary>
    /// Container that holds the entered input from an Xbox 360 Controller, Keyboard and Mouse.
    /// Input is updated after it's parent 'Input' class has run its update method.
    /// 
    /// IMPORTANT: This class should only be used when retrieved from an 'Input' class via the 'RetrieveInputContainer' getter. 
    /// </summary>
    public sealed class InputContainer
    {
        /// <summary>
        /// Enumeration for specifying which Player input to check. Used in Xbox controller checking.
        /// </summary>
        public enum IndexPlayers { Player1 = 0, Player2 = 1, Player3 = 2, Player4 = 3, None = -1 };
		
		private Input _parentInput;                                                                     // The parent Input class that this InputContainer was retrieved from.
        private List<Dictionary<Buttons, InputTypeComplex>> _gamePadCommandsC;                          // The List of Dictionaries of Xbox controller InputTypeComplex data that accesses the parent List via pointers.
        private Dictionary<Keys, InputTypeComplex> _keyboardCommandsC;                                  // The Dictionary of Keyboard InputTypeComplex data that accesses the parent Dictionary via pointers.
        private Dictionary<MouseInput.MouseButtonsXNA, InputTypeComplex> _mouseCommandsC;               // The Dictionary of Mouse InputTypeComplex data that accesses the parent Dictionary via pointers.
        private MouseInput _mouse;                                                                      // Data for the Mouse input that accesses the parent MouseInput data.

        public MouseInput MouseData { get { return _mouse; } }

        public InputContainer(List<Dictionary<Buttons, InputTypeComplex>> gamePadRefC, Dictionary<Keys, InputTypeComplex> keyboardRefC, Dictionary<MouseInput.MouseButtonsXNA, InputTypeComplex> mouseRefC, MouseInput mouse, Input parent)
        {
            _gamePadCommandsC = gamePadRefC;
            _keyboardCommandsC = keyboardRefC;
            _mouseCommandsC = mouseRefC;
            _mouse = mouse;
			_parentInput = parent;
        }
		
		~InputContainer()
		{
            _mouse = null;
            _parentInput = null;
		}

        public bool IsFirstPressed(MouseInput.MouseButtonsXNA button)
        {
			return _mouseCommandsC [button].FirstPressed;
        }

        public bool IsFirstPressed(Keys key)
        {
			return _keyboardCommandsC [key].FirstPressed;
        }

        public bool IsFirstPressed(Buttons button)
        {
			return _gamePadCommandsC [0][button].FirstPressed;
        }

        public bool IsFirstPressed(IndexPlayers player, Buttons button)
        {
			return _gamePadCommandsC [(int)player] [button].FirstPressed;
        }

        public bool IsFirstReleased(MouseInput.MouseButtonsXNA button)
        {
			return _mouseCommandsC [button].FirstReleased;
        }

        public bool IsFirstReleased(Keys key)
        {
			return _keyboardCommandsC [key].FirstReleased;
        }

        public bool IsFirstReleased(Buttons button)
        {
			return _gamePadCommandsC [0] [button].FirstReleased;
        }

        public bool IsFirstReleased(IndexPlayers player, Buttons button)
        {
			return _gamePadCommandsC [(int)player] [button].FirstReleased;
        }

        public void EmulateInput(MouseInput.MouseButtonsXNA button)
        {
			_mouseCommandsC [button].Pressed = true;
        }

        public void EmulateInput(Keys key)
        {
			_keyboardCommandsC [key].Pressed = true;
        }

        public void EmulateInput(Buttons button)
        {
            EmulateInput(IndexPlayers.Player1, button);
        }

        public void EmulateInput(IndexPlayers player, Buttons button)
        {
			_gamePadCommandsC [(int)player] [button].Pressed = true;
        }

        public double TimePressed(MouseInput.MouseButtonsXNA button)
		{
			return _mouseCommandsC [button].TimePressed;
		}

		public double TimePressed(Keys key)
		{
			return _keyboardCommandsC [key].TimePressed;
		}

		public double TimePressed(Buttons button)
		{
			return _gamePadCommandsC [0] [button].TimePressed;
		}

		public double TimePressed(IndexPlayers player, Buttons button)
		{
			return _gamePadCommandsC [(int)player] [button].TimePressed;
		}

        /// <summary>
        /// Checks the Xbox Controller input from the specified player to see if it has been pressed.
        /// </summary>
        /// <param name="player">'IndexPlayer' enum type.</param>
        /// <param name="button">'Button' enum type of which input to check for.</param>
        /// <returns>Returns true if button is pressed on specified player. Returns false if not.</returns>
        public bool this[IndexPlayers player, Buttons button]
        {
            get
            {
				return _gamePadCommandsC [(int)player] [button].Pressed;
            }
        }

        /// <summary>
        /// Checks the Xbox Controller input from Player1 to see if the specified button has been pressed.
        /// </summary>
        /// <param name="button">'Button' enum type of which input to check for.</param>
        /// <returns>Returns true if button is pressed on Player1. Returns false if not.</returns>
        public bool this[Buttons button]
        {
            get
            {
				return _gamePadCommandsC [0] [button].Pressed;
            }
        }

        /// <summary>
        /// Checks the Keyboard to see if the specified key has been pressed.
        /// </summary>
        /// <param name="key">'Keys' enum type of which input to check for.</param>
        /// <returns>Return true if key is pressed. Returns flase if not.</returns>
        public bool this[Keys key]
        {
            get
            {
				return _keyboardCommandsC [key].Pressed;
            }
        }

        /// <summary>
        /// Checks the Mouse to see if the specified mouse button has been pressed.
        /// </summary>
        /// <param name="button">'MouseButtonsXNA' enum type of which input to check for.</param>
        /// <returns>Return true if mouse button is pressed. Return false if not.</returns>
        public bool this[MouseInput.MouseButtonsXNA button]
        {
            get
            {
				return _mouseCommandsC [button].Pressed;
            }
        }
    }
}
