using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Reflection;
using System.IO;

namespace DisplaySpring
{
    /// <summary>
    /// A Delegate to represent specific callback events in a menu
    /// </summary>
    public delegate void MenuAction();

    /// <summary>
    /// Base class for a Menu.
    /// Helps provide easy interfacing with submenus and user input
    /// </summary>
    public abstract class Menu
    {
        #region Member variables
        internal static SpriteFont m_font;
        private static float m_menuItemDrawDepth = 0.3f;
        private static float m_menuBackgroundDrawDepth = 0.4f;

        /// <summary>
        /// Default sound played when a menu is closed
        /// </summary>
        public static SoundEffect DefaultCloseSound;

        /// <summary>
        /// Amount of time in milliseconds the menu will fade from StartAlpha to EndAlpha
        /// </summary>
        public const int DefaultFadeTime = 750;
        internal bool m_stayAlive = false;
        internal bool m_keepState = true;
        internal int m_framesRun = 0;
        internal int m_framesToActivate = 10;
        internal float m_startAlpha = 0;
        private Frame m_baseFrame;

        /// <summary>
        /// The main frame of the menu that spans the boudns of the menu.
        /// </summary>
        public Frame BaseFrame
        {
            get { return m_baseFrame; }
        }
        internal MenuAction m_cancelMenu;
        internal static GraphicsDevice m_graphicsDevice;

        /// <summary>
        /// List of controllers the menu uses for input
        /// </summary>
        internal MultiController m_controllers;
        /// <summary>
        /// List of sprites to draw for the background. These are static
        /// images, no interactions or animations. Draws in order given in list
        /// </summary>
        internal List<Sprite> m_bgs;
        internal SoundEffect m_closeSound;
        /// <summary>
        /// Represents the middle position of the menu 
        /// </summary>
        internal Vector2 m_position;
        internal Rectangle m_bounds;
        internal Animation m_alpha;
        private bool m_isAlive = true;
        private Menu m_activeSubMenu;
        #endregion

        #region Properties
        /// <summary>
        /// Default text to use for Menu Items that have text
        /// </summary>
        public static SpriteFont Font
        {
            get { return m_font; }
            set { m_font = value; }
        }

