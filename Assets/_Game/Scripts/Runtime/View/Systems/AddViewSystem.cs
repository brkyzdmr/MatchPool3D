﻿using System.Collections.Generic;
using Entitas;
using UnityEngine;
using static GameMatcher;

public sealed class AddViewSystem : ReactiveSystem<GameEntity>
{
    readonly Transform _parent;
    Contexts _contexts;
    public AddViewSystem(Contexts contexts) : base(contexts.game)
    {
        _parent = new GameObject("Views").transform;
        _contexts = contexts;
    }

    protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context) =>
        context.CreateCollector(Asset);

    protected override bool Filter(GameEntity entity) => entity.hasAsset && !entity.hasView;

    protected override void Execute(List<GameEntity> entities)
    {
        foreach (var e in entities)
            e.AddView(InstantiateView(e));
    }

    IView InstantiateView(GameEntity entity)
    {
        var prefab = Resources.Load<GameObject>(entity.asset.Value);
        var view = Object.Instantiate(prefab, _parent).GetComponent<IView>();
        view.Link(_contexts,entity);
        return view;
    }
}
