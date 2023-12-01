using Entitas;
using UnityEngine;
using UnityEngine.EventSystems;

public sealed class InputSystem : IExecuteSystem
{
    readonly Contexts _contexts;

    Vector2 _startTouchPosition;
    bool _hasSwipeStarted;
    private Vector3 direction;

    public InputSystem(Contexts contexts)
    {
        _contexts = contexts;
    }

    public void Execute()
    {
        if(_contexts.input.isInputBlock)
            return;

        EmitInput();
    }

    void EmitInput()
    {
        if (Input.GetMouseButtonDown(0) && !CheckOnUI())
        {
            _startTouchPosition = Input.mousePosition;
            _hasSwipeStarted = true;
        }

        if (Input.GetMouseButton(0) && _hasSwipeStarted)
        {
            var swipeDirection = (Vector2)Input.mousePosition - _startTouchPosition;
            direction = new Vector3(swipeDirection.x / Screen.width, 0, swipeDirection.y / Screen.height);
            _startTouchPosition = Input.mousePosition;
            Debug.Log("Input - Swipe Started!");
        }

        if (Input.GetMouseButtonUp(0) && _hasSwipeStarted)
        {
            // _contexts.game.ReplacePlayerMoveData(Vector3.zero, Quaternion.identity);
            _hasSwipeStarted = false;
            Debug.Log("Input - Swipe Ended!");
        }
    }

    bool CheckOnUI()
    {
        if (Application.isMobilePlatform)
        {
            return !IsOnUI(0);
        }

        return IsOnUI();
    }

    bool IsOnUI(int fingerId = -1)
    {
        if (EventSystem.current != null)
        {
            if (fingerId == -1)
            {
                return EventSystem.current.IsPointerOverGameObject();
            }

            return EventSystem.current.IsPointerOverGameObject(fingerId);
        }

        return false;
    }
}
