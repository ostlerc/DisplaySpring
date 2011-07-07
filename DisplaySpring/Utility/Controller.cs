using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace DisplaySpring
{
    /// <summary>
    /// Utility class for interfacing with the keyboard and gamepad
    /// </summary>
    public class Controller
    {
        KeyboardState m_oldKeyboardState;
        KeyboardState m_KeyboardState;
        GamePadState m_oldGamePadState;
        GamePadState m_GamePadState;
        PlayerIndex m_player;

        /// <summary>
        /// List of keys associated with Dpad and Thumbstick Up
        /// </summary>
        public List<Keys> keyUp = new List<Keys>() { Keys.Up };
        /// <summary>
        /// List of keys associated with Dpad and Thumbstick Down
        /// </summary>
        public List<Keys> keyDown = new List<Keys>() { Keys.Down };
        /// <summary>
        /// List of keys associated with Dpad and Thumbstick Left
        /// </summary>
        public List<Keys> keyLeft = new List<Keys>() { Keys.Left };
        /// <summary>
        /// List of keys associated with Dpad and Thumbstick Right
        /// </summary>
        public List<Keys> keyRight = new List<Keys>() { Keys.Right };

        /// <summary>
        /// List of keys associated with Button A
        /// </summary>
        public List<Keys> keyA = new List<Keys>() { Keys.A, Keys.Enter };
        /// <summary>
        /// List of keys associated with Button B
        /// </summary>
        public List<Keys> keyB = new List<Keys>() { Keys.S, Keys.Back };
        /// <summary>
        /// List of keys associated with Button X
        /// </summary>
        public List<Keys> keyX = new List<Keys>() { Keys.Q };
        /// <summary>
        /// List of keys associated with Button Y
        /// </summary>
        public List<Keys> keyY = new List<Keys>() { Keys.W };


        /// <summary>
        /// List of keys associated with Button Start
        /// </summary>
        public List<Keys> keyStart = new List<Keys>() { Keys.Space };

        /// <summary>
        /// List of keys associated with Button Back (select)
        /// </summary>
        public List<Keys> keyBack  = new List<Keys>() { Keys.Escape };

        /// <summary>
        /// List of keys associated with the left shoulder
        /// </summary>
        public List<Keys> leftShoulder = new List<Keys>() { Keys.OemOpenBrackets };
        /// <summary>
        /// List of keys associated with the right shoulder
        /// </summary>
        public List<Keys> rightShoulder = new List<Keys>() { Keys.OemCloseBrackets };

        /// <summary>
        /// List of keys associated with the left trigger
        /// </summary>
        public List<Keys> leftTrigger = new List<Keys>() { Keys.OemSemicolon };

        /// <summary>
        /// List of keys associated with the right trigger
        /// </summary>
        public List<Keys> rightTrigger = new List<Keys>() { Keys.OemQuotes };


        /// <summary>
        /// convert from MultiController to Controller
        /// </summary>
        public static implicit operator Controller(MultiController c)
        {
            List<Controller> l = new List<Controller>();
            l.Add(c);
            return new MultiController(l);
        }

        #region Getters and Setters

        /// <summary>
        /// Returns true if button b has been pressed
        /// </summary>
        public bool Pressed(Buttons b)
        {
            return m_GamePadState.IsButtonDown(b) && !m_oldGamePadState.IsButtonDown(b);
        }

        /// <summary>
        /// returns true if keybard key k has been pressed
        /// </summary>
        public bool Pressed(Keys k)
        {
            return m_KeyboardState.IsKeyDown(k) && !m_oldKeyboardState.IsKeyDown(k);
        }

        /// <summary>
        /// Returns true if one of the buttons in bts has been pressed
        /// </summary>
        public bool Pressed(List<Buttons> bts)
        {
            foreach (var b in bts)
                if (Pressed(b))
                    return true;

            return false;
        }

        /// <summary>
        /// Returns true if one of the keys in keys has been pressed
        /// </summary>
        public bool Pressed(List<Keys> keys)
        {
            foreach (var k in keys)
                if (Pressed(k))
                    return true;

            return false;
        }

        /// <summary>
        /// True if the up key was pressed
        /// </summary>
        public bool Up
        {
            get
            {
                return Pressed(keyUp) || Pressed(Buttons.DPadUp) || Pressed(Buttons.LeftThumbstickUp);
            }
        }

        /// <summary>
        /// True if the down key was pressed
        /// </summary>
        public bool Down
        {
            get
            {
                return Pressed(keyDown) || Pressed(Buttons.DPadDown) || Pressed(Buttons.LeftThumbstickDown);
            }
        }

        /// <summary>
        /// True if the left key was pressed
        /// </summary>
        public bool Left
        {
            get
            {
                return Pressed(keyLeft) || Pressed(Buttons.DPadLeft) || Pressed(Buttons.LeftThumbstickLeft);
            }
        }

        /// <summary>
        /// True if the right key was pressed
        /// </summary>
        public bool Right
        {
            get
            {
                return Pressed(keyRight) || Pressed(Buttons.DPadRight) || Pressed(Buttons.LeftThumbstickRight);
            }
        }

        /// <summary>
        /// True if the A key was pressed
        /// </summary>
        public bool A
        {
            get
            {
                return Pressed(keyA) || Pressed(Buttons.A);
            }
        }

        /// <summary>
        /// True if the B key was pressed
        /// </summary>
        public bool B
        {
            get
            {
                return Pressed(keyB) || Pressed(Buttons.B);
            }
        }

        /// <summary>
        /// True if the X key was pressed
        /// </summary>
        public bool X
        {
            get
            {
                return Pressed(keyX) || Pressed(Buttons.X);
            }
        }

        /// <summary>
        /// True if the Y key was pressed
        /// </summary>
        public bool Y
        {
            get
            {
                return Pressed(keyY) || Pressed(Buttons.Y);
            }
        }

        /// <summary>
        /// True if X and ONLY X is pressed
        /// </summary>
        public bool ExclusiveX
        {
            get
            {
                return X && !Y && !A && !B;
            }
        }

        /// <summary>
        /// True if Y and ONLY Y is pressed
        /// </summary>
        public bool ExclusiveY
        {
            get
            {
                return !X && Y && !A && !B;
            }
        }

        /// <summary>
        /// True if A and ONLY A is pressed
        /// </summary>
        public bool ExclusiveA
        {
            get
            {
                return !X && !Y && A && !B;
            }
        }

        /// <summary>
        /// True if B and ONLY B is pressed
        /// </summary>
        public bool ExclusiveB
        {
            get
            {
                return !X && !Y && !A && B;
            }
        }

        /// <summary>
        /// True if the LeftShoulder is pressed
        /// </summary>
        public bool LeftShoulder
        {
            get
            {
                return Pressed(leftShoulder) || Pressed(Buttons.LeftShoulder);
            }
        }

        /// <summary>
        /// True if the right shoulder is pressed
        /// </summary>
        public bool RightShoulder
        {
            get
            {
                return Pressed(rightShoulder) || Pressed(Buttons.RightShoulder);
            }
        }

        /// <summary>
        /// True if the left trigger is being held down
        /// </summary>
        public bool LeftTrigger
        {
            get
            {
                return Pressed(leftTrigger) || Pressed(Buttons.LeftTrigger);
            }
        }

        /// <summary>
        /// True if the right trigger is being held down
        /// </summary>
        public bool RightTrigger
        {
            get
            {
                return Pressed(rightTrigger) || Pressed(Buttons.RightTrigger);
            }
        }

        /// <summary>
        /// True if any mapped keyboard or gamepad button is pressed
        /// </summary>
        public bool AnyInput
        {
            get
            {
                return (AnyButton || A || B || X || Start || Y || Up || Down || Left || Right || LeftShoulder || RightShoulder);
            }
        }

        /// <summary>
        /// True if any button is pressed
        /// </summary>
        public bool AnyButton
        {
            get
            {
                return A || B || X || Y || Start;
            }
        }

        /// <summary>
        /// True if Start is pressed
        /// </summary>
        public bool Start
        {
            get
            {
                return Pressed(keyStart) || Pressed(Buttons.Start);
            }
        }

        /// <summary>
        /// Float for how much the stick is pushed up (from -1 to 1)
        /// </summary>
        public float MoveUp
        {
            get
            {
                if (m_KeyboardState.IsKeyDown(Keys.Up))
                {
                    if (m_KeyboardState.IsKeyDown(Keys.Left) || m_KeyboardState.IsKeyDown(Keys.Right))
                        return -0.707105f; //WTF?
                    else
                        return -1;
                }
                else
                {
                    return -m_GamePadState.ThumbSticks.Left.Y;
                }
            }
        }

        /// <summary>
        /// Float for how much the stick is pushed right (from -1 to 1)
        /// </summary>
        public float MoveRight
        {
            get
            {
                if (m_KeyboardState.IsKeyDown(Keys.Right))
                {
                    if (m_KeyboardState.IsKeyDown(Keys.Up) || m_KeyboardState.IsKeyDown(Keys.Down))
                        return 0.707105f;
                    else
                        return 1;
                }
                else
                {
                    return m_GamePadState.ThumbSticks.Left.X;
                }
            }
        }

        /// <summary>
        /// Float for how much the stick is pushed down (from -1 to 1)
        /// </summary>
        public float MoveDown
        {
            get
            {
                if (m_KeyboardState.IsKeyDown(Keys.Down))
                {
                    if (m_KeyboardState.IsKeyDown(Keys.Left) || m_KeyboardState.IsKeyDown(Keys.Right))
                        return 0.707105f;
                    else
                        return 1;
                }
                else
                {
                    return -m_GamePadState.ThumbSticks.Left.Y;
                }
            }
        }

        /// <summary>
        /// Float for how much the stick is pushed left (from -1 to 1)
        /// </summary>
        public float MoveLeft
        {
            get
            {
                if (m_KeyboardState.IsKeyDown(Keys.Left))
                {
                    if (m_KeyboardState.IsKeyDown(Keys.Up) || m_KeyboardState.IsKeyDown(Keys.Down))
                        return -0.707105f;
                    else
                        return -1;
                }
                else
                {
                    return m_GamePadState.ThumbSticks.Left.X;
                }
            }
        }

        /// <summary>
        /// True if the Back button is pushed
        /// </summary>
        public bool Back
        {
            get
            {
                return Pressed(keyBack) || Pressed(Buttons.Back);
            }
        }

        /// <summary>
        /// Returns the player index that the Controller references
        /// </summary>
        public PlayerIndex PlayerIndex
        {
            get
            {
                return m_player;
            }
        }

        /// <summary>
        /// Returns if the controller is active
        /// </summary>
        public bool IsActive()
        {
#if DEBUG
            return m_GamePadState.IsConnected || m_player == Microsoft.Xna.Framework.PlayerIndex.Two;
#else
        return m_GamePadState.IsConnected;
#endif

        }

        #endregion

        #region Constructors
        /// <summary>
        /// Create a new controller class
        /// </summary>
        public Controller(PlayerIndex currentPlayer, UpdateEvent update)
        {
            m_oldKeyboardState = new KeyboardState();
            m_oldGamePadState = new GamePadState();
            m_KeyboardState = new KeyboardState();
            m_GamePadState = new GamePadState();
            m_player = currentPlayer;
            update.AddEvent(Update);
        }
        #endregion

        private void Update()
        {
#if DEBUG   //keyboard states only in debug mode
            m_oldKeyboardState = m_KeyboardState;
            m_KeyboardState = Keyboard.GetState();
#endif
            m_oldGamePadState = m_GamePadState;
            m_GamePadState = GamePad.GetState(m_player);
        }
    }

}