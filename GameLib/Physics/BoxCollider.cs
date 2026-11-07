using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GameLib.Physics;

public class BoxCollider : PolygonCollider
{
    public Vector2 Position => GameObject.Position;
    public Vector2 Size { get; set; }

    public override List<Vector2> GetCorners()
    {
        
        return new List<Vector2>
        {
            new(Position.X - Size.X / 2, Position.Y - Size.Y / 2),
            new(Position.X + Size.X / 2, Position.Y - Size.Y / 2),
            new(Position.X + Size.X / 2, Position.Y + Size.Y / 2),
            new(Position.X - Size.X / 2, Position.Y + Size.Y / 2)
        };
    }

    public override AABB GetAABB()
    {
        return new AABB
        {
            Min = new Vector2(
                Position.X - Size.X / 2,
                Position.Y - Size.Y / 2
            ),
            Max = new Vector2(
                Position.X + Size.X / 2,
                Position.Y + Size.Y / 2
            )
        };
    }
}