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
    public class Sprite : Item
    {
        /// <summary>
        /// Texture for the sprite
        /// </summary>
        internal Texture2D m_texture;

        internal override float StaticWidth
        {
            get { return m_texture.Width; }
        }

        internal override float StaticHeight
        {
            get { return m_texture.Height; }
        }

        internal override ItemCollection Children
        {
            get { return base.Children; }
            set { throw new Exception("Cannot add children to a Sprite"); }
        }

        /// <summary>
        /// The main background of the button.
        /// This texture is shown when the item does not have focus
        /// </summary>
        public virtual Texture2D Texture
        {
            get { return m_texture; }
            set { Initialize(value); }
        }

        /// <summary>
        /// Create a Sprite with a texture
        /// StaticSize of a Sprite is the texture size
        /// </summary>
        public Sprite(Item parent, Texture2D texture)
            :base (parent)
        {
            Initialize(texture);
        }

        private void Initialize(Texture2D texture)
        {
            m_texture = texture;

            if (texture == null)
#if DEBUG
                throw new Exception("Cannot create a sprite with a null texture");
#else
                m_texture = Item.CreateFilledRectangle(1, 1, Color.White);
#endif
        }

        internal override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix parentTransform)
        {
            if (!Visible)
                return;

            //create transform matrix and multiply them backwards order, then decompose
            //to get out info we want
            Matrix local = CombineMatrix(AnimationTransform(gameTime), ref parentTransform);

            Vector2 position, scale;
            float rotation;
            DecomposeMatrix(ref local, out position, out rotation, out scale);

            Vector2 center = new Vector2(m_texture.Width, m_texture.Height) / 2f;

            spriteBatch.Draw(m_texture, position, null, Tint * ScreenAlpha, rotation, center, scale, SpriteEffects.None, Depth);
        }
    }
}
