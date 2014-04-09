using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using AnimationExample;

namespace ProjectSneakyGame
{
    /// <summary>
    /// The class that saves the information of input data.
    /// </summary>
    public class InputTypeComplex
    {
        protected enum Type { Pad, Key, Mouse, None = -1 };

        protected object _button;
        protected Type _type;
        protected double _timePressed;
        protected bool _pressed, _prevPressed;
        protected MouseInput _mouse;

        public double TimePressed { get { return _timePressed; } set { _timePressed = value; } }
        public bool Pressed { get { return _pressed; } set { _prevPressed = _pressed; _pressed = value; } }
        public MouseInput MouseData { get { return _mouse; } set { _mouse = value; } }

        public bool FirstPressed
        {
            get
            {
                if (_prevPressed)
                    return false;
                else if (_pressed)
                    return true;
                else
                    return false;
            }
        }

        public bool FirstReleased
        {
            get
            {
                if (_pressed)
                    return false;
                else if (_prevPressed)
                    return true;
                else
                    return false;
            }
        }

        public InputTypeComplex()
        {
            _button = -1;
            _type = Type.None;
            _timePressed = 0;
            _pressed = _prevPressed = false;
            _mouse = null;
        }

        public InputTypeComplex(Keys button)
        {
            _button = button;
            _type = Type.Key;
            _timePressed = 0;
            _pressed = _prevPressed = false;
            _mouse = null;
        }

        public InputTypeComplex(Buttons button)
        {
            _button = button;
            _type = Type.Pad;
            _timePressed = 0;
            _pressed = _prevPressed = false;
            _mouse = null;
        }

        public InputTypeComplex(MouseInput.MouseButtonsXNA button, Game1 game)
        {
            _button = button;
            _type = Type.Mouse;
            _timePressed = 0;
            _pressed = _prevPressed = false;
            _mouse = new MouseInput(game);
        }
    }

    public class MouseInput
    {
        public enum MouseButtonsXNA { LeftButton, RightButton, MiddleButton, XButton1, XButton2, None = -1 };

        private float _x, _prevX;
        private float _y, _prevY;
        private float _scrollV, _prevScrollV;
        private bool _mouseForcedCenter;
        private Vector2 _center;
        private bool _leftClicked, _prevLeftClick, _rightClicked, _prevRightClick, _middleClicked, _prevMiddleClick;
        private bool _xButton1Clicked, _prevXButton1Click, _xButton2Clicked, _prevXButton2Click;
        private Game1 _game;

        public float X { get { return _x; } }
        public float PrevX { get { return _prevX; } }
        public float Y { get { return _y; } }
        public float PrevY { get { return _prevY; } }
        public float ScrollWheelValue { get { return _scrollV; } }
        public float ScrollWheelBasicDifference { get { return ScrollWheelUpdateDifference / 120; } }
        public float ScrollWheelUpdateDifference { get { return _scrollV - _prevScrollV; } }
        public float XUpdateDifference { get { return _x - _prevX; } }
        public float YUpdateDifference { get { return _y - _prevY; } }
        public bool HasMovedUp { get { return (_y > _prevY); } }
        public bool HasMovedDown { get { return (_y < _prevY); } }
        public bool HasMovedLeft { get { return (_x < _prevX); } }
        public bool HasMovedRight { get { return (_x > _prevX); } }
        public bool MouseForcedCenter { get { return _mouseForcedCenter; } set { _mouseForcedCenter = value; } }
        public Vector2 CenterScreen { get { return _center; } set { _center = value; } }
        public bool LeftButtonFirstClick { get { if (_prevLeftClick) return false; else if (_leftClicked) return true; else return false; } }
        public bool RightButtonFirstClick { get { if (_prevRightClick) return false; else if (_rightClicked) return true; else return false; } }
        public bool MiddleButtonFirstClick { get { if (_prevMiddleClick) return false; else if (_middleClicked) return true; else return false; } }
        public bool XButton1FirstClick { get { if (_prevXButton1Click) return false; else if (_xButton1Clicked) return true; else return false; } }
        public bool XButton2FirstClick { get { if (_prevXButton2Click) return false; else if (_xButton2Clicked) return true; else return false; } }
        public bool LeftButtonFirstRelease { get { if (_leftClicked) return false; else if (_prevLeftClick) return true; else return false; } }
        public bool RightButtonFirstRelease { get { if (_rightClicked) return false; else if (_prevRightClick) return true; else return false; } }
        public bool MiddleButtonFirstRelease { get { if (_middleClicked) return false; else if (_prevMiddleClick) return true; else return false; } }
        public bool XButton1FirstRelease { get { if (_xButton1Clicked) return false; else if (_prevXButton1Click) return true; else return false; } }
        public bool XButton2FirstRelease { get { if (_xButton2Clicked) return false; else if (_prevXButton2Click) return true; else return false; } }
        public bool LeftButtonDown { get { if (_leftClicked) return true; else return false; } }
        public bool RightButtonDown { get { if (_rightClicked) return true; else return false; } }
        public bool MiddleButtonDown { get { if (_middleClicked) return true; else return false; } }
        public bool XButton1Down { get { if (_xButton1Clicked) return true; else return false; } }
        public bool XButton2Down { get { if (_xButton2Clicked) return true; else return false; } }
        public bool MouseVisibility { get { return _game.IsMouseVisible; } set { _game.IsMouseVisible = value; } }
        
        public MouseInput(Game1 game)
        {
            _x = _prevX = _y = _prevY = _scrollV = _prevScrollV = 0;
            _leftClicked = _prevLeftClick = _rightClicked = _prevRightClick = _middleClicked = _prevMiddleClick =
                _xButton1Clicked = _prevXButton1Click = _xButton2Clicked = _prevXButton2Click = false;
            _mouseForcedCenter = false;
            _game = game;
            _game.IsMouseVisible = false;
        }

        public void UpdatePosition()
        {
            MouseState state = Mouse.GetState();

            _prevX = _x;
            _prevY = _y;
            _prevScrollV = _scrollV;
            _x = state.X;
            _y = state.Y;
            _scrollV = state.ScrollWheelValue;
            _prevLeftClick = _leftClicked;
            if (state.LeftButton == ButtonState.Pressed)
                _leftClicked = true;
            else
                _leftClicked = false;
            _prevRightClick = _rightClicked;
            if (state.RightButton == ButtonState.Pressed)
                _rightClicked = true;
            else
                _rightClicked = false;
            _prevMiddleClick = _middleClicked;
            if (state.MiddleButton == ButtonState.Pressed)
                _middleClicked = true;
            else
                _middleClicked = false;
            _prevXButton1Click = _xButton1Clicked;
            if (state.XButton1 == ButtonState.Pressed)
                _xButton1Clicked = true;
            else
                _xButton1Clicked = false;
            _prevXButton2Click = _xButton2Clicked;
            if (state.XButton2 == ButtonState.Pressed)
                _xButton2Clicked = true;
            else
                _xButton2Clicked = false;

            if (_mouseForcedCenter)
                ForcedCenterControl();
        }

        private void ForcedCenterControl()
        {
            if ((int)_x == (int)_center.X)
                return;

            _prevX = (int)_center.X;
            _prevY = (int)_center.Y;

            Mouse.SetPosition((int)_center.X, (int)_center.Y);
        }
    }
}
