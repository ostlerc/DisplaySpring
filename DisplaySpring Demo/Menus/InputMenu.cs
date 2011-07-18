namespace DisplaySpringDemo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using DisplaySpring;
    using Microsoft.Xna.Framework;

    class InputMenu : Menu
    {
        public InputMenu(MultiController controllers, List<Controller> allControllers, Rectangle bounds)
            : base(controllers, bounds)
        {
            BaseFrame.Layout = Frame.LayoutType.HorizontalShared;

            ScrollList leftList = new ScrollList(BaseFrame);
            Frame statusFrame = new Frame(BaseFrame) { Layout = Frame.LayoutType.Vertical };

            new Label(statusFrame, "Using ButtonState " + leftList.InputState.ToString());
            new Label(statusFrame, "Using ButtonState " + leftList.InputState.ToString());
            ScrollList rightList = new ScrollList(BaseFrame);

            leftList.OnRight = delegate() { rightList.Focus = true; };
            rightList.OnLeft = delegate() { leftList.Focus = true; };

            OptionList options = new OptionList(leftList);

            Button btn = new Button(options, "Pressed")
            {
                FocusTexture = null,
                OnFocus = delegate()
                {
                }
            };
            btn.TextLabel.FontFocusColor = Color.White;

            btn = new Button(options, "Released") { FocusTexture = null };
            btn.TextLabel.FontFocusColor = Color.White;
            btn = new Button(options, "Held") { FocusTexture = null };
            btn.TextLabel.FontFocusColor = Color.White;
            btn = new Button(options, "Continuous") { FocusTexture = null };
            btn.TextLabel.FontFocusColor = Color.White;

            DefaultItem = leftList;
        }
    }
}
