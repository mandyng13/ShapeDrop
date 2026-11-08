using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace GameLib.Physics;

/// <summary>
/// Detects Collisions amongst a set of Colliders
/// </summary>
public static class CollisionDetection
{
    /// <summary>
    /// Returns the normal of the collision point of two colliding AABBs. Check beforehand that the boxes actually
    /// collide. If they don't an exception will be thrown. 
    /// </summary>
    /// <param name="incMin">Minimum coordinates of the incident collider's AABB.</param>
    /// <param name="refMin">Minimum coordinates of the reference collider's AABB.</param>
    /// <param name="incMax">Maximum coordinates of the incident collider's AABB.</param>
    /// <param name="refMax">Maximum coordinates of the reference collider's AABB.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static Vector2 GetAABBContactNormal(Vector2 incMin, Vector2 refMin, Vector2 incMax, Vector2 refMax)
    {
        if (incMin.X > incMax.X) throw new ArgumentException("incMin.X must be smaller than incMax.X");
        if (refMin.X > refMax.X) throw new ArgumentException("refMin.X must be smaller than refMax.X");
        if (incMin.Y > incMax.Y) throw new ArgumentException("incMin.Y must be smaller than incMax.Y");
        if (refMin.Y > refMax.Y) throw new ArgumentException("refMin.Y must be smaller than refMax.Y");
        
        if (!TryGetAxisOverlapDistance(incMin.X, refMin.X, incMax.X, refMax.X, out var dx))
            throw new ArgumentException("X Values have no overlap!");

        if (!TryGetAxisOverlapDistance(incMin.Y, refMin.Y, incMax.Y, refMax.Y, out var dy))
            throw new ArgumentException("Y Values have no overlap!");

        return MathF.Abs(dx) < MathF.Abs(dy) ? new Vector2(MathF.Sign(dx), 0) : new Vector2(0, MathF.Sign(dy));
    }

    private static bool TryGetAxisOverlapDistance(float minA, float minB, float maxA, float maxB, out float overlapDistance)
    {
        if (maxA >= minB && maxA < maxB)
        {
            overlapDistance = minB - maxA;
            return true;
        }
        
        if (minA <= maxB && minA > minB)
        {
            overlapDistance = maxB - minA;
            return true;
        }
        
        overlapDistance = 0;
        return false;
    }

    public static List<(Collider A, Collider B)> BroadPhase()
    {
        var sortedColliders = PhysicsWorld.ActiveColliders
            .OrderBy(c => c.GetAABB().Min.X)
            .ToList();

        List<(Collider A, Collider B)> collisionPairs = [];

        for (int i = 0; i < sortedColliders.Count; i++)
        {
            var a = sortedColliders[i];
            var aabbA = a.GetAABB();

            for (int j = i + 1; j < sortedColliders.Count; j++)
            {
                var b = sortedColliders[j];
                var aabbB = b.GetAABB();

                if (aabbB.Min.X > aabbA.Max.X)
                    break;
                
                if (aabbA.Min.Y <= aabbB.Max.Y && aabbA.Max.Y >= aabbB.Min.Y)
                {
                    collisionPairs.Add((a, b));
                }
            }
        }
        
        return collisionPairs;
    }
}