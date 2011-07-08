using System;
using System.Collections.Generic;
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

    public enum ButtonState
    {
        Pressed,
        Released,
        Held
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
        float m_heldInterval = 100;
        Dictionary<Buttons, float> m_heldButtons = new Dictionary<Buttons,float>();
        Dictionary<Keys, float> m_heldKeys = new Dictionary<Keys,float>();

        /// <summary>
        /// Gets the game pad state of the controller
        /// </summary>
        public GamePadState GamePadState
        {
            get { return m_GamePadState; }
        }

        /// <summary>
        /// The Held Interval is the interval of how often the Held will return true for
        /// how long a button has been held down
        /// </summary>
        public float HeldInterval
        {
            get { return m_heldInterval; }
            set { m_heldInterval = value; }
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

                return new Vector2(Left(ButtonState.Held) ? -1 : Right(ButtonState.Held) ? 1 : 0, Up(ButtonState.Held) ? -1 : Down(ButtonState.Held) ? 1 : 0);
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

                return new Vector2(Left(ButtonState.Held) ? -1 : Right(ButtonState.Held) ? 1 : 0, Up(ButtonState.Held) ? -1 : Down(ButtonState.Held) ? 1 : 0);
            }
        }

        internal bool State(Buttons b, ButtonState state)
        {
            if (state == ButtonState.Pressed)
                return Pressed(b);
            else if (state == ButtonState.Released)
                return Released(b);
            else
                return Held(b);
        }

        internal bool State(List<Buttons> bts, ButtonState state)
        {
            if (state == ButtonState.Pressed)
                return Pressed(bts);
            else if (state == ButtonState.Released)
                return Released(bts);
            else
                return Held(bts);
        }

        internal bool State(Keys b, ButtonState state)
        {
            if (state == ButtonState.Pressed)
                return Pressed(b);
            else if (state == ButtonState.Released)
                return Released(b);
            else
                return Held(b);
        }

        internal bool State(List<Keys> keys, ButtonState state)
        {
            if (state == ButtonState.Pressed)
                return Pressed(keys);
            else if (state == ButtonState.Released)
                return Released(keys);
            else
                return Held(keys);
        }

        #region Pressed
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
        #endregion
        #region Released
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
        #endregion

        /// <summary>
        /// Returns true if keyboard key k has been held. Only returns true once every HeldInterval
        /// </summary>
        public bool Held(Keys k)
        {
            if (!m_heldKeys.ContainsKey(k))
            {
                if (m_KeyboardState.IsKeyDown(k))
                {
                    m_heldKeys[k] = 0;
                    return true;
                }

                return false;
            }

            return m_heldKeys[k] == 0;
        }

        /// <summary>
        /// Returns true if Button btn has been Held
        /// </summary>
        public bool Held(Buttons btn)
        {
            if(!m_heldButtons.ContainsKey(btn))
            {
                if (m_GamePadState.IsButtonDown(btn))
                {
                    m_heldButtons[btn] = 0;
                    return true;
                }

                return false;
            }

            return m_heldButtons[btn] == 0;
        }

        /// <summary>
        /// Returns true if one of the Buttons btns has been Held
        /// </summary>
        internal bool Held(List<Buttons> btns)
        {
            bool ret = false;
            foreach (var b in btns)
                if (Held(b))
                    ret =  true;

            return ret;
        }

        internal bool Held(List<Keys> keys)
        {
            bool ret = false;
            foreach (var k in keys)
                if (Held(k))
                    ret =  true;

            return ret;
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
            return State(keyDown, state) || State(Buttons.DPadDown, state) || State(Buttons.LeftThumbstickDown, state);
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

        private void Update(GameTime gameTime)
        {
#if DEBUG   //keyboard states only in debug mode
            m_oldKeyboardState = m_KeyboardState;
            m_KeyboardState = Keyboard.GetState();
            List<Keys> toRemoveKeys = new List<Keys>();
            List<Keys> toUpdateKeys = new List<Keys>();
            foreach (var v in m_heldKeys.Keys)
            {
                if (m_heldKeys[v] + gameTime.ElapsedGameTime.Milliseconds >= m_heldInterval)
                    toRemoveKeys.Add(v);
                else if(!m_KeyboardState.IsKeyDown(v))
                    toRemoveKeys.Add(v);
                else
                    toUpdateKeys.Add(v);
            }

            foreach (var v in toRemoveKeys)
                m_heldKeys.Remove(v);

            foreach (var v in toUpdateKeys)
                m_heldKeys[v] += gameTime.ElapsedGameTime.Milliseconds;
#endif
            List<Buttons> toRemoveBtns = new List<Buttons>();
            List<Buttons> toUpdateBtns = new List<Buttons>();
            foreach (var v in m_heldButtons.Keys)
            {
                if (m_heldButtons[v] + gameTime.ElapsedGameTime.Milliseconds >= m_heldInterval)
                    toRemoveBtns.Add(v);
                else if(!m_GamePadState.IsButtonDown(v))
                    toRemoveBtns.Add(v);
                else
                    toUpdateBtns.Add(v);
            }

            foreach (var v in toRemoveBtns)
                m_heldButtons.Remove(v);

            foreach (var v in toUpdateBtns)
                m_heldButtons[v] += gameTime.ElapsedGameTime.Milliseconds;

            m_oldGamePadState = m_GamePadState;
            m_GamePadState = GamePad.GetState(m_player);
        }
    }

}