using Microsoft.Xna.Framework;

namespace GameLib.Physics;

public static class CollisionSolver
{
    private static float GetSeparatingVelocity(RigidBody incident, RigidBody reference, Vector2 normal)
    {
        Vector2 relativeVelocity = incident.Velocity - reference.Velocity;
        
        return Vector2.Dot(relativeVelocity, normal);
    }

    private static bool IsSeparating(RigidBody incident, RigidBody reference, Vector2 normal)
    {
        float separatingVelocity = GetSeparatingVelocity(incident, reference, normal);
        return separatingVelocity > 0f;
    }

    public static void ResolveCollision(Collider incident, Collider reference)
    {
        AABB incAABB = incident.GetAABB();
        AABB refAABB = reference.GetAABB();
        
        Vector2 normal = CollisionDetection.GetAABBContactNormal(incAABB.Min, refAABB.Min, incAABB.Max, refAABB.Max);

        if (IsSeparating(incident.RigidBody, reference.RigidBody, normal))
            return;
        
        float separatingVelocity = GetSeparatingVelocity(incident.RigidBody, reference.RigidBody, normal);
        float elasticity = incident.Elasticity;
        
        Vector2 newVelocity = incident.RigidBody.Velocity - (1 + elasticity) * separatingVelocity * normal;
        
        incident.RigidBody.Velocity = newVelocity;
    }
}