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
            Label lbl = new Label(BaseFrame, 
                "Create MultiTextures by placing items in a Frame\n" +
                "Press A (Enter) to change alignment on Frame\n" + 
                "Left Thumbstick (Arrow Keys) to move around\n" + 
                "+/- to scale\nStart or Space to reset");
            lbl.Depth += .11f;

            BaseFrame.InputState = ButtonState.Continuous;
            BaseFrame.ExclusiveInput = false;

            multiFrame = new Frame(BaseFrame, new Vector2(400, 200)) 
            { 
                HorizontalAlignment = HAlign.Left,
                VerticalAlignment = VAlign.Top,
            };

            Sprite background = new Sprite(multiFrame, Item.ButtonTexture) 
            {
                HorizontalAlignment = HAlign.Stretch,
                VerticalAlignment = VAlign.Stretch,
            };
            background.Depth += .1f;

            new Label(multiFrame, "Left") { HorizontalAlignment = HAlign.Left };
            new Label(multiFrame, "Bot Left") { HorizontalAlignment = HAlign.Left, VerticalAlignment = VAlign.Bottom };
            new Label(multiFrame, "Bot Right") { HorizontalAlignment = HAlign.Right, VerticalAlignment = VAlign.Bottom };
            new Label(multiFrame, "Top") { VerticalAlignment = VAlign.Top };
            new Label(multiFrame, "Top left") { VerticalAlignment = VAlign.Top, HorizontalAlignment = HAlign.Left };
            new Label(multiFrame, "Top right") { VerticalAlignment = VAlign.Top, HorizontalAlignment = HAlign.Right };
            new Label(multiFrame, "Bottom") { VerticalAlignment = VAlign.Bottom };
            new Label(multiFrame, "Right") { HorizontalAlignment = HAlign.Right };
            new Label(multiFrame, "400x200 frame\nTop Left Aligned");

            int at = 4;
            multiFrame.OnA = delegate() 
            {
                multiFrame.KeepFocus = true;

                if (at == 0)
                {
                    multiFrame.HorizontalAlignment = HAlign.Right;
                    multiFrame.VerticalAlignment = VAlign.Top;
                }
                else if (at == 1)
                    multiFrame.VerticalAlignment = VAlign.Bottom;
                else if (at == 2)
                    multiFrame.HorizontalAlignment = HAlign.Left;
                else if (at == 3)
                    multiFrame.VerticalAlignment = VAlign.Top;
                else if (at == 4)
                {
                    multiFrame.HorizontalAlignment = HAlign.Center;
                    multiFrame.VerticalAlignment = VAlign.Center;
                    at = -1;
                }

                at++;
            };

            BaseFrame.OnUp = delegate()
            {
                BaseFrame.KeepFocus = true;

                multiFrame.Offset -= Vector2.UnitY * 15;
            };

            BaseFrame.OnDown = delegate()
            {
                BaseFrame.KeepFocus = true;

                multiFrame.Offset += Vector2.UnitY * 15;
            };

            BaseFrame.OnLeft = delegate()
            {
                BaseFrame.KeepFocus = true;

                multiFrame.Offset -= Vector2.UnitX * 15;
            };

            BaseFrame.OnRight = delegate()
            {
                BaseFrame.KeepFocus = true;

                multiFrame.Offset += Vector2.UnitX * 15;
            };

            BaseFrame.OnStart = delegate()
            {
                BaseFrame.KeepFocus = true;
                multiFrame.Scale = Vector2.One;
                multiFrame.Offset = Vector2.Zero;
                multiFrame.HorizontalAlignment = HAlign.Left;
                multiFrame.VerticalAlignment = VAlign.Top;
            };

            Reset();
        }

        public override void Update(GameTime gameTime)
        {
            if (Controllers.Continuous(Microsoft.Xna.Framework.Input.Keys.OemPlus))
                multiFrame.Scale *= 1.05f;
            else if (Controllers.Continuous(Microsoft.Xna.Framework.Input.Keys.OemMinus))
                multiFrame.Scale *= .95f;

            base.Update(gameTime);
        }

        /// <summary>
        /// The reset button will provide a way to set focus to a button when changing
        /// to and from sub menus. It is best to override and implement this function
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            multiFrame.Enabled = true;
        }
    }
}


