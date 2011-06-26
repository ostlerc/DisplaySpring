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

namespace DisplaySpring.Menus
{
    public class Controller
    {
        KeyboardState m_oldKeyboardState;
        KeyboardState m_KeyboardState;
        GamePadState m_oldGamePadState;
        GamePadState m_GamePadState;
        PlayerIndex m_player;

        //convert from MultiController to Controller
        public static implicit operator Controller(MultiController c)
        {
            List<Controller> l = new List<Controller>();
            l.Add(c);
            return new MultiController(l);
        }

        #region Getters and Setters
        /// <summary>
        /// True if the up key was pressed
        /// </summary>
        public bool Up
        {
            get
            {
                return (m_KeyboardState.IsKeyDown(Keys.Up) && !m_oldKeyboardState.IsKeyDown(Keys.Up)) ||
                        (m_GamePadState.IsButtonDown(Buttons.DPadUp) && !m_oldGamePadState.IsButtonDown(Buttons.DPadUp)) ||
                        (m_GamePadState.IsButtonDown(Buttons.LeftThumbstickUp) && !m_oldGamePadState.IsButtonDown(Buttons.LeftThumbstickUp));
            }
        }

        /// <summary>
        /// True if the down key was pressed
        /// </summary>
        public bool Down
        {
            get
            {
                return (m_KeyboardState.IsKeyDown(Keys.Down) && !m_oldKeyboardState.IsKeyDown(Keys.Down)) ||
                        (m_GamePadState.IsButtonDown(Buttons.DPadDown) && !m_oldGamePadState.IsButtonDown(Buttons.DPadDown)) ||
                        (m_GamePadState.IsButtonDown(Buttons.LeftThumbstickDown) && !m_oldGamePadState.IsButtonDown(Buttons.LeftThumbstickDown));
            }
        }

        /// <summary>
        /// True if the left key was pressed
        /// </summary>
        public bool Left
        {
            get
            {
                return (m_KeyboardState.IsKeyDown(Keys.Left) && !m_oldKeyboardState.IsKeyDown(Keys.Left)) ||
                        (m_GamePadState.IsButtonDown(Buttons.DPadLeft) && !m_oldGamePadState.IsButtonDown(Buttons.DPadLeft)) ||
                        (m_GamePadState.IsButtonDown(Buttons.LeftThumbstickLeft) && !m_oldGamePadState.IsButtonDown(Buttons.LeftThumbstickLeft));
            }
        }

        /// <summary>
        /// True if the right key was pressed
        /// </summary>
        public bool Right
        {
            get
            {
                return (m_KeyboardState.IsKeyDown(Keys.Right) && !m_oldKeyboardState.IsKeyDown(Keys.Right)) ||
                        (m_GamePadState.IsButtonDown(Buttons.DPadRight) && !m_oldGamePadState.IsButtonDown(Buttons.DPadRight)) ||
                        (m_GamePadState.IsButtonDown(Buttons.LeftThumbstickRight) && !m_oldGamePadState.IsButtonDown(Buttons.LeftThumbstickRight));
            }
        }

        /// <summary>
        /// True if the A key was pressed
        /// </summary>
        public bool A
        {
            get
            {
                return (m_KeyboardState.IsKeyDown(Keys.Enter) && !m_oldKeyboardState.IsKeyDown(Keys.Enter)) ||
                       (m_GamePadState.IsButtonDown(Buttons.A) && !m_oldGamePadState.IsButtonDown(Buttons.A));
            }
        }

        /// <summary>
        /// True if the B key was pressed
        /// </summary>
        public bool B
        {
            get
            {
                return (m_KeyboardState.IsKeyDown(Keys.Back) && !m_oldKeyboardState.IsKeyDown(Keys.Back)) ||
                        (m_GamePadState.IsButtonDown(Buttons.B) && !m_oldGamePadState.IsButtonDown(Buttons.B));
            }
        }

        /// <summary>
        /// True if the X key was pressed
        /// </summary>
        public bool X
        {
            get
            {
                return (m_KeyboardState.IsKeyDown(Keys.Q) && !m_oldKeyboardState.IsKeyDown(Keys.Q)) ||
                        (m_GamePadState.IsButtonDown(Buttons.X) && !m_oldGamePadState.IsButtonDown(Buttons.X));
            }
        }

        /// <summary>
        /// True if the Y key was pressed
        /// </summary>
        public bool Y
        {
            get
            {
                return (m_KeyboardState.IsKeyDown(Keys.W) && !m_oldKeyboardState.IsKeyDown(Keys.W)) ||
                        (m_GamePadState.IsButtonDown(Buttons.Y) && !m_oldGamePadState.IsButtonDown(Buttons.Y));
            }
        }

        /// <summary>
        /// True if the D key was pressed on the keyboard
        /// </summary>
        public bool KeyboardD
        {
            get
            {
                return (m_KeyboardState.IsKeyDown(Keys.D) && !m_oldKeyboardState.IsKeyDown(Keys.D));
            }
        }

