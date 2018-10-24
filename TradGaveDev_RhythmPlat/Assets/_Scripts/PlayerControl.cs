using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerControl : MonoBehaviour
{

    public enum CharacterState
    {
        frozen,
        idle,
        moving
    }
    Animator anim;

    #region Fields and properties
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

    [Header("Action Delays and Cooldowns")]
    public float slideDelay = .5f;
    public float dodgeDelay = 1f;
    public float dashDelay = 1f;
    public float dashCooldown = .2f;
    public float dodgeCooldown = .2f;

    // getters and setters
    public bool isGrounded { get; private set; }
    public Vector3 GetCharacterVelocity() { return _characterVelocity; }
    public Vector3 GetMoveVector() { return moveVector; }
    public CharacterController GetCharacterController() { return _characterController; }

    //Private Memeber Variables
    private Vector3 moveVector;
    private Vector3 _characterVelocity = Vector3.zero;

    private CharacterController _characterController;
    private Vector3 _force = Vector3.zero;

    private bool _canMove = true;
    private bool _canJump = true;
    private bool _canDodge = true;
    private bool _canDash = true;

    public bool _inJump = false;
    public bool _inSlide = false;
    public bool _inDodge = false;
    public bool _inDash = false;

    private Vector3 _storedVelocity = Vector3.zero;

    //private CharacterState state = CharacterState.idle;
    #endregion

    void Awake()
    {
        _characterController = this.GetComponent<CharacterController>();
        anim = this.GetComponent<Animator>();
        _characterVelocity.x = maxSpeed;
        anim.SetFloat("Speed", _characterVelocity.x);
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is currently grounded, sets inJump to true or false (opposite of isGrounded)
        if (isGrounded = Grounded())
        {
            anim.SetBool("Jumping", false);
        }
        // Only executed if the game is not frozen and the player is not in a jump
        if (_canMove && !_inJump)
        {
            // Preconditions before using a dash. Player is not currently dodging or sliding. Player has to be able to dash and must also be pressing dash key
            if (!_inSlide && !_inDodge && Input.GetAxis("Dash") > 0f && _canDash)
            {
                StartCoroutine("Dash");
            }
            // Preconditions before using a dodge. Player is not currently dashing or sliding. Player has to be able to dodge and must also be pressing dodge key
            if (!_inSlide && !_inDash && Input.GetAxis("Dodge") > 0f && _canDodge)
            {
                StartCoroutine("Dodge");
            }
            // Preconditions before using a slide. Player is not currently dashing, dodging or sliding. Player is also pressing slide key
            if (!_inDodge && !_inDash && Input.GetAxis("Slide") > 0f && !_inSlide)
            {
                StartCoroutine("Slide");
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
    }
    IEnumerator Dash()
    {
        Debug.Log("In Dash");
        anim.SetBool("Dashing", true);
        _canJump = false;
        _inDash = true;
        _canDash = false;
        _inDodge = true;

        yield return new WaitForSeconds(dashDelay);

        Debug.Log("Out of Dash");
        anim.SetBool("Dashing", false);
        _canJump = true;
        _inDodge = false;
        _inDash = false;
        // temp _canDash variable. Will change when cooldown is implemented.
        _canDash = true;
        // cooldown code. yet to be tested
        /*yield return new WaitForSeconds(dashCooldown);
        _canDash = true;*/
    }

    IEnumerator Slide()
    {

        //Debug.Log("In Slide");
        anim.SetBool("Sliding", true);
        _canJump = false;
        _inSlide = true;
        _inDodge = true;

        yield return new WaitForSeconds(slideDelay);

        anim.SetBool("Sliding", false);
        _canJump = true;
        _inDodge = false;
        _inSlide = false;
    }

    IEnumerator Dodge()
    {
        Debug.Log("In Dodge");
        anim.SetBool("Dodging", true);
        _canJump = false;
        _inDodge = true;

        yield return new WaitForSeconds(dodgeDelay);

        Debug.Log("Out of Dodge");
        anim.SetBool("Dodging", false);
        _canJump = true;
        _inDodge = false;
        // temp _canDodge variable. Will change when cooldown is implemented.
        _canDodge = true;

        // cooldown code. yet to be tested
        /*yield return new WaitForSeconds(dodgeCooldown);
        _canDash = true;*/
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
        }
    }

    private bool Grounded()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(this.transform.position, Vector3.down, out raycastHit, 1.2f))
        {
            if (raycastHit.collider.gameObject != this.gameObject)
            {
                _characterVelocity.y = 0f;
                _inJump = false;

                //added for demo
                //_inDodge = false;

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
    private IEnumerator ActionWaiter()
    {
        yield return new WaitForSeconds(slideDelay);
    }
}
