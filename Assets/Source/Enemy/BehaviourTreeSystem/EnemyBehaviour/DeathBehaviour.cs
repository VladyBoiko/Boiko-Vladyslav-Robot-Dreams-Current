using UnityEngine;

namespace Enemy.BehaviourTreeSystem.EnemyBehaviour
{
    public class DeathBehaviour : BehaviourStateBase
    {
        private Vector3 _fallMarkPosition;
        private Quaternion _fallMarkRotation;
        private Vector3 _regularPosition;
        private Quaternion _regularRotation;

        private float _time;
        private float _reciprocal;

        public DeathBehaviour(byte stateId, EnemyController enemyController)
            : base(stateId, enemyController)
        {
            enemyController.FallMark.GetLocalPositionAndRotation(out _fallMarkPosition, out _fallMarkRotation);
            enemyController.MeshRendererTransform.GetLocalPositionAndRotation(out _regularPosition, out _regularRotation);
        }

        public override void Enter()
        {
            // Debug.Log("Death Behaviour Enter");
            
            base.Enter();
            
            _time = 0f;
            _reciprocal = 1f / enemyController.Data.HealthBarDelayTime;
            
            enemyController.CharacterController.enabled = false;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            // if (_time < enemyController.Data.HealthBarDelayTime)
            // {
            //     _time += deltaTime;
            //     EvaluateFall(_time * _reciprocal);
            //     return;
            // }
            //
            // EvaluateFall(1f);
            // Object.Destroy(enemyController.RootObject);
            // enemyController.gameObject.SetActive(false);
        }

        public override void Exit()
        {
            base.Exit();
            
            enemyController.MeshRendererTransform.SetLocalPositionAndRotation(_regularPosition, _regularRotation);
        }
        
        private void EvaluateFall(float progress)
        {
            float curveFactor = enemyController.Data.FallCurve.Evaluate(progress);

            Vector3 position = Vector3.Lerp(_regularPosition, _fallMarkPosition, curveFactor);
            Quaternion rotation = Quaternion.Slerp(_regularRotation, _fallMarkRotation, curveFactor);
            
            // Debug.Break();
            // Debug.Log($"Fall progress: {progress}, Position: {position}, Rotation: {rotation}");
            
            enemyController.MeshRendererTransform.SetLocalPositionAndRotation(position, rotation);
            
        }

        public override void Dispose()
        {
        }
    }
}