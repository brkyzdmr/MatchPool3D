using System.Collections.Generic;
using UnityEngine;

namespace Udo.PoolManager
{
    public interface IObjectPoolService
    {
        public void InitializePools(List<ObjectPool> pools);
        public GameObject Spawn(Pools.Types poolType, Vector3? position = null, Quaternion? rotation = null, Transform parent = null);
        public void Despawn(Pools.Types poolType, GameObject obj);
    }
}