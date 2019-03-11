using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityWeld.Binding;

[Binding]
public class DataBindObject : INotifyPropertyChanged
{
    public virtual event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Sets a property and notifies bindings to that property
    /// </summary>
    /// <param name="property">The variable underlying the property to be set</param>
    /// <param name="value">The value to set the property to</param>
    /// <param name="propertyName">The string name of the property</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>Returns true if the property has a new value</returns>
    protected bool SetProperty<T>(ref T property, T value, string propertyName)
    {
        var origProperty = property;
        try
        {
            if (!EqualityComparer<T>.Default.Equals(property, value))
            {
                property = value;
                OnPropertyChanged(propertyName);
                return true;
            }
            return false;
        }
        catch (Exception e)
        {
            property = origProperty;
            throw e;
        }
    }

    /// <summary>
    /// Notify for bindings to the property to update
    /// </summary>
    /// <param name="propertyName">The string name of the property</param>
    protected void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}