
using UnityEngine;

public class RigidbodyView : MonoBehaviour, IRigidbody
{
    public Vector2 Velocity
    {
        get
        {
            return _rigidbody.velocity;
        }
        set
        {
            _rigidbody.velocity = value;
        }
    }
    

    [SerializeField] private Rigidbody _rigidbody;

    public void AddForce(Vector3 force)
    {
        _rigidbody.AddForce(force, ForceMode.Force);
    }

    public void AddImpulse(Vector3 force)
    {
        var velocity = _rigidbody.velocity;
        velocity.y = 0f;
        _rigidbody.velocity = velocity;
        _rigidbody.AddForce(force, ForceMode.Impulse);    
    }
}
