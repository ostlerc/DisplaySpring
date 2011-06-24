﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GMA.Menus
{
    /// <summary>
    /// A collection of MenuItems that are linked to each other in a parent child relationship
    /// </summary>
    public class MenuFrame : MenuItem
    {
        /// <summary>
        /// Items controlled by this Frame. Each item's position is relative to the frames
        /// </summary>
        internal MenuItemCollection Items
        {
            get { return m_menuItemCollection; }
            set { m_menuItemCollection = value; }
        }
        private LayoutType m_layout = LayoutType.None;
        public LayoutType Layout
        {
            get { return m_layout; }
            set 
            { 
                m_layout = value;

                if(Parent != null)
                    Parent.refreshItem();
            }
        }

        public enum LayoutType
        {
            None,
            Horizontal,
            Vertical
        };

        public override float MeasureWidth
        {
            get 
            { 
                if(Width == 0)
                    return Children.MeasureWidth();
                return Width;
            }
        }

        public override float MeasureHeight
        {
            get 
            {
                if(Height == 0)
                    return Children.MeasureHeight();
                return Height;
            }
        }

        internal override float StaticHeight
        {
            get { return Height; }
        }

        internal override float StaticWidth
        {
            get { return Width; }
        }

        /// <summary>
        /// Default constructor, sets position to Vector2.Zero
        /// </summary>
        public MenuFrame(MenuItem parent) : base(parent)
        {
            Width = parent.Width;
            Height = parent.Height;
        }

        /// <summary>
        /// Create a menu frame with specified bounds.
        /// This is only for the menu class to use as the super parent node
        /// </summary>
        internal MenuFrame(Rectangle bounds) : base()
        {
            Width = bounds.Width;
            Height = bounds.Height;
            Position = new Vector2(bounds.Center.X, bounds.Center.Y);
        }

        internal override void Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix transform) 
        {
            foreach (MenuItem child in Children)
                child.Draw(gameTime, spriteBatch, MenuItem.CombineMatrix(ItemTransform, ref transform));
        }

        private bool _lockRefresh = false;
        #region Layout helpers
        internal override void refreshItem()
        {
            if (_lockRefresh)
                return;

            _lockRefresh = true;
            uint total = 0;
            float pos = 0;

            foreach (var v in Items)
            {
                total += v.LayoutStretch;
            }

            Vector2 dimensions = Size;

            switch (m_layout)
            {
                case LayoutType.Horizontal:
                    foreach (var v in Items)
                    {
                        if (v.LayoutStretch == 0)
                            continue;

                        float percentage = (float)v.LayoutStretch / (float)total;
                        float width = percentage * dimensions.X;

                        v.Position = new Vector2((width - dimensions.X) / 2 + pos, 0);
                        v.Width = width;
                        v.Height = dimensions.Y;
                        v.refreshItem();

                        pos += width;
                    }
                    break;
                case LayoutType.Vertical:
                    break;
                case LayoutType.None:
                default:
                    break;
            }

            _lockRefresh = false;
        }
        internal void childAdded(MenuItem mi)
        {
            refreshItem();
        }
        internal void childRemoved(MenuItem mi)
        {
            refreshItem();
        }
        #endregion
    }
}
