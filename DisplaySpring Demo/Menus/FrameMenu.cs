namespace DisplaySpringDemo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using DisplaySpring;
    using VAlign = DisplaySpring.Item.VerticalAlignmentType;
    using HAlign = DisplaySpring.Item.HorizontalAlignmentType;
    using Layout = DisplaySpring.Frame.LayoutType;

    class FrameMenu : Menu
    {
        /// <summary>
        /// Sample Frame Menu
        /// </summary>
        public FrameMenu(MultiController controllers, List<Controller> allControllers, Rectangle bounds)
            : base(controllers, bounds)
        {
            new Button(BaseFrame, "Top left aligned") { HorizontalAlignment = HAlign.Left, VerticalAlignment = VAlign.Top };
            new Button(BaseFrame, "Top right aligned") { HorizontalAlignment = HAlign.Right, VerticalAlignment = VAlign.Top };
            new Button(BaseFrame, "Bot left aligned") { HorizontalAlignment = HAlign.Left, VerticalAlignment = VAlign.Bottom };
            new Button(BaseFrame, "Bot right aligned") { HorizontalAlignment = HAlign.Right, VerticalAlignment = VAlign.Bottom };
            new Button(BaseFrame, "Bot aligned") { VerticalAlignment = VAlign.Bottom };
            new Button(BaseFrame, "Top aligned") { VerticalAlignment = VAlign.Top };

            new Button(BaseFrame, Item.ArrowRight, "Alignment Stretch") { HorizontalAlignment = HAlign.Right, LabelStyle = Button.Style.LabelLeft };
            new Button(BaseFrame, Item.ArrowLeft, "go left") { HorizontalAlignment = HAlign.Left, LabelStyle = Button.Style.LabelRight };

            new Label(BaseFrame, "BaseFrame Alignments");

            OnCloseSound = Item.DefaultFocusSound;

            RightMenu rMenu = new RightMenu(controllers, allControllers, bounds);
            rMenu.OnClosing = delegate() { BaseFrame.Visible = false;  OnCloseSound = Menu.DefaultCloseSound; IsAlive = false; };

            BaseFrame.OnRight = delegate() { BaseFrame.KeepFocus = true; ActiveSubMenu = rMenu; };
            BaseFrame.OnB = delegate() { OnCloseSound = Menu.DefaultCloseSound; };
         //   BaseFrame.OnLeft = delegate() { };
        }
    }

    class RightMenu : Menu
    {

        /// <summary>
        /// Sample Frame Menu
        /// </summary>
        public RightMenu(MultiController controllers, List<Controller> allControllers, Rectangle bounds)
            : base(controllers, bounds)
        {
            new Label(BaseFrame, "Alignment Stretch");

            Button top = new Button(BaseFrame, "Top Horizontal Stretch (Baseframe Width)") { VerticalAlignment = VAlign.Top, HorizontalAlignment = HAlign.Stretch };

            Reset();

            Frame constrainingFrame = new Frame(BaseFrame) 
            { 
                SizePolicy = Frame.SizeType.Greedy,
                HorizontalAlignment = HAlign.Right
            };

            Button vertBtn = new Button(constrainingFrame, "Alignment Right\nV Stretch (500 px)") 
            {
                VerticalAlignment = VAlign.Stretch,
                Animation = AnimateType.None
            };

            constrainingFrame.FixedSize = new Vector2(vertBtn.MeasureWidth, 500);

            new Button(BaseFrame, Item.ArrowLeft, "Alignments") 
            {
                HorizontalAlignment = HAlign.Left,
                LabelStyle = Button.Style.LabelRight 
            };

            OnCloseSound = Item.DefaultFocusSound;
            BaseFrame.OnLeft = delegate() { IsAlive = false; };
        }
    }
}
