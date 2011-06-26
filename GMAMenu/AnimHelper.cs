using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GMA.Menus
{
    internal class AnimHelper
    {
        protected float m_startVal;
        protected float m_endVal;
        protected int m_timeoutMS = Menu.DefaultFadeTime;
        protected float m_totalElapsedTime;
        protected float m_val;
        public float Val
        {
            get { return m_val; }
            set { m_val = value; }
        }

        public float StartVal
        {
            get { return m_startVal; }
            set { m_startVal = value; Reset(); }
        }

        public float EndVal
        {
            get { return m_endVal; }
            set { m_endVal = value; }
        }

        /// <summary>
        /// Time in MS animation will last
        /// </summary>
        public int TimeoutMS
        {
            get { return m_timeoutMS; }
            set { m_timeoutMS = value; }
        }

        public AnimHelper(float startVal, float endVal, int timeoutMS)
        {
            m_startVal = startVal;
            m_endVal = endVal;
            m_timeoutMS = timeoutMS;
            m_totalElapsedTime = 0;
            m_val = startVal;
        }

        public void Update(GameTime gameTIme)
        {
            m_totalElapsedTime += gameTIme.ElapsedGameTime.Milliseconds;

            if (m_totalElapsedTime > m_timeoutMS || m_val == m_endVal)
            {
                m_val = m_endVal;
                return;
            }

            float dx = m_endVal - m_startVal;
            float ratio = m_totalElapsedTime / m_timeoutMS;
            m_val = dx * ratio;
        }

        public void Reset()
        {
            m_totalElapsedTime = 0;
            m_val = m_startVal;
        }
    }
}
