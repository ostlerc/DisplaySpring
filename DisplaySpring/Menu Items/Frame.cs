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
        private SizeType m_sizePolicy = SizeType.Maximum;
        private bool m_lockRefresh = false;
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
        internal Vector2 ForceSize
        {
            get { return m_forceSize; }
            set { m_forceSize = value; }
        }

        /// <summary>
        /// Sets the fixed size for the Frame.
        /// </summary>
        public Vector2 FixedSize
        {
            get { return m_fixedSize; }
            set { m_fixedSize = value; }
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
            /// Horizontally layout Children using layoutstretch for guidance
            /// </summary>
            Horizontal,

            /// <summary>
            /// Vertically layout Children
            /// </summary>
            Vertical,
        };

        /// <summary>
        /// Determines how much space to use when children are layed out.
        /// </summary>
        public enum SizeType
        {
            /// <summary>
            /// Use all space inside of Frame to lay out children.
            /// LayoutStretch applies to this type.
            /// Alignments do not work with this type.
            /// </summary>
            Maximum,

            /// <summary>
            /// Use minimal space. This is calculated only by Width, Height, and Padding of Children.
            /// Alignments work with This type.
            /// </summary>
            Minimum
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

                if (m_fixedSize != Vector2.Zero)
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

                if (m_fixedSize != Vector2.Zero)
                    return m_fixedSize.X;

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

            switch (m_sizePolicy)
            {
                case SizeType.Minimum:
                    if(Layout != LayoutType.None)
                        layoutMinimum();
                    break;
                case SizeType.Maximum:
                    layoutMaximum();
                    break;
            }

            base.refreshItem();

            m_lockRefresh = false;
        }

        private void layoutMinimum()
        {
            float width = 0;
            float height = 0;

            foreach (var v in Children)
            {
                switch (Layout)
                {
                    case LayoutType.Horizontal:
                        float tWidth = v.StaticWidth + Padding * 2;
                        v.LayoutPosition = new Vector2(width + tWidth / 2, 0);
                        width += tWidth;
                        height = Math.Max(height, v.StaticHeight);
                        break;
                    case LayoutType.Vertical:
                        float tHeight = v.StaticHeight + Padding * 2;
                        v.LayoutPosition = new Vector2(0, height + tHeight / 2);
                        height += tHeight;
                        width = Math.Max(width, v.StaticWidth);
                        break;
                }
            }

            switch (Layout)
            {
                case LayoutType.Horizontal:
                    width -= Padding * 2;
                    break;
                case LayoutType.Vertical:
                    height -= Padding * 2;
                    break;
            }

            if (Parent != null)
            {
                m_fixedSize.X = width;
                m_fixedSize.Y = height;
            }

            foreach (var v in Children)
            {
                v.Height = StaticHeight;
                v.Width = StaticWidth;
                switch(Layout)
                {
                    case LayoutType.Horizontal:
                        v.LayoutPosition -= new Vector2(width/2 + Padding, 0);
                        break;
                    case LayoutType.Vertical:
                        v.LayoutPosition -= new Vector2(0, height/2 + Padding);
                        break;
                }
            }
        }

        private void layoutMaximum()
        {
            uint total = 0;
            float pos = 0;
            Vector2 dimensions = StaticSize;

            foreach (var v in Children)
                total += v.LayoutStretch;

            foreach (var v in Children)
            {
                if (v.LayoutStretch == 0)
                {
                    v.LayoutPosition = Vector2.Zero;
                    v.Width = StaticWidth;
                    v.Height = StaticHeight;
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
