
using System;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class ViewService : Service, IViewService
{
    private readonly Transform _root;
    
    public ViewService(Contexts contexts) : base(contexts)
    {
        _root = new GameObject("ViewRoot").transform;
    }

    // TODO: Pooling system
    public void LoadAsset(Contexts contexts, GameEntity entity, string assetName)
    {
        var viewObj = Object.Instantiate(Resources.Load<GameObject>(string.Format("Prefabs/{0}", assetName)), _root);

        if (viewObj == null)
        {
            throw new NullReferenceException(string.Format("Prefabs/{0} not found!", assetName));
        }

        var rigidbody = viewObj.GetComponent<Rigidbody>();
        
        if (rigidbody != null)
        {
            entity.AddRigidbody(rigidbody);
        }

        var view = viewObj.GetComponent<IView>();
        if (view != null)
        {
            view.InitializeView(contexts, entity);
            entity.AddView(view);
        }
    }
}
