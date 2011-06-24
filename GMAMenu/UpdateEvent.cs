using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GMA.Menus
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

        public UpdateEvent()
        {
            m_updates = new List<UpdateDelegate>();
        }

        public void AddEvent(UpdateDelegate d)
        {
            m_updates.Add(d);
        }

        public void Update()
        {
            foreach (UpdateDelegate d in m_updates)
            {
                d.Invoke();
            }
        }
    }
}