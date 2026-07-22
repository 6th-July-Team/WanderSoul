using System;
using System.ComponentModel;

public abstract class BaseModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public abstract void PropertyChangedOnInit();
}

public interface ContainerPropertyChanged<T>
{
    event Action<string, ContainerEventType, T> ContainerPropertyChanged;
}

public enum ContainerEventType
{
    Add,
    Remove,
    Update
}