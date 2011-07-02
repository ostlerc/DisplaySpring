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
        ScrollList sl;

        /// <summary>
        /// Sample Button Scroll List Menu
        /// </summary>
        public OptionButtonMenu(MultiController controllers, List<Controller> allControllers, Rectangle bounds)
            : base(controllers, bounds)
        {
            BaseFrame.Layout = Layout.Vertical;
            Label lbl = new Label(BaseFrame, "Left, Right controls index of OptionButton");
            lbl.VerticalAlignment = VAlign.Bottom;

            sl = new ScrollList(BaseFrame);
            sl.LayoutStretch = 3;

            OptionButton options = new OptionButton(sl);
            options.ArrowsOut = true;

            Button btn = new Button(options, Item.ButtonTexture, "Arrows Out");
            btn.TextLabel.FontFocusColor = Color.White;
            btn = new Button(options, Item.ButtonTexture, "two");
            btn.TextLabel.FontFocusColor = Color.White;
            btn.Scale *= 2;
            btn = new Button(options, Item.ButtonTexture, "three");
            btn.TextLabel.FontFocusColor = Color.White;
            btn = new Button(options, Item.ButtonTexture, "four");
            btn.TextLabel.FontFocusColor = Color.White;
            lbl = new Label(options, "a long text");

            options = new OptionButton(sl);

            btn = new Button(options, Item.ButtonTexture, "Arrows In");
            btn.TextLabel.FontFocusColor = Color.White;
            btn = new Button(options, Item.ButtonTexture, "two");
            btn.TextLabel.FontFocusColor = Color.White;
            btn.Scale *= 2;
            btn = new Button(options, Item.ButtonTexture, "three");
            btn.TextLabel.FontFocusColor = Color.White;
            btn = new Button(options, Item.ButtonTexture, "four");
            btn.TextLabel.FontFocusColor = Color.White;
            lbl = new Label(options, "a long text");

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



