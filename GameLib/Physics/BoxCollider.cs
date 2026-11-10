using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace GameLib.Physics;

public class BoxCollider : PolygonCollider
{
    public Vector2 Position => GameObject.Position;
    public Vector2 Size { get; set; }

    public override List<Vector2> GetCorners()
    {
        
        var corners = new List<Vector2>
        {
            new(-Size.X / 2, -Size.Y / 2),
            new(Size.X / 2, -Size.Y / 2),
            new(Size.X / 2, Size.Y / 2),
            new(-Size.X / 2, Size.Y / 2)
        };

        foreach (var corner in corners)
        {
            corner.Rotate(GameObject.Rotation);
        }

        return corners
            .Select(corner => corner + Position)
            .ToList();
    }

    public override AABB GetAABB()
    {
        var corners = GetCorners();

        float minX = corners[0].X;
        float maxX = corners[0].X;
        float minY = corners[0].Y;
        float maxY = corners[0].Y;

        foreach (var corner in corners)
        {
            minX = Math.Min(minX, corner.X);
            maxX = Math.Max(maxX, corner.X);
            minY = Math.Min(minY, corner.Y);
            maxY = Math.Max(maxY, corner.Y);
        }

        return new AABB
        {
            Min = new Vector2(minX, minY),
            Max = new Vector2(maxX, maxY),
        };
    }
}