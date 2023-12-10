using System.Numerics;
using Entitas;
using MoreMountains.NiceVibrations;
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
    private readonly IVibrationService _vibrationService;
    private Camera _mainCamera;

    public InputSystem(Contexts contexts)
    {
        _contexts = contexts;
        _inputService = Services.GetService<IInputService>();
        _vibrationService = Services.GetService<IVibrationService>();
        _mainCamera = Camera.main;
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
        if (Physics.Raycast(_mainCamera.ScreenPointToRay(screenPosition), out RaycastHit hit))
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
                _vibrationService.PlayHaptic(HapticTypes.SoftImpact);
            }
        }
    }

    private void UpdateDraggedObjectPosition(Vector2 screenPosition) 
    {
        if (_isDragging && _selectedEntity != null && _selectedEntity.isEnabled)
        {
            if (Physics.Raycast(_mainCamera.ScreenPointToRay(screenPosition), out RaycastHit hit, 1000))
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
        var direction = directionalVelocity.normalized;
        var magnitude = directionalVelocity.magnitude;
        var maxSpeed = _contexts.config.gameConfig.value.GameConfig.objectsMaxSpeed;
        
        magnitude = Mathf.Clamp(magnitude, 0, maxSpeed);
        var scaledVelocity = direction * magnitude;

        return new Vector3(scaledVelocity.x, 0, scaledVelocity.y);
    }
}
