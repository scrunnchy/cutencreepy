using UnityEngine;
using System.Collections;

namespace Platformer
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(Collider))]
    public class PlatformerCharacter_Physics : MonoBehaviour
    {
        public enum CharacterState
        {
            frozen,
            idle,
            moving,
        }

        [Header("Input Axes")]
        public string horizontalAxis = "Horizontal";
        public string verticalAxis = "Vertical";
        public string jumpAxis = "Jump";
        public string attackAxis = "Fire1";

        [Header("Movment Properties")]
        public float maxSpeed = 10f;
        public float acceleration = 60f;

        [Header("Jump Properties")]
        public float jumpForce = 15f;
        [Range(0f, 1f)]
        public float airControl = 0.85f;

        [Header("Attack Properties")]
        public Weapon primaryAttack;
        public Transform attackPoint;

        //Private Memeber Variables
        private Rigidbody _rigidbody;

        private bool _canMove = true;
        private bool _canAttack = true;
        private bool _canJump = true;
        private bool _inJump = false;

        private bool _isGrounded = false;

        private Vector3 _storedVelocity = Vector3.zero;
        private CharacterState _storedState;

        private CharacterState _currentState = CharacterState.idle;

        void Start()
        {
            _rigidbody = this.GetComponent<Rigidbody>();

            if (attackPoint == null) attackPoint = this.transform;
        }

        private void Update()
        {
            if (!_canMove) return;

            if (_canJump && _isGrounded) 
            {
                Jump();
            }
            else
            {
                // Force the player to release the jump button between jumps, catch for 2x jump power corner case
                if (Input.GetAxis("Jump") == 0f) _canJump = true;
            }

            if (_canAttack) Attack();
        }

        private void FixedUpdate()
        {
            if (!_canMove) return;

            Vector3 force = Vector3.right * Input.GetAxis(horizontalAxis) * acceleration;

            if (_inJump) force *= airControl;

            // Orient player in direction of force, pass in _rigidbody.velocity for facing direction that matches momentum
            Orient(force);

            //add acceleration force to player if moving slower than max speed, overly verbose to allow changes in direction at max speed
            if ((force.x >= 0f && _rigidbody.velocity.x < maxSpeed) || (force.x <= 0f && _rigidbody.velocity.x > -maxSpeed))
                _rigidbody.AddForce(force, ForceMode.Acceleration);

        }

        private void Jump()
        {
            if (Input.GetAxis(jumpAxis) > 0.5f)
            {
                //add vertical impulse force
                _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                _inJump = true;
                _canJump = false;

                _isGrounded = false;
            }
        }

        private void Orient(Vector3 direction)
        {
            Vector3 orientation = Vector3.zero;

            orientation.x = direction.x;

            if (orientation != Vector3.zero) this.transform.forward = orientation;
        }

        private void OnCollisionEnter (Collision col)
        {
            _isGrounded = true;

            //Prevents losing speed on landing
            if (_rigidbody.velocity.y < 0f)
                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0f, 0f);
        }

        private void Attack ()
        {
            if (Input.GetAxis(attackAxis) > 0.5f)
            {
                primaryAttack.Fire(attackPoint);
            }
        }

        private void OnCollisionExit (Collision col)
        {
            
        }

        public void Freeze(bool value)
        {
            _canMove = !value;

            if (value)
            {
                _storedVelocity = _rigidbody.velocity;
                _storedState = _currentState;
                _rigidbody.velocity = Vector3.zero;

            }
            else
            {
                _rigidbody.velocity = _storedVelocity;
                _currentState = _storedState;
            }
        }
    }
}


