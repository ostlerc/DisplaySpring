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

        /// <summary>
        /// Returns the multicontroller as a list of Controllers
        /// </summary>
        public List<Controller> ToList()
        {
            return m_controllers;
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
