﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace DisplaySpring.Menus
{
    public class ScrollBar : MenuItem
    {
        private Texture2D m_bg;
        private Texture2D m_slider;

        private int m_objectCount;
        private int m_visibleCount; //This represents the amount of objects shown on the screen before scrolling is necessary
        private int m_selectedIndex = 0;
        private int m_length; //the total amount of rows / columns for the filled section
        private int m_unitLength; //a unit in row/column length

        private Color m_selectedColor = new Color(91, 87, 84, 255);
        private Color m_borderColor = new Color(55,200,0,175);
        private Color m_color = new Color(0,0,0,0);

        public int VisibleCount
        {
            get { return m_visibleCount; }
            set { m_visibleCount = value; Init(); }
        }

        public int ObjectCount
        {
            get { return m_objectCount; }
            set { m_objectCount = value; Init(); }
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

        internal override float Width
        {
            set
            {
                base.Width = value;
                Init();
            }
        }

        internal override float Height
        {
            set
            {
                base.Height = value;
                Init();
            }
        }

        internal override float StaticHeight
        {
            get { return Height; }
        }

        internal override float StaticWidth
        {
            get { return Width; }
        }

        /// <summary>
        /// A simple scrollbar that can be vertical or horizontal with a few custom options
        /// </summary>
        /// <param name="width">width of the scrollbar</param>
        /// <param name="height">height of the scrollbar</param>
        /// <param name="objCount">amount of items in the container</param>
        /// <param name="visibleUnits">amount of items that can be seen before scrolling is necessary</param>
        public ScrollBar(int width, int height, int objCount, int visibleUnits) : base(null) //TODO fix this base call with a parent
        {
            m_objectCount = objCount;
            Width = width;
            Height = height;
            m_visibleCount = visibleUnits;

            Init();
        }

        private void Init()
        {
            if (m_objectCount < 1 || m_visibleCount < 1)
                return;

            if (Height > Width)
                m_unitLength = (int)(Height / m_objectCount);
            else
                m_unitLength = (int)(Width / m_objectCount);

            m_length = m_unitLength * m_visibleCount;

            m_bg = CreateBackgroundTexture();
            m_slider = CreateSliderTexture();
        }

        internal override void  Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix parentTransform)
        {
            if (m_slider == null || m_bg == null || !m_visible)
                return;

            if (m_visibleCount >= m_objectCount)
                return;

            Matrix local = MenuItem.CombineMatrix(ItemTransform, ref parentTransform);

            Vector2 position, scale;
            float rotation;
            DecomposeMatrix(ref local, out position, out rotation, out scale);

            Vector2 center = new Vector2(Width, Height) / 2f;

            int len = m_unitLength * m_selectedIndex;

            spriteBatch.Draw(m_bg, position, null, Color.White*Alpha, rotation, center, scale, SpriteEffects.None, 0);

            if (Width > Height) //horizontal
                position += Vector2.UnitX * len;
            else
                position += Vector2.UnitY * len;

            spriteBatch.Draw(m_slider, position, null, Color.White*Alpha, rotation, center, scale, SpriteEffects.None, Depth + .0001f);
        }

        private void SetBorderColor(Color[] colors)
        {
            //top
            for (int x = 0; x < (int)Width; x++)
                colors[x] = m_borderColor;

            //left and right edges
            for (int x = (int)Width; x < Width * Height; x+=(int)Width)
            {
                colors[x] = m_borderColor;
                colors[x + (int)Width - 1] = m_borderColor;
            }

            //bottom
            for (int x = (int)Width * ((int)Height - 1); x < Width * Height; x++)
                colors[x] = m_borderColor;
        }

        private Texture2D CreateBackgroundTexture()
        {
            if ( Width == 0 || Height == 0)
                return null;

            return CreateRectangleBorder((int)Width, (int)Height, 2, m_borderColor);
        }

        private Texture2D CreateSliderTexture()
        {
            if (Width == 0 || Height == 0)
                return null;

            Color[] colors;
            Texture2D sliderTexture;

            if (Width > Height) //horizontal
            {
                sliderTexture = new Texture2D(Menu.GraphicsDevice, m_length, (int)Height);
                colors = new Color[(int)(m_length * Height)];
            }
            else //vertical
            {
                sliderTexture = new Texture2D(Menu.GraphicsDevice, (int)Width, m_length);
                colors = new Color[(int)(Width * m_length)];
            }

            for (int i = 0; i < colors.Length; i++)
                colors[i] = m_selectedColor;

            sliderTexture.SetData(colors);
            return sliderTexture;
        }

        public static Texture2D CreateRectangleBorder(int width, int height, int thickness, Color col)
        {  
            if ( width == 0 || height == 0 || thickness < 0 || col == null)
                return null;

            Color[] colors;
            Texture2D border = new Texture2D(Menu.GraphicsDevice, width, height);
            colors = new Color[width * height];

            for (int i = 0; i < colors.Length; i++)
                colors[i] = new Color(0, 0, 0, 0); //completely invisible

            //top
            for (int i = 0; i < width * thickness; i++)
                colors[i] = col;

            //sides
            for (int i = 0; i < colors.Length; i += width)
            {
                for (int j = 0; j < thickness && j < width; j++) //left side
                    colors[i + j] = col;
                for (int j = width - 1; j >= width - thickness && j > 0; j--) //right side
                    colors[i + j] = col;
            }

            //bottom
            for (int i = width * (height - thickness); i < colors.Length; i++)
                colors[i] = col;

            border.SetData(colors);
            return border;
        }

        public static Texture2D CreateFilledRectangle(int width, int height, Color col)
        {
            if ( width == 0 || height == 0 || col == null)
                return null;

            Color[] colors;
            Texture2D rect = new Texture2D(Menu.GraphicsDevice, width, height);
            colors = new Color[width * height];

            for (int i = 0; i < colors.Length; i++)
                colors[i] = col;

            rect.SetData(colors);

            return rect;
        }
    }
}