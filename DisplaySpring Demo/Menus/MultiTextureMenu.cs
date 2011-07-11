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

    class MultiTextureMenu : Menu
    {
        Frame multiFrame;

        /// <summary>
        /// Sample Button Scroll List Menu
        /// </summary>
        public MultiTextureMenu(MultiController controllers, List<Controller> allControllers, Rectangle bounds)
            : base(controllers, bounds)
        {
            BaseFrame.Layout = Layout.Vertical;
            multiFrame = new Frame(BaseFrame, new Vector2(400, 200));// { HorizontalAlignment = HAlign.Left, VerticalAlignment = VAlign.Top };
            multiFrame.Animation = AnimateType.Size;
            multiFrame.HorizontalAlignment = HAlign.Left;
            multiFrame.VerticalAlignment = VAlign.Top;

            Button b = new Button(multiFrame, Item.ButtonTexture);
            b.Depth += .1f;
            b.HorizontalAlignment = HAlign.Stretch;
            b.VerticalAlignment = VAlign.Stretch;
            Label lbl = new Label(multiFrame, "left");
            lbl.HorizontalAlignment = HAlign.Left;
            lbl = new Label(multiFrame, "right");
            lbl.HorizontalAlignment = HAlign.Right;

            Frame parent = new Frame(BaseFrame);
            parent.SizePolicy = Frame.SizeType.Minimum;
            parent.Layout = Layout.Horizontal;
            parent.HorizontalAlignment = HAlign.Right;
            parent.VerticalAlignment = VAlign.Bottom;
            new Button(parent, Item.ButtonTexture, "one");
            new Button(parent, Item.ButtonTexture, "two");
            new Button(parent, Item.ButtonTexture, "three");

            parent = new Frame(BaseFrame);
            parent.SizePolicy = Frame.SizeType.Minimum;
            parent.Layout = Layout.Vertical;
            parent.HorizontalAlignment = HAlign.Right;
            parent.VerticalAlignment = VAlign.Top;
            new Button(parent, Item.ButtonTexture, "one");
            new Button(parent, Item.ButtonTexture, "two");
            new Button(parent, Item.ButtonTexture, "three");

            Reset();
        }

        /// <summary>
        /// The reset button will provide a way to set focus to a button when changing
        /// to and from sub menus. It is best to override and implement this function
        /// </summary>
        public override void Reset()
        {
            base.Reset();
        }
    }
}


