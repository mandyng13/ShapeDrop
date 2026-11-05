using System.Collections.Generic;
using System.Numerics;

namespace GameLib.Physics;

public abstract class PolygonCollider : Collider
{
    public abstract List<Vector2> GetCorners();
}