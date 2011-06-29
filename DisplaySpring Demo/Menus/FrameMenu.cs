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
            horizontalFrame.LayoutStretch = 4;

            btn  = new Button(horizontalFrame, controllers, App.Button, "One");
            btn.VerticalAlignment = VAlign.Stretch;
            btn.HorizontalAlignment = HAlign.Stretch;
            btn.TextLabel.Scale = Vector2.One / 2;

            btn  = new Button(horizontalFrame, controllers, App.Button, "Two");
            btn.VerticalAlignment = VAlign.Stretch;
            btn.HorizontalAlignment = HAlign.Stretch;
            btn.TextLabel.Scale = Vector2.One / 2;

            horizontalFrame = new Frame(BaseFrame);
            horizontalFrame.LayoutStretch = 4;
            horizontalFrame.Layout = Layout.Horizontal;

            btn  = new Button(horizontalFrame, controllers, App.Button, "Three");
            btn.VerticalAlignment = VAlign.Stretch;
            btn.HorizontalAlignment = HAlign.Stretch;
            btn.TextLabel.Scale = Vector2.One / 2;

            btn = new Button(null, controllers, App.Button, "Four");
            btn.VerticalAlignment = VAlign.Stretch;
            btn.HorizontalAlignment = HAlign.Stretch; btn.TextLabel.Scale = Vector2.One / 2;
            btn.Parent = horizontalFrame;

            Reset();
        }

        public override void Draw(GameTime gameTime, Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
        }
    }
}
