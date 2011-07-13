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
        internal Sprite m_bg;
        internal Sprite m_focused;
        internal Label m_label;
        internal Style m_style = Style.LabelCenter;
        internal bool m_scaleLabel = false;

        public enum Style
        {
            LabelCenter,
            LabelLeft,
            LabelRight
        }

        /// <summary>
        /// If true, the label will scale with the button. Otherwise the label will only use its scale value
        /// </summary>
        public bool LabelScaled
        {
            get { return m_scaleLabel; }
            set { m_scaleLabel = value; }
        }

        public Style LabelStyle
        {
            get { return m_style; }
            set { m_style = value; forceRefresh(); }
        }

        /// <summary>
        /// The focus background of the button.
        /// This texture is shown when the item has focus
        /// </summary>
        public Texture2D FocusTexture
        {
            get { return m_focused != null ? m_focused.Texture : null; }
            set 
            {
                if (m_focused != null)
                    m_focused.Texture = value;
                else
                {
                    m_focused = new Sprite(this, value);
                    m_focused.Fade = false;
                }
            }
        }

        public override Color Tint
        {
            get { return base.Tint; }
            set
            {
                if (m_bg != null)
                    m_bg.Tint = value;
                if (m_focused != null)
                    m_focused.Tint = value;
                base.Tint = value;
            }
        }

        /// <summary>
        /// The main background of the button.
        /// This texture is shown when the item does not have focus
        /// </summary>
        public Texture2D Background
        {
            get { return m_bg.Texture; }
            set 
            {
                if (m_bg != null)
                    m_bg.Texture = value;
                else
                {
                    m_bg = new Sprite(this, value);
                    m_bg.Fade = false;
                }
            }
        }

        /// <summary>
        /// Label of the Button
        /// </summary>
        public Label TextLabel { get { return m_label; } set { m_label = value; } }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the focus for the menu item
        /// On set to true the OnFocus delegate will be called and FocusSound will play
        /// </summary>
        public override bool Focus
        {
            get { return base.Focus; }
            set
            {
                if(m_label != null)
                    m_label.Enabled = value;
                if (m_bg != null)
                    m_bg.Enabled = value;
                if (m_focused != null)
                    m_focused.Enabled = value;

                base.Focus = value;
            }
        }

        /// <summary>
        /// A disabled button will not process in the update function
        /// This is a simple way to set focus to false without any consequence
        /// or side effect (like playing sounds, or causing delegates to be called)
        /// </summary>
        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                if(m_label != null)
                    m_label.Enabled = value;
                if (m_bg != null)
                    m_bg.Enabled = value;
                if (m_focused != null)
                    m_focused.Enabled = value;

                base.Enabled = value;
            }
        }

        public override float StaticWidth
        {
            get 
            {
                float w = 0;

                if (Focus && m_focused != null)
                    w = m_focused.MeasureWidth;
                else if (m_bg != null)
                    w = m_bg.MeasureWidth;

                float labelWidth = 0;

                if (m_label != null)
                    labelWidth = m_label.MeasureWidth;

                switch(LabelStyle)
                {
                    case Style.LabelLeft:
                    case Style.LabelRight:
                        return w + labelWidth;
                    case Style.LabelCenter:
                    default:
                        return w;
                }
            }
        }
        public override float StaticHeight
        {
            get
            {
                float h = 0;
                if (Focus && m_focused != null)
                    h = m_focused.StaticHeight;
                else if (m_bg != null)
                    h = m_bg.StaticHeight;

                float labelHeight = m_label != null ? m_label.StaticHeight : 0;
                switch(LabelStyle)
                {
                    case Style.LabelCenter:
                    default:
                        return h;
                    case Style.LabelLeft:
                    case Style.LabelRight:
                        return h + labelHeight;
                }
            }
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
                if (m_bg != null)
                    m_bg.Depth = value;
                if (m_focused != null)
                    m_focused.Depth = value;
            }
        }
        #endregion

        #region Constructors

        /// <summary>
        /// Create a default button
        /// </summary>
        public Button(Item parent) 
            : base(parent)
        {
            Initialize(Button.ButtonTexture, Button.ButtonHighlightTexture, "");
        }

        /// <summary>
        /// Create a default button with a label
        /// </summary>
        public Button(Item parent, string label) 
            : base(parent)
        {
            Initialize(Button.ButtonTexture, Button.ButtonHighlightTexture, label);
        }
        /// <summary>
        /// Create a default button
        /// </summary>
        public Button(Item parent, MultiController c) 
            : base(parent, c)
        {
            Initialize(Button.ButtonTexture, Button.ButtonHighlightTexture, "");
        }

        /// <summary>
        /// Create a default button with a label
        /// </summary>
        public Button(Item parent, MultiController c, string label) 
            : base(parent, c)
        {
            Initialize(Button.ButtonTexture, Button.ButtonHighlightTexture, label);
        }

        /// <summary>
        /// Create a button with specific background
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
        /// Create button with specific background and label text
        /// </summary>
        public Button(Item parent, Texture2D background, string text) 
            : base(parent)
        {
            Initialize(background, null, text);
        }

        /// <summary>
        /// Create button with specific background and focus background
        /// </summary>
        public Button(Item parent, Texture2D background, Texture2D focused) 
            : base(parent)
        {
            Initialize(background, focused, "");
        }

        /// <summary>
        /// Create button with specific background, focus background and label text
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

            if (background != null)
            {
                m_bg = new Sprite(this, background);
                m_bg.Fade = false;
            }
            if (focused != null)
            {
                m_focused = new Sprite(this, focused);
                m_focused.Fade = false;
            }

            if (Background != null)
            {
                if (FocusTexture != null && (FocusTexture.Height != Background.Height || FocusTexture.Width != Background.Width))
                    Console.WriteLine("Warning: The background and highlighed textures should be the same size");
            }

            //TODO: make sure height and width of button are correct with label
            m_label = new Label(this, text);
            m_label.Animation = AnimateType.None;
            m_label.FocusSound = null;
            m_label.Fade = false;

            Animation = AnimateType.Size;

            forceRefresh();
        }

        /// <summary>
        /// Reset the Item to a fresh state
        /// </summary>
        public override void Reset(bool isFocus)
        {
            m_label.Reset(isFocus);
            if(m_bg != null)
                m_bg.Reset(isFocus);
            if(m_focused != null)
                m_focused.Reset(isFocus);
            base.Reset(isFocus);
        }
        #endregion
        #endregion

        #region Class Functions

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

            Sprite curSprite = null;

            if (Background != null && (!Enabled || FocusTexture == null))//regular background
                curSprite = m_bg;
            else if (Enabled && FocusTexture != null)//focused background
                curSprite = m_focused;

            switch(LabelStyle)
            {
                case Style.LabelCenter:
                default:
                    m_label.LayoutPosition = Vector2.Zero;
                    break;
                case Style.LabelLeft:
                    m_label.LayoutPosition = -new Vector2(StaticWidth - m_label.StaticWidth, 0) / 2;
                    curSprite.LayoutPosition = new Vector2(StaticWidth - curSprite.StaticWidth, 0) / 2;
                    break;
                case Style.LabelRight:
                    m_label.LayoutPosition = new Vector2(StaticWidth - m_label.StaticWidth, 0) / 2;
                    curSprite.LayoutPosition = -new Vector2(StaticWidth - curSprite.StaticWidth, 0) / 2;
                    break;
            }

            curSprite.Draw(gameTime, spriteBatch, local);

            foreach (var v in Children)
            {
                if (v == m_bg || v == m_focused)
                    continue;

                if(v == m_label && !LabelScaled)
                    v.Draw(gameTime, spriteBatch, Matrix.Multiply(Matrix.CreateScale(1 / Scale.X, 1 / Scale.Y, 1), local));
                else
                    v.Draw(gameTime, spriteBatch, local);
            }
        }

        #endregion
    }
}
