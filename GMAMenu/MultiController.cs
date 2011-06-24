using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GMA.Menus
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

        public List<Controller> ToList()
        {
            return m_controllers;
        }

        #region input helpers
        public bool Up 
        { 
            get
            {
                foreach (Controller c in m_controllers)
                    if (c.Up)
                        return true;

                return false;
            }
        }

        public bool Down 
        { 
            get
            {
                foreach (Controller c in m_controllers)
                    if (c.Down)
                        return true;

                return false;
            }
        }

        public bool Right 
        {
            get
            {
                foreach (Controller c in m_controllers)
                    if (c.Right)
                        return true;

                return false;
            }
        }

        public bool Left 
        {
            get
            {
                foreach (Controller c in m_controllers)
                    if (c.Left)
                        return true;

                return false;
            }
        }

        public bool A 
        {
            get
            {
                foreach (Controller c in m_controllers)
                    if (c.A)
                        return true;

                return false;
            }
        }

        public bool Start 
        {
            get
            {
                foreach (Controller c in m_controllers)
                    if (c.Start)
                        return true;

                return false;
            }
        }

        public bool Back 
        {
            get
            {
                foreach (Controller c in m_controllers)
                    if (c.Back)
                        return true;

                return false;
            }
        }

        public bool B 
        {
            get
            {
                foreach (Controller c in m_controllers)
                    if (c.B)
                        return true;

                return false;
            }
        }

        public bool Y 
        {
            get
            {
                foreach (Controller c in m_controllers)
                    if (c.Y)
                        return true;

                return false;
            }
        }

        public bool X
        {
            get
            {
                foreach (Controller c in m_controllers)
                    if (c.X)
                        return true;

                return false;
            }
        }
        #endregion

    }
}
