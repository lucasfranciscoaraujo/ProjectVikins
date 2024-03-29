﻿ using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Script.Helpers;

namespace Assets.Script.View
{
    public class EnemyView : MonoBehaviour
    {
        [HideInInspector] public BoxCollider2D BoxCollider2D;
        [HideInInspector] public Animator EnemyAnimator;
        [HideInInspector] public SpriteRenderer EnemySpriteRenderer;

        public LayerMask playerLayer;
        BoxCollider2D colliderTransform;
        [HideInInspector] public Controller.EnemyController enemyController;
        [SerializeField] GameObject FieldOfViewGameObj;
        [HideInInspector]  public CountDown attackCountDown = new CountDown(3);
        [HideInInspector] public Models.EnemyViewModel model;
        
        void Start()
        {
            #region GetComponent
            BoxCollider2D = GetComponent<BoxCollider2D>();
            EnemyAnimator = GetComponent<Animator>();
            EnemySpriteRenderer = GetComponent<SpriteRenderer>();
            #endregion

            enemyController = new Controller.EnemyController();
            model = enemyController.GetInitialData(gameObject);
            enemyController.SetFieldOfView(FieldOfViewGameObj.GetComponent<FieldOfView>());
            colliderTransform = GetComponents<BoxCollider2D>().Where(x => x.isTrigger == false).First();
        }

        public void EnemyUpdate()
        {
            CountDown.DecreaseTime(enemyController.followPlayer);
            CountDown.DecreaseTime(attackCountDown);
            
            var input = enemyController.GetInput();
            EnemySpriteRenderer.flipX = input.Flip.Value;
            EnemyAnimator.SetFloat("speedX", input.Vector2.x);
            EnemyAnimator.SetFloat("speedY", input.Vector2.y);
        }
        private void Update()
        {
            transform.position = Utils.SetPositionZ(transform, colliderTransform.bounds.min.y);
        }

        public void GetDamage(int damage)
        {
            model.CurrentLife -= damage;
            if (model.CurrentLife <= 0)
            {
                model.IsDead = true;
                DAL.MVC_Game2Context.aliveEnemieModels.Remove(model);
                GetComponents<BoxCollider2D>().ToList().ForEach(x => x.enabled = false);
                EnemyAnimator.SetBool("isWalking", false);
                //Destroy(this.gameObject);
            }
        }
    }
}
