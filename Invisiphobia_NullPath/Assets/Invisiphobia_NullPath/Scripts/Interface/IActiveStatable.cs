using System;

public interface IActiveStatable<T> where T : Enum
{
    public event Action BasicStateEvent;
    public event Action ActiveStateEvent;

    public event Action<T> ShotEvent;
}