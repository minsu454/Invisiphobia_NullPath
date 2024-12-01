using System;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEditor.MapEditor;
using UnityEditorInternal;
using UnityEngine;

public sealed class ComstomMapEditorController
{
    public event Action leftMouseDownEvent;
    public event Action leftMouseUpEvent;

    public event Action rightMouseDownEvent;
    public event Action rightMouseUpEvent;

    public Event GetEvent()
    {
        int controlId = GUIUtility.GetControlID(FocusType.Passive);
        HandleUtility.AddDefaultControl(controlId);

        return Event.current;
    }

    public void InputMouse(Event e)
    {
        if (e.button == 0)
        {
            MouseEvent(e.type, leftMouseDownEvent, leftMouseUpEvent);
        }
        else if (e.button == 1)
        {
            MouseEvent(e.type, rightMouseDownEvent, rightMouseUpEvent);
        }
    }

    private void MouseEvent(EventType eventType, Action mouseDown, Action mouseUp)
    {
        switch (eventType)
        {
            case EventType.MouseDown:
                Debug.Log("MouseDown");
                mouseDown?.Invoke();
                break;
            case EventType.MouseDrag:

                break;
            case EventType.MouseUp:
                Debug.Log("MouseUp");
                mouseUp?.Invoke();
                break;
        }
        InternalEditorUtility.RepaintAllViews();
    }
}