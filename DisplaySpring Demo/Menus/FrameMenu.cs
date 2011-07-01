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
        Button btn;
        /// <summary>
        /// Sample Frame Menu
        /// </summary>
        public FrameMenu(MultiController controllers, List<Controller> allControllers, Rectangle bounds)
            : base(controllers, bounds)
        {
            //BaseFrame is a Menu Frame that inherits the size of the menu
            //Set Baseframe properties
            BaseFrame.Layout = Layout.Vertical;

            //Create a title, and scale the text so it is a little bigger
            Label title = new Label(BaseFrame, "This menu was created through layouts and alignments");
            title.FontColor = Color.Gold;

            Frame horizontalFrame = new Frame(BaseFrame);
            horizontalFrame.Layout = Layout.Horizontal;

            Button btn = new Button(horizontalFrame, Item.ButtonTexture, "One");
            btn.VerticalAlignment = VAlign.Top;
            btn.HorizontalAlignment = HAlign.Left;

            btn = new Button(horizontalFrame, Item.ButtonTexture, "Two");
            btn.VerticalAlignment = VAlign.Top;
            btn.HorizontalAlignment = HAlign.Right;

            horizontalFrame = new Frame(BaseFrame);
            horizontalFrame.Layout = Layout.Horizontal;

            btn = new Button(horizontalFrame, Item.ButtonTexture, "Three");
            btn.VerticalAlignment = VAlign.Stretch;
            btn.HorizontalAlignment = HAlign.Left;

            btn = new Button(horizontalFrame, Item.ButtonTexture, "Four");
            btn.VerticalAlignment = VAlign.Top;
            btn.HorizontalAlignment = HAlign.Right;

            horizontalFrame = new Frame(BaseFrame);
            horizontalFrame.Layout = Layout.Horizontal;

            new Button(horizontalFrame, Item.ButtonTexture, "third");

            horizontalFrame = new Frame(BaseFrame);
            horizontalFrame.Layout = Layout.Horizontal;

            Label lbl = new Label(horizontalFrame, "One");
            lbl.HorizontalAlignment = HAlign.Stretch;

            lbl = new Label(horizontalFrame, "Two");
            lbl.VerticalAlignment = VAlign.Top;
            lbl.HorizontalAlignment = HAlign.Right;

            horizontalFrame = new Frame(BaseFrame);
            horizontalFrame.Layout = Layout.Horizontal;

            lbl = new Label(horizontalFrame, "Three");
            lbl.VerticalAlignment = VAlign.Bottom;
            lbl.HorizontalAlignment = HAlign.Left;

            lbl = new Label(horizontalFrame, "Four");
            lbl.VerticalAlignment = VAlign.Bottom;
            lbl.HorizontalAlignment = HAlign.Right;

            horizontalFrame = new Frame(BaseFrame);
            horizontalFrame.Layout = Layout.Horizontal;
            lbl = new Label(horizontalFrame, "third");

            Reset();
        }

        public override void Draw(GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }
    }
}
