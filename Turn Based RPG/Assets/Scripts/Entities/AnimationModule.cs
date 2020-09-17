using System.Collections;
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
        animator.SetBool("Defending", false);
        animator.SetTrigger(attackAnimationName);
	}

    public void CancelAttack()
	{
        animator.SetTrigger("CancelAttack");
	}

    public void SetDefend(bool defending)
	{
        animator.SetBool("Defending", defending);
	}

    public void SetWalking(bool walking, float speed = 1f)
	{
        animator.SetFloat("WalkingSpeed", speed);
        animator.SetBool("Walking", walking);

        if (walking == false)
            animator.SetFloat("WalkingSpeed", 1f);

    }

    public void PerformAbility(string abilityName)
	{
        animator.SetBool("PerformingAbility", true);
        animator.SetTrigger(abilityName);
	}

    public void AbilityEnded()
	{
        animator.SetBool("PerformingAbility", false);
	}

    public void ReceivedDamage()
	{
        animator.SetTrigger("TakeDamage");
	}

    public void BlockedAttack()
	{
        animator.SetTrigger("Block");
	}

    public void AvoidedAttack()
	{
        animator.SetTrigger("Evade");
    }

    public void Jump()
	{
        animator.SetBool("InAir", true);
        animator.SetTrigger("JumpStart");
	}

    public void JumpEnd()
	{
        animator.SetBool("InAir", false);
    }

    //Animation that loops is not playing
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