        /// <summary>
        /// True if X and ONLY X is pressed
        /// </summary>
        public bool ExclusiveX
        {
            get
            {
                return ((m_GamePadState.IsButtonDown(Buttons.X) && !m_oldGamePadState.IsButtonDown(Buttons.X))
                    && !(m_GamePadState.IsButtonDown(Buttons.Y)) && !(m_GamePadState.IsButtonDown(Buttons.A))
                        && !(m_GamePadState.IsButtonDown(Buttons.B)));
            }
        }

        /// <summary>
        /// True if Y and ONLY Y is pressed
        /// </summary>
        public bool ExclusiveY
        {
            get
            {
                return ((m_GamePadState.IsButtonDown(Buttons.Y) && !m_oldGamePadState.IsButtonDown(Buttons.Y))
                    && !(m_GamePadState.IsButtonDown(Buttons.X)) && !(m_GamePadState.IsButtonDown(Buttons.A))
                        && !(m_GamePadState.IsButtonDown(Buttons.B)));
            }
        }

        /// <summary>
        /// True if A and ONLY A is pressed
        /// </summary>
        public bool ExclusiveA
        {
            get
            {
                return ((m_GamePadState.IsButtonDown(Buttons.A) && !m_oldGamePadState.IsButtonDown(Buttons.A))
                    && !(m_GamePadState.IsButtonDown(Buttons.Y)) && !(m_GamePadState.IsButtonDown(Buttons.X))
                        && !(m_GamePadState.IsButtonDown(Buttons.B)));
            }
        }

        /// <summary>
        /// True if B and ONLY B is pressed
        /// </summary>
        public bool ExclusiveB
        {
            get
            {
                return ((m_GamePadState.IsButtonDown(Buttons.B) && !m_oldGamePadState.IsButtonDown(Buttons.B))
                    && !(m_GamePadState.IsButtonDown(Buttons.Y)) && !(m_GamePadState.IsButtonDown(Buttons.A))
                        && !(m_GamePadState.IsButtonDown(Buttons.X)));
            }
        }

        /// <summary>
        /// True if the LeftShoulder is pressed
        /// </summary>
        public bool LeftShoulder
        {
            get
            {
                return ((m_GamePadState.IsButtonDown(Buttons.LeftShoulder) && !m_oldGamePadState.IsButtonDown(Buttons.LeftShoulder)));
            }
        }

        /// <summary>
        /// True if the right shoulder is pressed
        /// </summary>
        public bool RightShoulder
        {
            get
            {
                return ((m_GamePadState.IsButtonDown(Buttons.RightShoulder) && !m_oldGamePadState.IsButtonDown(Buttons.RightShoulder)));
            }
        }

        /// <summary>
        /// True if the left trigger is being held down
        /// </summary>
        public bool LeftTriggerPressed
        {
            get
            {
                return (m_GamePadState.IsButtonDown(Buttons.LeftTrigger) || m_KeyboardState.IsKeyDown(Keys.Z));
            }
        }

        /// <summary>
        /// True if the right trigger is being held down
        /// </summary>
        public bool RightTriggerPressed
        {
            get
            {
                return (m_GamePadState.IsButtonDown(Buttons.RightTrigger) || m_KeyboardState.IsKeyDown(Keys.X));
            }
        }

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
                return ((m_GamePadState.IsButtonDown(Buttons.B) && !m_oldGamePadState.IsButtonDown(Buttons.B)) ||
                    (m_GamePadState.IsButtonDown(Buttons.A) && !m_oldGamePadState.IsButtonDown(Buttons.A)) ||
                    (m_GamePadState.IsButtonDown(Buttons.X) && !m_oldGamePadState.IsButtonDown(Buttons.X)) ||
                    (m_GamePadState.IsButtonDown(Buttons.Start) && !m_oldGamePadState.IsButtonDown(Buttons.Start)) ||
                    (m_GamePadState.IsButtonDown(Buttons.Y) && !m_oldGamePadState.IsButtonDown(Buttons.Y)));
            }
        }

        /// <summary>
        /// True if Start is pressed
        /// </summary>
        public bool Start
        {
            get
            {
                return (m_KeyboardState.IsKeyDown(Keys.Space) && !m_oldKeyboardState.IsKeyDown(Keys.Space)) ||
                        (m_GamePadState.IsButtonDown(Buttons.Start) && !m_oldGamePadState.IsButtonDown(Buttons.Start));
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
                        return -0.707105f;
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
                return (m_KeyboardState.IsKeyDown(Keys.Escape) && !m_oldKeyboardState.IsKeyDown(Keys.Escape)) ||
                        (m_GamePadState.IsButtonDown(Buttons.Back) && !m_oldGamePadState.IsButtonDown(Buttons.Back));
            }
        }

        /// <summary>
        /// True if the D key on the keyboard is pressed (used for entering debug mode)
        /// </summary>
        public bool D
        {
            get
            {
                return (m_KeyboardState.IsKeyDown(Keys.D) && !m_oldKeyboardState.IsKeyDown(Keys.D));
            }
        }

        public PlayerIndex PlayerIndex
        {
            get
            {
                return m_player;
            }
        }

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
            if (PlayerIndex.Two == m_player)
            {
                m_oldKeyboardState = m_KeyboardState;
                m_KeyboardState = Keyboard.GetState();
            }
            m_oldGamePadState = m_GamePadState;
            m_GamePadState = GamePad.GetState(m_player);
        }
    }

}