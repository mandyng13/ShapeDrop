using Microsoft.Xna.Framework;

namespace GameLib.Physics;

public static class CollisionSolver
{
    private static float GetSeparatingVelocity(RigidBody incident, Vector2 normal)
    { 
        return Vector2.Dot(incident.Velocity, normal);
    }

    private static bool IsSeparating(RigidBody incident, Vector2 normal)
    {
        float separatingVelocity = GetSeparatingVelocity(incident, normal);
        return separatingVelocity > 0f;
    }

    public static void ResolveCollision(Collider incident, Collider reference)
    {
        AABB incAABB = incident.GetAABB();
        AABB refAABB = reference.GetAABB();
        
        Vector2 normal = CollisionDetection.GetAABBContactNormal(incAABB.Min, refAABB.Min, incAABB.Max, refAABB.Max);

        if (IsSeparating(incident.RigidBody, normal))
            return;  
        
        float separatingVelocity = GetSeparatingVelocity(incident.RigidBody, normal);
        float elasticity = reference.Elasticity;
        
        Vector2 newVelocity = incident.RigidBody.Velocity - (1 + elasticity) * separatingVelocity * normal;
        
        incident.RigidBody.Velocity = newVelocity;
    }
}