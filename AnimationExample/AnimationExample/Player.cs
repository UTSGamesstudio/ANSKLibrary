using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ProjectSneakyGame
{
    public class Player
    {
        private bool _isGamePad;
        private GamePadType _padType;
        private bool _isPC;
        private PlayerIndex _frameworkPlayerIndex;
        private bool _active;
        private GameObject _playerObj;
        private float _score;
        private int _health;

        public GamePadType PadType { get { return _padType; } set { _padType = value; } }
        public bool IsActive { get { return _active; } set { _active = value; } }
        public GameObject PlayerObject { get { return _playerObj; } set { _playerObj = value; } }
        public float Score { get { return _score; } set { _score = value; } }
        public int Health { get { return _health; } set { _health = value; } }

        public Player(PlayerIndex playerIndex)
        {
            _isPC = true;
            _frameworkPlayerIndex = playerIndex;

            EstablishPadConnection();
        }

        public void EstablishPadConnection()
        {
            if (GamePad.GetState(_frameworkPlayerIndex).IsConnected)
                _isGamePad = true;
            else
                _isGamePad = false;
        }

        public bool TestPadConnection()
        {
            if (GamePad.GetState(_frameworkPlayerIndex).IsConnected)
                return true;
            else
                return false;
        }

        public bool IsInputDownPad(Buttons input)
        {
            if (_isGamePad)
            {
                if (GamePad.GetState(_frameworkPlayerIndex).IsButtonDown(input))
                    return true;
                else
                    return false;
            }

            return false;

            // Mouse input checking is not included as there cannot be more than one mouse per local machine game,
            // meaning it's either universally set to Player1 in single player game, or non-player attached in local machine multiplayer.
            //
            // This method is meant to determine if a specific numbered 'Player' has entered an input.
        }

        public bool IsInputDownKeyboard(Keys input)
        {
            if (_isPC)
            {
                if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(input))
                    return true;
                else
                    return false;
            }

            return false;
        }

        static public implicit operator GameObject(Player obj)
        {
            return obj._playerObj;
        }
    }
}
