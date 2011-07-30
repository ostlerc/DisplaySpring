namespace DisplaySpring
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Class that handles creating / using textures that represent a ScrollBar
    /// </summary>
    internal class ScrollBar : Item
    {
        private Texture2D m_bg;
        private Button m_slider = null;

        private int m_objectCount;
        private int m_visibleCount; //This represents the amount of objects shown on the screen before scrolling is necessary
        private int m_selectedIndex = 0;
        private int m_length; //the total amount of rows / columns for the filled section
        private int m_unitLength; //a unit in row/column length

        private Color m_selectedColor = new Color(91, 87, 84, 255);
        private Color m_borderColor = new Color(55,200,0,175);
        private Color m_color = new Color(0,0,0,0);

        internal int VisibleCount
        {
            get { return m_visibleCount; }
            set { m_visibleCount = value; forceRefresh(); }
        }

        internal int ObjectCount
        {
            get { return m_objectCount; }
            set { m_objectCount = value; forceRefresh(); }
        }

        /// <summary>
        /// This is the primary way to scroll the scrollbar
        /// The SelectedIndex is 0 based
        /// </summary>
        public int SelectedIndex
        {
            get { return m_selectedIndex; }
            set { m_selectedIndex = value; }
        }

        public override float Height
        {
            get { return m_bg != null ? m_bg.Height : 0; }
        }

        public override float Width
        {
            get { return m_bg != null ? m_bg.Width : 0; }
        }

        /// <summary>
        /// A simple scrollbar that can be vertical or horizontal with a few custom options
        /// </summary>
        /// <param name="parent">parent of the scrollbar</param>
        /// <param name="width">width of the scrollbar</param>
        /// <param name="height">height of the scrollbar</param>
        /// <param name="objCount">amount of items in the container</param>
        /// <param name="visibleUnits">amount of items that can be seen before scrolling is necessary</param>
        public ScrollBar(Item parent, float width, float height, int objCount, int visibleUnits) : base(parent) //TODO fix this base call with a parent
        {
            m_objectCount = objCount;
            LayoutWidth = width;
            LayoutHeight = height;
            m_visibleCount = visibleUnits;

            forceRefresh();
        }

        bool lockRefresh = false;

        public override void  refreshItem()
        {
            if (m_objectCount < 1 || m_visibleCount < 1 || lockRefresh)
                return;

            lockRefresh = true;

            if (LayoutHeight > LayoutWidth)
                m_unitLength = (int)(LayoutHeight / m_objectCount);
            else
                m_unitLength = (int)(LayoutWidth / m_objectCount);

            m_length = m_unitLength * m_visibleCount;

            m_bg = CreateBackgroundTexture();

            if (m_slider != null)
                Children.Remove(m_slider);

            Texture2D bg = CreateSliderTexture();

            if (bg != null)
            {
                m_slider = new Button(this, bg);
                m_slider.Depth += .01f;
                m_slider.Fade = false;
            }

            base.refreshItem();

            lockRefresh = false;
        }

        internal override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix parentTransform)
        {
            if (m_slider == null || m_bg == null || !Visible)
                return;

            if (m_visibleCount >= m_objectCount)
                return;

            Matrix local = Item.CombineMatrix(AnimationTransform(gameTime), ref parentTransform);

            Vector2 position, scale;
            float rotation;
            DecomposeMatrix(ref local, out position, out rotation, out scale);

            spriteBatch.Draw(m_bg, position, null, Tint*Alpha, rotation, Center, scale, SpriteEffects.None, Depth);

            int offset = m_unitLength * (m_selectedIndex);
            int len = m_unitLength * m_visibleCount;

            if (LayoutWidth > LayoutHeight) //horizontal
            {
                m_slider.Position = Vector2.UnitX * ((len - m_bg.Width) / 2 + offset);
            }
            else
            {
                m_slider.Position = Vector2.UnitY * ((len - m_bg.Height) / 2 + offset);
            }

            m_slider.Draw(gameTime, spriteBatch, CombineMatrix(AnimationTransform(gameTime), ref parentTransform));
        }

        private Texture2D CreateBackgroundTexture()
        {
            if ( LayoutWidth == 0 || LayoutHeight == 0)
                return null;

            return CreateRectangleBorder((int)LayoutWidth, (int)LayoutHeight, 2, m_borderColor);
        }

        private Texture2D CreateSliderTexture()
        {
            if (LayoutWidth == 0 || LayoutHeight == 0)
                return null;

            Color[] colors;
            Texture2D sliderTexture;

            if (LayoutWidth > LayoutHeight) //horizontal
            {
                sliderTexture = new Texture2D(Menu.GraphicsDevice, m_length, (int)LayoutHeight);
                colors = new Color[(int)(m_length * LayoutHeight)];
            }
            else //vertical
            {
                sliderTexture = new Texture2D(Menu.GraphicsDevice, (int)LayoutWidth, m_length);
                colors = new Color[(int)(LayoutWidth * m_length)];
            }

            for (int i = 0; i < colors.Length; i++)
                colors[i] = m_selectedColor;

            sliderTexture.SetData(colors);
            return sliderTexture;
        }
    }
}
