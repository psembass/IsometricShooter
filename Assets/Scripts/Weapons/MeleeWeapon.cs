using Unity.VisualScripting;
using UnityEngine;

public class MeleeWeapon : IWeapon
{
    private Transform Owner;
    private float attackRange = 1f;
    private float attackAngle = 45;
    private float attackDamage = 10f;
    private LayerMask targetMask;

    public bool Attack(Vector3 direction)
    {
        bool attacked = false;
        Vector3 position = Owner.position;
        Collider[] colliders = Physics.OverlapSphere(Owner.position, attackRange, targetMask);
        foreach (var item in colliders)
        {
            Vector3 directionToTarget = (item.transform.position - position).normalized;
            float angleToTarget = Vector3.Angle(Owner.forward, directionToTarget);
            if (angleToTarget <= attackAngle / 2f)
            {
                IDamageable target;
                item.GameObject().TryGetComponent(out target);
                target?.TakeDamage(attackDamage);
                attacked = true;
            }
        }
        return attacked;
    }

    public void SetOwner(Transform transform)
    {
        Owner = transform;
    }

    public void SetTargetMask(LayerMask mask)
    {
        targetMask = mask;
    }
}
