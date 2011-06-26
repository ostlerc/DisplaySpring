using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace GMA.Menus
{

    /// <summary>
    /// MenuButton is basically a text button with callbacks
    /// You can give the button no background and no foreground with just text
    /// </summary>
    public class MenuButton : MenuItem
    {
        #region Member Variables
        protected Texture2D m_bg;
        protected Texture2D m_focused;
        protected float m_textureHeight;
        protected float m_textureWidth;

        /// <summary>
        /// The focus background of the button.
        /// This texture is shown when the item has focus
        /// </summary>
        public Texture2D FocusTexture
        {
            get { return m_focused; }
            set { m_focused = value; }
        }

        /// <summary>
        /// The main background of the button.
        /// This texture is shown when the item does not have focus
        /// </summary>
        public Texture2D Background
        {
            get { return m_bg; }
            set { m_bg = value; }
        }

        protected String m_label;
        internal SpriteFont m_font;
        protected Color m_color = Color.Gold;
        protected Color m_focusColor = Color.Black;
        private Vector2 fontSize;
        private float m_textScale = 1.0f;
        #endregion

        #region Properties

        /// <summary>
        /// Gets the scaled height of the object
        /// Layout space is not included
        /// </summary>
        public override float MeasureHeight 
        { 
            get 
            {
                if (m_bg == null)
                    return StaticHeight * TextScale;
                else
                    return StaticHeight * Scale.Y;
            } 
        }

        /// <summary>
        /// Gets the scaled StaticWidth of the object
        /// Layout space is not included
        /// </summary>
        public override float  MeasureWidth
        { 
            get 
            {
                if (m_bg == null)
                    return StaticWidth * TextScale;
                else
                    return StaticWidth * Scale.X;
            } 
        }

        internal override float StaticWidth
        {
            get { return m_textureWidth; }
        }

        internal override float StaticHeight
        {
            get { return m_textureHeight; }
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
        /// Gets or sets the scale value for the Text
        /// </summary>
        public virtual float TextScale { get { return m_textScale; } set { m_textScale = value; } }

        /// <summary>
        /// Get's or sets the text for the object
        /// </summary>
        public virtual string Text 
        { 
            get { return m_label; } 
            set 
            { 
                m_label = value; 
                fontSize =  Font.MeasureString(m_label);
                if (m_bg == null)
                {
                    m_textureWidth = fontSize.X * TextScale;
                    m_textureHeight = fontSize.Y * TextScale;
                }
            } 
        }
        /// <summary>
        /// Get's or sets the font value for the object
        /// </summary>
        public virtual SpriteFont Font 
        { 
            get 
            {
                return m_font;
            }
            set { m_font = value; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Internal debugging constructor
        /// </summary>
        internal MenuButton(Texture2D debugTexture, MenuItem parent) : base()
        {
            LayoutStretch = 0;
            Depth = 0;
            Parent = parent;
            Initialize(debugTexture, null);
        }

        /// <summary>
        /// Create a menu item with specific background and focus background with text
        /// </summary>
        public MenuButton(MenuItem parent, MultiController c, Texture2D background, Texture2D focused, string label)
            : base(parent, c)
        {
            Initialize(label);
            Initialize(background, focused);
        }

        /// <summary>
        /// Create a menu item with specific background and highlight textures
        /// </summary>
        public MenuButton(MenuItem parent, MultiController c, Texture2D background, Texture2D focused) 
            : base(parent, c)
        {
            Initialize();
            Initialize(background, focused);
        }

        /// <summary>
        /// Create a menu item with specific background
        /// </summary>
        public MenuButton(MenuItem parent, MultiController c, Texture2D background) 
            : base(parent, c)
        {
            Initialize();
            Initialize(background, null);
        }

        /// <summary>
        /// Creates a text label with user input
        /// </summary>
        public MenuButton(MenuItem parent, MultiController c, string text)
            : base (parent, c)
        {
            Initialize(text);
        }

        /// <summary>
        /// Creates a text label. Cannot receive focus from user
        /// </summary>
        /// <param name="text"></param>
        public MenuButton(MenuItem parent, string text) 
            : base(parent)
        {
            Initialize(text);
        }

        /// <summary>
        /// Creates a text label at position Pos. Cannot receive focus from user
        /// </summary>
        public MenuButton(MenuItem parent, string text, Vector2 Pos) 
            : base(parent)
        {
            Initialize(text);
            Position = Pos;
        }

        /// <summary>
        /// Creates a scaled text label at position Pos. Cannot receive focus from user
        /// </summary>
        public MenuButton(MenuItem parent, string text, Vector2 Pos, float scale) 
            : base(parent)
        {
            Initialize(text);
            TextScale = scale;
            Position = Pos;
        }

        /// <summary>
        /// Creates a scaled text label. Cannot receive focus from user
        /// </summary>
        public MenuButton(MenuItem parent, string text, float scale) 
            : base(parent)
        {
            Initialize(text);
            TextScale = scale;
        }

        /// <summary>
        /// Creates a static image element
        /// </summary>
        /// <param name="background"></param>
        public MenuButton(MenuItem parent, Texture2D background) 
            : base(parent)
        {
            Initialize(background, null);
        }

        /// <summary>
        /// Create menu button with specific background and label text
        /// </summary>
        public MenuButton(MenuItem parent, Texture2D background, string text) 
            : base(parent)
        {
            Initialize(text);
            Initialize(background, null);
        }

        #region subInitialize Helpers
        private void Initialize()
        {
            Font = Menu.Font;
            if (Font == null)
                throw new Exception("You must call Menu.LoadContent() to use a MenuButton");
            refreshItem();
        }

        private void Initialize(string text)
        {
            Initialize();
            Text = text;
            fontSize = Font.MeasureString(text);
            m_textureWidth = fontSize.X;
            m_textureHeight = fontSize.Y;
        }

        private void Initialize(Texture2D background, Texture2D focused)
        {
            Initialize();
            Background = background;
            FocusTexture = focused;

            if (Background != null)
            {
                m_textureWidth = Background.Width;
                m_textureHeight = Background.Height;

                if (FocusTexture != null && (FocusTexture.Height != Background.Height || FocusTexture.Width != Background.Width))
                    Console.WriteLine("Warning: The background and highlighed textures should be the same size");
            }
            else
            {
                m_textureWidth = 0;
                m_textureHeight = 0;
            }
        }
        #endregion
        #endregion

        /// <summary>
        /// Transform of button with animation transforming included. (scale, grow / shrink)
        /// </summary>
        internal virtual Matrix AnimatedItemTransform(GameTime gameTime)
        {
            Matrix animScale, local;
            local = ItemTransform;
            float fAnimVal = AnimationValue(gameTime);
            Matrix.CreateScale(fAnimVal, fAnimVal, 1, out animScale);
            return MenuItem.CombineMatrix(animScale, ref local);
        }

        internal override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix parentTransform)
        {
            //create transform matrix and multiply them backwards order, then decompose
            //to get out info we want
            Matrix local = MenuItem.CombineMatrix(AnimatedItemTransform(gameTime), ref parentTransform);

            Vector2 position, scale;
            float rotation;
            DecomposeMatrix(ref local, out position, out rotation, out scale);

            Vector2 center = new Vector2(m_textureWidth, m_textureHeight) / 2f;
            Vector2 fontCenter = fontSize / 2f;

            Texture2D tempTexture = null;

            if (Background != null && (!Enabled || FocusTexture == null))//regular background
                tempTexture = Background;
            else if (Enabled && FocusTexture != null)//focused background
                tempTexture = FocusTexture;

            if(tempTexture != null)//draw background
                spriteBatch.Draw(tempTexture, position, null, Tint * ScreenAlpha, rotation, center, scale, SpriteEffects.None, Depth);

            Color textColor = (Focus ? FontFocusColor : FontColor) * ScreenAlpha;

            Vector2 textScale = TextScale * scale; //TODO: fix text scale to have parent -> child like stuff...

            if (!String.IsNullOrEmpty(Text))//label
                spriteBatch.DrawString(Font, Text, position, textColor, rotation, fontCenter, textScale, SpriteEffects.None, Depth - .01f);

            foreach (MenuItem child in Children)
                child.Draw(gameTime, spriteBatch, local);
        }
    }
}
