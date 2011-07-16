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
        private SizeType m_sizePolicy = SizeType.Minimum;
        private Vector2 m_fixedSize = Vector2.Zero;
        private Vector2 m_forceSize = Vector2.Zero;

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
            { m_layout = value; forceRefresh(); }
        }

        /// <summary>
        /// Changes the SizeType for the Frame. Maximum is the default.
        /// </summary>
        public SizeType SizePolicy
        {
            get { return m_sizePolicy; }
            set { m_sizePolicy = value; forceRefresh(); }
        }

        /// <summary>
        /// Sets the fixed size for the Frame.
        /// </summary>
        public Vector2 FixedSize
        {
            get { return m_fixedSize; }
            set { m_fixedSize = value; refreshItem(); }
        }

        internal Vector2 ForcedSize
        {
            get { return m_forceSize; }
            set { m_forceSize = value; Width = value.X; Height = value.Y; refreshItem(); }
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
            /// Horizontally layout Children by LayoutStretch
            /// </summary>
            HorizontalShared,

            /// <summary>
            /// Vertically layout Children by LayoutStretch
            /// </summary>
            VerticalShared,

            /// <summary>
            /// Layout children Horizontally, centered in frame
            /// </summary>
            Horizontal,

            /// <summary>
            /// Layout children Vertically, centered in frame
            /// </summary>
            Vertical
        };

        /// <summary>
        /// Determines how much space to use when children are layed out.
        /// </summary>
        public enum SizeType
        {
            /// <summary>
            /// Evenly distribute Frame boundaries between children
            /// LayoutStretch defines distribution ratios
            /// </summary>
            Minimum,

            /// <summary>
            /// Give children as much space as they want to claim
            /// </summary>
            Maximum,

            /// <summary>
            /// Use Fixed size
            /// </summary>
            Fixed
        }

        /// <summary>
        /// Height of the item. Layout space is not included. Scale is not included.
        /// </summary>
        public override float StaticHeight 
        { 
            get 
            {
                if (m_forceSize != Vector2.Zero)
                    return m_forceSize.Y;

                if (m_fixedSize.Y != 0)
                    return m_fixedSize.Y;

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
                if (m_forceSize != Vector2.Zero)
                    return m_forceSize.X;

                if (m_fixedSize.X != 0)
                    return m_fixedSize.X;

                return Width;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create an empty frame
        /// </summary>
        public Frame(Item parent) 
            : base(parent)
        { }

        /// <summary>
        /// Create a Frame with given fixed size.
        /// </summary>
        public Frame(Item parent, Vector2 size) 
            : base(parent)
        {
            m_sizePolicy = SizeType.Fixed;
            m_fixedSize = size;
        }

        /// <summary>
        /// Create a menu frame with specified bounds.
        /// This is only for the menu class to use as the super parent of a Menu
        /// </summary>
        internal Frame(Rectangle bounds)
            : base(null)
        {
            m_forceSize = new Vector2(bounds.Width, bounds.Height);
            LayoutPosition = new Vector2(bounds.Center.X, bounds.Center.Y);
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
            uint total = 0;
            float pos = 0;
            Vector2 dimensions = Vector2.Zero;

            if(Layout == LayoutType.HorizontalShared || Layout == LayoutType.VerticalShared )
                dimensions = Size;

            float widthOrHeight = 0;

            foreach (var v in Children)
                total += v.LayoutStretch;

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
                        float tWidth = v.MeasureWidth + Padding * 2;
                        v.LayoutPosition = new Vector2(dimensions.X + tWidth / 2, 0);
                        dimensions.X += tWidth;
                        dimensions.Y = Math.Max(dimensions.Y, v.MeasureHeight);
                        break;
                    case LayoutType.Vertical:
                        float tHeight = v.MeasureHeight + Padding * 2;
                        v.LayoutPosition = new Vector2(0, dimensions.Y + tHeight / 2);
                        dimensions.Y += tHeight;
                        dimensions.X = Math.Max(dimensions.X, v.MeasureWidth);
                        break;
                    case LayoutType.HorizontalShared:
                        float width = percentage * dimensions.X;

                        v.LayoutPosition = new Vector2((width - dimensions.X) / 2 + pos, 0);
                        v.Width = width;
                        v.Height = dimensions.Y;
                        widthOrHeight = Math.Max(widthOrHeight, v.StaticHeight);

                        pos += width;
                        break;
                    case LayoutType.VerticalShared:
                        float height = percentage * dimensions.Y;
                        widthOrHeight = Math.Max(widthOrHeight, v.StaticWidth);

                        v.LayoutPosition = new Vector2(0, (height - dimensions.Y) / 2 + pos);
                        v.Width = dimensions.X;
                        v.Height = height;

                        pos += height;
                        break;
                    case LayoutType.None:
                        if (SizePolicy != SizeType.Fixed)
                        {
                            v.Width = Width;
                            v.Height = Height;
                        }
                        break;
                }
            }

            switch (Layout)
            {
                case LayoutType.Horizontal:
                    dimensions.X -= Padding * 2;
                    break;
                case LayoutType.Vertical:
                    dimensions.Y -= Padding * 2;
                    break;
                case LayoutType.HorizontalShared:
                    dimensions.Y = widthOrHeight;
                    break;
                case LayoutType.VerticalShared:
                    dimensions.X = widthOrHeight;
                    break;
            }

            switch (SizePolicy)
            {
                case SizeType.Minimum:
                    m_fixedSize.X = dimensions.X;
                    m_fixedSize.Y = dimensions.Y;
                    break;
                case SizeType.Maximum:
                    if (Parent != null)
                    {
                        m_fixedSize.X = Parent.Width;
                        m_fixedSize.Y = Parent.Height;
                    }
                    break;
            }

            foreach (var v in Children)
            {
                switch (Layout)
                {
                    case LayoutType.Horizontal:
                        v.Width = StaticWidth;
                        v.Height = StaticHeight;
                        v.LayoutPosition -= new Vector2(dimensions.X/2 + Padding, 0);
                        break;

                    case LayoutType.Vertical:
                        v.Width = StaticWidth;
                        v.Height = StaticHeight;
                        v.LayoutPosition -= new Vector2(0, dimensions.Y/2 + Padding);
                        break;
                    case LayoutType.None:
                        v.LayoutPosition = Vector2.Zero;
                        if(SizePolicy == SizeType.Fixed)
                        {
                            v.Width = m_fixedSize.X;
                            v.Height = m_fixedSize.Y;
                        }
                        break;
                }
            }

            base.refreshItem();
        }

        private void layoutGreedy()
        {
            foreach (var v in Children)
            {
                v.Height = StaticHeight;
                v.Width = StaticWidth;

                switch(Layout)
                {
                    case LayoutType.Horizontal:
                        break;
                    case LayoutType.Vertical:
                        break;
                }
            }
        }

        #endregion
    }
}
