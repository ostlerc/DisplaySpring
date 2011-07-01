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

    class StackedItemMenu : Menu
    {
        StackedItem stack;

        /// <summary>
        /// Sample Button Scroll List Menu
        /// </summary>
        public StackedItemMenu(MultiController controllers, List<Controller> allControllers, Rectangle bounds)
            : base(controllers, bounds)
        {
            BaseFrame.Layout = Layout.Vertical;
            Label lbl = new Label(BaseFrame, "Left, Right controls index of StackedItem");
            lbl.VerticalAlignment = VAlign.Bottom;

            lbl = new Label(BaseFrame, "Up, Down controls Scale of StackedItem");
            lbl.VerticalAlignment = VAlign.Top;

            stack = new StackedItem(BaseFrame);
            stack.LayoutStretch = 10;

            Button btn = new Button(stack, Item.ButtonTexture, "one");
            btn.TextLabel.FontFocusColor = Color.White;
            btn = new Button(stack, Item.ButtonTexture, "two");
            btn.TextLabel.FontFocusColor = Color.White;
            btn = new Button(stack, Item.ButtonTexture, "three");
            btn.TextLabel.FontFocusColor = Color.White;
            btn = new Button(stack, Item.ButtonTexture, "four");
            btn.TextLabel.FontFocusColor = Color.White;

            stack.OnLeft = delegate() { stack.KeepFocus = true; stack.CurrentIndex--; };
            stack.OnRight = delegate() { stack.KeepFocus = true; stack.CurrentIndex++; };
            stack.OnUp = delegate() { stack.KeepFocus = true; stack.Scale += Vector2.One * .1f; };
            stack.OnDown = delegate() { stack.KeepFocus = true; stack.Scale -= Vector2.One * .1f; };

            Reset();
        }

        /// <summary>
        /// The reset button will provide a way to set focus to a button when changing
        /// to and from sub menus. It is best to override and implement this function
        /// </summary>
        public override void Reset()
        {
            base.Reset(stack);
        }
    }
}



