using UnityEngine;

namespace Enemy.BehaviourTreeSystem.EnemyBehaviour
{
    public class ShootBehaviour : BehaviourStateBase
    {
        public enum State
        {
            Aiming,
            Cooldown,
            Reload,
            Shoot,
        }

        private Transform _characterTransform;
        private Transform _weaponTransform;

        private State _state;
        private float _time;
        private int _charge;
        private float _inaccuracy;

        public ShootBehaviour(byte stateId, EnemyController enemyController)
            : base(stateId, enemyController)
        {
        }

        public override void Enter()
        {
            base.Enter();
            _time = 0f;
            _state = State.Aiming;
            
            _characterTransform = enemyController.CharacterTransform;
            _weaponTransform = enemyController.WeaponTransform;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            if (_state != State.Shoot)
                UpdateRotation();

            switch (_state)
            {
                case State.Aiming:
                    AimingUpdate(deltaTime);
                    break;
                case State.Cooldown:
                    CooldownUpdate(deltaTime);
                    break;
                case State.Reload:
                    ReloadUpdate(deltaTime);
                    break;
                case State.Shoot:
                    ShootUpdate(deltaTime);
                    break;
            }
        }

        private void AimingUpdate(float deltaTime)
        {
            _time += deltaTime;
            if (_time < enemyController.Data.AimTime)
                return;

            _inaccuracy = enemyController.Data.Inaccuracy;
            _charge = enemyController.WeaponData.MaxCharge;
            Shoot();
        }

        private void CooldownUpdate(float deltaTime)
        {
            _time += deltaTime;
            if (_time < enemyController.WeaponData.CooldownTime)
                return;

            Shoot();
        }

        private void ReloadUpdate(float deltaTime)
        {
            _time += deltaTime;
            if (_time < enemyController.WeaponData.ReloadTime)
                return;

            _charge = enemyController.WeaponData.MaxCharge;
            Shoot();
        }

        private void ShootUpdate(float deltaTime)
        {
            _time += deltaTime;
            if (_time < enemyController.Data.ShotDelay)
                return;

            enemyController.Shooter.Shoot();

            _time = 0f;
            _charge -= enemyController.WeaponData.ChargePerShot;

            _inaccuracy = Mathf.Max(_inaccuracy - enemyController.Data.AccuracyPerShot, enemyController.Data.MinInaccuracy);
            _state = _charge > 0 ? State.Cooldown : State.Reload;
        }

        private void Shoot()
        {
            // Debug.Break();
            
            var target = enemyController.Playerdar.CurrentTarget;
            if (target == null)
                return;

            Vector3 aimTarget = GetAimingTarget(target.TargetPivot.position, _inaccuracy);
            Vector3 weaponDirection = (aimTarget - _weaponTransform.position).normalized;
            
            _weaponTransform.rotation = Quaternion.LookRotation(weaponDirection);

            _state = State.Shoot;
            _time = 0f;
        }

        private Vector3 GetAimingTarget(Vector3 playerPosition, float inaccuracyMultiplier)
        {
            Vector3 localPlayerPos = _characterTransform.InverseTransformPoint(playerPosition);
            float distance = localPlayerPos.z;
            float distanceFactor = 1f - distance / enemyController.Data.PreferredRange;
            float inaccuracyFactor = distanceFactor * enemyController.Data.InaccuracyFactor;

            inaccuracyMultiplier -= inaccuracyFactor;

            Vector2 randomOffset = Random.insideUnitCircle * inaccuracyMultiplier;
            localPlayerPos.x += randomOffset.x;
            localPlayerPos.y += randomOffset.y + (inaccuracyMultiplier - 0.5f);

            return _characterTransform.TransformPoint(localPlayerPos);
        }

        private void UpdateRotation()
        {
            var target = enemyController.Playerdar.CurrentTarget;
            if (target == null)
                return;

            Vector3 direction = Vector3.ProjectOnPlane(
                target.TargetPivot.position - _characterTransform.position,
                Vector3.up).normalized;

            if (direction.sqrMagnitude > 0.001f)
                _characterTransform.rotation = Quaternion.LookRotation(direction);

            Vector3 weaponDirection = (target.TargetPivot.position - _weaponTransform.position).normalized;
            if (weaponDirection.sqrMagnitude > 0.001f)
                _weaponTransform.rotation = Quaternion.LookRotation(weaponDirection);
        }

        public override void Dispose()
        {
        }
    }
}
