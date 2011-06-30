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
        private Button m_leftArrow;
        private Button m_rightArrow;
        private float m_highlightTimer = -1;
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
            if (m_leftArrow == null)
            {
                m_leftArrow = new Button(null, Item.ArrowLeft, Item.DefaultArrowLeftHighlight);
                m_leftArrow.HorizontalAlignment = HorizontalAlignmentType.Left;
                m_leftArrow.Animation = AnimateType.None;

                m_rightArrow = new Button(null, Item.ArrowRight, Item.DefaultArrowRightHighlight);
                m_rightArrow.HorizontalAlignment = HorizontalAlignmentType.Right;
                m_rightArrow.Animation = AnimateType.None;
            }
        }

        #endregion

        #region Class Functions

        internal override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix parentTransform)
        {
            if (m_leftArrow.Parent == null || m_rightArrow.Parent == null)
            {
                Item cur = CurrentItem();

                m_leftArrow.Parent = cur;
                m_leftArrow.Width = cur.StaticWidth;
                m_leftArrow.Height = cur.StaticHeight;

                m_rightArrow.Parent = cur;
                m_rightArrow.Width = cur.StaticWidth;
                m_rightArrow.Height = cur.StaticHeight;
            }

            base.Draw(gameTime, spriteBatch, parentTransform);
        }

        public override void Update(GameTime gameTime)
        {
            if (m_highlightTimer == 0)
            {
                m_highlightTimer = -1;
                m_leftArrow.Enabled = false;
                m_rightArrow.Enabled = false;
            }
            else if (m_highlightTimer > 0)
            {
                m_highlightTimer = Math.Max(m_highlightTimer - gameTime.ElapsedGameTime.Milliseconds, 0);
            }

            base.Update(gameTime);
        }

        internal override bool Left()
        {
            CurrentIndex--;
            m_leftArrow.Enabled = true;
            m_highlightTimer = 60;
            return base.Left();
        }

        internal override bool Right()
        {
            CurrentIndex++;
            m_rightArrow.Enabled = true;
            m_highlightTimer = 60;
            return base.Right();
        }

        public override void SetCurrentItem(Item item)
        {
            if (m_leftArrow != null)
                m_leftArrow.Parent = null;

            if (m_rightArrow != null)
                m_rightArrow.Parent = null;

            base.SetCurrentItem(item);
        }

        internal override void  childAdded(Item mi)
        {
            base.childAdded(mi);
        }

        #endregion
    }
}


