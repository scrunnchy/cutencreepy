using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {

        public enum CharacterState
        {
            frozen,
            idle,
            moving
        }
        Animator anim;

        [Header("Player Information")]
        public int playerHealth;
        public float delayBetweenBlinks;

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
        bool isGrounded;
        float magicNumber = 0.0001f;
        private Vector3 moveVector;
        float startTime;
        float currentTime;

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
        private bool isFlipped;

        void Start()
        {
            isFlipped = false;
            _characterController = this.GetComponent<CharacterController>();
            anim = this.GetComponent<Animator>();
            _characterVelocity.x = maxSpeed;
            anim.SetFloat("Speed", _characterVelocity.x);

            //listeners
            Checkpoint.CheckpointReverse.AddListener(flipPlayer);
            Enemy.enemyPlayerCollision.AddListener(DecrementHealth);
        }

        private void Update()
        {
            //Debug.Log(_characterController.transform.position.y);
            if (isGrounded = Grounded())
            {
                moveVector.y = magicNumber;
                //Debug.Log("isGrounded");
                // nuke character velocity
                //_characterVelocity.y = 0f;
                anim.SetBool("Jumping", false);
            }
            if (_canMove && !_inJump)
            {
                if (!_inSlide && !_inDodge)
                {
                    Dash();
                }
                if (!_inSlide && !_inDash)
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
                //Debug.Log("is not grounded");
                anim.SetFloat("yHeight", _characterController.transform.position.y);
                //&& _characterController.transform.position.y > 2
                if (_characterVelocity.y < yVelocityLowerLimit)
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
            _characterController.Move(moveVector * Time.deltaTime);
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
                //added for demo
                _inDodge = true;
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
                    //added for demo
                    _inDodge = false;
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

                //added for demo
                _inDodge = true;
            }
            if (Time.realtimeSinceStartup >= startTime + .3f)
            {
                anim.SetBool("Sliding", false);
                _canJump = true;
                //added for demo
                //_inDodge = false;

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
                anim.SetBool("Jumping", true);
                // Nuke player y velocity and set jump force
                _characterVelocity.y = 0f;
                _characterVelocity.y = 20f;
                //_force.y = jumpForce;

                _inJump = true;
                _canJump = false;

                //added for demo
                _inDodge = true;
            }
        }

        private bool Grounded()
        {
            /*bool controllerGrounded = _characterController.isGrounded;

            _inJump = !controllerGrounded;

            return controllerGrounded;*/

            RaycastHit raycastHit;
            if (Physics.Raycast(this.transform.position, Vector3.down, out raycastHit, 1.2f))
            {
                if (raycastHit.collider.gameObject != this.gameObject)
                {
                    _characterVelocity.y = 0f;
                    _inJump = false;

                    //added for demo
                    _inDodge = false;

                    return true;
                }
            }
            return false;
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

        /// <summary>
        /// Flips the player on reversal point
        /// </summary>
        private void flipPlayer()
        {
            GetComponent<SpriteRenderer>().flipX = true;
            isFlipped = true;
        }

        /// <summary>
        /// Decrements health
        /// </summary>
        private void DecrementHealth()
        {
            if (playerHealth > 1)
            {
                playerHealth -= 1;
                StartCoroutine(waiter());
            }

            else
            {
                Debug.Log("Dead");
                SceneManager.LoadScene(2);
            }
        }


        /// <summary>
        /// Flashes player sprite
        /// </summary>
        /// <returns></returns>
        private IEnumerator waiter()
        {
            for (int i = 0; i < 3; i++)
            {
                GetComponent<SpriteRenderer>().enabled = false;
                yield return new WaitForSeconds(delayBetweenBlinks);
                GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }
}