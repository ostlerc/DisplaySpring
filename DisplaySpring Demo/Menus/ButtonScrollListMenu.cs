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

    class ButtonScrollListMenu : Menu
    {
        ScrollList sl;

        /// <summary>
        /// Sample Button Scroll List Menu
        /// </summary>
        public ButtonScrollListMenu(MultiController controllers, List<Controller> allControllers, Rectangle bounds)
            : base(controllers, bounds)
        {
            BaseFrame.Layout = Layout.Vertical;

            //Create a title, and scale the text so it is a little bigger
            Label title = new Label(BaseFrame, "This menu shows a sample of buttons in a scroll list");
            title.FontColor = Color.Gold;

            sl = new ScrollList(BaseFrame, controllers);
            sl.LayoutStretch = 2;
            sl.ViewCount = 3;
            Button btn = new Button(sl, App.Button, "Button texture");
            btn = new Button(sl, App.Button, App.menuButtonHighlighted, "Highlight texture");
            btn = new Button(sl, App.Button, "White Font");
            btn.TextLabel.FontFocusColor = Color.White;
            btn = new Button(sl, App.Button, App.menuButtonHighlighted, "Swap Scroll Position");
            btn.OnA = delegate() 
            {
                btn.KeepFocus = true; //This prevents button from losing focus in a delegate invoke cycle

                if (sl.ScrollPosition == ScrollList.ScrollBarPosition.Left)
                    sl.ScrollPosition = ScrollList.ScrollBarPosition.Right;
                else
                    sl.ScrollPosition = ScrollList.ScrollBarPosition.Left;
            };

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

