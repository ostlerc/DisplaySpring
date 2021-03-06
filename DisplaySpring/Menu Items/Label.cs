﻿namespace DisplaySpring
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Class that draws a Item as a text object
    /// </summary>
    public class Label : Item
    {
        #region Member Variables

        internal String m_text;
        internal SpriteFont m_font;
        internal Vector2 m_textSize;
        internal Color m_color = Color.Gold;
        internal Color m_focusColor = Color.Black;

        #endregion

        #region Constructors

        /// <summary>
        /// Create a label with text
        /// </summary>
        public Label(Item parent, string text)
            : base(parent)
        {
            Initialize(text);
        }

        internal override ItemCollection Children
        {
            get { return base.Children; }
            set { throw new Exception("Cannot add children to a Label"); }
        }

        private void Initialize(string text)
        {
            Depth -= .001f; //by default let labels be closer to front
            Font = Menu.Font;
            if (Font == null)
                throw new Exception("You must call Menu.LoadContent() to use a Button");
            m_textSize = Font.MeasureString(text);
            m_text = text;
            Animation = AnimateType.Size;
        }

        #endregion

        #region Properties

        public override float Width
        {
            get { return m_textSize.X; }
        }

        public override float Height
        {
            get { return m_textSize.Y; }
        }

        /// <summary>
        /// Get's or sets the text for the object
        /// </summary>
        public virtual string Text 
        { 
            get { return m_text; } 
            set 
            { 
                m_text = value; 
                m_textSize =  Font.MeasureString(m_text);
            } 
        }

        /// <summary>
        /// Get's or sets the default font color for the object, when object is not focused
        /// </summary>
        public virtual Color FontColor { get { return m_color; } set { m_color = value; } }

        /// <summary>
        /// Get's or sets the focused font color of the object
        /// </summary>
        public virtual Color FontFocusColor { get { return m_focusColor; } set { m_focusColor = value; } }

        /// <summary>
        /// Get's or sets the font value for the object
        /// </summary>
        public virtual SpriteFont Font { get { return m_font; } set { m_font = value; } }

        #endregion

        #region Member Functions

        internal override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix parentTransform)
        {
            if (!Visible)
                return;

            //create transform matrix and multiply them backwards order, then decompose
            //to get out info we want
            Matrix local = Item.CombineMatrix(AnimationTransform(gameTime), ref parentTransform);

            Vector2 position, scale;
            float rotation;
            DecomposeMatrix(ref local, out position, out rotation, out scale);

            Color textColor = (Focus ? FontFocusColor : FontColor) * ScreenAlpha;

            if (!String.IsNullOrEmpty(Text))//label
                spriteBatch.DrawString(Font, Text, position, textColor, rotation, Center, scale, SpriteEffects.None, Depth);

            foreach (Item child in Children)
                child.Draw(gameTime, spriteBatch, local);
        }

        #endregion
    }
}
