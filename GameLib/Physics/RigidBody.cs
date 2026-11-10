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
    public float Mass { get; set; }
    public Vector2 Force { get; set; }
    
    public override void Connect(GameObject gameObject)
    {
        PhysicsWorld.RigidBodies.Add(this);
        base.Connect(gameObject);
    }
    
    public void Update(double deltaTime)
    {
        AddForce(new Vector2(0, Mass * 9.81f));
        
        if (Mass <= 0)
            return;
        
        Vector2 acceleration = Force / Mass;
        
        GameObject.Position += Velocity * (float)deltaTime * 100f;
        
        Velocity += acceleration * (float)deltaTime;

        Force = Vector2.Zero;
    }

    public void AddForce(Vector2 force)
    {
        Force += force;
    }
    
    public void AddImpulse(Vector2 impulse)
    {
        if (Mass <= 0)
            return;
        
        Velocity += impulse / Mass;
    }

    public override void Destroy()
    {
        PhysicsWorld.RigidBodies.Remove(this);
    }
}