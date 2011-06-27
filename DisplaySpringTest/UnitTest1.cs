namespace DisplaySpringTest
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xna.Framework;
    using DisplaySpring;

    public class testMenu : Menu 
    {
        public testMenu(Rectangle bounds)
            : base(null, bounds)
        { }
    }
    [TestClass]
    public class UnitTest1
    {
        internal Menu mainMenu;

        [TestInitialize]
        public void SetupMenu()
        {
            mainMenu = new testMenu(new Rectangle(0, 0, 800, 600));
        }

        [TestMethod]
        public void LayoutTester()
        {
            Label l = 
        }
    }
}
