namespace DisplaySpring
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// A collection of Items, with only one visible or in focus at a time
    /// </summary>
    public class OptionButton : StackedItem
    {
        #region Member Variables
        private int m_currentIndex = -1;
        private bool m_ignorechildAdded = false;
        private Button m_leftArrow;
        private Button m_rightArrow;
        #endregion

        #region Properties

        #endregion

        #region Constructors

        /// <summary>
        /// Create a StackedItem with parents size.
        /// </summary>
        public OptionButton(Frame parent, MultiController controllers) 
            : base(parent)
        {
            ItemController = controllers;
            Initialize();
        }

        void Initialize()
        {
            m_ignorechildAdded = true;
            m_leftArrow = new Button(this, Item.ArrowLeft, Item.DefaultArrowLeftHighlight);
            m_leftArrow.HorizontalAlignment = HorizontalAlignmentType.Left;
            m_leftArrow.Depth = 0;

            m_rightArrow = new Button(this, Item.ArrowRight, Item.DefaultArrowRightHighlight);
            m_rightArrow.HorizontalAlignment = HorizontalAlignmentType.Right;
            m_rightArrow.Depth = 0;
            m_ignorechildAdded = false;

            OnLeft += delegate() { KeepFocus = true; CurrentIndex--; };
            OnRight += delegate() { KeepFocus = true; CurrentIndex++; };
        }

        #endregion

        #region Class Functions

        internal override void childAdded(Item mi)
        {
            if (m_ignorechildAdded)
                return;

            base.childAdded(mi);
        }

        internal override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix parentTransform) 
        {
            if (!Visible)
                return;

            Matrix m = CombineMatrix(AnimationTransform(gameTime), ref parentTransform);

            base.Draw(gameTime, spriteBatch, parentTransform);

            m_leftArrow.Draw(gameTime, spriteBatch, m);
            m_rightArrow.Draw(gameTime, spriteBatch, m);
        }

        #endregion
    }
}


