namespace DisplaySpring
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Standard menu animation types
    /// </summary>
    public enum AnimateType
    {
        /// <summary>
        /// No Animation Type
        /// </summary>
        None,

        /// <summary>
        /// The object will animate on its size. Growing and Shrinking over a period of time
        /// </summary>
        Size
    }

    /// <summary>
    /// A static image that can change its size
    /// </summary>
    public class Sprite
    {
        internal float m_width;
        internal float m_height;

        /// <summary>
        /// Width of the Sprite
        /// </summary>
        public float Width { get { return m_texture.Width; } }

        /// <summary>
        /// Height of the Sprite
        /// </summary>
        public float Height { get { return m_texture.Height; } }

        /// <summary>
        /// Returns the Vector2(Width,Height). Can be set.
        /// </summary>
        public Vector2 Size
        {
            get { return new Vector2(m_width, m_height); }
            set { m_width = value.X; m_height = value.Y; }
        }

        /// <summary>
        /// Texture for the sprite
        /// </summary>
        internal Texture2D m_texture;

        /// <summary>
        /// pos of this Sprite. This value should be used not as an absolute pos
        /// but as an offset value from its container class
        /// </summary>
        internal Vector2 pos;

        /// <summary>
        /// Create a Sprite with a texture and position.
        /// Size will initially be set to the texture size.
        /// </summary>
        public Sprite(Texture2D texture, Vector2 Pos)
        {
            m_texture = texture;
            pos = Pos;
            m_width = m_texture.Width;
            m_height = m_texture.Height;
        }

        internal Rectangle destRect()
        {
            return new Rectangle((int)pos.X, (int)pos.Y, (int)m_width, (int)m_height);
        }
    }
}
