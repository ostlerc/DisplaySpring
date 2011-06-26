using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace DisplaySpring.Menus
{
    /// <summary>
    /// A MenuItemCollection is a list of items that when added parents and children are handled for the user
    /// </summary>
    public class MenuItemCollection : Collection<MenuItem>
    {
        internal MenuItemCollection(MenuItem parent) { m_parent = parent; }

        protected override void ClearItems()
        {
            foreach (var s in Items)
                s.Parent = null;
            base.ClearItems();
        }

        internal List<MenuItem> visibleItems()
        {
            List<MenuItem> visibles = new List<MenuItem>();

            foreach (var v in Items)
                if (v.Visible)
                    visibles.Add(v);

            return visibles;
        }

        /*internal float MeasureWidth()
        {
            float left = 0, right = 0;
            foreach (var v in Items)
            {
                if (v.Position.X - v.MeasureWidth / 2 < left || left == 0)
                    left = v.Position.X- v.MeasureWidth / 2;

                if (v.Position.X + v.MeasureWidth / 2 > right || right == 0)
                    right = v.Position.X + v.MeasureWidth / 2;
            }

            if (Math.Abs(left) > Math.Abs(right))
                return Math.Abs(left * 2);
            else
                return Math.Abs(right * 2);
        }

        internal float MeasureHeight()
        {
            float top = 0f, bot = 0f;

            foreach (var v in Items)
            {
                if (v.Position.Y - v.MeasureHeight / 2 < top || top == 0)
                    top = v.Position.Y- v.MeasureHeight / 2;

                if (v.Position.Y + v.MeasureHeight / 2 > bot || bot == 0)
                    bot = v.Position.Y + v.MeasureHeight / 2;
            }

            if (Math.Abs(bot) > Math.Abs(top))
                return Math.Abs(bot * 2);
            else
                return Math.Abs(top * 2);
        }*/

        protected override void InsertItem(int index, MenuItem item)
        {
            base.InsertItem(index, item);

            if (m_parent is MenuFrame)
            {
                MenuFrame mf = m_parent as MenuFrame;
                mf.childAdded(item);
            }
        }

        protected override void RemoveItem(int index)
        {
            if (Items[index] is MenuFrame)
                (Items[index] as MenuFrame).childRemoved(Items[index]);

            Items[index].Parent = null;
            base.RemoveItem(index);
        }

        protected override void SetItem(int index, MenuItem item)
        {
            MenuItem oldItem = this[index];

            item.Parent = m_parent;
            base.SetItem(index, item);

            if (m_parent is MenuFrame)
            {
                MenuFrame mf = m_parent as MenuFrame;
                mf.childRemoved(oldItem);
                mf.childAdded(item);
            }
        }
        
        internal MenuItem m_parent;
    }
}
