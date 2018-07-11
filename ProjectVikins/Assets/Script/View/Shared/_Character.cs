﻿using Assets.Script.Controller;
using Assets.Script.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Script.View.Shared
{
    public class _Character : MonoBehaviour
    {
        public PlayerController playerController;
        public Utils utils = new Utils();

        [HideInInspector] public Animator PlayerAnimator;

        [HideInInspector] public SpriteRenderer PlayerSpriteRenderer;

        [HideInInspector] public BoxCollider2D PlayerBoxCollider2D;

        [HideInInspector] public BoxCollider2D colliderTransform;
        [HideInInspector] public KeyMove input = new KeyMove(null, new Vector2(), false);
        [HideInInspector] public bool isPlayable;
        public GameObject FieldOfViewObj;
        public float DistanceOfPlayer;
        [HideInInspector] public GameObject camera;
        [HideInInspector] public CameraView cv;

        public Models.PlayerViewModel model;

        public Helpers.CountDown changeCharacterCountDown = new Helpers.CountDown();

        private void Start()
        {
            #region GetComponents
            PlayerAnimator = gameObject.GetComponent<Animator>();
            PlayerSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            var allBoxColliders = GetComponents<BoxCollider2D>();
            colliderTransform = allBoxColliders.Single(x => x.isTrigger == false);
            PlayerBoxCollider2D = allBoxColliders.Single(x => x.isTrigger == true);
            #endregion
            
            camera = GameObject.FindGameObjectWithTag("camera");
            cv = camera.GetComponent<CameraView>();
            playerController = new PlayerController(this.gameObject);
            model = playerController.GetInitialData(gameObject);
            playerController.SetFieldOfView(FieldOfViewObj.GetComponent<FieldOfView>());
            if (model.IsBeingControllable) camera.SendMessage("UpdatePlayerTranform");
        }
        public void CharacterUpdate()
        {
            CountDown.DecreaseTime(changeCharacterCountDown);
            CountDown.DecreaseTime(playerController.followEnemy);

            var tempIsControllable = playerController.GetIsControllable();

            if (isPlayable != tempIsControllable)
                changeCharacterCountDown.StartToCount();

            isPlayable = tempIsControllable;

            if (isPlayable)
            {
                #region Mover

                input.Vector2 = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

                foreach (var keyMove in utils.moveKeyCode)
                {
                    if (Input.GetKey(keyMove.KeyCode.Value))
                    {
                        PlayerAnimator.SetBool("isWalking", true);

                        playerController.SetLastMoviment(input.Vector2.x, input.Vector2.y);
                        PlayerAnimator.SetFloat("speedX", input.Vector2.x);
                        PlayerAnimator.SetFloat("speedY", input.Vector2.y);

                        playerController.Walk(keyMove.Vector2);
                        if (!keyMove.Flip.HasValue) continue;
                        PlayerSpriteRenderer.flipX = keyMove.Flip.Value;
                    }
                    if (Input.GetKeyUp(keyMove.KeyCode.Value))
                    {
                        PlayerAnimator.SetBool("isWalking", false);
                    }
                }

                #endregion

                #region Change Character

                if (changeCharacterCountDown.CoolDown <= 0 && Input.GetKeyDown(KeyCode.K))
                {
                    playerController.ChangeControllableCharacter();

                    camera.SendMessage("UpdatePlayerTranform");
                }

                #endregion
            }

            else
            {
                #region Follow

                if (model.PlayerMode == PlayerModes.Follow)
                {
                    if (Mathf.Abs(Vector3.Distance(transform.position, cv.playerGameObj.transform.position)) > DistanceOfPlayer)
                    {
                        playerController.WalkToPlayer(transform, cv.playerGameObj.transform, ref model);
                        PlayerAnimator.SetBool("isWalking", true);
                    }
                    else
                        PlayerAnimator.SetBool("isWalking", false);
                }

                #endregion

                #region Walk Input

                input = playerController.GetInput();
                PlayerSpriteRenderer.flipX = input.Flip.Value;
                PlayerAnimator.SetFloat("speedX", input.Vector2.x);
                PlayerAnimator.SetFloat("speedY", input.Vector2.y);

                #endregion
                
                if (Vector3.Distance(transform.position, cv.playerGameObj.transform.position) > 15)
                {
                    model.PlayerMode = PlayerModes.Follow;
                }
            }
            
            transform.position = Utils.SetPositionZ(transform, colliderTransform.bounds.min.y);
            
            #region Change PlayerMode

            foreach (var playerMode in utils.playerModes)
            {
                if (Input.GetKey(playerMode.KeyButton[0]) && Input.GetKey(playerMode.KeyButton[1]))
                {
                    model.PlayerMode = playerMode.Value;
                    if (playerMode.Value == PlayerModes.Wait)
                        PlayerAnimator.SetBool("isWalking", false);
                }
            }

            #endregion
        }

        public void GetDamage(int damage)
        {
            model.Life -= damage;
            if (model.Life <= 0)
                Destroy(this.gameObject);
        }

    }
}
