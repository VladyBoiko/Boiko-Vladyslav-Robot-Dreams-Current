using DamageSystems;
using HealthSystems;
using UnityEngine;

namespace Enemy
{
    public class EnemyDamageDealer : DamageDealerBase
    {
        protected override void HandleDamage(HealthArea health, Vector3 hitPoint, Collider hitCollider)
        {
            int damage = _damage;

            if (health.isCritical)
            {
                damage = (int)(damage * health.damageMultiplier);
            }

            health.health.TakeDamage(damage);
            
            // InvokeHit(1, 0);
        }
    }
}