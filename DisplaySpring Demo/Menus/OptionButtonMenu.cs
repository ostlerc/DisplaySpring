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

    class OptionButtonMenu : Menu
    {
        OptionButton options;

        /// <summary>
        /// Sample Button Scroll List Menu
        /// </summary>
        public OptionButtonMenu(MultiController controllers, List<Controller> allControllers, Rectangle bounds)
            : base(controllers, bounds)
        {
            BaseFrame.Layout = Layout.Vertical;
            Label lbl = new Label(BaseFrame, "Left, Right controls index of OptionButton");
            lbl.VerticalAlignment = VAlign.Bottom;

            options = new OptionButton(BaseFrame, controllers);
            options.LayoutStretch = 2;

            Button btn = new Button(options, Item.ButtonTexture, "one");
            btn.TextLabel.FontFocusColor = Color.White;
            btn = new Button(options, Item.ButtonTexture, "two");
            btn.TextLabel.FontFocusColor = Color.White;
            btn = new Button(options, Item.ButtonTexture, "three");
            btn.TextLabel.FontFocusColor = Color.White;
            btn = new Button(options, Item.ButtonTexture, "four");
            btn.TextLabel.FontFocusColor = Color.White;

            Reset();
        }

        /// <summary>
        /// The reset button will provide a way to set focus to a button when changing
        /// to and from sub menus. It is best to override and implement this function
        /// </summary>
        public override void Reset()
        {
            base.Reset(options);
        }
    }
}



