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
    public class StackedItem : Item
    {
        #region Member Variables
        private int m_currentIndex = -1;
        private List<Item> m_items;
        #endregion

        #region Properties

        internal override void childAdded(Item mi)
        {
            Items.Add(mi);

            base.childAdded(mi);

            if (m_currentIndex == -1)
            {
                mi.Visible = true;
                CurrentIndex = 0;
            }
            else if (CurrentItem() == mi)
                SetCurrentItem(mi);
            else
                mi.Visible = false;

            forceRefresh();
        }

        /// <summary>
        /// Height of the item. Layout space is not included. Scale is not included.
        /// </summary>
        public virtual int CurrentIndex 
        {
            get { return m_currentIndex; }
            set 
            {
                if (value < Items.Count && value >= 0)
                {
                    m_currentIndex = value;
                    SetCurrentItem(Items[m_currentIndex]);
                }
            }
        }

        /// <summary>
        /// Gets or sets the current Item list of the stack
        /// </summary>
        public virtual List<Item> Items
        {
            get { return m_items; }
            set { m_items = value; }
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
        public StackedItem(Frame parent) 
            : base(parent)
        {
            Initialize();
        }

        /// <summary>
        /// Create a StackedItem with given size.
        /// </summary>
        public StackedItem(Frame parent, Vector2 size) 
            : base(parent)
        {
            Width = size.X;
            Height = size.Y;
            Initialize();
        }

        void Initialize()
        {
            m_items = new List<Item>();
        }

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
            if(!Items.Contains(item))
                return;

            m_currentIndex = Items.IndexOf(item);

            Width = item.StaticWidth;
            Height = item.StaticHeight;

            foreach (var v in Items)
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
            if (m_currentIndex >= 0 && m_currentIndex < Items.Count)
                return Items[m_currentIndex];

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

