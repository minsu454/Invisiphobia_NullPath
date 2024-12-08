using System;

public interface IActiveStatable
{
    public event Action BasicStateEvent;
    public event Action ActiveStateEvent;
}