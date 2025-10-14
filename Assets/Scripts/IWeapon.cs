using UnityEngine;

public interface IWeapon
{
    void SetOwner(Transform transform);

    void Attack(Vector3 direction);
}
