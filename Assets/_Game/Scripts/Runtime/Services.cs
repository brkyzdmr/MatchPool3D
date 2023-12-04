using System;
using System.Collections.Generic;

public static class Services
{
    private static readonly Dictionary<Type, object> _services = new Dictionary<Type, object>();

    public static T GetService<T>()
    {
        var type = typeof(T);
        if (!_services.TryGetValue(type, out var service))
        {
            throw new InvalidOperationException($"Service {type.Name} not found.");
        }
        return (T)service;
    }

    public static void RegisterService<T>(T service)
    {
        var type = typeof(T);
        _services[type] = service;
    }

    public static void UnregisterService<T>()
    {
        var type = typeof(T);
        _services.Remove(type);
    }
}