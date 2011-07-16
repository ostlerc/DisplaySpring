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
            set { m_forceSize = value; LayoutWidth = value.X; LayoutHeight = value.Y; refreshItem(); }
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
        public override float Height 
        { 
            get 
            {
                if (m_forceSize != Vector2.Zero)
                    return m_forceSize.Y;

                if (m_fixedSize.Y != 0)
                    return m_fixedSize.Y;

                return LayoutHeight;
            }
        } 

        /// <summary>
        /// Width of the item. Layout space is not included. Scale is not included.
        /// </summary>
        public override float Width 
        {
            get 
            {
                if (m_forceSize != Vector2.Zero)
                    return m_forceSize.X;

                if (m_fixedSize.X != 0)
                    return m_fixedSize.X;

                return LayoutWidth;
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
                dimensions = LayoutSize;

            float widthOrHeight = 0;

            foreach (var v in Children)
                total += v.LayoutStretch;

            foreach (var v in Children)
            {
                if (v.LayoutStretch == 0)
                {
                    v.LayoutWidth = LayoutWidth;
                    v.LayoutHeight = LayoutHeight;
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
                        v.LayoutWidth = width;
                        v.LayoutHeight = dimensions.Y;
                        widthOrHeight = Math.Max(widthOrHeight, v.Height);

                        pos += width;
                        break;
                    case LayoutType.VerticalShared:
                        float height = percentage * dimensions.Y;
                        widthOrHeight = Math.Max(widthOrHeight, v.Width);

                        v.LayoutPosition = new Vector2(0, (height - dimensions.Y) / 2 + pos);
                        v.LayoutWidth = dimensions.X;
                        v.LayoutHeight = height;

                        pos += height;
                        break;
                    case LayoutType.None:
                        if (SizePolicy != SizeType.Fixed)
                        {
                            v.LayoutWidth = LayoutWidth;
                            v.LayoutHeight = LayoutHeight;
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
                        m_fixedSize.X = Parent.LayoutWidth;
                        m_fixedSize.Y = Parent.LayoutHeight;
                    }
                    break;
            }

            foreach (var v in Children)
            {
                if (v.LayoutStretch == 0)
                    continue;

                switch (Layout)
                {
                    case LayoutType.Horizontal:
                        v.LayoutWidth = Width;
                        v.LayoutHeight = Height;
                        v.LayoutPosition -= new Vector2(dimensions.X/2 + Padding, 0);
                        break;

                    case LayoutType.Vertical:
                        v.LayoutWidth = Width;
                        v.LayoutHeight = Height;
                        v.LayoutPosition -= new Vector2(0, dimensions.Y/2 + Padding);
                        break;
                    case LayoutType.None:
                        v.LayoutPosition = Vector2.Zero;
                        if(SizePolicy == SizeType.Fixed)
                        {
                            v.LayoutWidth = m_fixedSize.X;
                            v.LayoutHeight = m_fixedSize.Y;
                        }
                        break;
                }
            }

            base.refreshItem();
        }

        #endregion
    }
}
