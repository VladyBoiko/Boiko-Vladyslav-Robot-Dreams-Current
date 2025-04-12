using DamageSystems;

namespace Enemy
{
    public class EnemyShooter : ShooterBase
    {
        public void Shoot()
        {
            ObjectSpawnShoot();
            InvokeShot();
        }
    }
}