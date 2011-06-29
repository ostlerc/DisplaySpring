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
            multiFrame = new Frame(BaseFrame, new Vector2(400, 200));
            Button b = new Button(multiFrame, Item.ButtonTexture);
            b.Depth += .1f;
            b.HorizontalAlignment = HAlign.Stretch;
            b.VerticalAlignment = VAlign.Stretch;
            Label lbl = new Label(multiFrame, "left");
            lbl.HorizontalAlignment = HAlign.Left;
            lbl = new Label(multiFrame, "right");
            lbl.HorizontalAlignment = HAlign.Right;

            Reset();
        }

        /// <summary>
        /// The reset button will provide a way to set focus to a button when changing
        /// to and from sub menus. It is best to override and implement this function
        /// </summary>
        public override void Reset()
        {
            base.Reset(multiFrame);
        }
    }
}


