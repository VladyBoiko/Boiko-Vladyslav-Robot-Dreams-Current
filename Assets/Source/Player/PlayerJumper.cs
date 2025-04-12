using UnityEngine;

namespace Player
{
    public class PlayerJumper : MonoBehaviour
    {
        [SerializeField] private CharacterController _controller;

        [SerializeField] private float _jumpHeight = 2f;
        [SerializeField] private float _gravity = 9.81f;
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _groundCheckRadius = 0.2f;
    
        private Vector3 _velocity;
        private bool _isGrounded;
    
        // public event Action<bool> OnGroundedStatusChanged;
        private void Start()
        {
            if (_controller == null)
            {
                Debug.LogError("No CharacterController attached!");
                enabled = false;
                return;
            }

            if (_groundCheck == null)
            {
                Debug.LogError("GroundCheck transform is not assigned!");
                enabled = false;
                return;
            }
        
            InputController.OnJumpInput += JumpHandler;
        }

        private void Update()
        {
            // bool wasGrounded = _isGrounded;
            _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _groundLayer);
        
            // if (wasGrounded != _isGrounded)
            // {
            //     OnGroundedStatusChanged?.Invoke(_isGrounded);
            // }
        
            if (_isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }
            _velocity.y -= _gravity * Time.deltaTime;
            _controller.Move(_velocity * Time.deltaTime);
        }

        // private void LateUpdate()
        // {
        //     bool wasGrounded = _isGrounded;
        //     _isGrounded = _controller.isGrounded;
        //     if (wasGrounded != _isGrounded)
        //     {
        //         OnGroundedStatusChanged?.Invoke(_isGrounded);
        //     }
        // }

        private void JumpHandler(bool jumpPressedDown)
        {
            if (jumpPressedDown)
            {
                if (_isGrounded)
                {
                    _velocity.y = Mathf.Sqrt(2f * _jumpHeight * _gravity);
                }
            }
        }

        // private void OnDrawGizmos()
        // {
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        // }
    }
}
