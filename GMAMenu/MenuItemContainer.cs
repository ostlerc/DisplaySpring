using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GMA.Menus
{
    /// <summary>
    /// A class to make scrollable viewable lists of menuItems
    /// </summary>
    public class MenuItemContainer : MenuButton
    {
        private delegate bool BaseAction();

        #region Member variables

        public enum Orientation
        {
            Horizontal,
            Vertical
        }

        public enum ScrollBarPosition
        {
            Left,
            Right
        };

        protected int m_viewCount;
        protected int m_selectedIndex = -1;
        protected int m_startIndex = 0;
        protected float m_spacing = 20f;
        protected float m_scrollSpacing = 15f;
        protected bool m_wrap = true;
        public delegate void MenuActionInt(int idx);
        public MenuActionInt IndexChanged;
        protected ScrollBar m_scroll;
        protected List<MenuButton> m_items;
        protected Orientation m_direction = Orientation.Vertical;
        protected ScrollBarPosition m_scrollPosition = ScrollBarPosition.Right;
        #endregion

        #region Properties
        /// <summary>
        /// The max amount of items in the list to show. Items not visible do not count
        /// This is a scrolling functionality
        /// </summary>
        public int ViewCount 
        {
            get { return m_viewCount; } 
            set 
            { 
                if (value < 1)
                    return;

                m_viewCount = value;

                UpdateScrollBar(true);

                if (visibleButtons(m_startIndex).Count < value)
                {
                    int prev = previousVisible(m_startIndex);
                    if (prev != -1)
                    {
                        m_startIndex = prev;
                        UpdateScrollBar(true);
                    }
                }
                else
                    selectIndex(m_selectedIndex);
            }
        }
        public int SelectedIndex { get { return m_selectedIndex; } set { m_selectedIndex = value; } }
        public int StartIndex { get { return m_startIndex; } }

        public Orientation Direction
        {
            get { return m_direction; }
            set 
            { 
                m_direction = value;
                UpdateScrollBar(true);
            }
        }

        public ScrollBarPosition ScrollPosition
        {
            get { return m_scrollPosition; }
            set { m_scrollPosition = value; UpdateScrollBar(true); }
        }

        public override float Alpha
        {
            get { return base.Alpha; }
            set
            {
                foreach (MenuButton mb in m_items)
                    mb.Alpha = value;

                base.Alpha = value;
            }
        }

        public override float EndAlpha
        {
            get { return base.EndAlpha; }
            set
            {
                foreach (MenuButton mb in m_items)
                    mb.EndAlpha = value;

                base.EndAlpha = value;
            }
        }

        public override Color Tint
        {
            get { return base.Tint; }
            set
            {
                foreach (var v in m_items)
                    v.Tint = value;

                base.Tint = value;
            }
        }

        public override AnimateType Animation
        {
            get { return base.Animation; }
            set
            {
                foreach (MenuButton mb in m_items)
                    mb.Animation = value;

                base.Animation = value;
            }
        }

        public override SpriteFont Font
        {
            get { return base.Font; }
            set 
            { 
                foreach (MenuButton mb in m_items)
                    mb.Font = value;

                base.Font = value;
            }
        }

        public override Color FontColor
        {
            get { return base.FontColor; }
            set
            {
                foreach (MenuButton mb in m_items)
                    mb.FontColor = value;

                base.FontColor = value;
            }
        }

        public override Color fontFocusColor
        {
            get { return base.fontFocusColor; }
            set
            {
                foreach (MenuButton mb in m_items)
                    mb.fontFocusColor = value;

                base.fontFocusColor = value;
            }
        }

        public override float StartAlpha
        {
            get
            {
                return base.StartAlpha;
            }
            set
            {
                foreach (MenuButton mb in m_items)
                    mb.StartAlpha = value;

                base.StartAlpha = value;
            }
        }

        public override MultiController ItemController 
        {
            get { return base.ItemController; } 
            set 
            {
                foreach (MenuButton mb in m_items)
                    mb.ItemController = value;

                base.ItemController = value; 
            }
        }

        private void CreateScrollBar()
        {
            int dir = m_scrollPosition == ScrollBarPosition.Left ? 1 : -1;

            if (m_scroll != null)
                Children.Remove(m_scroll);

            if (m_direction == Orientation.Horizontal)
                m_scroll = new ScrollBar((int)MeasureWidth, 20, 0, 0)
                {
                    Position = Vector2.UnitY * ((MeasureHeight / 2) + 25) * dir,
                };

            else
                m_scroll = new ScrollBar(20, (int)MeasureHeight, 0, 0)
                {
                    Position = -Vector2.UnitX * ((MeasureWidth / 2) - 25) * dir,
                };

            m_scroll.VisibleCount = m_viewCount;
            Children.Add(m_scroll);
        }
        public List<MenuButton> Items { get { return m_items; } }

        /// <summary>
        /// Boolean toggle to define if the container wraps at the ends of the list or does not.
        /// </summary>
        public bool Wrap { get { return m_wrap; } set { m_wrap = value; } }
        public override bool Focus
        {
            get { return base.Focus; }
            set
            {
                m_items[m_selectedIndex].Focus = value;

                base.Focus = value;
            }
        }

        public override bool  KeepFocus
        {
            get
            {
                return base.KeepFocus;
            }
            set
            {
                m_items[m_selectedIndex].KeepFocus = value;
                base.KeepFocus = value;
            }
        }

        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                foreach (MenuButton mb in m_items)
                    mb.Visible = value;

                m_scroll.Visible = value;

                base.Visible = value;
            }
        }

        public override bool Enabled
        {
            get
            {
                return base.Enabled;
            }
            set
            {
                foreach (MenuButton mb in m_items)
                    mb.Enabled = value;

                m_scroll.Enabled = value;

                base.Enabled = value;
            }
        }
        /// <summary>
        /// total Height of all visible buttons
        /// if Width oritentation, Height of first available item is returned
        /// otherwise it is summed for all items including spacing
        /// </summary>
        public override float MeasureHeight
        { 
            get 
            {
                if (m_items.Count == 0)
                    return 0;

                float h = 0;
                List<MenuButton> vb = visibleButtons();

                foreach( MenuButton mb in vb)
                {
                    if (m_direction == Orientation.Horizontal)
                        h = Math.Max(h, mb.MeasureHeight * Scale.Y);
                    else
                        h += mb.MeasureHeight * Scale.Y;
                }

                if (m_direction == Orientation.Horizontal)
                    return h;

                h += Spacing * (vb.Count - 1) * Scale.Y;

                return Math.Max(h,0);
            }
        }

        /// <summary>
        /// Total width of all visible buttons
        /// if Vertical oritentation, width of first available item is returned
        /// otherwise it is summed for all items including spacing
        /// </summary>
        public override float MeasureWidth
        {
            get
            {
                if (m_items.Count == 0)
                    return 0;

                float w = 0;
                List<MenuButton> vb = visibleButtons();

                foreach( MenuButton mb in vb)
                {
                    if (m_direction == Orientation.Vertical)
                        w = Math.Max(w, mb.MeasureWidth * Scale.X);
                    else
                        w += mb.MeasureWidth * Scale.X;
                }

                if (m_direction == Orientation.Vertical)
                    return w;

                w += Spacing * (vb.Count - 1) * Scale.X;

                return Math.Max(w,0);
            }
        }

        public override Vector2 Position
        {
            get { return base.Position; }
            set 
            { 
                base.Position = value; 

                UpdateScrollBar(true);
            }
        }

        /// <summary>
        /// Amount of spacing in pixels between objects
        /// </summary>
        public float Spacing 
        { 
            get { return m_spacing; }
            set 
            { 
                m_spacing = value;
                UpdateScrollBar(true);
            }
        }

        /// <summary>
        /// Spacing between the scrollbar and the items in the list
        /// </summary>
        public float ScrollSpacing 
        { 
            get { return m_scrollSpacing; }
            set 
            { 
                m_scrollSpacing = value;
                UpdateScrollBar(true);
            }
        }
        /// <summary>
        /// gets selected item
        /// </summary>
        public MenuItem SelectedItem
        {
            get
            {
                if (m_selectedIndex < 0 || m_selectedIndex >= m_items.Count)
                    return null;

                return m_items[m_selectedIndex];
            }
        }
        #endregion

        #region Constructors
        public MenuItemContainer(MultiController c, int viewCount)
            : base(c, "")
        {
            Initialize(viewCount);
        }

        public MenuItemContainer(MultiController c)
            : base(c, "")
        {
            Initialize(-1);
        }

        private void Initialize(int viewCount)
        {
            m_viewCount = viewCount;
            m_items = new List<MenuButton>();
            CreateScrollBar();
            Direction = Orientation.Vertical;
            FocusSound = null; //we don't want the container to play the sound for focus, only the individual items
            m_animation = AnimateType.None;
        }
        #endregion

        public void Insert(int index, MenuButton mb)
        {
            if (mb.ItemController == null && ItemController != null)
                mb.ItemController = ItemController;

            if (m_selectedIndex == -1)
            {
                m_selectedIndex = nextVisible(0);
                m_startIndex = nextVisible(0);
                mb.Enabled = true;
            }

            m_items.Insert(index, mb);
            if (m_viewCount != -1)
            {
                UpdateScrollBar(true);
            }
        }

        public void AddItem(MenuButton mb)
        {
            Insert(m_items.Count, mb);
        }

        public void RemoveItem(MenuButton mb)
        {
            int index = m_items.IndexOf(mb);

            if (index >= m_selectedIndex)
                m_selectedIndex--;

            m_items.Remove(mb);
            if (m_viewCount != -1)
            {
                UpdateScrollBar(true);
            }
        }

        public void RemoveAt(int index)
        {
            m_items.RemoveAt(index);

            if (index >= m_selectedIndex)
                m_selectedIndex--;

            if (m_viewCount != -1)
            {
                UpdateScrollBar(true);
            }
        }

        private void UpdateScrollBar(bool resize)
        {
            if (m_items.Count < 1)
                return;

            int visibleItems = 0;
            for (int x = 0; x < m_items.Count; x++)
                if ( m_items[x].Visible )
                    visibleItems++;

            m_scroll.ObjectCount = visibleItems;
            m_scroll.VisibleCount = m_viewCount;
            int invis = 0;
            for (int x = 0; x < m_startIndex; x++)
                if (!m_items[x].Visible)
                    invis++;

            m_scroll.SelectedIndex = m_startIndex - invis;

            if (resize)
            {
                int dir = m_scrollPosition == ScrollBarPosition.Left ? 1 : -1;

                if (Direction == Orientation.Horizontal)
                {
                    m_scroll.SetWidth((int)MeasureWidth);
                    m_scroll.SetHeight(20);
                    m_scroll.Position = new Vector2(0, ((MeasureHeight / 2) + m_scrollSpacing)*dir);
                }
                else
                {
                    m_scroll.SetHeight((int)MeasureHeight);
                    m_scroll.SetWidth(20);
                    m_scroll.Position = -new Vector2(((MeasureWidth / 2) + m_scrollSpacing)*dir, 0);
                }
            }
        }

        public override void Reset(bool isFocus)
        {
            for ( int x = 1; x < m_items.Count; x ++)
                m_items[x].Reset(false);

            if (m_items.Count > 0)
                m_items[0].Reset(isFocus);

            m_startIndex = 0;
            m_scroll.SelectedIndex = 0;
            m_selectedIndex = 0;
            for ( int x = 0; x < m_items.Count; x++)
                if (m_items[x].Visible)
                {
                    m_selectedIndex = x;
                    break;
                }

            base.Reset(isFocus);

            UpdateScrollBar(true);
        }

        public override float TextScale
        {
            get { return base.TextScale; }
            set
            {
                base.TextScale = value;
            }
        }

        public override Vector2 Scale //do nothing special for this. scale is passed on to children automatically
        {
            get { return base.Scale; }
            set { base.Scale = value; UpdateScrollBar(true); }
        }

        #region Movement functions
        protected override bool Right()
        {
            if (m_direction == Orientation.Horizontal)
                return IncrementIndex(base.Right, OnRight);

            return base.Right();
        }

        protected override bool Left()
        {
            if (m_direction == Orientation.Horizontal)
                return PreviousIndex(base.Left, OnLeft);

            return base.Left();
        }

        protected override bool Down()
        {
            if (m_direction == Orientation.Vertical)
                return IncrementIndex(base.Down, OnDown);

            return base.Down();
        }

        protected override bool Up()
        {
            if (m_direction == Orientation.Vertical)
                return PreviousIndex(base.Up, OnUp);

            return base.Up();
        }

        private bool IncrementIndex(BaseAction ba, MenuAction ma)
        {
            //case that we don't want to roll around but instead perform ba()
            if (m_selectedIndex + 1 == m_items.Count && (ma != null || m_selectedIndex == -1))
                return ba();

            setIndexForward();

            return false;
        }

        private bool PreviousIndex(BaseAction ba, MenuAction ma)
        {
            //case that we don't want to roll around but instead perform ba()
            if (m_selectedIndex - 1 == -1 && (ma != null || m_selectedIndex == -1))
                return ba();

            setIndexReverse();

            return false;
        }

        private void setIndexForward()
        {
            if (m_selectedIndex == -1)
                return;

            int startIndex = nextVisible(m_selectedIndex);

            if (startIndex == m_items.Count)
            {
                if (!m_wrap) //don't wrap around
                    return;

                startIndex = nextVisible(-1);

                if (startIndex == m_items.Count)
                    return;
            }

            selectIndex(startIndex);
        }

        //this functions assumes m_startIndex is in a previous state
        //it determines if this start index needs to change given new index
        private void updateStartIndex()
        {
            int vcount = 0;
            for (int x = m_startIndex; x < m_items.Count && vcount < m_scroll.VisibleCount; x++)//make sure we need to change index
            {
                if (x == m_selectedIndex)
                {
                    if (vcount < m_scroll.VisibleCount)
                        return;

                    break;
                }

                if (m_items[x].Visible)
                    vcount++;
            }

            if (m_selectedIndex < m_startIndex)
            {
                m_startIndex = m_selectedIndex;
                return;
            }

            m_startIndex = m_selectedIndex;

            for (int past = 0; past < m_scroll.VisibleCount - 1 && m_startIndex > 0; past++)
                while (--m_startIndex > 0 && !m_items[m_startIndex].Visible) ; //find previous item that is visible
        }

        private void setIndexReverse()
        {
            if (m_selectedIndex == -1)
                return;

            int startIndex = previousVisible(m_selectedIndex);

            if (startIndex == -1)
            {
                if (!m_wrap) //don't wrap around
                    return;

                startIndex = previousVisible(m_items.Count);

                if (startIndex == -1)
                    return;
            }

            selectIndex(startIndex);
        }

        private void selectIndex(int index)
        {
            int oldIndex = m_selectedIndex;
            if (index != m_selectedIndex)
            {
                m_items[m_selectedIndex].Focus = false;
                m_selectedIndex = index;
                m_items[index].Focus = true;
            }

            if(IndexChanged != null)
                IndexChanged(oldIndex);

            if (m_scroll.VisibleCount > 0)
            {
                updateStartIndex();
                UpdateScrollBar(true);
            }
        }

        private int previousVisible(int from)
        {
            while (--from >= 0 && !m_items[from].Visible) ; //find previous item that is visible
            return from;
        }

        private int nextVisible(int from)
        {
            while (++from < m_items.Count && !m_items[from].Visible) ; //find next item that is visible
            return from;
        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            foreach( MenuButton mb in m_items)
                mb.Update(gameTime);

            base.Update(gameTime);
        }

        private List<MenuButton> visibleButtons()
        {
            return visibleButtons(m_startIndex);
        }

        private List<MenuButton> visibleButtons(int startIndex)
        {
            List<MenuButton> showingList = new List<MenuButton>();
            int shown = 0;
            for (int x = startIndex; (shown < m_scroll.VisibleCount || m_scroll.VisibleCount == -1) && x < m_items.Count; x++)
            {
                if (m_items[x].Visible)
                {
                    shown++;
                    showingList.Add(m_items[x]);
                }
            }

            return showingList;
        }

        internal override void  Draw(GameTime gameTime, SpriteBatch spriteBatch, Matrix parentTransform)
        {
            float offset;
            Matrix local = MenuItem.CombineMatrix(ItemTransform, ref parentTransform);

            if (m_direction == Orientation.Horizontal)
                offset = -MeasureWidth / 2f;
            else
                offset = -MeasureHeight / 2f;

            foreach (MenuButton mb in visibleButtons())
            {
                switch (m_direction)
                {
                    case Orientation.Horizontal:
                        offset += mb.MeasureWidth / 2f * Scale.X;
                        mb.Position = new Vector2(offset, 0);
                        offset += mb.MeasureWidth / 2f * Scale.X;
                        offset += Spacing * Scale.X;
                        break;
                    case Orientation.Vertical:
                    default:
                        offset += mb.MeasureHeight / 2f * Scale.Y;
                        mb.Position = new Vector2(0, offset);
                        offset += mb.MeasureHeight / 2f * Scale.Y;
                        offset += Spacing * Scale.Y;
                        break;
                }

                mb.Draw(gameTime, spriteBatch, local);
            }

            base.Draw(gameTime, spriteBatch, local);
        }
    }
}
