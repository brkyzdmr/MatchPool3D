using System.Numerics;
using Entitas;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


public sealed class InputSystem : IExecuteSystem
{
    readonly Contexts _contexts;
    readonly IInputService _inputService;

    private Vector2 _startTouchPosition;
    private Vector2 _endTouchPosition;
    private Vector3 _direction;
    
    private GameEntity _selectedEntity;
    private bool _isDragging;
    private ObjectView _selectedObjectView;
    private float _selectedObjectY;

    private Vector2 _previousFramePosition;

    public InputSystem(Contexts contexts, Services services)
    {
        _contexts = contexts;
        _inputService = services.InputService;
    }

    public void Execute()
    {
        if(_contexts.input.isInputBlock) { return; }

        EmitInput();
    }

    private void EmitInput()
    {
        if (_inputService.GetMouseButtonDown(0) && !_inputService.IsPointerOverUI())
        {
            _startTouchPosition = _inputService.GetMousePosition();
            TrySelectObject(_inputService.GetMousePosition());
        }

        if (_isDragging)
        {
            UpdateDraggedObjectPosition(_inputService.GetMousePosition());
        }

        if (_inputService.GetMouseButtonUp(0))
        {
            if (_isDragging)
            {
                StopDraggingObject();
            }
            
            _isDragging = false;
        }

        _previousFramePosition = _inputService.GetMousePosition(); 
    }

    private void TrySelectObject(Vector2 screenPosition)
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(screenPosition), out RaycastHit hit))
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
            if (Physics.Raycast(Camera.main.ScreenPointToRay(screenPosition), out RaycastHit hit, 1000))
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
            _endTouchPosition = _inputService.GetMousePosition();
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
}
