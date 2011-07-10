namespace DisplaySpring
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// A collection of Items that can be handled layouts
    /// </summary>
    public class Frame : Item
    {
        #region Member Variables

        private LayoutType m_layout = LayoutType.None;
        private bool m_lockRefresh = false;
        private Vector2 m_forcedSize = Vector2.Zero;

        #endregion

        #region Properties

        /// <summary>
        /// Defines the LayoutType of the frame.
        /// </summary>
        /// <seealso cref="LayoutType"/>
        public LayoutType Layout
        {
            get { return m_layout; }
            set 
            {
                m_layout = value;
                forceRefresh();
            }
        }

        internal Vector2 ForcedSize
        {
            get { return m_forcedSize; }
            set { m_forcedSize = value; }
        }

        /// <summary>
        /// The layout type represents how the Children will be
        /// organized in the space the frame has.
        /// None is the default type
        /// </summary>
        public enum LayoutType
        {
            /// <summary>
            /// No layout, Children will not be arranged. This is the default
            /// </summary>
            None,

            /// <summary>
            /// Horizontally layout Children
            /// </summary>
            Horizontal,

            /// <summary>
            /// Vertically layout Children
            /// </summary>
            Vertical
        };

        /// <summary>
        /// Height of the item. Layout space is not included. Scale is not included.
        /// </summary>
        public override float StaticHeight 
        { 
            get 
            {
                if (m_forcedSize != Vector2.Zero)
                    return m_forcedSize.Y;

                return Height;
            }
        } 

        /// <summary>
        /// Width of the item. Layout space is not included. Scale is not included.
        /// </summary>
        public override float StaticWidth 
        {
            get 
            {
                if (m_forcedSize != Vector2.Zero)
                    return m_forcedSize.X;

                return Width;
            }
        }

        /// <summary>
        /// Position of the object. Default is Vector2.Zero, which is center based.
        /// </summary>
        public override Vector2 Position 
        { 
            get { return base.Position; } 
            set
            {
                base.Position = value;

                //This stops the frame from being able to change its position when in a layout.
                if (Layout != LayoutType.None)
                    forceRefresh();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a Frame with parents size.
        /// </summary>
        public Frame(Item parent) 
            : base(parent)
        { }

        /// <summary>
        /// Create a Frame with given size.
        /// </summary>
        public Frame(Item parent, Vector2 size) 
            : base(parent)
        {
            m_forcedSize = size;
        }

        /// <summary>
        /// Create a menu frame with specified bounds.
        /// This is only for the menu class to use as the super parent of a Menu
        /// </summary>
        internal Frame(Rectangle bounds)
            : base(null)
        {
            Width = bounds.Width;
            Height = bounds.Height;
            Position = new Vector2(bounds.Center.X, bounds.Center.Y);
        }

        #endregion

        #region Class Functions

        private float m_padding = 5;

        /// <summary>
        /// When layout stretch is zero, this changes the spacing in pixels between items
        /// </summary>
        public virtual float Padding
        {
            get { return m_padding; }
            set { m_padding = value; }
        }

        /// <summary>
        /// Used to know the stretch percentage of a Item.
        /// Default is 1. This number is taken relative to other Items in a layout.
        /// It determines the percentage of space given to the item.
        /// Example: a layout of two items with stretch 1 and 2 would receive 33% and 66%
        /// of the space respectively
        /// </summary>
        public override uint LayoutStretch
        {
            get
            {
                return base.LayoutStretch;
            }
            set
            {
                base.LayoutStretch = value;
            }
        }

        internal override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix parentTransform) 
        {
            if (!Visible)
                return;

            foreach (Item child in Children)
                child.Draw(gameTime, spriteBatch, CombineMatrix(AnimationTransform(gameTime), ref parentTransform));
        }

        #endregion

        #region Layout Helpers
        internal override void refreshItem()
        {
            //refreshItem can be called from within this function to this object again.
            //Don't allow this
            if (m_lockRefresh)
                return;

            m_lockRefresh = true;
            uint total = 0;
            float pos = 0;

            foreach (var v in Children)
                total += v.LayoutStretch;

            if (LayoutStretch == 0)
            {
                float width = 0;
                float height = 0;

                if (Parent != null)
                {
                    Width = Parent.StaticWidth;
                    Height = Parent.StaticHeight;
                }

                foreach (var v in Children)
                {
                    switch(Layout)
                    {
                        case LayoutType.Horizontal:
                            width += v.StaticWidth + Padding*2;
                            height = Math.Max(height, v.StaticHeight);
                            break;
                        case LayoutType.Vertical:
                            height += v.StaticHeight + Padding*2;
                            width = Math.Max(width, v.StaticWidth);
                            break;
                    }
                }

                if (Layout != LayoutType.None)
                {
                    m_forcedSize.X = width;
                    m_forcedSize.Y = height;
                }
            }

            Vector2 dimensions = StaticSize;

            foreach (var v in Children)
            {
                if (v.LayoutStretch == 0)
                {
                    v.LayoutPosition = Vector2.Zero;
                    continue;
                }

                float percentage = (float)v.LayoutStretch / (float)total;

                switch (Layout)
                {
                    case LayoutType.Horizontal:
                        float width = percentage * dimensions.X;

                        v.LayoutPosition = new Vector2((width - dimensions.X) / 2 + pos, 0);
                        v.Width = width;
                        v.Height = dimensions.Y;

                        pos += width;
                        break;
                    case LayoutType.Vertical:
                        float height = percentage * dimensions.Y;

                        v.LayoutPosition = new Vector2(0, (height - dimensions.Y) / 2 + pos);
                        v.Width = dimensions.X;
                        v.Height = height;

                        pos += height;
                        break;
                    case LayoutType.None:
                        v.Width = dimensions.X;
                        v.Height = dimensions.Y;
                        break;
                }
            }

            base.refreshItem();

            m_lockRefresh = false;
        }

        /// <summary>
        /// Reset the Item to a fresh state
        /// </summary>
        public override void Reset(bool isFocus)
        {
            refreshItem();
            base.Reset(isFocus);
        }

        #endregion
    }
}
