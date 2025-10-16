using ModestTree;
using UnityEngine;

public class HitscanWeapon : IWeapon
{
    private Transform _transform;
    private float maxDistance = 10f;
    private float damage = 20f;
    private float lastShootTime = 0;

    private float shootRate = 0.2f;
    private LayerMask targetMask;
    
    private RaycastHit[] raycastHits = new RaycastHit[10];

    public void Attack(Vector3 direction)
    {
        if (Time.time < lastShootTime + shootRate) { return; }
        // todo check if weapon can shoot, i.e. bullets left
        lastShootTime = Time.time;
        Ray ray = new Ray(_transform.position, direction);
        int hitCount = Physics.RaycastNonAlloc(ray, raycastHits, maxDistance);
        for (int i = 0; i < hitCount; i++)
        {
            Debug.DrawRay(_transform.position, direction * maxDistance, Color.blue, 2);
            // todo check layer mask
            raycastHits[i].collider.TryGetComponent<IDamageable>(out IDamageable damageable);
            damageable?.TakeDamage(damage);
        }
    }

    public void SetOwner(Transform transform)
    {
        _transform = transform;
    }

    public void SetTargetMask(LayerMask mask)
    {
        targetMask = mask;
    }
}
