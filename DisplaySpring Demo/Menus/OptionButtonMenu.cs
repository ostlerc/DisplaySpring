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
        /// <summary>
        /// Sample Button Scroll List Menu
        /// </summary>
        public OptionButtonMenu(MultiController controllers, List<Controller> allControllers, Rectangle bounds)
            : base(controllers, bounds)
        {
            BaseFrame.Layout = Layout.VerticalShared;

            new Label(BaseFrame, "Left, Right controls index of OptionButton") 
            { VerticalAlignment = VAlign.Bottom };

            ScrollList sl = new ScrollList(BaseFrame)
            { LayoutStretch = 3 };

            OptionList options = createOptionButton(sl, "Arrows Out");
            options.ArrowsOut = true;
            createOptionButton(sl, "Arrows In");
            DefaultItem = sl;
        }

        OptionList createOptionButton(Item parent, string text)
        {
            OptionList options = new OptionList(parent);

            Button btn = new Button(options, Item.ButtonTexture, "One, " + text);
            btn.TextLabel.FontFocusColor = Color.White;
            btn = new Button(options, Item.ButtonTexture, "Two");
            btn.TextLabel.FontFocusColor = Color.White;
            btn.Scale *= 2;
            btn = new Button(options, Item.ButtonTexture, "Three");
            btn.TextLabel.FontFocusColor = Color.White;
            btn = new Button(options, Item.ButtonTexture, "Four");
            btn.TextLabel.FontFocusColor = Color.White;
            new Label(options, "a long text");

            return options;
        }
    }
}



