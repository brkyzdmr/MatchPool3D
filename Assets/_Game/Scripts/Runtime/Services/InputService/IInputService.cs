using UnityEngine;

public interface IInputService
{
    public bool GetMouseButtonDown(int button);
    public bool GetMouseButtonUp(int button);
    public Vector2 GetMousePosition();
    public bool IsPointerOverUI(int fingerId = -1);
}