using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace AnimationEditor
{
    public class SortableObservableCollection<T> : ObservableCollection<T>
    {
        public void Sort<TKey>(Func<T, TKey> keySelector)
        {
            InternalSort(Items.OrderBy(keySelector));
        }

        public void Sort<TKey1, TKey2>(Func<T, TKey1> keySelector1, Func<T, TKey2> keySelector2)
        {
            InternalSort(Items.OrderBy(keySelector1).ThenBy(keySelector2));
        }

        private void InternalSort(IEnumerable<T> sortedItems)
        {
            var sortedItemsList = sortedItems.ToList();

            foreach(var item in sortedItemsList)
            {
                Move(IndexOf(item), sortedItemsList.IndexOf(item));
            }
        }
    }
}
