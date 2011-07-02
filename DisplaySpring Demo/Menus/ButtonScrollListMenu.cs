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
            new Label(BaseFrame, "This menu shows a sample of buttons in a scroll list.") 
            { 
                Scale = new Vector2(1.25f, 1.25f),
                  FontColor = Color.Gold 
            };

            sl = new ScrollList(BaseFrame)
            {
                Direction = ScrollList.Orientation.Horizontal,
                LayoutStretch = 2,
                ViewCount = 2,
                ScrollPosition = ScrollList.ScrollBarPosition.Left //left means top for horizontal orientation
            };

            ScrollList sl1 = createScrollList(sl);
            ScrollList sl2 = createScrollList(sl); sl2.Scale *= 2f;
            ScrollList sl3 = createScrollList(sl);

            sl.ScaleImageToWidth(BaseFrame.Width);

            Reset();
        }

        ScrollList createScrollList(Item parent)
        {
            ScrollList sl = new ScrollList(parent);
            sl.ViewCount = 2;

            Button btn = new Button(sl, App.Button, "Button texture");

            btn = new Button(sl, App.Button, App.menuButtonHighlighted, "Highlight texture")
            { Scale = new Vector2(1.35f, 1.35f) };

            btn = new Button(sl, App.Button, "White Font")
            { Scale = new Vector2(1.35f, 1.35f), };

            btn.TextLabel.FontFocusColor = Color.White;
            btn = new Button(sl, App.Button, App.menuButtonHighlighted, "Swap Scroll Position");
            btn.OnA = delegate() 
            {
                sl.KeepFocus = true; //This prevents button from losing focus in a delegate invoke cycle

                if (sl.ScrollPosition == ScrollList.ScrollBarPosition.Left)
                    sl.ScrollPosition = ScrollList.ScrollBarPosition.Right;
                else
                    sl.ScrollPosition = ScrollList.ScrollBarPosition.Left;
            };

            return sl;
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

