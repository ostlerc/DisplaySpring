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
            lbl.FontColor = Color.Black;

            sl = new ScrollList(BaseFrame, controllers);
            sl.Focus = true;
            sl.LayoutStretch = 4;

            lbl = new Label(sl, "Frames - Layout example");
            lbl.OnA = delegate() { ActiveSubMenu = new FrameMenu(controllers, allControllers, bounds); };
            lbl = new Label(sl, "Open submenu of this menu (recursion!)");
            lbl.OnA = delegate() { ActiveSubMenu = new MainMenu(controllers, allControllers, bounds); };
            lbl = new Label(sl, "Scroll List Menu");
            lbl.OnA = delegate() { ActiveSubMenu = new ButtonScrollListMenu(controllers, allControllers, bounds); };
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
