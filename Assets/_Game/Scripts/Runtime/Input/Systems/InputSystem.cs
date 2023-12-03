using System.Numerics;
using Entitas;
using Entitas.Unity;
using UnityEngine;
using UnityEngine.EventSystems;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public sealed class InputSystem : IExecuteSystem
{
    readonly Contexts _contexts;

    private Vector2 _startTouchPosition;
    private Vector2 _endTouchPosition;
    private Vector3 _direction;
    
    private GameEntity _selectedEntity;
    private bool _isDragging;
    private ObjectView _selectedObjectView;
    private float _selectedObjectY;

    private Vector2 _previousFramePosition;

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
            TrySelectObject(Input.mousePosition);
        }

        if (_isDragging)
        {
            UpdateDraggedObjectPosition(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_isDragging)
            {
                StopDraggingObject();
            }
            
            _isDragging = false;
        }

        _previousFramePosition = Input.mousePosition; 
    }

    private void TrySelectObject(Vector2 screenPosition)
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        {
            _selectedObjectView = hit.collider.transform.GetComponentInParent<ObjectView>();
            
            if (_selectedObjectView == null || _selectedObjectView == null) { return; }

            if (_selectedObjectView.LinkedEntity.hasRigidbody)
            {
                var position = _selectedObjectView.transform.position;
                _selectedEntity = _selectedObjectView.LinkedEntity;
                _selectedEntity.ReplacePosition(position);
                _selectedObjectY = position.y + 1;
                _selectedEntity.ReplaceRigidbody(true, Vector3.zero);
                _isDragging = true;
            }
        }
    }

    private void UpdateDraggedObjectPosition(Vector2 screenPosition) 
    {
        if (_isDragging && _selectedEntity != null && _selectedEntity.isEnabled)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 1000))
            {
                var newPosition = new Vector3(hit.point.x, _selectedObjectY, hit.point.z);
                _selectedEntity.ReplacePosition(newPosition);
            }
        }
        else
        {
            _selectedEntity = null;
        }
    }


    private void StopDraggingObject() 
    {
        if (_selectedEntity != null)
        {
            _endTouchPosition = Input.mousePosition;
            var directionalVelocity = CalculateDirectionalVelocity();

            _selectedEntity.ReplaceRigidbody(false, directionalVelocity);
            _selectedEntity = null;
        }
    }

    private Vector3 CalculateDirectionalVelocity() 
    {
        var directionalVelocity = _endTouchPosition - _previousFramePosition;
    
        var speed = 10f; 
        directionalVelocity = directionalVelocity.normalized * speed;

        return new Vector3(directionalVelocity.x, 0, directionalVelocity.y);
    }

    bool CheckOnUI(int fingerId = -1)
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
