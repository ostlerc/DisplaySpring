namespace DisplaySpringDemo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Graphics;
    using DisplaySpring;
    using Layout = DisplaySpring.Frame.LayoutType;
    using Microsoft.Xna.Framework.Input;

    public static class MenuCreator
    {
        /// <summary>
        /// Creates a Main Menu
        /// </summary>
        public static Menu createMainMenu(MultiController controllers, List<Controller> allControllers, Rectangle bounds)
        {
            Menu MainMenu = new Menu(controllers, bounds);
            MainMenu.BaseFrame.Layout = Layout.Vertical;

            new Label(MainMenu.BaseFrame, "Display Spring Demo") { Scale = new Vector2(2, 2), FontColor = Color.White };

            ScrollList sl = new ScrollList(MainMenu.BaseFrame);
            sl.Focus = true;
            sl.LayoutStretch = 4;
            sl.Scale = new Vector2(2,2);

            Label lbl = new Label(sl, "Frames Menu");
            lbl.OnA = delegate() { MainMenu.ActiveSubMenu = new FrameMenu(controllers, allControllers, bounds); };
            lbl = new Label(sl, "Open submenu of this menu (recursion!)");
            lbl.OnA = delegate() { MainMenu.ActiveSubMenu = MenuCreator.createMainMenu(controllers, allControllers, bounds); };
            lbl = new Label(sl, "Scroll List Menu");
            lbl.OnA = delegate() { MainMenu.ActiveSubMenu = new ButtonScrollListMenu(controllers, allControllers, bounds); };
            lbl = new Label(sl, "Multi Texture Menu");
            lbl.OnA = delegate() { MainMenu.ActiveSubMenu = new MultiTextureMenu(controllers, allControllers, bounds); };
            lbl = new Label(sl, "Input Menu");
            lbl.OnA = delegate() { MainMenu.ActiveSubMenu = new InputMenu(controllers, allControllers, bounds); };
            lbl = new Label(sl, "Option Button Menu");
            lbl.OnA = delegate() { MainMenu.ActiveSubMenu = new OptionButtonMenu(controllers, allControllers, bounds); };
            lbl = new Label(sl, "Exit");
            lbl.OnA = delegate() { MainMenu.Close(); };
            return MainMenu;
        }
    }
}
