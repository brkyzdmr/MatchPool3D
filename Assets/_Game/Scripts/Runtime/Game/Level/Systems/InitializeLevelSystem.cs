using System.Collections.Generic;
using Entitas;
using UnityEngine;

public sealed class InitializeLevelSystem :IInitializeSystem
{
    private readonly Contexts _contexts;
    private Transform _playAreaContainer;
    private readonly ILevelService _levelService;
    private readonly ISaveService _saveService;

    public InitializeLevelSystem(Contexts contexts)
    {
        _contexts = contexts;
        _levelService = Services.GetService<ILevelService>();
        _saveService = Services.GetService<ISaveService>();
    }

    public void Initialize()
    {
        _saveService.Load();
        _levelService.SetupLevel();
    }
}
