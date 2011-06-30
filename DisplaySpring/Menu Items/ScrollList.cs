namespace DisplaySpring
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// A class to make scrollable viewable lists of Items
    /// </summary>
    public class ScrollList : Item
    {
        private delegate bool BaseAction();

        #region Member variables

        /// <summary>
        /// Orientation of the ScrollList
        /// </summary>
        public enum Orientation
        {
            /// <summary>
            /// Items will be layed out horizontally
            /// </summary>
            Horizontal,

            /// <summary>
            /// Items will be layed out Vertically
            /// </summary>
            Vertical
        }

        /// <summary>
        /// Defines on which side of the list the scroll bar will appear
        /// Note: For Horizontal, Left signifies Top and Right signifies Bottom
        /// </summary>
        public enum ScrollBarPosition
        {
            /// <summary>
            /// ScrolLBar will be on the left side of the ScrollList
            /// </summary>
            Left,

            /// <summary>
            /// ScrolLBar will be on the right side of the ScrollList
            /// </summary>
            Right
        };

        internal int m_viewCount;
        internal int m_selectedIndex = -1;
        internal int m_startIndex = 0;
        internal float m_spacing = 20f;
        internal float m_scrollSpacing = 35f;
        internal bool m_wrap = true;
        internal bool m_ignoreEvents = false; 

        /// <summary>
        /// Delegate that comes with a index parameter
        /// </summary>
        public delegate void MenuActionInt(int idx);

        /// <summary>
        /// This is a delegate that is invoked when the index of the ScrollList changes
        /// </summary>
        public MenuActionInt IndexChanged;

        internal ScrollBar m_scroll;
        internal List<Item> m_items = new List<Item>();
        internal Orientation m_direction = Orientation.Vertical;
        internal ScrollBarPosition m_scrollPosition = ScrollBarPosition.Right;
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

        /// <summary>
        /// The currently selected index
        /// </summary>
        public int SelectedIndex { get { return m_selectedIndex; } set { m_selectedIndex = value; } }

        internal int StartIndex { get { return m_startIndex; } }

        /// <summary>
        /// Stores the orientation of the object
        /// </summary>
        public Orientation Direction
        {
            get { return m_direction; }
            set 
            { 
                m_direction = value;
                UpdateScrollBar(true);
            }
        }

        /// <summary>
        /// ScrollBarPosition for the ScrollList's ScrollBar
        /// </summary>
        public ScrollBarPosition ScrollPosition
        {
            get { return m_scrollPosition; }
            set { m_scrollPosition = value; UpdateScrollBar(true); forceRefresh(); }
        }

        private void CreateScrollBar()
        {
            int dir = m_scrollPosition == ScrollBarPosition.Left ? 1 : -1;

            if (m_scroll != null)
            {
                Children.Remove(m_scroll);
                m_scroll = null;
            }

            m_ignoreEvents = true;
            if (m_direction == Orientation.Horizontal)
            {
                m_scroll = new ScrollBar(this, (int)StaticWidth, 20, 0, 0)
                {
                    Position = Vector2.UnitY * ((StaticHeight / 2) + 25) * dir,
                };
            }
            else
            {
                m_scroll = new ScrollBar(this, 20, (int)StaticHeight, 0, 0)
                {
                    Position = -Vector2.UnitX * ((StaticWidth / 2) - 25) * dir,
                };
            }
            m_ignoreEvents = false;

            m_scroll.VisibleCount = m_viewCount;
        }

        /// <summary>
        /// Items in the ScrollList
        /// </summary>
        public List<Item> Items { get { return m_items; } }

        /// <summary>
        /// Boolean toggle to define if the container wraps at the ends of the list or does not.
        /// </summary>
        public virtual bool Wrap { get { return m_wrap; } set { m_wrap = value; } }

        /// <summary>
        /// Gets or sets the focus for the menu item
        /// On set to true the OnFocus delegate will be called and FocusSound will play
        /// </summary>
        public override bool Focus
        {
            get { return base.Focus; }
            set
            {
                if(m_selectedIndex >= 0)
                    m_items[m_selectedIndex].Focus = value;

                base.Focus = value;
            }
        }

        /// <summary>
        /// If set to true, object will keep Focus in an invoke cycle
        /// This property resets itself to false at the end of the cycle
        /// </summary>
        public override bool  KeepFocus
        {
            get { return base.KeepFocus; }
            set
            {
                if(m_selectedIndex >= 0)
                    m_items[m_selectedIndex].KeepFocus = value;

                base.KeepFocus = value;
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
                List<Item> vb = visibleItems();

                foreach( Item item in vb)
                {
                    if (m_direction == Orientation.Horizontal)
                        h = Math.Max(h, item.MeasureHeight);
                    else
                        h += item.MeasureHeight;
                }


                if (m_direction == Orientation.Horizontal)
                    h += ScrollSpacing + m_scroll.Height;
                else
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
                List<Item> items = visibleItems();

                foreach( Item item in items)
                {
                    if (m_direction == Orientation.Vertical)
                        w = Math.Max(w, item.MeasureWidth);
                    else
                        w += item.MeasureWidth;
                }

                if (m_direction == Orientation.Vertical)
                    w += ScrollSpacing + m_scroll.Width;
                else
                    w += Spacing * (items.Count - 1) * Scale.X;

                return Math.Max(w,0);
            }
        }

        internal override float StaticHeight
        {
            get 
            {
                if (m_items.Count == 0)
                    return 0;

                float h = 0;
                List<Item> vb = visibleItems();

                foreach( Item item in vb)
                {
                    if (m_direction == Orientation.Horizontal)
                        h = Math.Max(h, item.StaticHeight);
                    else
                        h += item.StaticHeight;
                }

                if (m_direction == Orientation.Horizontal)
                    return h;

                h += Spacing * (vb.Count - 1);

                return Math.Max(h,0);
            }
        }

        internal override float StaticWidth
        {
            get
            {
                if (m_items.Count == 0)
                    return 0;

                float w = 0;
                List<Item> items = visibleItems();

                foreach( Item item in items)
                {
                    if (m_direction == Orientation.Vertical)
                        w = Math.Max(w, item.StaticWidth);
                    else
                        w += item.StaticWidth;
                }

                if (m_direction == Orientation.Vertical)
                    return w;

                w += Spacing * (items.Count - 1);

                return Math.Max(w,0);
            }
        }

        /// <summary>
        /// Amount of spacing in pixels between objects
        /// </summary>
        public virtual float Spacing 
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
        public Item SelectedItem
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
        /// <summary>
        /// Create an empty scroll list that can only show up to 'viewCount' items at one time
        /// without needing a ScrollBar present.
        /// </summary>
        public ScrollList(Item parent, MultiController c, int viewCount)
            : base(parent, c)
        {
            Initialize(viewCount);
        }

        /// <summary>
        /// Create and empty ScrollList
        /// </summary>
        public ScrollList(Item parent, MultiController c)
            : base(parent, c)
        {
            Initialize(-1);
        }

        private void Initialize(int viewCount)
        {
            m_viewCount = viewCount;
            CreateScrollBar();
            FocusSound = null; //we don't want the container to play the sound for focus, only the individual items
            m_animation = AnimateType.None;
        }
        #endregion

        internal void Insert(int index, Item item)
        {
            if (item.ItemController == null && ItemController != null)
                item.ItemController = ItemController;

            if (m_selectedIndex == -1)
            {
                m_selectedIndex = nextVisible(-1);
                m_startIndex = nextVisible(-1);
                item.Enabled = true;
            }

            m_items.Insert(index, item);
            if (m_viewCount != -1)
            {
                UpdateScrollBar(true);
            }
        }

        internal void AddItem(Item item)
        {
            Insert(m_items.Count, item);
        }

        internal void RemoveItem(Item item)
        {
            int index = m_items.IndexOf(item);

            if (index >= m_selectedIndex)
                m_selectedIndex--;

            m_items.Remove(item);
            if (m_viewCount != -1)
            {
                UpdateScrollBar(true);
            }
        }

        internal void RemoveAt(int index)
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
            m_scroll.VisibleCount = Math.Min(m_viewCount, visibleItems);
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
                    m_scroll.Width = (int)MeasureWidth;
                    m_scroll.Height = 20;
                    m_scroll.Position = new Vector2(0, (MeasureHeight / 2)*dir);
                }
                else
                {
                    m_scroll.Height = (int)MeasureHeight;
                    m_scroll.Width = 20;
                    m_scroll.Position = -new Vector2((MeasureWidth / 2) *dir, 0);
                }
            }
        }

        /// <summary>
        /// Reset the ScrollList to a fresh state
        /// </summary>
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

        #region Movement functions
        internal override bool Right()
        {
            if (m_direction == Orientation.Horizontal)
                return IncrementIndex(base.Right, OnRight);

            return base.Right();
        }

        internal override bool Left()
        {
            if (m_direction == Orientation.Horizontal)
                return PreviousIndex(base.Left, OnLeft);

            return base.Left();
        }

        internal override bool Down()
        {
            if (m_direction == Orientation.Vertical)
                return IncrementIndex(base.Down, OnDown);

            return base.Down();
        }

        internal override bool Up()
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

        private List<Item> visibleItems()
        {
            return visibleButtons(m_startIndex);
        }

        private List<Item> visibleButtons(int startIndex)
        {
            List<Item> showingList = new List<Item>();
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
            if (!Visible)
                return;

            float offset;
            Matrix local = Item.CombineMatrix(AnimationTransform(gameTime), ref parentTransform);

            if (m_direction == Orientation.Horizontal)
                offset = -MeasureWidth / 2f;
            else
                offset = -MeasureHeight / 2f;

            foreach (Item item in visibleItems())
            {
                switch (m_direction)
                {
                    case Orientation.Horizontal:
                        offset += item.MeasureWidth / 2f;
                        item.Position = new Vector2(offset, 0);
                        offset += item.MeasureWidth / 2f;
                        offset += Spacing * Scale.X;
                        break;
                    case Orientation.Vertical:
                    default:
                        offset += item.MeasureHeight / 2f;
                        item.Position = new Vector2(0, offset);
                        offset += item.MeasureHeight / 2f;
                        offset += Spacing * Scale.Y;
                        break;
                }

                item.Draw(gameTime, spriteBatch, local);
            }

            m_scroll.Draw(gameTime, spriteBatch, local);
        }

        internal override void childAdded(Item mi)
        {
            if (!m_ignoreEvents)
                AddItem(mi);

            base.childAdded(mi);
        }
    }
}