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
            new Button(BaseFrame, Item.ArrowLeft, "Size policies") { HorizontalAlignment = HAlign.Left, LabelStyle = Button.Style.LabelRight };

            new Label(BaseFrame, "Buttons with Alignments");

            OnCloseSound = Item.DefaultFocusSound;

            RightMenu rMenu = new RightMenu(controllers, allControllers, bounds);
            rMenu.OnClosing = delegate() { BaseFrame.Visible = false; OnCloseSound = Menu.DefaultCloseSound; Close(); };

            LeftMenu lMenu = new LeftMenu(controllers, allControllers, bounds);
            lMenu.OnClosing = delegate() { BaseFrame.Visible = false; OnCloseSound = Menu.DefaultCloseSound; Close(); };

            BaseFrame.OnRight = delegate() { BaseFrame.KeepFocus = true; ActiveSubMenu = rMenu; };
            BaseFrame.OnB = delegate() { OnCloseSound = Menu.DefaultCloseSound; };
            BaseFrame.OnLeft = delegate() {  BaseFrame.KeepFocus = true; ActiveSubMenu = lMenu; };
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

            new Button(BaseFrame, "Top Horizontal Stretch (Baseframe Width)") { VerticalAlignment = VAlign.Top, HorizontalAlignment = HAlign.Stretch };
            new Button(BaseFrame, "Bottom Horizontal Stretch (Baseframe Width)") { VerticalAlignment = VAlign.Bottom, HorizontalAlignment = HAlign.Stretch };

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

    class LeftMenu : Menu
    {

        /// <summary>
        /// Sample Frame Menu
        /// </summary>
        public LeftMenu(MultiController controllers, List<Controller> allControllers, Rectangle bounds)
            : base(controllers, bounds)
        {
            new Label(BaseFrame, "Size policies") { Offset = new Vector2(0, -250) };

            Frame topSharedFrame = new Frame(BaseFrame) { Layout = Frame.LayoutType.Horizontal, VerticalAlignment = VAlign.Top };
            new Button(topSharedFrame, "Top Aligned"){ Animation = AnimateType.None };
            new Button(topSharedFrame, "Horizontal Layout"){ Animation = AnimateType.None };
            new Button(topSharedFrame, "Sharing"){ Animation = AnimateType.None };
            new Button(topSharedFrame, "Screen Width"){ Animation = AnimateType.None };

            Frame leftSharedFrame = new Frame(BaseFrame) { Layout = Frame.LayoutType.Vertical, HorizontalAlignment = HAlign.Left };
            new Button(leftSharedFrame, "Left Aligned"){ Animation = AnimateType.None };
            new Button(leftSharedFrame, "Vertical Layout"){ Animation = AnimateType.None };
            new Button(leftSharedFrame, "Sharing"){ Animation = AnimateType.None };
            new Button(leftSharedFrame, "Screen Height"){ Animation = AnimateType.None };

            Frame greedyMiddleFrame = new Frame(BaseFrame) { Padding = 0, Layout = Frame.LayoutType.Vertical, SizePolicy = Frame.SizeType.Greedy };
            new Button(greedyMiddleFrame, "Center Aligned") { Animation = AnimateType.None };
            new Button(greedyMiddleFrame, "Vertical Layout"){ Animation = AnimateType.None };
            new Button(greedyMiddleFrame, "Greedily using"){ Animation = AnimateType.None };
            new Button(greedyMiddleFrame, "Screen Height"){ Animation = AnimateType.None };

            Frame greedyBottomFrame = new Frame(BaseFrame) { Padding = 0, Layout = Frame.LayoutType.Horizontal, SizePolicy = Frame.SizeType.Greedy, VerticalAlignment = VAlign.Bottom };
            new Button(greedyBottomFrame, "Bottom Aligned") { Animation = AnimateType.None };
            new Button(greedyBottomFrame, "Horizontal Layout") { Animation = AnimateType.None };
            new Button(greedyBottomFrame, "Greedily using") { Animation = AnimateType.None };
            new Button(greedyBottomFrame, "Screen Width") { Animation = AnimateType.None };

            Reset();

            new Button(BaseFrame, Item.ArrowRight, "Alignments") { HorizontalAlignment = HAlign.Right, LabelStyle = Button.Style.LabelLeft };

            OnCloseSound = Item.DefaultFocusSound;
            BaseFrame.OnRight = delegate() { IsAlive = false; };
        }
    }
}
