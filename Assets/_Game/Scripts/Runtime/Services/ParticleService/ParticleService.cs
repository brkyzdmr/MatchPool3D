using DG.Tweening;
using UnityEngine;

public class ParticleService : Service, IParticleService
{
    private readonly IObjectPoolService _objectPoolService;

    public ParticleService(Contexts contexts) : base(contexts)
    {
        _objectPoolService = Services.GetService<IObjectPoolService>();
    }

    public void PlayMergeParticle(Vector3 position)
    {
        var particleGameObject = _objectPoolService.Spawn(Pools.Types.MergeExplosionParticle, position);
        if (particleGameObject != null)
        {
            PlayAllParticleSystemsAndDespawn(particleGameObject, Pools.Types.MergeExplosionParticle);
        }
    }

    public void PlayGoldExplosionParticle(Vector3 position)
    {
        var particleGameObject = _objectPoolService.Spawn(Pools.Types.GoldExplosionParticle, position);
        if (particleGameObject != null)
        {
            PlayAllParticleSystemsAndDespawn(particleGameObject, Pools.Types.GoldExplosionParticle);
        }
    }

    private void PlayAllParticleSystemsAndDespawn(GameObject particleGameObject, Pools.Types poolType)
    {
        float maxDuration = 0f;
        foreach (var particleSystem in particleGameObject.GetComponentsInChildren<ParticleSystem>())
        {
            particleSystem.Play();
            if (particleSystem.main.duration > maxDuration)
            {
                maxDuration = particleSystem.main.duration;
            }
        }
        
        DOVirtual.DelayedCall(maxDuration, () => _objectPoolService.Despawn(poolType, particleGameObject));
    }
}