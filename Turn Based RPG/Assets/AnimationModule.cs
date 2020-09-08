﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationModule : MonoBehaviour
{

    Animator animator;
    [SerializeField] float normalizedTime = 0f;
    [SerializeField] bool animatiorIsTransitioning = false;
    [SerializeField] bool animationPlaying;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

	private void Start()
	{
        SetCombatState(true);
	}

	// Update is called once per frame
	void Update()
    {
        normalizedTime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        animatiorIsTransitioning = animator.IsInTransition(0);

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && animator.IsInTransition(0) == false)
        {
            animationPlaying = false;
        }
        else animationPlaying = true;
    }

    public void SetCombatState(bool combat)
	{
        animator.SetBool("InCombat", combat);
	}

    public void TriggerAttack(string attackAnimationName)
	{
        animator.SetBool("Blocking", false);
        animator.SetTrigger(attackAnimationName);
	}

    public void CancelAttack()
	{
        animator.SetTrigger("CancelAttack");
	}

    public void SetBlock(bool blocking)
	{
        animator.SetBool("Blocking", false);
	}

    public void SetWalking(bool walking)
	{
        animator.SetBool("Walking", walking);
	}

    public bool IsAnimationPlaying()
	{
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && animator.IsInTransition(0) == false)
        {
            return false;
        }
        else return true;

	}

    public bool IsAnimationPlaying(string animationName)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }
}
