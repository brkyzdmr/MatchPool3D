using System;
using UnityEngine;

public class Pools : MonoBehaviour
{
    public enum Types
    {
        Example = 5,
    }

    public static string GetTypeToString(Types poolType)
    {
        return Enum.GetName(typeof(Types), poolType);
    }
}