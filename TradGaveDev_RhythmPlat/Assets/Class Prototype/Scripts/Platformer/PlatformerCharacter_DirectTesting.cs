using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    [RequireComponent(typeof(CharacterController))]
    public class PlatformerCharacter_DirectTesting : MonoBehaviour
    {

        public enum CharacterState
        {
            frozen,
            idle,
            moving
        }

        [Header("Input Axes")]
        public string horizontalAxis = "Horizontal";
        public string verticalAxis = "Vertical";
        public string jumpAxis = "Jump";
        public string attackAxis = "Fire1";

        [Header("Movment Properties")]
        public float maxSpeed = 10f;
        public float acceleration = 2f;
        [Range(0f, 1f)]
        public float frictionCoefficient = 0.85f;
        public float massCoeficcient = 0.85f;

        [Header("Jump Properties")]
        public float jumpForce = 3f;
        [Range(0f, 1f)]
        public float airControl = 0.85f;
        public float gravityModifier = 2.5f;
        public float terminalVelocity = 25f;

        [Header("Attack Properties")]
        public Weapon primaryAttack;
        public Transform attackPoint;


        //Private Memeber Variables
        private CharacterController _characterController;
        private Vector3 _characterVelocity = Vector3.zero;
        private Vector3 _force = Vector3.zero;

        private bool _canMove = true;
        private bool _canJump = true;
        private bool _canAttack = true;

        private bool _inJump = false;

        private float _jumpMomentum = 0f;
        private Vector3 _storedVelocity = Vector3.zero;

        private CharacterState state = CharacterState.idle;

        void Start()
        {
            _characterController = this.GetComponent<CharacterController>();


        }

        private void Update()
        {
            bool isGrounded = Grounded();

            if (_canMove)
            {
                Move();
            }


            if (_canJump && isGrounded)
            {
                Jump();
            }
            else
            {
                // Force the player to release the jump button between jumps, catch for 2x jump power corner case
                if (Input.GetAxis("Jump") == 0f)
                    _canJump = true;
            }


            // If the character is in the air: apply gravity, reduce force by air control
            if (!isGrounded)
            {
                // If we haven't reached terminal velocity, apply gravity
                if (_characterVelocity.y > -terminalVelocity)
                {
                    _characterVelocity.y += gravityModifier * Physics.gravity.y * Time.deltaTime;
                }

                _force.x *= airControl;
            }

            if (_inJump)
            {
                _characterVelocity.x += _jumpMomentum;
            }

            _force *= massCoeficcient;
            _characterVelocity += _force;
            _characterVelocity.x *= frictionCoefficient;

            _characterController.Move((_characterVelocity) * Time.deltaTime);

            // Orients the player toward the character velocity direction
            //if (_canMove) Orient();

            if (_canAttack) Attack();

        }

        private void Move()
        {
            if (Mathf.Abs(_characterVelocity.x) < maxSpeed)
            {
                _force.x = Input.GetAxis(horizontalAxis) * acceleration;
            }
        }

        private void Jump()
        {
            if (Input.GetAxis(jumpAxis) > 0f)
            {
                // Nuke player y velocity and set jump force
                _characterVelocity.y = 0f;
                _force.y = jumpForce;

                // Set jump momentum in relation to air control, keeps the player moving after leaving the ground if air control is low
                _jumpMomentum = _force.x * ((airControl - 1f) * -1f);

                _inJump = true;
                _canJump = false;
            }
        }

        private void Orient()
        {
            Vector3 orientation = Vector3.zero;

            orientation.x = _characterVelocity.x;

            if (orientation != Vector3.zero) this.transform.forward = orientation;
        }

        private bool Grounded()
        {
            bool controllerGrounded = _characterController.isGrounded;

            _inJump = !controllerGrounded;

            return controllerGrounded;


            // NOTE: "isGrounded" has been unreliable in the past, use raycast or Physics.CheckSphere if it becomes an issue

            //RaycastHit raycastHit;

            //if (Physics.Raycast(this.transform.position, Vector3.down, out raycastHit, 1.2f))
            //{
            //    if (raycastHit.collider.gameObject != this.gameObject)
            //    {
            //        _characterVelocity.y = 0f;
            //        _inJump = false;
            //        return true; 
            //    }
            //}
        }


        private void Attack()
        {
            if (Input.GetAxis(attackAxis) > 0.5f)
            {
                primaryAttack.Fire(attackPoint);
            }
        }

        /// <summary>
        /// Freeze the character in place, store the current character velocity, or unfreeze the character and resume character velocity.
        /// </summary>
        /// <param name="value">If set to <c>true</c> value.</param>
        public void Freeze(bool value)
        {
            _canMove = !value;
            _canJump = !value;

            _force = Vector3.zero;

            if (value)
            {
                _storedVelocity = _characterController.velocity;
                _characterVelocity = Vector3.zero;
            }
            else
            {
                _characterVelocity = _storedVelocity;

            }
        }
    }
}