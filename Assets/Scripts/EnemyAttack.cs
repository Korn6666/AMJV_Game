﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float swordAttackCoolDown = 1;
    [SerializeField] private float swordAttackAnimationTime = 0.03f; //Temps d'animation    
    [SerializeField] private float swordAttackDamages = 5;    

    public bool canAttack = false;
    private GameObject player;
    private Animator Animator;
    private bool firstAttack = true;




    void Start()
    {
        StartCoroutine(WaitAndAttack()); 
        Animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            firstAttack = true;
            canAttack = true;
            player = collision.gameObject;
            Animator.SetBool("onPlayerContact", true);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            firstAttack = false;
            canAttack = false;
            Animator.SetBool("onPlayerContact", false);
        }
    }
    public void SwordAttack()
    {
        player.GetComponent<PlayerHealth>().TakeDamage(swordAttackDamages);
    }

    private IEnumerator WaitAndAttack()
    {
        while (true)
        {
            if (firstAttack) { yield return new WaitForSeconds(swordAttackCoolDown - swordAttackAnimationTime); }

            if (canAttack)
            {
                Animator.SetBool("attack", true);
                yield return new WaitForSeconds(swordAttackAnimationTime); //Histoire d'attendre de faire les degats quand l'épée touche le joueur
                SwordAttack();
                Animator.SetBool("attack", false);
                yield return new WaitForSeconds(swordAttackCoolDown - swordAttackAnimationTime);
            }
            yield return null;
        }
    }
}
