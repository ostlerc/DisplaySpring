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
            leftList.onState = delegate(ButtonSet set) 
            {
                switch (set)
                {
                }
            };
            ScrollList rightList = new ScrollList(BaseFrame);


        }
    }
}
