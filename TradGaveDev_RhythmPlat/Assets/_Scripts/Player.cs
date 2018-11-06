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
        public float durationOfParticles;
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
            Goal.playerGoalReached.AddListener(LoadWinScreen);
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

                //play damage animation (white sprite)
            }
            else
            {
                //Debug.Log("Dead");
                SceneManager.LoadScene(2);
            }
        }

        /// <summary>
        /// Handles alll visual effects and animations for player after they reach the goal.
        /// Stops the music for the final moments (to be changed, perhaps)
        /// Then, it loads the end scene.
        /// </summary>
        private void LoadWinScreen()
        {
            
            Debug.Log("goalReached");
            //stop level music, potentially. 
            AudioSource song = (Camera.main).GetComponent<AudioSource>();
            if(song != null)
            {
                song.Pause();
            }

            //TODO: play animation or que with visual effect


            //load the True ending scene "VictoryScreen"
            SceneManager.LoadScene("VictoryScreen");
        }

        private void checkForKillZone()
        {
            if (_characterController.transform.position.y <= killZone)
            {
                Debug.Log("Kill Zone");
                SceneManager.LoadScene(2);
            }
        }
        
    }
}
