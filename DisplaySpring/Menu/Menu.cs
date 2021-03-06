﻿namespace DisplaySpring
{
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
    using Microsoft.Xna.Framework.Input;

    /// <summary>
    /// A Delegate to represent specific callback events in a menu
    /// </summary>
    public delegate void MenuAction();
    public delegate void InputAction(ButtonSet set);

    /// <summary>
    /// Base class for a Menu.
    /// Helps provide easy interfacing with submenus and user input
    /// </summary>
    public class Menu
    {
        #region Member variables
        internal static SpriteFont m_font;
        private static float m_ItemDrawDepth = 0.3f;
        private static float m_menuBackgroundDrawDepth = 0.4f;
        private static int m_defaultFadeTime = 750;
        private static bool m_initialized = false;
        private Item m_defaultItem;

        /// <summary>
        /// Default sound played when a menu is closed
        /// </summary>
        public static SoundEffect DefaultCloseSound;

        internal bool m_stayAlive = false;
        internal bool m_keepState = true;
        internal int m_framesRun = 0;
        internal int m_framesToActivate = 10;
        private Frame m_baseFrame;

        /// <summary>
        /// The main frame of the menu that spans the bounds of the menu.
        /// </summary>
        public Frame BaseFrame
        {
            get { return m_baseFrame; }
        }

        /// <summary>
        /// The item that will initially recieve focus on a Menu Reset.
        /// This is the BaseFrame by default
        /// </summary>
        public Item DefaultItem
        {
            get { return m_defaultItem; }
            set { m_defaultItem = value; }
        }

        /// <summary>
        /// Amount of time in milliseconds the menu will fade from StartAlpha to EndAlpha
        /// </summary>
        public static int DefaultFadeTime
        {
            get { return m_defaultFadeTime; }
            set { m_defaultFadeTime = value; }
        }

        internal MenuAction m_cancelMenu;
        internal static GraphicsDevice m_graphicsDevice;

        /// <summary>
        /// List of controllers the menu uses for input
        /// </summary>
        internal MultiController m_controllers;

        /// <summary>
        /// Controllers that control the menu
        /// </summary>
        public MultiController Controllers
        {
            get { return m_controllers; }
        }
        /// <summary>
        /// List of sprites to draw for the background. These are static
        /// images, no interactions or animations. Draws in order given in list
        /// </summary>
        internal SoundEffect m_closeSound;
        internal Rectangle m_bounds;
        private bool m_isAlive = true;
        private Menu m_activeSubMenu;
        #endregion

        #region Properties

        /// <summary>
        /// Get or Set the bounds for the Menu
        /// </summary>
        public Rectangle Bounds
        {
            get { return m_bounds; }
            set { m_bounds = value; }
        }

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
        public virtual Menu ActiveSubMenu
        {
            get { return m_activeSubMenu; }
            set 
            {
                m_activeSubMenu = value;

                if (value != null)
                {
                    if (KeepState)
                    {
                        foreach (var mi in BaseFrame.Children)
                        {
                            if (mi.Focus)
                            {
                                mi.KeepFocus = true; //sub menus to keep state
                            }
                        }
                        m_activeSubMenu.Reset();
                    }

                    m_activeSubMenu.LayoutSize = LayoutSize;
                    m_activeSubMenu.IsAlive = true;
                }
            }
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
        public static float ItemDrawDepth
        {
            get { return m_ItemDrawDepth; }
            set { m_ItemDrawDepth = value; }
        }
        #endregion

        private Vector2 m_layoutSize;
        /// <summary>
        /// Sets the size of the base frame. The base frame will be
        /// scaled to draw within the bounds given at menu initialization
        /// </summary>
        public Vector2 LayoutSize
        {
            get { return m_layoutSize; }
            set { m_layoutSize = value; Init();  }
        }

        #region Constructors

        private void Init()
        {
            m_baseFrame.ForcedSize = LayoutSize;
            m_baseFrame.LayoutPosition = new Vector2(m_bounds.Center.X, m_bounds.Center.Y);
            m_baseFrame.ScaleImageToWidth(m_bounds.Width);
            m_baseFrame.ScaleImageToHeight(m_bounds.Height);
        }

        /// <summary>
        /// default constructor for a menu
        /// </summary>
        /// <param name="c">Controllers that the menu receives input from</param>
        /// <param name="bounds">Bounds the menu will reside in</param>
        public Menu(MultiController c, Rectangle bounds)
        {
            if(m_font == null)
                throw new Exception("Error: Cannot construct a menu without calling Menu.LoadContent()");

            m_defaultItem = m_baseFrame;
            m_closeSound = DefaultCloseSound;
            m_bounds = bounds;
            m_controllers = c;
            m_baseFrame = new Frame(null);
            m_baseFrame.ItemController = m_controllers;
            LayoutSize = new Vector2(bounds.Width, bounds.Height);
            Init();
        }
        #endregion

        #region Functions

        /// <summary>
        /// Loads default content for All Menus and Items
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
            _stream = _assembly.GetManifestResourceStream("DisplaySpring.Content.Buttons.button.png");
            Item.ButtonTexture = Texture2D.FromStream(gd, _stream);
            _stream = _assembly.GetManifestResourceStream("DisplaySpring.Content.Buttons.buttonHighlighted.png");
            Item.ButtonHighlightTexture = Texture2D.FromStream(gd, _stream);
            _stream = _assembly.GetManifestResourceStream("DisplaySpring.Content.Buttons.grayArrowLeft.png");
            Item.ArrowLeft = Texture2D.FromStream(gd, _stream);
            _stream = _assembly.GetManifestResourceStream("DisplaySpring.Content.Buttons.grayArrowRight.png");
            Item.ArrowRight = Texture2D.FromStream(gd, _stream);
            _stream = _assembly.GetManifestResourceStream("DisplaySpring.Content.Buttons.yellowArrowLeft.png");
            Item.DefaultArrowLeftHighlight = Texture2D.FromStream(gd, _stream);
            _stream = _assembly.GetManifestResourceStream("DisplaySpring.Content.Buttons.yellowArrowRight.png");
            Item.DefaultArrowRightHighlight = Texture2D.FromStream(gd, _stream);
        }

        /// <summary>
        /// Updates menu screen based on user input
        /// </summary>
        public virtual void Update(GameTime gameTime)
        {
            if (m_framesRun < m_framesToActivate)
            {
                if (!m_initialized)
                    Reset();
                m_framesRun++;
                return;
            }

            if (ActiveSubMenu != null)
            {
                if (!ActiveSubMenu.IsAlive)
                {
                    ActiveSubMenu = null;
                }
                else
                {
                    ActiveSubMenu.Update(gameTime);
                    return;
                }
            }

            m_baseFrame.Update(gameTime);

            if ( m_controllers != null)
            {
                if (m_controllers.Pressed(ButtonSet.Back) || m_controllers.Pressed(ButtonSet.B))
                    Close();
            }
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
                    m_closeSound.Play(Item.MasterVolume, 0f, 0f);

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

        /// <summary>
        /// Activates the MenuAction if it is not null
        /// </summary>
        public static void StaticInvoke(MenuAction action)
        {
            if (action != null)
                action();
        }

        /// <summary>
        /// Resets the menu. 
        /// Removes focus from every menu item but the given item
        /// All sub menus are reset
        /// </summary>
        public virtual void Reset(Item item)
        {
            m_framesRun = 0;

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
        /// All sub menus are reset
        /// </summary>
        public virtual void Reset()
        {
            m_initialized = true;

            if(m_defaultItem != null)
                Reset(m_defaultItem);

            if (m_defaultItem != m_baseFrame)
                Reset(m_baseFrame);
        }
        #endregion
    }
}
