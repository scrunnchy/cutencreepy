using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using UnityEngine.Events;
using Platformer;

public class Enemy : MonoBehaviour {
    // track if enemy is expended from hitting player
    private bool isExpended;
    // track if enemy is in the reversed state
    private bool isReversed = false;
    //Produces the behaviour that an enemy will no longer be a threat or visible during reversals
    public bool DisapearsAtReverse;
    //Produces the behaviour that an enemy will only become visible and a threat during reversals
    public bool AppearsAtReverse;
    //determine what enemy this is by
    //associating to key 0-4: 0 - BeatBat, 1 - FunkyFrankenstein, 2 - MelodyMummy,
    //3 - SalsaSpider, 4 - ZumbaZombie
    public int enemyTypeKey;
    //store sprites from folder across all enemies.
    private static Dictionary<string, Sprite> spriteSet;
    //store reference to sprite renderer component
    private SpriteRenderer spriteR;
    //store reference to box collider component
    private BoxCollider boxC;
    //reference particle generator transform
    private Transform particlesT;

    private Animator animator;

    private GameObject[] pieces;
    private void Awake()
    {

        spriteR = GetComponent<SpriteRenderer>();
        boxC = GetComponent<BoxCollider>();
        animator = this.GetComponent<Animator>();
        animator.SetBool("Reversed", isReversed);
    }
    // On start, an enemy will:
    // retrieve collider component
    // register as an event listener for beats and level reversal
    void Start () {

        particlesT = transform.GetChild(0);

        //set sprites in dictionary with names
        if (spriteSet == null)
        {
            spriteSet = new Dictionary<string, Sprite>();
            foreach (Sprite s in Resources.LoadAll("_Art Assets/_Sprites/_Enemies", typeof(Sprite)))
            {
                spriteSet.Add(s.name, s);
            }
        }

        //set sprite to normal version based on type.
        if (enemyTypeKey == 0)
        {
            spriteR.sprite = spriteSet["BEAT_BAT"];
        }
        else if (enemyTypeKey == 1)
        {
            spriteR.sprite = spriteSet["FUNKY_FRANKENSTEIN"];
        }
        else if (enemyTypeKey == 2)
        {
            spriteR.sprite = spriteSet["MELODY_MUMMY"];
        }
        else if (enemyTypeKey == 3)
        {
            spriteR.sprite = spriteSet["SALSA_SPIDER"];
        }
        else if (enemyTypeKey == 4)
        {
            spriteR.sprite = spriteSet["ZUMBA_ZOMBIE"];
        }
        
        //Register for reversal.
        LevelManager.CheckpointReverse.AddListener(ChangeAppearanceOnReverse);

        pieces = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            pieces[i] = transform.GetChild(i).gameObject;
        }

        //disappear if enemy is to appear during reversals
        if (AppearsAtReverse)
        {
            boxC.enabled = false;
            spriteR.enabled = false;
            //turn off children
            foreach (GameObject go in pieces)
            {
                ParticleSystem tempP = go.GetComponent<ParticleSystem>();
                if (tempP != null)
                {
                    tempP.Pause();
                }
                SpriteRenderer tempS = go.GetComponent<SpriteRenderer>();
                if (tempS != null)
                {
                    tempS.enabled = false;
                }
                BoxCollider tempB = go.GetComponent<BoxCollider>();
                if (tempB != null && tempB.isTrigger)
                {
                    tempB.enabled = false;
                }
            }
        }

        //indicate that the enemy starts out normally
        isReversed = false;
    }
	
	// Once per frame, 
	void Update () {
        if (isReversed)
        {
            this.animator.SetBool("Reversed", true);
        }
        else
        {
            this.animator.SetBool("Reversed", false);
        }
	}

    /// <summary>
    /// Check for player within danger zone box collider
    /// If player detected AND they are not dodging with the correct dodge, trigger damage event.
    /// an enemy will only bbe able to deal damage once, and then it is expended until reversal.
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerStay(Collider collider)
    {
        if (!isExpended && collider.gameObject.tag == "Player")
        {
            PlayerControl playerInfo = collider.gameObject.GetComponent<PlayerControl>();
            bool gotHit = false;

            if (enemyTypeKey == 0 && !playerInfo._inSlide) // bat
            {
                gotHit = true;
            }
            else if ((enemyTypeKey == 1 || enemyTypeKey == 2) && !playerInfo._inDodge) // mummy or frankenstein
            {
                gotHit = true;
            }
            else if (enemyTypeKey == 3 && !playerInfo._inJump) // spider
            {
                gotHit = true;
            }
            else if (enemyTypeKey == 4 && !playerInfo._inDodge) // zombie
            {
                gotHit = true;
            }
            // check if the layer is in the specific avoid state that matches the enemy type.
            if (gotHit)
            {
                // trigger Damage event
                LevelManager.enemyPlayerCollision.Invoke();
                isExpended = true;
            }
        }
    }

    /// <summary>
    /// Triggers as soon as the reversal point is reached.
    /// Flips the enemy sprite such that it faces the player. 
    /// resets the enemy to be no longer expended. 
    /// 
    /// Will also manage appearing/dissappearing the enemy based on settings.
    /// </summary>
    private void ChangeAppearanceOnReverse() 
    {

        //appear/disapear based on state depending on settings
        if (AppearsAtReverse || DisapearsAtReverse)
        {
            if (spriteR.enabled)
            {
                boxC.enabled = false;
                spriteR.enabled = false;
            }
            else
            {
                boxC.enabled = true;
                spriteR.enabled = true;
            }
            foreach (GameObject go in pieces)
            {
                //swap relevant components to active or innactive based on current state
                SpriteRenderer tempS = go.GetComponent<SpriteRenderer>();
                if (tempS != null)
                {
                    tempS.enabled = !tempS.enabled;
                }
                BoxCollider tempB = go.GetComponent<BoxCollider>();
                if (tempB != null && tempB.isTrigger)
                {
                    tempB.enabled = !tempB.enabled;
                }
                ParticleSystem tempP = go.GetComponent<ParticleSystem>();
                if (tempP != null)
                {
                    if (tempP.isPaused)
                    {
                        tempP.Play();
                    }
                    else
                    {
                        tempP.Pause();
                    }
                }
            }
        }
        // take action depending on enemy's current state 
        if (isReversed)
        {
            if (this.isActiveAndEnabled)
            {
                animator.SetBool("Reversed", false);
            }
            //toggle orientation
            isReversed = false;
        }
        else
        {
            if (this.isActiveAndEnabled)
            {
                //Debug.Log("isReversed");
                Debug.Log(this.gameObject.name);
                this.animator.SetBool("Reversed", true);
            }
            //toggle orientation
            isReversed = true;
            //flip the sprite direction accordingly
            spriteR.flipX = false;
            
        }
        //flip particle effect.
        particlesT.Translate(3f, 0f, 0f);
        particlesT.Rotate(180f, 180f, 0f);
        isExpended = false;
    }
}
