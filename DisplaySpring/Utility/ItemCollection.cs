namespace DisplaySpring
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Collections.ObjectModel;

    /// <summary>
    /// A ItemCollection is a list of items that when added parents and children are handled for the user
    /// </summary>
    public class ItemCollection : Collection<Item>
    {
        internal ItemCollection(Item parent) { m_parent = parent; }

        protected override void ClearItems()
        {
            foreach (var s in Items)
                s.Parent = null;
            base.ClearItems();
        }

        internal List<Item> visibleItems()
        {
            List<Item> visibles = new List<Item>();

            foreach (var v in Items)
                if (v.Visible)
                    visibles.Add(v);

            return visibles;
        }

        protected override void InsertItem(int index, Item item)
        {
            base.InsertItem(index, item);

            if(m_parent != null)
                m_parent.childAdded(item);
        }

        protected override void RemoveItem(int index)
        {
            if (Items[index] != null)
                Items[index].childRemoved(Items[index]);

            Items[index].Parent = null;
            base.RemoveItem(index);
        }

        protected override void SetItem(int index, Item item)
        {
            Item oldItem = this[index];

            item.Parent = m_parent;
            base.SetItem(index, item);
        }
        
        internal Item m_parent;
    }
}
