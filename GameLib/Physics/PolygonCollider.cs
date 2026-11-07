using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GameLib.Physics;

public abstract class PolygonCollider : Collider
{
    public abstract List<Vector2> GetCorners();
}