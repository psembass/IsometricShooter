using ModestTree;
using UnityEngine;
using Zenject;

public class HitscanWeapon : IWeapon
{
    private Transform _transform;
    private float maxDistance = 15f;
    private float damage = 25f;
    private float lastShootTime = 0;

    private float shootRate = 0.2f;
    private LayerMask targetMask;
    
    private RaycastHit[] raycastHits = new RaycastHit[10];
    private ParticlesService particles;
    private IAudioService audioService;
    private AudioConfig audioConfig;

    public HitscanWeapon(ParticlesService particles, AudioConfig audioConfig, IAudioService audioService)
    {
        this.particles = particles;
        this.audioConfig = audioConfig;
        this.audioService = audioService;
    }

    public bool Attack(Vector3 direction)
    {
        if (Time.time < lastShootTime + shootRate) { return false; }
        // todo check if weapon can shoot, i.e. bullets left
        lastShootTime = Time.time;
        particles.PlayEffect(ParticlesService.MUZZLE, _transform.position);
        Ray ray = new Ray(_transform.position, direction);
        int hitCount = Physics.RaycastNonAlloc(ray, raycastHits, maxDistance);
        for (int i = 0; i < hitCount; i++)
        {
            // todo check layer mask
            raycastHits[i].collider.TryGetComponent<IDamageable>(out IDamageable damageable);
            if (damageable != null)
            {
                particles.PlayEffect(ParticlesService.BLOOD, raycastHits[i].point);
                audioService.PlayOneShot(audioConfig.Blood, raycastHits[i].point);
                damageable.TakeDamage(damage);
            }
        }
        return true;
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
