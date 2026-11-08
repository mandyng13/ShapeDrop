using GameLib.Engine;
using Microsoft.Xna.Framework;

namespace GameLib.Physics;

/// <summary>
/// Component that represents a gameobjects physical presence in the world.
/// Does not define size or bounds of the object. 
/// Handles Mass and Force Application.
/// </summary>
/// <param name="Mass"></param>
public class RigidBody : Component
{
    public Vector2 Velocity { get; set; }
    
    public override void Connect(GameObject gameObject)
    {
        PhysicsWorld.RigidBodies.Add(this);
        base.Connect(gameObject);
    }
    
    public void Update(double deltaTime)
    {
       
    }

    public override void Destroy()
    {
        PhysicsWorld.RigidBodies.Remove(this);
    }
}