using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public abstract class DataBindMonobehaviour : MonoBehaviour, INotifyPropertyChanged
{
    public virtual event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Sets a property and notifies bindings to that property
    /// </summary>
    /// <param name="property">The variable underlying the property to be set</param>
    /// <param name="value">The value to set the property to</param>
    /// <param name="propertyName">The string name of the property</param>
    /// <returns>Returns true if the property has a new value</returns>
    protected bool SetProperty<T>(ref T property, T value, string propertyName)
    {
        if (!EqualityComparer<T>.Default.Equals(property, value))
        {
            property = value;
            try
            {
                OnPropertyChanged(propertyName);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            return true;
        }
        return false;
    }

    /// <summary>
    /// Notify for bindings to the property to update
    /// </summary>
    /// <param name="propertyName">The string name of the property</param>
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
