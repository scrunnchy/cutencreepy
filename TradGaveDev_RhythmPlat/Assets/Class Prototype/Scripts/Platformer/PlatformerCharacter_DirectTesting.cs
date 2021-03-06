﻿using System.Collections;
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
        public bool isDodging;

        float startTime;
        float currentTime;

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
        private bool _canDodge = true;
        private bool _canDash = true;
        private bool _canAttack = true;

        private bool _inJump = false;
        private bool _inSlide = false;
        public bool _inDodge = false;
        private bool _inDash = false;

        private float _jumpMomentum = 0f;
        private Vector3 _storedVelocity = Vector3.zero;

        private CharacterState state = CharacterState.idle;

        void Start()
        {
            _characterController = this.GetComponent<CharacterController>();
            anim = this.GetComponent<Animator>();
            _characterVelocity.x = maxSpeed;
            anim.SetFloat("Speed", _characterVelocity.x);
        }

        private void Update()
        {
            //Debug.Log(_characterVelocity.y);
            //Debug.Log(_characterController.transform.position.y);
            if (isGrounded = Grounded())
            {
                // nuke character velocity
                _characterVelocity.y = 0f;
                anim.SetBool("inJump", false);
            }
            if (_canMove && !_inJump)
            {
                if (!_inSlide && !_inDodge)
                {
                    Dash();
                }
                if(!_inSlide && !_inDash)
                {
                    Dodge();
                }
                if (!_inDodge && !_inDash)
                {
                    Slide();
                }
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
                anim.SetFloat("yHeight", _characterController.transform.position.y);
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
        }

        private void Dash()
        {
            //currentTime = Time.realtimeSinceStartup + 2f;
            if (Input.GetAxis("Dash") > 0f && _canDash)
            {
                // Still need to implement the actual dash
                resetTimer();
                anim.SetBool("Dashing", true);
                _canJump = false;
                _inDash = true;
                _canDash = false;
            }

            if (_inDash)
            {
                //Debug.Log("In Dash");
                if (Time.realtimeSinceStartup >= startTime + .5f)
                {
                    currentTime = Time.time;
                    anim.SetBool("Dashing", false);
                    _canJump = true;
                    _inDash = false;
                }
            }
            if (!_inDash && !_canDash)
            {
                //Debug.Log("Dash Cooldown");
                //Debug.Log(currentTime);
                if (Time.time >= currentTime + .3f)
                {
                    _canDash = true;
                }
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
            currentTime = Time.realtimeSinceStartup + 2f;
            if (Input.GetAxis("Dodge") > 0f && _canDodge)
            {
                resetTimer();
                anim.SetBool("Dodging", true);
                _canJump = false;
                _inDodge = true;
                _canDodge = false;
            }
        if (_inDodge)
        { 
            if (Time.realtimeSinceStartup >= startTime + .5f)
            {
                Debug.Log("Dodged");
                currentTime = Time.realtimeSinceStartup;
                anim.SetBool("Dodging", false);
                _canJump = true;
                _inDodge = false;
                _canDodge = true;
                }
        }

            /*if (!_inDodge && !_canDodge)
            {
                if (Time.realtimeSinceStartup >= currentTime + .3f)
                {
                }
            }*/
        }

        private void Jump()
        {
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