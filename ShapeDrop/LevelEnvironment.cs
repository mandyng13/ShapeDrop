using System.Collections.Generic;
using GameLib.Engine;
using GameLib.Physics;
using GameLib.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
// ReSharper disable NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract

namespace ShapeDrop;

public class LevelEnvironment
{
    private ContentManager _content;

    private GameObject.Template _yellowBoardTemplate3x1;
    private GameObject.Template _yellowBoardTemplate4x1;
    private GameObject.Template _sideBorderTemplate;
    private GameObject.Template _topBorderTemplate;
    private GameObject.Template _bounceBallTemplate;

    private const int TileSize = 40;
    private const float SpriteScale = 0.5f;
    
    public LevelEnvironment(ContentManager content)
    {
        _content = content;
        CreateTemplates();
    }
    
    public void Build()
    {
        _yellowBoardTemplate4x1.Instantiate(new Vector2(180, 200), MathHelper.ToRadians(45f));
        _yellowBoardTemplate4x1.Instantiate(new Vector2(420, 300), MathHelper.ToRadians(-45f));
        _yellowBoardTemplate3x1.Instantiate(new Vector2(210, 700), MathHelper.ToRadians(-25f));
        _yellowBoardTemplate3x1.Instantiate(new Vector2(390, 700), MathHelper.ToRadians(25f));
        _bounceBallTemplate.Instantiate(new Vector2(120, 500));
        _bounceBallTemplate.Instantiate(new Vector2(230, 500));
        _bounceBallTemplate.Instantiate(new Vector2(370, 500));
        _bounceBallTemplate.Instantiate(new Vector2(480, 500));
        _bounceBallTemplate.Instantiate(new Vector2(100, 800));
        _bounceBallTemplate.Instantiate(new Vector2(500, 800));
        _sideBorderTemplate.Instantiate(new Vector2(TileSize/2, 12 * TileSize));
        _sideBorderTemplate.Instantiate(new Vector2(14.5f * TileSize, 12 * TileSize));
        _topBorderTemplate.Instantiate(new Vector2(7.5f * TileSize, TileSize/2));
    }

    private void CreateTemplates()
    {
        var yellowBoardRuleTextures = new ConvexTilemapRuleTextures
        {
            EdgeLeftOneHigh = _content.Load<Texture2D>("Environment/tile_left"),
            EdgeRightOneHigh = _content.Load<Texture2D>("Environment/tile_right"),
            CenterOneHigh = _content.Load<Texture2D>("Environment/tile_center")
        };
        _yellowBoardTemplate3x1 = CreateConvexTileShapeTemplate(3, 1, yellowBoardRuleTextures);
        _yellowBoardTemplate4x1 = CreateConvexTileShapeTemplate(4, 1, yellowBoardRuleTextures);
        var borderWallsRuleTextures = new ConvexTilemapRuleTextures()
        {
            Default = _content.Load<Texture2D>("Environment/tile_grey")
        };
        _sideBorderTemplate = CreateConvexTileShapeTemplate(1, 24, borderWallsRuleTextures);
        _topBorderTemplate = CreateConvexTileShapeTemplate(13, 1, borderWallsRuleTextures);
        _bounceBallTemplate = new GameObject.Template([
            new SpriteRenderer.Builder(_content.Load<Texture2D>("Environment/blue_circle")).WithScale(SpriteScale).Build(),
            // Todo: Add a Circle collider with the radius of 20 and an Elasticity of 2
            new CircleCollider { Radius = 20, Elasticity = 2f },
            new BounceBallCollisionEffect(_content.Load<Texture2D>("Environment/yellow_circle")),
        ]);
    }

    private GameObject.Template CreateConvexTileShapeTemplate(int width, int height,
        ConvexTilemapRuleTextures ruleTextures)
    {
        var components = new List<Component>();
        var dimensions = new Vector2Int(width, height);
        var bottomLeft = new Vector2(
            width % 2 != 0 ? -width / 2 * TileSize : (-width / 2 + 0.5f) * TileSize,
            height % 2 != 0 ? height / 2 * TileSize : (height / 2 - 0.5f) * TileSize
        );
        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                var texture = ruleTextures.GetTextureFromPositionAndDimensions(new Vector2Int(x, y), dimensions);
                var offset = bottomLeft + new Vector2(x * TileSize, -y * TileSize);
                components.Add(new SpriteRenderer.Builder(texture).WithCenterOffset(offset).WithScale(SpriteScale)
                    .Build());
            }
        }

        var boxColliderWidth = TileSize * width - 1;
        var boxColliderHeight = TileSize * height - 1;
        // Todo: Create the fitting boxCollider
        var boxCollider = new BoxCollider
        {
            Size = new Vector2(boxColliderWidth, boxColliderHeight)
        };
        // Todo: Add the new boxCollider to the template's components using components.Add();
        components.Add(boxCollider);
        return new GameObject.Template(components);
    }
}