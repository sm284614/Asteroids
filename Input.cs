using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Asteroids
{
    public static class Input //deals with keyboard/mouse
    {
        private static KeyboardState _keyboard_state; //what keys are being pressed?
        private static KeyboardState _keyboard_state_previous; //what keys were being pressed last frame?
        private static MouseState _mouse_state; //where's the cursor/what mouse buttons are pressed?
        private static MouseState _mouse_state_previous;
        public static void Update()
        {
            _keyboard_state_previous = _keyboard_state; //remember what keys were pressed this frame so they can be compared to next frame
            _mouse_state_previous = _mouse_state;
            _keyboard_state = Keyboard.GetState(); //get data from keyboard for this frame
            _mouse_state = Mouse.GetState(); //get data from mouse for this frame
        }

        public static Point MousePosition { get => _mouse_state.Position; } //Point is an [X,Y] int structure
        
        public static bool KeyPressedOnce(Keys key)
        {
            return _keyboard_state.IsKeyDown(key) && !_keyboard_state_previous.IsKeyDown(key); //down this fram but not last
        }
        public static bool KeyDown(Keys key)
        {
            return _keyboard_state.IsKeyDown(key); //help down this frame
        }
        public static bool MouseInside(Rectangle rectangle)
        {
            return rectangle.Contains(_mouse_state.Position);
        }
        public static bool MouseClickedOn(Rectangle rectangle)
        {
            return (_mouse_state.LeftButton == ButtonState.Pressed && MouseInside(rectangle)); //left mouse down inside rectangle
        }
        public static bool KeyPressedandReleased(Keys k)
        {
            return _keyboard_state.IsKeyUp(k) && _keyboard_state_previous.IsKeyDown(k); //released this frame, but down last frame
        }
        public static bool RightMouseButtonClickedOnce()
        {
            return _mouse_state.RightButton == ButtonState.Pressed && _mouse_state_previous.RightButton == ButtonState.Released;
        }
        public static bool LeftMouseButtonClickedOnce()
        {
            return _mouse_state.LeftButton == ButtonState.Pressed && _mouse_state_previous.LeftButton == ButtonState.Released;
        }
        public static bool LeftMouseButtonHeldDown()
        {
            return _mouse_state.LeftButton == ButtonState.Pressed && _mouse_state_previous.LeftButton == ButtonState.Pressed;
        }
    }
}
