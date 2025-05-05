using UnityEngine;

namespace Audio.FootSoundControllerSystem
{
    /// <summary>
    /// Script that on each collider hit checks if it can be considered ground
    /// if angle between hit normal and world up direction is less than slope limit of CharacterController, then
    /// CharacterController is standing on a collider
    /// Otherwise, it's sliding, even if collider is below character controller
    /// </summary>
    public class GroundDetector : MonoBehaviour
    {
        [SerializeField] private CharacterController _characterController;

        public Collider Collider { get; private set; }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if ((hit.controller.collisionFlags & CollisionFlags.Below) != 0)
            {
                float cosine = Mathf.Cos(hit.controller.slopeLimit * Mathf.Deg2Rad);
                if (Vector3.Dot(Vector3.up, hit.normal) > cosine)
                    Collider = hit.collider;
                //Debug.Log($"frame {Time.frameCount}, {hit.collider.name}, cos {cosine}, dot {Vector3.Dot(Vector3.up, hit.normal)}");
            }
        }
    }
}