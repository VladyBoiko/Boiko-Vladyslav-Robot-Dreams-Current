using DamageSystems;
using HealthSystems;
using UnityEngine;

namespace Player
{
    public class PlayerDamageDealer : DamageDealerBase
    {
        [SerializeField] private DamagePopup _damagePopupPrefab;
        [SerializeField] private Camera _camera;

        protected override void HandleDamage(HealthArea health, Vector3 hitPoint, Collider hitCollider)
        {
            int damage = _damage;
            int score = 0;

            if (_currentHitMode != "ObjectSpawn")
            {
                damage = _railGunDamage;
            }
            else
            {
                if (health.isCritical)
                {
                    damage = (int)(damage * health.damageMultiplier);
                    score = 2;
                }
                else
                {
                    score = 1;
                }
            }

            health.health.TakeDamage(damage);
            
            var popup = Instantiate(_damagePopupPrefab, hitPoint, Quaternion.identity);
            popup.Initialize(damage, _camera);

            InvokeHit(1, score);
        }
    }
}
