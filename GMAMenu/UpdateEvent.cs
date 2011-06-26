using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DisplaySpring.Menus
{
    /// <summary>
    /// List of delegates to invoke on an update loop
    /// </summary>
    public class UpdateEvent
    {
        /// <summary>
        /// Delegates for running in an update event
        /// </summary>
        public delegate void UpdateDelegate();

        private List<UpdateDelegate> m_updates;

        /// <summary>
        /// Default Constructor
        /// </summary>
        public UpdateEvent()
        {
            m_updates = new List<UpdateDelegate>();
        }

        /// <summary>
        /// Add an event to delegate
        /// </summary>
        public void AddEvent(UpdateDelegate d)
        {
            m_updates.Add(d);
        }

        /// <summary>
        /// Invoke event for all delegates registered with the event
        /// </summary>
        public void Update()
        {
            foreach (UpdateDelegate d in m_updates)
            {
                d.Invoke();
            }
        }
    }
}