        internal static GraphicsDevice GraphicsDevice
        {
            get { return m_graphicsDevice; }
        }
        /// <summary>
        /// If true the menu will skip Reset() between sub menu calls
        /// Default true
        /// </summary>
        public virtual bool KeepState
        {
            get { return m_keepState; }
            set { m_keepState = value; }
        }
        /// <summary>
        /// If set to true, menu will not close/die 
        /// </summary>
        public virtual bool StayAlive
        {
            get { return m_stayAlive; }
            set { m_stayAlive = value; }
        }
        /// <summary>
        /// if set to false, will take the menu up one level in heirarchy - making this menu not the active menu anymore
        /// The property is instantly reverted to true after menu has been popped off Menu stack
        /// </summary>
        public virtual bool IsAlive 
        {
            get { return m_isAlive; }
            set 
            {
                m_isAlive = value;
                if (!value)
                    Reset();
            }
        }
        /// <summary>
        /// The sub menu to recieve updates.
        /// When there is a sub menu, the parent menu will not recieve updates
        /// </summary>
        protected virtual Menu ActiveSubMenu
        {
            get { return m_activeSubMenu; }
            set 
            {
                m_activeSubMenu = value;

                if (value != null)
                {
                    if (KeepState)
                    {
                        /*foreach (var mi in m_items)
                        {
                            if (mi.Focus)
                            {
                                mi.KeepFocus = true; //sub menus to keep state
                            }
                        }*/
                        m_activeSubMenu.Reset();
                    }

                    m_activeSubMenu.IsAlive = true;
                }
            }
        }
        /// <summary>
        /// When Fade is set to true, this is the starting alpha value
        /// </summary>
        public virtual float StartAlpha
        {
            get { return m_alpha.StartVal; }
            set { m_alpha.StartVal = value; }
        }
        /// <summary>
        /// When Fade is set to true, this is the ending alpha value
        /// </summary>
        public virtual float EndAlpha
        {
            get { return m_alpha.EndVal; }
            set { m_alpha.EndVal = value; }
        }
        /// <summary>
        /// Delegate called when the menu is about to close
        /// </summary>
        public MenuAction OnClosing { get { return m_cancelMenu; } set { m_cancelMenu = value; } }
        /// <summary>
        /// Sound played when menu closes
        /// </summary>
        public SoundEffect OnCloseSound { get { return m_closeSound; } set { m_closeSound = value; } }
        /// <summary>
        /// Default draw depth for menu items
        /// </summary>
        public static float MenuItemDrawDepth
        {
            get { return m_menuItemDrawDepth; }
            set { m_menuItemDrawDepth = value; }
        }
        /// <summary>
        /// Default draw depth for backgrounds
        /// </summary>
        public static float MenuBackgroundDrawDepth
        {
            get { return m_menuBackgroundDrawDepth; }
            set { m_menuBackgroundDrawDepth = value; }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// default constructor for a menu
        /// </summary>
        /// <param name="c">Controllers that the menu receives input from</param>
        /// <param name="bounds">Bounds the menu will reside in</param>
        public Menu(MultiController c, Rectangle bounds)
        {
            if(m_font == null)
                throw new Exception("Error: Cannot construct a menu without calling Menu.LoadContent()");

            m_closeSound = DefaultCloseSound;
            m_bgs = new List<Sprite>();
            m_position = new Vector2(bounds.Center.X, bounds.Center.Y);
            m_bounds = bounds;
            m_controllers = c;
            m_alpha = new Animation(0, 1, DefaultFadeTime);
            m_baseFrame = new Frame(m_bounds);
        }
        #endregion

        #region Functions

        /// <summary>
        /// convert from Menu to MenuItem
        /// </summary>
        public static implicit operator Item(Menu m)
        {
            return m.m_baseFrame;
        }
        /// <summary>
        /// Loads default content for All Menus and MenuItems
        /// </summary>
        /// <param name="gd">Graphics Device of the Game</param>
        /// <param name="DefaultFont">Default Font the menus will use for drawing text</param>
        public static void LoadContent(GraphicsDevice gd, SpriteFont DefaultFont)
        {
            if (DefaultFont != null)
                m_font = DefaultFont;

            m_graphicsDevice = gd;

            Assembly _assembly = Assembly.GetExecutingAssembly();
            Stream _stream = null;

            //Sounds
            _stream =  _assembly.GetManifestResourceStream("DisplaySpring.Content.Sound.menuHighlight.wav");
            Item.DefaultFocusSound = SoundEffect.FromStream(_stream);

            _stream =  _assembly.GetManifestResourceStream("DisplaySpring.Content.Sound.menuSelect.wav");
            Item.DefaultSelectSound = SoundEffect.FromStream(_stream);

            _stream =  _assembly.GetManifestResourceStream("DisplaySpring.Content.Sound.menuBack.wav");
            Menu.DefaultCloseSound = SoundEffect.FromStream(_stream);
            Item.DefaultCancelSound = Menu.DefaultCloseSound;

            //Textures
            _stream = _assembly.GetManifestResourceStream("DisplaySpring.Content.Buttons.menuButton.png");
            Item.DefaultButtonTexture = Texture2D.FromStream(gd, _stream);
            _stream = _assembly.GetManifestResourceStream("DisplaySpring.Content.Buttons.menuButtonHighlighted.png");
            Item.DefaultButtonHighlightTexture = Texture2D.FromStream(gd, _stream);
            _stream = _assembly.GetManifestResourceStream("DisplaySpring.Content.Buttons.menuGrayArrowLeft.png");
            Item.DefaultArrowLeft = Texture2D.FromStream(gd, _stream);
            _stream = _assembly.GetManifestResourceStream("DisplaySpring.Content.Buttons.menuGrayArrowRight.png");
            Item.DefaultArrowRight = Texture2D.FromStream(gd, _stream);
            _stream = _assembly.GetManifestResourceStream("DisplaySpring.Content.Buttons.menuYellowArrowLeft.png");
            Item.DefaultArrowLeftHighlight = Texture2D.FromStream(gd, _stream);
            _stream = _assembly.GetManifestResourceStream("DisplaySpring.Content.Buttons.menuYellowArrowRight.png");
            Item.DefaultArrowRightHighlight = Texture2D.FromStream(gd, _stream);
        }

        /// <summary>
        /// Updates menu screen based on user input
        /// </summary>
        public virtual void Update(GameTime gameTime)
        {
            m_alpha.Update(gameTime);

            if (m_framesRun < m_framesToActivate)
            {
                m_framesRun++;
                return;
            }

            if (ActiveSubMenu != null)
            {
                if (!ActiveSubMenu.IsAlive)
                {
                    ActiveSubMenu = null;

                    /*foreach (MenuItem mi in m_items)
                    {
                        mi.KeepFocus = false;
                        mi.StartAlpha = m_alpha.StartVal;
                    }*/

                    m_alpha.Reset();
                }
                else
                {
                    ActiveSubMenu.Update(gameTime);
                    return;
                }
            }

            if ( m_controllers != null)
            {
                if (m_controllers.Back || m_controllers.B)
                    Close();
            }

            m_baseFrame.Update(gameTime);
        }

        /// <summary>
        /// Exits the given menu.
        /// Does nothing if StayAlive is true
        /// </summary>
        public virtual void Close()
        {
            if (!StayAlive)
            {
                if (m_closeSound != null)
                    m_closeSound.Play(0.5f, 0f, 0f);

                Invoke(OnClosing);

                IsAlive = false;
            }
        }

        /// <summary>
        /// Draws the menu
        /// </summary>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (ActiveSubMenu != null)
            {
                ActiveSubMenu.Draw(gameTime, spriteBatch);
                return;
            }

            foreach (var s in m_bgs)
            {
                Rectangle rect = s.destRect();
                spriteBatch.Draw(s.m_texture, rect, null, Color.White * m_alpha.Val, 0f, new Vector2(s.Width / 2, s.Height / 2), SpriteEffects.None, MenuBackgroundDrawDepth);
            }

            m_baseFrame.Draw(gameTime, spriteBatch);
        }

