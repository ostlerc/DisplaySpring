﻿namespace DisplaySpringDemo
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

    public class MainMenu : Menu
    {
        ScrollList sl;

        /// <summary>
        /// Sample Main Menu
        /// </summary>
        public MainMenu(MultiController controllers, List<Controller> allControllers, Rectangle bounds)
            : base(controllers, bounds)
        {
            BaseFrame.Layout = Layout.Vertical;

            Label lbl = new Label(BaseFrame, "Display Spring Demo");
            lbl.Scale = new Vector2(2, 2);
            lbl.FontColor = Color.White;

            sl = new ScrollList(BaseFrame);
            sl.Focus = true;
            sl.LayoutStretch = 4;
            sl.Scale = new Vector2(2,2);

            lbl = new Label(sl, "Frames Menu");
            lbl.OnA = delegate() { ActiveSubMenu = new FrameMenu(controllers, allControllers, bounds); };
            lbl = new Label(sl, "Open submenu of this menu (recursion!)");
            lbl.OnA = delegate() { ActiveSubMenu = new MainMenu(controllers, allControllers, bounds); };
            lbl = new Label(sl, "Scroll List Menu");
            lbl.OnA = delegate() { ActiveSubMenu = new ButtonScrollListMenu(controllers, allControllers, bounds); };
            lbl = new Label(sl, "Multi Texture Menu");
            lbl.OnA = delegate() { ActiveSubMenu = new MultiTextureMenu(controllers, allControllers, bounds); };
            lbl = new Label(sl, "Input Menu");
            lbl.OnA = delegate() { ActiveSubMenu = new InputMenu(controllers, allControllers, bounds); };
            lbl = new Label(sl, "Option Button Menu");
            lbl.OnA = delegate() { ActiveSubMenu = new OptionButtonMenu(controllers, allControllers, bounds); };
            lbl = new Label(sl, "Exit");
            lbl.OnA = delegate() { Close(); };
            Reset();
        }

        /// <summary>
        /// The reset button will provide a way to set focus to a button when changing
        /// to and from sub menus. It is best to override and implement this function
        /// </summary>
        public override void Reset()
        {
            base.Reset(sl);
        }
    }
}
