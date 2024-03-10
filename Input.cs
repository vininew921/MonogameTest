namespace StreamGame;

public static class Input
{
    private static KeyboardState _keyboardState, _lastKeyboardState;
    private static MouseState _mouseState, _lastMouseState;
    private static GamePadState _gamepadState, _lastGamepadState;

    private static bool _isAimingWithMouse = false;

    public static Vector2 MousePosition => new Vector2(_mouseState.X, _mouseState.Y);

    public static void Update()
    {
        _lastKeyboardState = _keyboardState;
        _lastMouseState = _mouseState;
        _lastGamepadState = _gamepadState;

        _keyboardState = Keyboard.GetState();
        _mouseState = Mouse.GetState();
        _gamepadState = GamePad.GetState(PlayerIndex.One);

        //Switch between aiming with mouse and aiming with gamepad
        if (new[] { Keys.Left, Keys.Right, Keys.Up, Keys.Down }.Any(x => _keyboardState.IsKeyDown(x)) || _gamepadState.ThumbSticks.Right != Vector2.Zero)
        {
            _isAimingWithMouse = false;
        }
        else if (MousePosition != new Vector2(_lastMouseState.X, _lastMouseState.Y))
        {
            _isAimingWithMouse = true;
        }
    }

    public static bool WasKeyPressed(Keys key) => _lastKeyboardState.IsKeyUp(key) && !_keyboardState.IsKeyDown(key);

    public static bool WasButtonPressed(Buttons button) => _lastGamepadState.IsButtonUp(button) && !_gamepadState.IsButtonDown(button);

    public static Vector2 GetMovementDirection()
    {
        Vector2 direction = _gamepadState.ThumbSticks.Left;
        direction.Y *= -1; //Invert Y axis

        if (_keyboardState.IsKeyDown(Keys.A))
        {
            direction.X -= 1;
        }
        if (_keyboardState.IsKeyDown(Keys.D))
        {
            direction.X += 1;
        }
        if (_keyboardState.IsKeyDown(Keys.W))
        {
            direction.Y -= 1;
        }
        if (_keyboardState.IsKeyDown(Keys.S))
        {
            direction.Y += 1;
        }

        //Clamp direction vector to maximum of 1
        if (direction.LengthSquared() > 1)
        {
            direction.Normalize();
        }

        return direction;
    }

    public static Vector2 GetAimDirection()
    {
        if (_isAimingWithMouse)
        {
            return GetMouseAimDirection();
        }

        Vector2 direction = _gamepadState.ThumbSticks.Right;
        direction.Y *= -1; //Invert Y axis

        if (_keyboardState.IsKeyDown(Keys.Left))
        {
            direction.X -= 1;
        }
        if (_keyboardState.IsKeyDown(Keys.Right))
        {
            direction.X += 1;
        }
        if (_keyboardState.IsKeyDown(Keys.Up))
        {
            direction.Y -= 1;
        }
        if (_keyboardState.IsKeyDown(Keys.Down))
        {
            direction.Y += 1;
        }

        return direction == Vector2.Zero ? Vector2.Zero : Vector2.Normalize(direction);
    }

    private static Vector2 GetMouseAimDirection()
    {
        Vector2 direction = MousePosition - PlayerShip.Instance.Position;

        return direction == Vector2.Zero ? Vector2.Zero : Vector2.Normalize(direction);
    }
}
