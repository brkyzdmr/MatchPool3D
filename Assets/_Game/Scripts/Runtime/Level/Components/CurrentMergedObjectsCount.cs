﻿using Entitas;
using Entitas.CodeGeneration.Attributes;

[Unique, Event(EventTarget.Any)]
public class CurrentMergedObjectsCount : IComponent
{
    public int Value;
}
