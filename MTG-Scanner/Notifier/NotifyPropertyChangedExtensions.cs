using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace MTG_Scanner.Notifier
{
    public static class NotifyPropertyChangedExtensions
    {
        /// <summary>
        /// Verifies that a property name exists in a given class. This method
        /// can be called before the property is used, for instance before
        /// calling RaisePropertyChanged. It avoids errors when a property name
        /// is changed but some places are missed.
        ///   <para>This method is only active in DEBUG mode.</para>
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="propertyName">The name of the property to check.</param>
        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public static void VerifyPropertyName(this INotifyPropertyChanged source, string propertyName)
        {
            // null or empty for the property string means all properties have changed.
            if (string.IsNullOrEmpty(propertyName))
                return;

            if (source.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic |
                                                             BindingFlags.Instance | BindingFlags.Static) == null)
                throw new ArgumentException(@"Property not found", propertyName);
        }
    }
}
