using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace DisplaySpring
{
    /// <summary>
    /// A MultiController handles finding input from multiple controllers.
    /// </summary>
    public class MultiController
    {
        protected List<Controller> m_controllers;

        public MultiController(List<Controller> controllers)
        {
            m_controllers = controllers;
        }

        public MultiController(Controller controller)
        {
            m_controllers = new List<Controller>();
            m_controllers.Add(controller);
        }

        /// <summary>
        /// code to convert from Controller to MultiController
        /// </summary>
        public static implicit operator MultiController(Controller c)
        {
            return new MultiController(c);
        }

        public bool Continuous(Buttons b)
        {
            foreach (var c in m_controllers)
                if (c.Continuous(b))
                    return true;

            return false;
        }

        public bool Continuous(Keys k)
        {
            foreach (var c in m_controllers)
                if (c.Continuous(k))
                    return true;

            return false;
        }

        public bool Held(Buttons b)
        {
            foreach (var c in m_controllers)
                if (c.Held(b))
                    return true;

            return false;
        }

        public bool Held(Keys k)
        {
            foreach (var c in m_controllers)
                if (c.Held(k))
                    return true;

            return false;
        }

        /// <summary>
        /// Returns true if button b has been pressed
        /// </summary>
        public bool Pressed(Buttons b)
        {
            foreach (var c in m_controllers)
                if (c.Pressed(b))
                    return true;

            return false;
        }

        /// <summary>
        /// returns true if keybard key k has been pressed
        /// </summary>
        public bool Pressed(Keys k)
        {
            foreach (var c in m_controllers)
                if (c.Pressed(k))
                    return true;

            return false;
        }

        /// <summary>
        /// Returns the multicontroller as a list of Controllers
        /// </summary>
        public List<Controller> ToList()
        {
            return m_controllers;
        }

        public bool State(ButtonSet button)
        {
            foreach (var c in m_controllers)
                if (c.State(button))
                    return true;

            return false;
        }

        /// <summary>
        /// Get a specific state out of the multi controller.
        /// If any controller has the required state true is returned
        /// </summary>
        public bool State(ButtonSet button, ButtonState state)
        {
            foreach (var c in m_controllers)
                if (c.State(button, state))
                    return true;

            return false;
        }
    }
}
