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
        Animator anim;

        bool isGrounded;
        bool isDodging;

        float startTime;

        [Header("Input Axes")]
        public string horizontalAxis = "Horizontal";
        public string verticalAxis = "Vertical";
        public string jumpAxis = "Jump";

        [Header("Movment Properties")]
        // Unity meters 10 : 12.2 (Track 1)
        // Unity meters 5 : 6.05 (Track 2)
        // Unity meters 3 : 3.6 (Track 3)
        public float maxSpeed = 6.4f;
        public float acceleration = 2f;
        [Range(0f, 1f)]
        public float frictionCoefficient = 0.85f;
        public float massCoeficcient = 0.85f;

        [Header("Jump Properties")]
        public float jumpForce = 5f;
        public float fallMultiplier = 20f;
        public float lowJumpMultiplier = 2f;
        [Range(0f, 1f)]
        public float airControl = 0.85f;
        public float gravityModifier = 2.5f;
        public float terminalVelocity = 25f;
        public float yVelocityLowerLimit = 5f;


        //Private Memeber Variables
        private CharacterController _characterController;
        private Vector3 _characterVelocity = Vector3.zero;
        private Vector3 _force = Vector3.zero;

        private bool _canMove = true;
        private bool _canJump = true;
        private bool _canAttack = true;

        private bool _inJump = false;
        private bool _inSlide = false;
        private bool _inDodge = false;
        private bool _inDash = false;

        private float _jumpMomentum = 0f;
        private Vector3 _storedVelocity = Vector3.zero;

        private CharacterState state = CharacterState.idle;

        void Start()
        {
            _characterController = this.GetComponent<CharacterController>();
            anim = GetComponent<Animator>();
            startTime = Time.realtimeSinceStartup;
        }

        private void Update()
        {
            anim.SetFloat("Speed", _characterVelocity.x);
            //Debug.Log(Input.GetAxis(jumpAxis));
            //Debug.Log(_characterVelocity.y);
            //Debug.Log(_characterVelocity);
            bool isGrounded = Grounded();
            if (isGrounded)
            {
                anim.SetBool("inJump", false);
            }
            if (_canMove)
            {
                Move();
                Dash();
                Slide();
                Dodge();
            }


            if (_canJump && isGrounded && !_inSlide && !_inDash && !_inDodge)
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
                // anim.SetBool("inJump", true);
                if (_characterVelocity.y > 0 && _characterVelocity.y < .5)
                {

                }
                if (_characterVelocity.y < yVelocityLowerLimit && _characterController.transform.position.y > 2)
                {
                    _characterVelocity += Vector3.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
                }
                // If we haven't reached terminal velocity, apply gravity
                if (_characterVelocity.y > -terminalVelocity)
                {
                    _characterVelocity.y += gravityModifier * Physics.gravity.y * Time.deltaTime;
                }
            }

            _force *= massCoeficcient;
            _characterVelocity += _force;
            _characterController.Move((_characterVelocity) * Time.deltaTime);

            // Orients the player toward the character velocity direction -- commented for testing purposes
            //if (_canMove) Orient();

        }

        private void Move()
        {
            if (Mathf.Abs(_characterVelocity.x) < maxSpeed)
            {
                _characterVelocity.x = maxSpeed;
            }
        }

        private void Dash()
        {
            if (Input.GetAxis("Dash") > 0f)
            {
                anim.SetBool("Dashing", true);
                _canJump = false;
            }
        }

        private void Slide()
        {
            if (Input.GetAxis("Slide") > 0f && !_inSlide)
            {
                resetTimer();
                anim.SetBool("Sliding", true);
                _canJump = false;
                _inSlide = true;
            }
            if (Time.realtimeSinceStartup >= startTime + .3f)
            {
                anim.SetBool("Sliding", false);
                _canJump = true;
                if (Input.GetAxis("Slide") == 0f)
                    _inSlide = false;
            }
        }

        public void resetTimer()
        {
            startTime = Time.realtimeSinceStartup;
        }

        private void Dodge()
        {
            if (Input.GetAxis("Dodge") > 0f)
            {
                anim.SetBool("Dodging", true);
                _canJump = false;
                isDodging = true;
            }
        }

        private void Jump()
        {
            //anim.SetFloat("yHeight", _characterController.transform.position.y);
            //Input.GetAxis(jumpAxis) > 0f
            // !_inJump
            if (Input.GetAxis(jumpAxis) > 0f)
            {
                anim.SetBool("inJump", true);
                // Nuke player y velocity and set jump force
                _characterVelocity.y = 0f;
                _force.y = jumpForce;

                _inJump = true;
                _canJump = false;
            }
        }

        /*
        private void Orient()
        {
            Vector3 orientation = Vector3.zero;

            orientation.x = _characterVelocity.x;

            if (orientation != Vector3.zero) this.transform.forward = orientation;
        }*/

        private bool Grounded()
        {
            bool controllerGrounded = _characterController.isGrounded;

            _inJump = !controllerGrounded;

            return controllerGrounded;
        }



        /// <summary>
        /// Freeze the character in place, store the current character velocity, or unfreeze the character and resume character velocity.
        /// Called when pause screen is called
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