
using System.Collections.Generic;
using Entitas.CodeGeneration.Attributes;

[Config, Unique, ComponentName("ObjectsConfig")]
public interface IObjectsConfig
{
    ObjectsConfigData Config { get; }
}
