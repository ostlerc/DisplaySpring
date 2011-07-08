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
    public enum ButtonSet
    {
        Up,
        Down,
        Left,
        Right,
        A,
        B,
        X,
        Y,
        Start,
        Back,
        LeftShoulder,
        RightShoulder,
        LeftTrigger,
        RightTrigger,
        ExclusiveA,
        ExclusiveB,
        ExclusiveX,
        ExclusiveY,
        AnyButton,
        AnyInput
    }

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

        public GamePadState GamePadState
        {
            get { return m_GamePadState; }
        }

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
        /// Get the Vector2 representing the movement of the left thumb stick
        /// </summary>
        public Vector2 LeftThumbStick
        {
            get
            {
                if (m_GamePadState.ThumbSticks.Left != Vector2.Zero)
                    return m_GamePadState.ThumbSticks.Left;

                return new Vector2(Left(ButtonState.Pressed) || Right(ButtonState.Pressed) ? 1 : 0, Up(ButtonState.Pressed) || Down(ButtonState.Pressed) ? 1 : 0);
            }
        }

        /// <summary>
        /// Get the Vector2 representing the movement of the right thumb stick
        /// </summary>
        public Vector2 RightThumbStick
        {
            get
            {
                if (m_GamePadState.ThumbSticks.Right != Vector2.Zero)
                    return m_GamePadState.ThumbSticks.Right;

                return new Vector2(Left(ButtonState.Pressed) || Right(ButtonState.Pressed) ? 1 : 0, Up(ButtonState.Pressed) || Down(ButtonState.Pressed) ? 1 : 0);
            }
        }

        /// <summary>
        /// Returns true if button b has been pressed
        /// </summary>
        public bool Pressed(Buttons b)
        {
            return m_GamePadState.IsButtonDown(b) && !m_oldGamePadState.IsButtonDown(b);
        }

        internal bool State(Buttons b, ButtonState state)
        {
            if (state == ButtonState.Pressed)
                return Pressed(b);
            else
                return Released(b);
        }

        internal bool State(List<Buttons> bts, ButtonState state)
        {
            if (state == ButtonState.Pressed)
                return Pressed(bts);
            else
                return Released(bts);
        }

        internal bool State(Keys b, ButtonState state)
        {
            if (state == ButtonState.Pressed)
                return Pressed(b);
            else
                return Released(b);
        }

        internal bool State(List<Keys> keys, ButtonState state)
        {
            if (state == ButtonState.Pressed)
                return Pressed(keys);
            else
                return Released(keys);
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
        internal bool Pressed(List<Buttons> bts)
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
        /// Returns true if keyboard key k has been released
        /// </summary>
        public bool Released(Keys k)
        {
            return !m_KeyboardState.IsKeyDown(k) && m_oldKeyboardState.IsKeyDown(k);
        }

        /// <summary>
        /// Returns true if Button btn has been released
        /// </summary>
        public bool Released(Buttons btn)
        {
            return !m_GamePadState.IsButtonDown(btn) && m_oldGamePadState.IsButtonDown(btn);
        }

        /// <summary>
        /// Returns true if one of the Buttons btns has been released
        /// </summary>
        internal bool Released(List<Buttons> btns)
        {
            foreach (var b in btns)
                if (Released(b))
                    return true;

            return false;
        }

        internal bool Released(List<Keys> keys)
        {
            foreach (var k in keys)
                if (Released(k))
                    return true;

            return false;
        }

        public bool State(ButtonSet set, ButtonState state)
        {
            switch (set)
            {
                case ButtonSet.Up:
                    return Up(state);
                case ButtonSet.Down:
                    return Down(state);
                case ButtonSet.Left:
                    return Left(state);
                case ButtonSet.Right:
                    return Right(state);
                case ButtonSet.A:
                    return A(state);
                case ButtonSet.B:
                    return B(state);
                case ButtonSet.X:
                    return X(state);
                case ButtonSet.Y:
                    return Y(state);
                case ButtonSet.ExclusiveA:
                    return ExclusiveA(state);
                case ButtonSet.ExclusiveB:
                    return ExclusiveB(state);
                case ButtonSet.ExclusiveX:
                    return ExclusiveX(state);
                case ButtonSet.ExclusiveY:
                    return ExclusiveY(state);
                case ButtonSet.LeftShoulder:
                    return LeftShoulder(state);
                case ButtonSet.RightShoulder:
                    return RightShoulder(state);
                case ButtonSet.Start:
                    return Start(state);
                case ButtonSet.Back:
                    return Back(state);
                case ButtonSet.LeftTrigger:
                    return LeftTrigger(state);
                case ButtonSet.RightTrigger:
                    return RightTrigger(state);
                case ButtonSet.AnyButton:
                    return AnyButton(state);
                case ButtonSet.AnyInput:
                    return AnyInput(state);
            }
            return false;
        }

        /// <summary>
        /// True if the up key was pressed
        /// </summary>
        internal bool Up(ButtonState state)
        {
            return State(keyUp, state) || State(Buttons.DPadUp, state) || State(Buttons.LeftThumbstickUp, state);
        }

        /// <summary>
        /// True if the down key was pressed
        /// </summary>
        internal bool Down(ButtonState state)
        {
            return Pressed(keyDown) || Pressed(Buttons.DPadDown) || Pressed(Buttons.LeftThumbstickDown);
        }

        /// <summary>
        /// True if the left key was pressed
        /// </summary>
        internal bool Left(ButtonState state)
        {
            return State(keyLeft, state) || State(Buttons.DPadLeft, state) || State(Buttons.LeftThumbstickLeft, state);
        }

        /// <summary>
        /// True if the right key was pressed
        /// </summary>
        internal bool Right(ButtonState state)
        {
            return State(keyRight, state) || State(Buttons.DPadRight, state) || State(Buttons.LeftThumbstickRight, state);
        }

        /// <summary>
        /// True if the A key was pressed
        /// </summary>
        internal bool A(ButtonState state)
        {
            return State(keyA, state) || State(Buttons.A, state);
        }

        /// <summary>
        /// True if the B key was pressed
        /// </summary>
        internal bool B(ButtonState state)
        {
            return State(keyB, state) || State(Buttons.B, state);
        }

        /// <summary>
        /// True if the X key was pressed
        /// </summary>
        internal bool X(ButtonState state)
        {
            return State(keyX, state) || State(Buttons.X, state);
        }

        /// <summary>
        /// True if the Y key was pressed
        /// </summary>
        internal bool Y(ButtonState state)
        {
            return State(keyY, state) || State(Buttons.Y, state);
        }

        /// <summary>
        /// True if X and ONLY X is pressed
        /// </summary>
        internal bool ExclusiveX(ButtonState state)
        {
            return X(state) && !Y(state) && !A(state) && !B(state);
        }

        /// <summary>
        /// True if Y and ONLY Y is pressed
        /// </summary>
        internal bool ExclusiveY(ButtonState state)
        {
            return !X(state) && Y(state) && !A(state) && !B(state);
        }

        /// <summary>
        /// True if A and ONLY A is pressed
        /// </summary>
        internal bool ExclusiveA(ButtonState state)
        {
            return !X(state) && !Y(state) && A(state) && !B(state);
        }

        /// <summary>
        /// True if B and ONLY B is pressed
        /// </summary>
        internal bool ExclusiveB(ButtonState state)
        {
            return !X(state) && !Y(state) && !A(state) && B(state);
        }

        /// <summary>
        /// True if the LeftShoulder is pressed
        /// </summary>
        internal bool LeftShoulder(ButtonState state)
        {
            return State(leftShoulder, state) || State(Buttons.LeftShoulder, state);
        }

        /// <summary>
        /// True if the right shoulder is pressed
        /// </summary>
        internal bool RightShoulder(ButtonState state)
        {
            return State(rightShoulder, state) || State(Buttons.RightShoulder, state);
        }

        /// <summary>
        /// True if the left trigger is being held down
        /// </summary>
        internal bool LeftTrigger(ButtonState state)
        {
            return State(leftTrigger, state) || State(Buttons.LeftTrigger, state);
        }

        /// <summary>
        /// True if the right trigger is being held down
        /// </summary>
        internal bool RightTrigger(ButtonState state)
        {
            return State(rightTrigger, state) || State(Buttons.RightTrigger, state);
        }

        /// <summary>
        /// True if any mapped keyboard or gamepad button is pressed
        /// </summary>
        internal bool AnyInput(ButtonState state)
        {
            return AnyButton(state) || A(state) || B(state) || X(state) || Start(state) || Y(state) || Up(state) || Down(state) || Left(state) || Right(state) || LeftShoulder(state) || RightShoulder(state);
        }

        /// <summary>
        /// True if any button is pressed
        /// </summary>
        internal bool AnyButton(ButtonState state)
        {
            return A(state) || B(state) || X(state) || Y(state) || Start(state);
        }

        /// <summary>
        /// True if Start is pressed
        /// </summary>
        internal bool Start(ButtonState state)
        {
            return State(keyStart, state) || State(Buttons.Start, state);
        }

        /// <summary>
        /// True if the Back button is pushed
        /// </summary>
        internal bool Back(ButtonState state)
        {
            return State(keyBack, state) || State(Buttons.Back, state);
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