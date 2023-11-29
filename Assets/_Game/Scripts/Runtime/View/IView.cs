using UnityEngine;

public interface IView
{
    bool Enabled { get; set; }
    int Id { get; set; }
    Vector3 Position { get; set; }
    Transform Transform { get; }
    
    void InitializeView(Contexts contexts, GameEntity entity);
}