using Microsoft.Xna.Framework;

namespace GameLib.Physics;

public class CircleCollider : Collider
{
    public float Radius { get; set; }
    
    public override AABB GetAABB()
    {
        Vector2 position = GameObject.Position;

        return new AABB
        {
            Min = new Vector2(position.X - Radius, position.Y - Radius),
            Max = new Vector2(position.X + Radius, position.Y + Radius)
        };
    }
}