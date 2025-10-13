using UnityEngine;

public interface IInputHandler
{
	Vector2 MoveAxis();
	Vector2 LookAxis();
	bool isFiring();
}
