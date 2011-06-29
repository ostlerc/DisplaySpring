namespace DisplaySpringDemo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Storage;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Content;
    using System.IO;
    using DisplaySpring;
    using System.Reflection;

    static class App
    {
        static public Rectangle TitleSafeArea;

        #region [Textures, Music, Sound FX, Fonts
        // TEXTURES:

        static public Texture2D menuButtonHighlighted;
        static public Texture2D Button;

        //FONTS:
        /// <summary>
        /// Used for large text in menus
        /// </summary>
        static public SpriteFont MenuFont;
        static public SpriteFont LargeMenuFont;
        static public SpriteFont LegalFont;

        static public GraphicsDevice GraphicsDevice;
        /// <summary>
        /// Used for loading content while the game is running (things not loaded up front, such as level assets)
        /// </summary>
        static public ContentManager Content;

        /// <summary>
        /// Loads all content (textures, sound, fonts) for the game.  Called when the game starts up
        /// </summary>
        /// <param name="Content">Global content manager</param>
        /// <param name="gd">Global GraphicsDevice</param>
        static public void LoadContent(ContentManager content, GraphicsDevice gd)
        {
            Content = content;
            //TEXTURES:
            GraphicsDevice = gd;

            Button = Content.Load<Texture2D>("Menus/menuButton");
            menuButtonHighlighted = Content.Load<Texture2D>("Menus/menuButtonHighlighted");

            //FONTS:
            MenuFont = Content.Load<SpriteFont>("Fonts/Hud");
            LargeMenuFont = Content.Load<SpriteFont>("Fonts/LargeHud");
            LegalFont = Content.Load<SpriteFont>("Fonts/legalFont");
        }
        #endregion

        static public float WorldWidth = 1280f;
        static public float WorldHeight = 720f;

        /// <summary>
        /// List of Controllers used to gather input from players
        /// </summary>
        static public List<Controller> Controllers;

        /// <summary>
        /// Used for menus when only the principal player should have control
        /// </summary>
        static public Controller PrimaryController;

        /// <summary>
        /// Setup initial game values (such as setting the back buffer, etc...)
        /// </summary>
        /// <param name="graphics"></param>
        static public void Initialize(GraphicsDeviceManager graphics, UpdateEvent updates)
        {
            graphics.PreferredBackBufferWidth = (int)WorldWidth;
            graphics.PreferredBackBufferHeight = (int)WorldHeight;
            graphics.ApplyChanges();
            TitleSafeArea = graphics.GraphicsDevice.Viewport.TitleSafeArea;
            Controllers = new List<Controller>();
            Controllers.Add(new Controller(PlayerIndex.One, updates));
            Controllers.Add(new Controller(PlayerIndex.Two, updates));
            Controllers.Add(new Controller(PlayerIndex.Three, updates));
            Controllers.Add(new Controller(PlayerIndex.Four, updates));

#if DEBUG
            PrimaryController = Controllers[1];
#else
            PrimaryController = Controllers[0];
#endif
        }
    }
}
