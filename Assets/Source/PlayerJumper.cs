using System;
using UnityEngine;

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
    
    public event Action<bool> OnGroundedStatusChanged;
    private void Start()
    {
        InputController.OnJumpInput += JumpHandler;
        if (_controller) return;
        Debug.LogError("No CharacterController attached");
        enabled = false;
    }

    private void Update()
    {
        bool wasGrounded = _isGrounded;
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundCheckRadius, _groundLayer);
        
        if (wasGrounded != _isGrounded)
        {
            OnGroundedStatusChanged?.Invoke(_isGrounded);
        }
        
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }
        
        _velocity.y -= _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }
    
    private void JumpHandler(bool isGrounded)
    {
        _isGrounded = isGrounded;
        if (_isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * 2f * _gravity);
        }
    }
}
