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
    public class OptionList : StackedItem
    {
        #region Member Variables
        private Sprite m_leftArrow;
        private Sprite m_rightArrow;
        private float m_highlightTimer = -1;
        private bool m_arrowsOut = false;
        #endregion

        #region Properties

        /// <summary>
        /// Defines if the arrows for the option button are outside the items boundaries
        /// </summary>
        public bool ArrowsOut { get { return m_arrowsOut; } set { m_arrowsOut = value; refreshArrows(); } }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a StackedItem with parents size.
        /// </summary>
        public OptionList(Item parent, MultiController controllers) 
            : base(parent)
        {
            Animation = AnimateType.None;
            ItemController = controllers;
            Initialize();
        }

        /// <summary>
        /// Create a StackedItem with parents size.
        /// </summary>
        public OptionList(Item parent) 
            : base(parent)
        {
            Initialize();
        }

        void Initialize()
        {
            m_leftArrow = new Sprite(null, Item.ArrowLeft);
            m_leftArrow.HorizontalAlignment = HorizontalAlignmentType.Left;
            m_leftArrow.Depth = Depth - .01f;

            m_rightArrow = new Sprite(null, Item.ArrowRight);
            m_rightArrow.HorizontalAlignment = HorizontalAlignmentType.Right;
            m_rightArrow.Depth = Depth - .01f;
        }

        #endregion

        #region Class Functions

        /// <summary>
        /// Updates the Item
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (m_highlightTimer == 0)
            {
                m_highlightTimer = -1;
                m_leftArrow.Enabled = false;
                m_rightArrow.Enabled = false;
                m_leftArrow.Texture  = Item.ArrowLeft;
                m_rightArrow.Texture = Item.ArrowRight;
            }
            else if (m_highlightTimer > 0)
            {
                m_highlightTimer = Math.Max(m_highlightTimer - gameTime.ElapsedGameTime.Milliseconds, 0);
            }

            base.Update(gameTime);
        }

        internal override bool Left()
        {
            int oldIndex = CurrentIndex;
            CurrentIndex--;
            if (oldIndex != CurrentIndex)
            {
                m_leftArrow.Enabled = true;
                m_leftArrow.Texture = Item.DefaultArrowLeftHighlight;
                m_highlightTimer = 90;
            }
            return base.Left();
        }

        internal override bool Right()
        {
            int oldIndex = CurrentIndex;
            CurrentIndex++;
            if (oldIndex != CurrentIndex)
            {
                m_rightArrow.Enabled = true;
                m_rightArrow.Texture = Item.DefaultArrowRightHighlight;
                m_highlightTimer = 90;
            }

            return base.Right();
        }

        public override float Width
        {
            get
            {
                float arrowWidth = 0;
                if (ArrowsOut)
                {
                    if (m_leftArrow != null)
                        arrowWidth += m_leftArrow.MeasureWidth;

                    if (m_rightArrow != null)
                        arrowWidth += m_rightArrow.MeasureWidth;
                }
                return base.Width + arrowWidth;
            }
        }

        public override float Height
        {
            get
            {
                float height = 0;

                if (m_leftArrow != null)
                    height = m_leftArrow.MeasureHeight;

                if (m_rightArrow != null)
                    height = Math.Max(m_rightArrow.MeasureHeight, height);

                return Math.Max(base.Height, height);
            }
        }

        internal void refreshArrows()
        {
            Item item = CurrentItem();
            if (item != null)
            {
                m_leftArrow.Parent = item;
                m_leftArrow.LayoutWidth = item.Width;
                m_leftArrow.LayoutHeight = item.Height;

                m_rightArrow.Parent = item;
                m_rightArrow.LayoutHeight = item.Height;
                m_rightArrow.LayoutWidth = item.Width;
                forceRefresh();
            }

            if (ArrowsOut)
            {
                m_leftArrow.LayoutPosition = new Vector2(-m_leftArrow.Width * 1.08f, 0);
                m_rightArrow.LayoutPosition = new Vector2(m_rightArrow.Width * 1.08f, 0);
                forceRefresh();
            }
        }

        /// <summary>
        /// Reset the Item to a fresh state
        /// </summary>
        public override void Reset(bool isFocus)
        {
            base.Reset(isFocus);

            refreshArrows();

            if(m_leftArrow != null)
                m_leftArrow.Reset(false);

            if (m_rightArrow != null)
                m_rightArrow.Reset(false);
        }

        internal override void childAdded(Item mi)
        {
            refreshArrows();
            base.childAdded(mi);
        }

        /// <summary>
        /// Sets the current item for the stack
        /// </summary>
        public override void SetCurrentItem(Item item)
        {
            refreshArrows();

            base.SetCurrentItem(item);
        }

        #endregion
    }
}


