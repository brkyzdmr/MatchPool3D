
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
    public void LoadAsset(Contexts contexts, GameEntity entity, string assetName) {
        var viewObj = Object.Instantiate(Resources.Load<GameObject>($"Prefabs/{assetName}"), _root);

        if (viewObj == null) {
            throw new NullReferenceException($"Prefabs/{assetName} not found!");
        }

        var view = viewObj.GetComponent<IView>();
        if (view != null) {
            view.InitializeView(contexts, entity);
            entity.AddView(view);
        }

        AddColliderToEntity(viewObj, entity);
    }

    private void AddColliderToEntity(GameObject viewObj, GameEntity entity) {
        var boxCollider = viewObj.GetComponent<BoxCollider>();
        if (boxCollider != null) {
            entity.AddBoxCollider(boxCollider.center, boxCollider.size);
        }

        var sphereCollider = viewObj.GetComponent<SphereCollider>();
        if (sphereCollider != null) {
            entity.AddSphereCollider(sphereCollider.center, sphereCollider.radius);
        }
        
        // TODO: Additional Colliders
    }
}