﻿namespace DisplaySpring
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
        private bool m_arrowsOut = false;
        #endregion

        #region Properties

        /// <summary>
        /// Defines if the arrows for the option button are outside the items boundaries
        /// </summary>
        public bool ArrowsOut { get { return m_arrowsOut; } set { m_arrowsOut = value; } }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a StackedItem with parents size.
        /// </summary>
        public OptionButton(Item parent, MultiController controllers) 
            : base(parent)
        {
            ItemController = controllers;
            Initialize();
        }

        /// <summary>
        /// Create a StackedItem with parents size.
        /// </summary>
        public OptionButton(Item parent) 
            : base(parent)
        {
            Initialize();
        }

        void Initialize()
        {
            m_leftArrow = new Button(null, Item.ArrowLeft, Item.DefaultArrowLeftHighlight);
            m_leftArrow.HorizontalAlignment = HorizontalAlignmentType.Left;
            m_leftArrow.Animation = AnimateType.None;

            m_rightArrow = new Button(null, Item.ArrowRight, Item.DefaultArrowRightHighlight);
            m_rightArrow.HorizontalAlignment = HorizontalAlignmentType.Right;
            m_rightArrow.Animation = AnimateType.None;
        }

        #endregion

        #region Class Functions

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
            int oldIndex = CurrentIndex;
            CurrentIndex--;
            if (oldIndex != CurrentIndex)
            {
                m_leftArrow.Enabled = true;
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
                m_highlightTimer = 90;
            }

            return base.Right();
        }

        internal override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix parentTransform)
        {
            Item cur = CurrentItem();

            m_leftArrow.Width = cur.StaticWidth;
            m_leftArrow.Height = cur.StaticHeight;

            m_rightArrow.Width = cur.StaticWidth;
            m_rightArrow.Height = cur.StaticHeight;

            base.Draw(gameTime, spriteBatch, parentTransform);
        }

        public override void SetCurrentItem(Item item)
        {
            m_leftArrow.Parent = item;
            m_leftArrow.Width = item.StaticWidth;
            m_leftArrow.Height = item.StaticHeight;
            if (ArrowsOut)
            {
                m_leftArrow.Position = new Vector2(m_leftArrow.Position.X - m_leftArrow.StaticWidth * 1.15f, m_leftArrow.Position.Y);
            }

            m_rightArrow.Parent = item;
            m_rightArrow.Width = item.StaticWidth;
            m_rightArrow.Height = item.StaticHeight;

            if (ArrowsOut)
            {
                m_rightArrow.Position = new Vector2(m_rightArrow.Position.X + m_rightArrow.StaticWidth * 1.15f, m_rightArrow.Position.Y);
            }

            base.SetCurrentItem(item);
        }

        #endregion
    }
}

