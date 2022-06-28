using System.Collections;
using System.Collections.Specialized;

namespace Caliburn.Light
{
    internal static class BindableCollectionHelper
    {
        public static void AreChildrenOf(this INotifyCollectionChanged children, object parentViewModel)
        {
            children.CollectionChanged += (s, e) =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        SetParent(e.NewItems, parentViewModel);
                        break;
                    case NotifyCollectionChangedAction.Remove:
                        SetParent(e.OldItems, null);
                        break;
                    case NotifyCollectionChangedAction.Replace:
                        SetParent(e.OldItems, null);
                        SetParent(e.NewItems, parentViewModel);
                        break;
                    case NotifyCollectionChangedAction.Reset:
                        SetParent((IList)s, parentViewModel);
                        break;
                }
            };
        }

        private static void SetParent(IList items, object parent)
        {
            foreach (var x in items)
            {
                if (x is IChild child)
                    child.Parent = parent;
            }
        }
    }
}
