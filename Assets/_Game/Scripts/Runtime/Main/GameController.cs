
using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Contexts _contexts;
    private Services _services;
    private UpdateSystems _updateSystems;
    private FixedUpdateSystems _fixedUpdateSystems;

    private void Awake()
    {
        _contexts = Contexts.sharedInstance;
        _services = new Services();
        
        Configure(_contexts);
        CreateServices(_contexts, _services);
        
        _updateSystems = new UpdateSystems(_contexts, _services);
        _fixedUpdateSystems = new FixedUpdateSystems(_contexts, _services);
        
        _updateSystems.Initialize();
        _fixedUpdateSystems.Initialize();
    }
    
    private void Update()
    {
        _updateSystems.Execute();
        _updateSystems.Cleanup();
    }

    private void FixedUpdate()
    {
        _fixedUpdateSystems.Execute();
        _fixedUpdateSystems.Cleanup();
    }

    private void OnDestroy()
    {
        _updateSystems.DeactivateReactiveSystems();
        _updateSystems.ClearReactiveSystems();
        _updateSystems.TearDown();

        _fixedUpdateSystems.DeactivateReactiveSystems();
        _fixedUpdateSystems.ClearReactiveSystems();
        _fixedUpdateSystems.TearDown();
    }

    private void CreateServices(Contexts contexts, Services services)
    {
        services.ViewService = new ViewService(contexts);
        services.ObjectProduceService = new ObjectProduceService(contexts);
        services.IdService = new IdService(contexts);
    }

    private void Configure(Contexts contexts)
    {
        // contexts.config.SetShopItems();
    }
}
