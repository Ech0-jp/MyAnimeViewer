using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MyAnimeViewer.Utility
{
    public static class UIHelper
    {
        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            if (child == null)
                throw new ArgumentNullException("child", "This argument cannot be null.");

            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }

        public static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        { 
            if (parent == null)
                throw new ArgumentNullException("parent", "This argument cannot be null.");

            T foundChild = null;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                T childType = child as T;

                if (childType == null)
                {
                    foundChild = FindChild<T>(child, childName);
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        public static T FindChildOfType<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null)
                throw new ArgumentNullException("parent", "This argument cannot be null.");

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                var result = (child as T) ?? FindChildOfType<T>(child);
                if (result != null) return result;
            }
            return null;
        }

        public static List<T> FindChildrenOfType<T>(DependencyObject parent) where T : DependencyObject
        {
            if (parent == null)
                throw new ArgumentNullException("parent", "This argument cannot be null.");

            List<T> results = new List<T>();
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T)
                {
                    results.Add(child as T);
                }
                else
                {
                    var result = FindChildrenOfType<T>(child);
                    if (result != null)
                    {
                        foreach (var item in result)
                        {
                            results.Add(item);
                        }
                    }
                }
            }
            return results;
        }
    }
}
