namespace DisplaySpring
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;

    /// <summary>
    /// Button is basically a text button with callbacks
    /// You can give the button no background and no foreground with just text
    /// </summary>
    public class Button : Item
    {
        #region Member Variables
        internal Texture2D m_bg;
        internal Texture2D m_focused;
        internal float m_textureHeight;
        internal float m_textureWidth;
        internal Label m_label;

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

        /// <summary>
        /// Label of the Button
        /// </summary>
        public Label ButtonLabel { get { return m_label; } set { m_label = value; } }

        #endregion

        #region Properties

        internal override float StaticWidth
        {
            get { return m_textureWidth; }
        }
        internal override float StaticHeight
        {
            get { return m_textureHeight; }
        }

        /// <summary>
        /// Depth of the button. Default is Menu.ButtonDrawDepth
        /// By default 0 = front -> 1 = back
        /// </summary>
        public override float Depth
        {
            get { return base.Depth; }
            set
            {
                base.Depth = value;

                if (m_label != null)
                    m_label.Depth = value - .01f;
            }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Internal debugging constructor
        /// </summary>
        
        internal Button(Texture2D debugTexture, Item parent) 
            : base()
        {
            //TODO: make debug not a Button but a sprite
            LayoutStretch = 0;
            Depth = 0;
            Parent = parent;
            Initialize(debugTexture, null, "");
        }

        /// <summary>
        /// Create a menu button with specific background
        /// </summary>
        public Button(Item parent, MultiController c, Texture2D background) 
            : base(parent, c)
        {
            Initialize(background, null, "");
        }

        /// <summary>
        /// Create a menu item with specific background and text label
        /// </summary>
        public Button(Item parent, MultiController c, Texture2D background, string text) 
            : base(parent, c)
        {
            Initialize(background, null, text);
        }

        /// <summary>
        /// Create a menu item with specific background and highlight textures
        /// </summary>
        public Button(Item parent, MultiController c, Texture2D background, Texture2D focused) 
            : base(parent, c)
        {
            Initialize(background, focused, "");
        }

        /// <summary>
        /// Create a menu item with specific background and focus background with text
        /// </summary>
        public Button(Item parent, MultiController c, Texture2D background, Texture2D focused, string text)
            : base(parent, c)
        {
            Initialize(background, focused, text);
        }

        /// <summary>
        /// Creates a static image element
        /// </summary>
        public Button(Item parent, Texture2D background) 
            : base(parent)
        {
            Initialize(background, null, "");
        }

        /// <summary>
        /// Create menu button with specific background and label text
        /// </summary>
        public Button(Item parent, Texture2D background, string text) 
            : base(parent)
        {
            Initialize(background, null, text);
        }

        /// <summary>
        /// Create menu button with specific background and focus background
        /// </summary>
        public Button(Item parent, Texture2D background, Texture2D focused) 
            : base(parent)
        {
            Initialize(background, focused, "");
        }

        /// <summary>
        /// Create menu button with specific background, focus background and label text
        /// </summary>
        public Button(Item parent, Texture2D background, Texture2D focused, string text) 
            : base(parent)
        {
            Initialize(background, focused, text);
        }

        #region subInitialize Helpers

        private void Initialize(Texture2D background, Texture2D focused, string text)
        {
            if (background == null && focused == null)
                throw new Exception("Cannot create a button with no background or focus background");

            m_bg = background;
            m_focused = focused;

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

            //TODO: make sure height and width of button are correct with label
            m_label = new Label(this, text);
            m_label.Depth = Depth - .01f;

            refreshItem();
        }
        #endregion
        #endregion

        #region Class Functions

        /// <summary>
        /// Transform of button with animation transform included. (scale, grow / shrink)
        /// </summary>
        internal virtual Matrix AnimationTransform(GameTime gameTime)
        {
            Matrix animScale, local;
            local = ItemTransform;
            float fAnimVal = AnimationValue(gameTime);
            Matrix.CreateScale(fAnimVal, fAnimVal, 1, out animScale);
            return Item.CombineMatrix(animScale, ref local);
        }

        internal override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix parentTransform)
        {
            //create transform matrix and multiply them backwards order, then decompose
            //to get out info we want
            Matrix local = Item.CombineMatrix(AnimationTransform(gameTime), ref parentTransform);

            Vector2 position, scale;
            float rotation;
            DecomposeMatrix(ref local, out position, out rotation, out scale);

            Vector2 center = new Vector2(m_textureWidth, m_textureHeight) / 2f;

            Texture2D tempTexture = null;

            if (Background != null && (!Enabled || FocusTexture == null))//regular background
                tempTexture = Background;
            else if (Enabled && FocusTexture != null)//focused background
                tempTexture = FocusTexture;

            if(tempTexture != null)//draw background
                spriteBatch.Draw(tempTexture, position, null, Tint * ScreenAlpha, rotation, center, scale, SpriteEffects.None, Depth);

            foreach (Item child in Children)
                child.Draw(gameTime, spriteBatch, local);
        }

        #endregion
    }
}
