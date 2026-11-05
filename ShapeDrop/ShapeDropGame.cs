using GameLib.Engine;
using GameLib.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShapeDrop;

public class ShapeDropGame() : Core("ShapeDrop", 600, 960)
{
    protected override void Initialize()
    {
        base.Initialize();
        ClearColor = new Color(225, 232, 240);
        
        var levelEnvironment = new LevelEnvironment(Content);
        levelEnvironment.Build();
        
        CreateShapeCannon();
    }

    private void CreateShapeCannon()
    {
        var ballTemplate = new GameObject.Template([
            new SpriteRenderer.Builder(Content.Load<Texture2D>("Sprites/red_circle")).WithScale(0.5f).Build(),
            // Todo: Add a rigidbody with the mass of 5
            // Todo: Add a circle collider with a radius of 20
            new SelfDestroyWhenOffScreen(),
        ]);
        var squareTemplate = new GameObject.Template([
            new SpriteRenderer.Builder(Content.Load<Texture2D>("Sprites/red_square")).WithScale(0.5f).Build(),
            // Todo: Add a rigidbody with the mass of 5
            // Todo: Add a box collider with a width and height of 40
            new SelfDestroyWhenOffScreen(),
        ]);
        var ballCannonTemplate = new GameObject.Template([
            new SpriteRenderer.Builder(Content.Load<Texture2D>("UI/arrow")).WithCenterOffset(new Vector2(0, 30)).Build(),
            new ShapeCannonController(ballTemplate, squareTemplate)
        ]);
        ballCannonTemplate.Instantiate(new Vector2(300, 65));
    }
}