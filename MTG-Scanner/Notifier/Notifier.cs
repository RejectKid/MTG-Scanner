using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace MTG_Scanner.Notifier
{
    /// <summary>
    /// An implementation of the observable pattern that implements
    /// the standard interfaces used by WPF to provide change notifications.
    /// </summary>
    public abstract class Notifier : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs after a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Determines whether the specified property has changed from a
        ///   <see cref="PropertyChanged" /> event.
        /// </summary>
        /// <param name="e">
        /// The <see cref="PropertyChangedEventArgs" /> instance containing the
        /// event data.
        /// </param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns>
        /// Returns <c>true</c> if the property has changed; otherwise, <c>false</c> is returned.
        /// </returns>
        public static bool HasPropertyChanged(PropertyChangedEventArgs e, string propertyName)
        {
            return string.IsNullOrEmpty(e.PropertyName) || e.PropertyName == propertyName;
        }

        /// <summary>
        /// Sets the specified observable field and raises the
        ///   <see cref="PropertyChanged" /> events if needed.
        /// </summary>
        /// <typeparam name="T">The type of the field.</typeparam>
        /// <param name="field">Reference to the field to set.</param>
        /// <param name="value">The new value for the field.</param>
        /// <param name="propertyName">Name of the property that is being set.</param>
        /// <returns>
        /// Returns <c>true</c> if the property was set; otherwise, returns <c>false</c>
        /// if the field wasn't set (due to a no-op set or cancellation by an event handler).
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "<unset>")
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            RaisePropertyChanged(propertyName);

            return true;
        }

        /// <summary>
        /// Raises the PropertyChanged event if needed.
        /// </summary>
        /// <remarks>
        /// If the propertyName parameter
        /// does not correspond to an existing property on the current class, an
        /// exception is thrown in DEBUG configuration only.
        /// </remarks>
        /// <param name="propertyName">The name of the property that changed.</param>
        [SuppressMessage("Microsoft.Design", "CA1030:UseEventsWhereAppropriate")]
        protected void RaisePropertyChanged(string propertyName)
        {
            this.VerifyPropertyName(propertyName);

            var handler = PropertyChanged;

            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