        internal virtual void Invoke(MenuAction action)
        {
            if (action != null)
            {
                action();

                Reset();
            }
        }

        internal static void StaticInvoke(MenuAction action)
        {
            if (action != null)
                action();
        }

        /// <summary>
        /// Adds a background sprite to the menu.
        /// Backgrounds are special in that they recieve no
        /// updates, and are faded in by the menus fade
        /// </summary>
        /// <param name="background"></param>
        public virtual void AddBackground(Sprite background)
        {
            m_bgs.Add(background);
        }

        /// <summary>
        /// Clear all background sprites for this Menu
        /// </summary>
        public virtual void ClearBackgrounds()
        {
            m_bgs.Clear();
        }

        /// <summary>
        /// Resets the menu. 
        /// Removes focus from every menu item but the given item
        /// Alpha is reset to StartAlpha
        /// All sub menus are reset
        /// </summary>
        public virtual void Reset(Item item)
        {
            m_framesRun = 0;
            m_alpha.Reset();

            m_baseFrame.Reset(false);

            item.Reset(true);

            if (ActiveSubMenu != null)
            {
                ActiveSubMenu.Reset();
                ActiveSubMenu = null;
            }
        }

        /// <summary>
        /// Resets the menu. 
        /// Removes focus from every menu item but the first item in the list.
        /// Alpha is reset to StartAlpha
        /// All sub menus are reset
        /// </summary>
        public virtual void Reset()
        {
            Reset(m_baseFrame);
        }
        #endregion
    }
}
