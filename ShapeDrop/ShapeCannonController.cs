using System;
using GameLib.Engine;
using GameLib.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ShapeDrop;

public class ShapeCannonController(GameObject.Template ballTemplate, GameObject.Template squareTemplate) : Controller
{
    private float _currentRotationAngle;
    private bool _pressedSpaceLastFrame;
    private bool _isLoaded = true;
    private float _inputStrength;
    private SpriteRenderer _arrowRenderer;
    private bool _shotBallLast;
    
    private const float MaxRotation = 75f;
    private const float RotationSpeed = 5f;
    private const float ShootStrengthIncreasePerFrame = 0.01f;

    public override void Connect(GameObject gameObject)
    {
        base.Connect(gameObject);
        _arrowRenderer = GameObject.GetComponent<SpriteRenderer>();
    }

    public override void HandleInput(KeyboardState state, MouseState _)
    {
        if (!_isLoaded)
            return;
        
        if (state.IsKeyDown(Keys.Left))
        {
            _currentRotationAngle += RotationSpeed;
        }
        else if (state.IsKeyDown(Keys.Right))
        {
            _currentRotationAngle -= RotationSpeed;
        }
        _currentRotationAngle = Math.Clamp(_currentRotationAngle, -MaxRotation, MaxRotation);
        GameObject.Rotation = MathHelper.ToRadians(_currentRotationAngle);

        if (_pressedSpaceLastFrame)
        {
            if (state.IsKeyDown(Keys.Space))
            {
                _inputStrength = Math.Min(_inputStrength + ShootStrengthIncreasePerFrame, 1.0f);
                _arrowRenderer.Color = Color.Lerp(Color.White, Color.Red, _inputStrength);
            }
            else
            {
                var projectileTemplate = _shotBallLast ? squareTemplate : ballTemplate;
                var projectile = projectileTemplate.Instantiate(GameObject.Position);
                var shotStrength = MathHelper.Lerp(20, 100, _inputStrength);
                var shotDirection = Vector2.Rotate(Vector2.UnitY, GameObject.Rotation);
                ShootProjectile(projectile, shotStrength, shotDirection);
                _shotBallLast = !_shotBallLast;
                
                _isLoaded = false;
                _arrowRenderer.Disable();
                _inputStrength = 0;
            }
        }
        _pressedSpaceLastFrame = state.IsKeyDown(Keys.Space);
    }

    private void ShootProjectile(GameObject projectile, float shotStrength, Vector2 shotDirection)
    {
        projectile.GetComponent<SelfDestroyWhenOffScreen>().OnDestroy += Reload;
        var projectileRigidBody = projectile.GetComponent<RigidBody>();
        // Todo: add an impulse with the shotStrength and direction to the projectile
        projectileRigidBody.AddImpulse(shotDirection * shotStrength);
    }

    private void Reload()
    {
        _isLoaded = true;
        _arrowRenderer.Color = Color.White;
        _arrowRenderer.Enable();
    }
}