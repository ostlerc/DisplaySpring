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
    public class StackedItem : Frame
    {
        #region Member Variables
        private int m_currentIndex = -1;
        #endregion

        #region Properties

        internal override void childAdded(Item mi)
        {
            if (m_currentIndex == -1)
            {
                mi.Visible = true;
                m_currentIndex = 0;
            }
            else if (CurrentItem() == mi)
                SetCurrentItem(mi);
            else
                mi.Visible = false;

            base.childAdded(mi);
        }

        /// <summary>
        /// Height of the item. Layout space is not included. Scale is not included.
        /// </summary>
        public virtual int CurrentIndex 
        {
            get { return m_currentIndex; }
            set 
            {
                if (value < Children.Count && value >= 0)
                {
                    m_currentIndex = value;
                    SetCurrentItem(Children[m_currentIndex]);
                }
            }
        }

        /// <summary>
        /// Gets or sets the current Item list of the stack
        /// </summary>
        public virtual ItemCollection Items
        {
            get { return Children; }
            set { m_ItemCollection = value; }
        }

        /// <summary>
        /// Height of the item. Layout space is not included. Scale is not included.
        /// </summary>
        internal override float StaticHeight 
        { 
            get 
            {
                Item current = CurrentItem();

                if (current != null)
                    return current.StaticHeight;

                return Height;
            }
        } 

        /// <summary>
        /// Width of the item. Layout space is not included. Scale is not included.
        /// </summary>
        internal override float StaticWidth 
        { 
            get 
            { 
                Item current = CurrentItem();

                if (current != null)
                    return current.StaticWidth;

                return Width;
            } 
        }

        public override bool Focus
        {
            get { return base.Focus; }
            set
            {
                Item current = CurrentItem();

                if (current != null)
                    current.Enabled = value;

                base.Focus = value;
            }
        }

        public override bool Enabled
        {
            get { return base.Enabled; }
            set
            {
                Item current = CurrentItem();

                if (current != null)
                    current.Enabled = value;

                base.Enabled = value;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a StackedItem with parents size.
        /// </summary>
        public StackedItem(Item parent) 
            : base(parent)
        { }

        /// <summary>
        /// Create a StackedItem with given size.
        /// </summary>
        public StackedItem(Item parent, Vector2 size) 
            : base(parent, size)
        { }

        /// <summary>
        /// Create a menu frame with specified bounds.
        /// This is only for the menu class to use as the super parent of a Menu
        /// </summary>
        internal StackedItem(Rectangle bounds)
            : base(bounds)
        { }

        #endregion

        #region Class Functions

        internal override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix parentTransform) 
        {
            if (!Visible)
                return;

            if(CurrentItem() != null)
                CurrentItem().Draw(gameTime, spriteBatch, CombineMatrix(AnimationTransform(gameTime), ref parentTransform));
        }

        /// <summary>
        /// Sets the current item for the stack
        /// </summary>
        public virtual void SetCurrentItem(Item item)
        {
            if(!Children.Contains(item))
                return;

            m_currentIndex = Children.IndexOf(item);

            foreach (var v in Children)
            {
                v.Visible = false;
                v.Enabled = false;
            }

            item.Visible = true;
            item.Focus = Focus;
        }

        /// <summary>
        /// Get's the currently visible item
        /// </summary>
        public virtual Item CurrentItem()
        {
            if (m_currentIndex >= 0 && m_currentIndex < Children.Count)
                return Children[m_currentIndex];

            return null;
        }

        public override void Reset(bool isFocus)
        {
            Item current = CurrentItem();

            if (current != null)
                current.Reset(isFocus);

            base.Reset(isFocus);
        }

        #endregion
    }
}

