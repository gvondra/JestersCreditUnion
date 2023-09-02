using System;
using System.Windows;
using System.Windows.Media;

namespace JCU.Internal
{
    public static class ParentFinder
    {
        public static DependencyObject Find(Type type, DependencyObject target)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(target);
            if (parent != null && !parent.GetType().Equals(type))
            {
                parent = Find(type, parent);
            }
            return parent;
        }
    }
}
