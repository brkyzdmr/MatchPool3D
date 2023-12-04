using UnityEngine;

public class UnityInputService : Service, IInputService
{
    public UnityInputService(Contexts contexts) : base(contexts) { }

    public bool GetMouseButtonDown(int button)
    {
        return Input.GetMouseButtonDown(button);
    }

    public bool GetMouseButtonUp(int button)
    {
        return Input.GetMouseButtonUp(button);
    }

    public Vector2 GetMousePosition()
    {
        return Input.mousePosition;
    }

    public bool IsPointerOverUI(int fingerId = -1)
    {
        if (UnityEngine.EventSystems.EventSystem.current != null)
        {
            if (fingerId == -1)
            {
                return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
            }
            return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(fingerId);
        }
        return false;
    }
}