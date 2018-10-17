using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SonicBloom.Koreo;
using UnityEngine.Events;
using Platformer;

public class Enemy : MonoBehaviour {
    // store enemy collider and isExpended
    private bool isExpended;
    //Produces the behaviour that an enemy will no longer be a threat or visible after a reversal event
    public bool DisapearsAtReverse;
    //Produces the behaviour that an enemy will only become visible and a threat after a reversal event
    public bool AppearsAtReverse;
    // create event for damage "enemyPlayerCollision"
    public static UnityEvent enemyPlayerCollision = new UnityEvent();
    //determine what enemy this is by
    //associating to key 0-4: 0 - BeatBat, 1 - FunkyFrankenstein, 2 - MelodyMummy,
    //3 - SalsaSpider, 4 - ZumbaZombie
    public int enemyTypeKey;
    //store sprites from folder across all enemies.
    private static Dictionary<string, Sprite> spriteSet;
    //store reference to sprite renderer component
    private SpriteRenderer spriteR;

    // On start, an enemy will:
    // retrieve collider component
    // register as an event listener for beats and level reversal
    void Start () {
        spriteR = GetComponent<SpriteRenderer>();
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

        //Register for koreography beats.
        Koreographer.Instance.RegisterForEvents("SingleBeatTrack", IdleAnimation);
        //Register for reversal.
        Checkpoint.CheckpointReverse.AddListener(TurnAroundAndChangeTexture);
        //disappear if enemy is to appear later
        if (AppearsAtReverse)
        {
            GetComponent<BoxCollider>().enabled = false;
            spriteR.enabled = false;
        }
    }
	
	// Once per frame, 
	void Update () {

	}

    /// <summary>
    /// Check for player within danger zone box collider
    /// If player detected AND they are not dodging, trigger damage event.
    /// an enemy will only bbe able to deal damage once, and then it is expended until reversal.
    /// </summary>
    /// <param name="collider"></param>
    private void OnTriggerStay(Collider collider)
    {
        if (!isExpended && collider.gameObject.tag == "Player")
        {
            Player playerInfo = collider.gameObject.GetComponent<Player>();
            // check if the layer is in any valid avoid state.
            if (!(playerInfo._inDodge || playerInfo._inDash || playerInfo._inJump || playerInfo._inSlide)) 
            {
                Debug.Log("damage dealt");
                //trigger Damage event
                enemyPlayerCollision.Invoke();
                isExpended = true;
            }
        }
    }

    /// <summary>
    /// On the relevant beat, an enemy will initiate its dance animation.
    /// </summary>
    /// <param name="beat"></param>
    private void IdleAnimation(KoreographyEvent beat)
    {
        ////Dance (blink on and off)
        //SpriteRenderer enemySpriteRen = (SpriteRenderer)GetComponent("SpriteRenderer");
        //enemySpriteRen.enabled = enemySpriteRen.enabled ? false : true;
    }

    /// <summary>
    /// Triggers as soon as the reversal point is reached.
    /// Flips the enemy sprite such that it is facing the player.
    /// Once flipped, the sprite will not re-flip. 
    /// resets the enemy to be no longer expended. 
    /// 
    /// Will also manage appearing/dissappearing the enemy based on settings.
    /// </summary>
    private void TurnAroundAndChangeTexture() 
    {
        //set sprite to reverse version based on type.
        if (enemyTypeKey == 0)
        {
            spriteR.sprite = spriteSet["R_BEAT_BAT"];
        }
        else if (enemyTypeKey == 1)
        {
            spriteR.sprite = spriteSet["R_FUNKY_FRANKENSTEIN"];
        }
        else if (enemyTypeKey == 2)
        {
            spriteR.sprite = spriteSet["R_MELODY_MUMMY"];
        }
        else if (enemyTypeKey == 3)
        {
            spriteR.sprite = spriteSet["R_SALSA_SPIDER"];
        }
        else if (enemyTypeKey == 4)
        {
            spriteR.sprite = spriteSet["R_ZUMBA_ZOMBIE"];
        }
        //if it has not already been flipped
        if (!spriteR.flipX) 
        {
            spriteR.flipX = true;
        }
        //appear if necessary
        if (AppearsAtReverse)
        {
            GetComponent<BoxCollider>().enabled = true;
            spriteR.enabled = true;
        }
        //dissapear if necessary
        if (DisapearsAtReverse)
        {
            GetComponent<BoxCollider>().enabled = false;
            spriteR.enabled = false;
        }
        isExpended = false;
    }
}
