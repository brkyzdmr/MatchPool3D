using Entitas.Unity;
using UnityEngine;

public class View : MonoBehaviour, IView/*, IDestroyedListener*/
{
    private GameObject _gameObject;
    private Transform _transform;
    private GameEntity _entity;

    public void InitializeView(Contexts contexts, GameEntity entity)
    {
        _gameObject = gameObject;
        _transform = transform;
        _entity = entity;
        // _entity.AddDestroyedListener(this);
        //
        // Id = _entity.id.Value;
        
        gameObject.Link(_entity);
    }

    public void OnDestroyed(GameEntity entity)
    {

        gameObject.Unlink();
        Destroy(gameObject);
    }
    
    #region IView implementation
    public int Id { get; set; }
    
    public bool Enabled
    {
        get
        {
            return _gameObject.activeSelf;
        }
        set
        {
            _gameObject.SetActive(value);
        }
    }
    
    public Vector3 Position
    {
        get { return _transform.position;}
        set { _transform.position = value; }
    }
    
    public Transform Transform
    {
        get { return _transform; }
    }
    #endregion
}