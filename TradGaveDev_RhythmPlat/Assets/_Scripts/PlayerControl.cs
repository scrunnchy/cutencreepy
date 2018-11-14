using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;

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
    private float currentBpm = 127f;
    public float slowDownSpeed;

    [Header("Jump Properties")]
    public float fallMultiplier = 20f;
    
    [Range(0f, 1f)]
    public float airControl = 0.85f;
    public float regularGravityMultiplier = 2.5f;
    public float terminalVelocity = 25f;
    public float yVelocityLowerLimit = 5f;

    [Header("Action Delays and Cooldowns")]
    public float slideDelay = .5f;
    public float dodgeDelay = .5f;
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

    public bool _inSlidePhase2 = false;
    public bool _inSlidePhase1 = false;

    private bool isReversed = false;

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
        Koreographer.Instance.RegisterForEvents("SingleBeatTrack", CheckIfBpmChanged);
        LevelManager.CheckpointReverse.AddListener(flipPlayer);
    }

    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("q"))
        {
            Time.timeScale = slowDownSpeed;
        }
        if (Input.GetKeyDown("e"))
        {
            Time.timeScale = 1f;
        }
        // Check if the player is currently grounded, sets inJump to true or false (opposite of isGrounded)
        if (isGrounded = Grounded())
        {
            //_characterVelocity += Vector3.up * Physics2D.gravity.y * (regularGravityMultiplier - 1) * Time.deltaTime;
            anim.SetBool("Jumping", false);
        }
        // Only executed if the game is not frozen and the player is not in a jump
        if (_canMove && !_inJump)
        {
            // Preconditions before using a dodge. Player is not currently dashing or sliding. Player has to be able to dodge and must also be pressing dodge key
            if (!_inSlide && !_inDash && Input.GetAxis("Dodge") > 0f && _canDodge)
            {
                StartCoroutine("Dodge");
            }
            // Preconditions before using a slide. Player is not currently dashing, dodging or sliding. Player is also pressing slide key
            if ((!_inDodge && !_inDash && Input.GetAxis("Slide") > 0f) || _inSlidePhase2)
            {
                StartCoroutine("Slide");
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
            //Physics.gravity = new Vector3(0, -9.8 * gMDown, 0);
                //Debug.Log("is not grounded");
                anim.SetFloat("yHeight", _characterController.transform.position.y);
            if (_characterVelocity.y <= 0)
            _characterVelocity += Vector3.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;

            // If we haven't reached terminal velocity, apply gravity
            if (_characterVelocity.y > -terminalVelocity)
            {
                _characterVelocity.y += regularGravityMultiplier * Physics.gravity.y * Time.deltaTime;
            }
        }

            _force *= massCoeficcient;
            _characterVelocity += _force;
            //_characterController.Move(moveVector * Time.deltaTime);
            _characterController.Move((_characterVelocity) * Time.deltaTime);
    }

    IEnumerator Slide()
    {
        if (!_inSlidePhase2 && !_inSlidePhase1)
        {
            anim.SetBool("Sliding", true);
            _inSlidePhase1 = true;
            _canJump = false;
            _inSlide = true;
            yield return new WaitForSeconds(slideDelay);
            _inSlidePhase1 = false;
            _inSlidePhase2 = true;
        }
        if ((_inSlidePhase2 && !(Input.GetAxis("Slide") > 0f)))
        {
            anim.SetBool("Sliding", false);
            _inSlidePhase2 = false;
            _canJump = true;
            _inDodge = false;
            _inSlide = false;
        }
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
        Debug.Log(_characterVelocity.y);
        RaycastHit raycastHit;
        if (Physics.Raycast(this.transform.position, Vector3.down, out raycastHit, 1.2f) && Input.GetAxis(jumpAxis) > 0f)
        {
            if (raycastHit.collider.gameObject == GameObject.FindGameObjectWithTag("Flat2_VM"))
            {
                anim.SetBool("Jumping", true);
                _characterVelocity.y = 40f;
                _inJump = true;
                _canJump = false;
            }
            else
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

    private void CheckIfBpmChanged(KoreographyEvent beat)
    {
        /*if (beat.HasFloatPayload() && !(beat.Payload.Equals(currentBpm)))
        {
            currentBpm = beat.GetFloatValue();
            float newDelay = 60 / currentBpm;
            slideDelay = newDelay;
            dashDelay = newDelay;
            dodgeDelay = newDelay;
        }*/
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

    /// <summary>
    /// Flips the player on reversal point
    /// </summary>
    private void flipPlayer()
    {
        Debug.Log("reversePlayer");
        if (!isReversed)
        {
            anim.SetBool("Reversed", true);
            isReversed = true;
        }
        else
        {
            anim.SetBool("Reversed", false);
            isReversed = false;
        }
        _characterVelocity.x = -_characterVelocity.x;
        GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
        moveVector.x = -moveVector.x;
    }
}
