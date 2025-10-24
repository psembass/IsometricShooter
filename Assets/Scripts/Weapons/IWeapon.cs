using UnityEngine;

public interface IWeapon
{
    void SetOwner(Transform transform);

    bool Attack(Vector3 direction);

    void SetTargetMask(LayerMask mask);
}
