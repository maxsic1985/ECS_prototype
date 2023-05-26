using Leopotam.EcsLite;
using UnityEngine;


namespace MSuhininTestovoe.B2B
{
    
    public class CollisionCheckerView : MonoBehaviour, IHaveActor
    {
        public IActor Actor { get; set; }
        public EcsWorld EcsWorld { get; set; }

        
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out IActor otherActor))
            {
                CollisionHandle(otherActor);
            }
        }

        
        private void OnTriggerEnter(Collider otherCollider)
        {
            if (otherCollider.gameObject.TryGetComponent(out IActor otherActor))
            {
                CollisionHandle(otherActor);
            }
        }
        

        private void CollisionHandle(IActor otherActor)
        {
            int hit = EcsWorld.NewEntity();
            EcsPool<HitComponent> hitPool = EcsWorld.GetPool<HitComponent>();
            hitPool.Add(hit);
            ref HitComponent hitComponent = ref hitPool.Get(hit);

            hitComponent.FirstEntity = Actor.Entity;
            hitComponent.OtherEntity = otherActor.Entity;

            switch (otherActor)
            {
                case CoinActor:
                    Debug.Log("Is Coin Actor");
                    EcsPool<IsHitCoinsComponent> hitCoinsPool = EcsWorld.GetPool<IsHitCoinsComponent>();
                    hitCoinsPool.Add(hit);

                    break;
                case PillarLampActor:
                    Debug.Log("Is Pillar Actor");
                    EcsPool<IsHitPillarComponent> hitPillarsPool = EcsWorld.GetPool<IsHitPillarComponent>();
                    hitPillarsPool.Add(hit);
                    break;

                case WrenchActor:
                    Debug.Log("Is Wrench Actor");
                    EcsPool<IsHitWrenchComponent> hitWrenchPool = EcsWorld.GetPool<IsHitWrenchComponent>();
                    hitWrenchPool.Add(hit);
                    break;

                default: break;
            }
            
            otherActor.Handle();
        }
    }
}