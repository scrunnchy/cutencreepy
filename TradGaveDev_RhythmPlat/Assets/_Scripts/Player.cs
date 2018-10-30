using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer
{
    //[RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {

        public enum CharacterState
        {
            frozen,
            idle,
            moving
        }
        Animator anim;
        PlayerControl pc;

        #region Fields and properties
        [Header("Player Information")]
        public int playerHealth;
        public float delayBetweenBlinks;
        public float killZone;

        //private variables
        private bool isGrounded;
        private Vector3 _characterVelocity;
        private Vector3 moveVector;
        private CharacterController _characterController;
        #endregion

        private void Awake()
        {
            //initialize
            pc = GetComponent<PlayerControl>();
            anim = GetComponent<Animator>();
        }
        void Start()
        {
            //initialize
            _characterController = pc.GetCharacterController();
            isGrounded = pc.isGrounded;
            _characterVelocity = pc.GetCharacterVelocity();
            moveVector = pc.GetMoveVector();

            //listeners
            Enemy.enemyPlayerCollision.AddListener(DecrementHealth);
        }

        private void Update()
        {
            //Check if player has fallen into the kill zone
            if (!isGrounded)
            {
                checkForKillZone();
            }
        }

        /// <summary>
        /// Decrements health
        /// </summary>
        private void DecrementHealth()
        {
            if (playerHealth > 1)
            {
                playerHealth -= 1;

                int numberOfBlinks = 3;
                while (numberOfBlinks < 0)
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                    IEnumerator wait = waiter();
                    GetComponent<SpriteRenderer>().enabled = false;
                    numberOfBlinks -= 1;
                }

                StartCoroutine(waiter());

            }

            else
            {
                //Debug.Log("Dead");
                SceneManager.LoadScene(2);
            }
        }

        private void checkForKillZone()
        {
            if (_characterController.transform.position.y <= killZone)
            {
                Debug.Log("Kill Zone");
                SceneManager.LoadScene(2);
            }
        }

        /// <summary>
        /// Flashes player sprite
        /// </summary>
        /// <returns></returns>
        private IEnumerator waiter()
        {
            GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(delayBetweenBlinks);
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
