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
            ScrollList rightList = new ScrollList(BaseFrame);

            OptionList options = new OptionList(leftList);

            Button btn = new Button(options, "Pressed")
            {
                Animation = AnimateType.None,
                FocusTexture = null,
                OnFocus = delegate()
                {
                }
            };
            btn.TextLabel.FontFocusColor = Color.White;

            btn = new Button(options, "Released") { FocusTexture = null, Animation = AnimateType.None };
            btn.TextLabel.FontFocusColor = Color.White;
            btn = new Button(options, "Held") { FocusTexture = null, Animation = AnimateType.None };
            btn.TextLabel.FontFocusColor = Color.White;
            btn = new Button(options, "Continuous") { FocusTexture = null, Animation = AnimateType.None };
            btn.TextLabel.FontFocusColor = Color.White;

            DefaultItem = options;
        }
    }
}
