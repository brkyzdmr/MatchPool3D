using System;
using UnityEngine;

public class Pools : MonoBehaviour
{
    public enum Types
    {
        MergeExplosionParticle = 1,
        GoldExplosionParticle = 2
    }

    public static string GetTypeToString(Types poolType)
    {
        return Enum.GetName(typeof(Types), poolType);
    }
}