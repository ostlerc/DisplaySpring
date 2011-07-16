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
        StackedItem stack;
        /// <summary>
        /// Sample Frame Menu
        /// </summary>
        public FrameMenu(MultiController controllers, List<Controller> allControllers, Rectangle bounds)
            : base(controllers, bounds)
        {
            stack = new StackedItem(BaseFrame);

            Frame menu_left = new Frame(stack) { SizePolicy = Frame.SizeType.Maximum };
            Frame menu_mid = new Frame(stack) { SizePolicy = Frame.SizeType.Maximum };
            Frame menu_right = new Frame(stack) { SizePolicy = Frame.SizeType.Maximum };

            stack.CurrentIndex = 1;
            stack.OnRight = delegate() { stack.KeepFocus = true; stack.CurrentIndex++; };
            stack.OnLeft = delegate() { stack.KeepFocus = true; stack.CurrentIndex--; };

            new Button(menu_mid, "Top left aligned") { HorizontalAlignment = HAlign.Left, VerticalAlignment = VAlign.Top };
            new Button(menu_mid, "Top right aligned") { HorizontalAlignment = HAlign.Right, VerticalAlignment = VAlign.Top };
            new Button(menu_mid, "Bot left aligned") { HorizontalAlignment = HAlign.Left, VerticalAlignment = VAlign.Bottom };
            new Button(menu_mid, "Bot right aligned") { HorizontalAlignment = HAlign.Right, VerticalAlignment = VAlign.Bottom };
            new Button(menu_mid, "Bot aligned") { VerticalAlignment = VAlign.Bottom };
            new Button(menu_mid, "Top aligned") { VerticalAlignment = VAlign.Top };

            new Button(menu_mid, Item.ArrowRight, "Alignment Stretch") { HorizontalAlignment = HAlign.Right, LabelStyle = Button.Style.LabelLeft };
            new Button(menu_mid, Item.ArrowLeft, "Size policies") { HorizontalAlignment = HAlign.Left, LabelStyle = Button.Style.LabelRight };

            new Label(menu_mid, "Buttons with Alignments\nThese menus are in a stacked item.\nThis allows the left and right to\nswitch menus easily");

            new Label(menu_right, "Alignment Stretch");

            new Button(menu_right, "Top Horizontal Stretch (menuTwo Width)") { VerticalAlignment = VAlign.Top, HorizontalAlignment = HAlign.Stretch };
            new Button(menu_right, "Bottom Horizontal Stretch (menuTwo Width)") { VerticalAlignment = VAlign.Bottom, HorizontalAlignment = HAlign.Stretch };

            Frame constrainingFrame = new Frame(menu_right) 
            { 
                SizePolicy = Frame.SizeType.Fixed,
                HorizontalAlignment = HAlign.Right
            };

            Button vertBtn = new Button(constrainingFrame, "Alignment Right\nVertical Align Stretch\nFixed height (500 px)") 
            {
                VerticalAlignment = VAlign.Stretch,
                Animation = AnimateType.None,
            };

            constrainingFrame.FixedSize = new Vector2(vertBtn.MeasureWidth, 500);

            new Button(menu_right, Item.ArrowLeft, "Alignments") 
            {
                HorizontalAlignment = HAlign.Left,
                LabelStyle = Button.Style.LabelRight 
            };

            new Label(menu_left, "Size policies") { Offset = new Vector2(0, -250) };

            Frame topSharedFrame = new Frame(menu_left) { Layout = Frame.LayoutType.HorizontalShared, VerticalAlignment = VAlign.Top };
            new Button(topSharedFrame, "Top Aligned"){ Animation = AnimateType.None, HorizontalAlignment = HAlign.Left };
            new Button(topSharedFrame, "Horizontal Layout"){ Animation = AnimateType.None };
            new Button(topSharedFrame, "Sharing"){ Animation = AnimateType.None };
            new Button(topSharedFrame, "Screen Width"){ Animation = AnimateType.None, HorizontalAlignment = HAlign.Right };

            Frame leftSharedFrame = new Frame(menu_left) { Layout = Frame.LayoutType.VerticalShared, HorizontalAlignment = HAlign.Left };
            new Button(leftSharedFrame, "Left Aligned"){ Animation = AnimateType.None };
            new Button(leftSharedFrame, "Vertical Layout"){ Animation = AnimateType.None };
            new Button(leftSharedFrame, "Sharing"){ Animation = AnimateType.None };
            new Button(leftSharedFrame, "Screen Height"){ Animation = AnimateType.None };

            Frame greedyMiddleFrame = new Frame(menu_left) { Padding = 0, Layout = Frame.LayoutType.Vertical };
            new Button(greedyMiddleFrame, "Center Aligned") { Animation = AnimateType.None };
            new Button(greedyMiddleFrame, "Vertical Layout"){ Animation = AnimateType.None };
            new Button(greedyMiddleFrame, "Not Sharing"){ Animation = AnimateType.None };
            new Button(greedyMiddleFrame, "Screen Height"){ Animation = AnimateType.None };

            Frame greedyBottomFrame = new Frame(menu_left) { Padding = 0, Layout = Frame.LayoutType.HorizontalShared, VerticalAlignment = VAlign.Bottom };
            Button temp = new Button(greedyBottomFrame, "Bottom Aligned") { Animation = AnimateType.None, HorizontalAlignment = HAlign.Left };
            new Button(greedyBottomFrame, "Horizontal Layout") { Animation = AnimateType.None };
            new Button(greedyBottomFrame, "Sharing") { Animation = AnimateType.None };
            new Button(greedyBottomFrame, "Screen Width") { Animation = AnimateType.None, HorizontalAlignment = HAlign.Right };

            new Button(menu_left, Item.ArrowRight, "Alignments") { HorizontalAlignment = HAlign.Right, LabelStyle = Button.Style.LabelLeft };

            Reset();
        }

        public override void Reset()
        {
            base.Reset(stack);
        }
    }
}
