using UnityEngine;
using UnityEngine.Serialization;

public class Panel : MonoBehaviour
{
    public enum Type
    {
        None = 0,
        Fail = 1,
        Win = 2,
        Pause = 3,
        Shop = 4,
        Game = 5
    }

    public Type panelType;
    public bool isAlwaysPopup;
    [HideInInspector] public bool isCurrentlyPopup;
    [HideInInspector] public bool isCurrentlyOpen;
    public GameObject filterImage;
}