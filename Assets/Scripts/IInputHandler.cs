using UnityEngine;

public interface IInputHandler
{
	Vector2 MoveAxis();
	Vector2 LookPoint();
    Vector2 LookVector();
    bool isFiring();
    bool GamepadConnected();
}
