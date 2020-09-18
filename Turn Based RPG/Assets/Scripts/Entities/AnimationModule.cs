using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationModule : MonoBehaviour
{

    Entity entity;

    Animator animator;
    AnimatorOverrideController overrideController;

    [SerializeField] AnimationClip defaultAbilityAnimation;

    [SerializeField] float normalizedTime = 0f;
    [SerializeField] bool animatiorIsTransitioning = false;
    [SerializeField] bool animationPlaying;
    [SerializeField] string currentAnimation;




    void Awake()
    {
        entity = GetComponent<Entity>();
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

        currentAnimation = animator.GetCurrentAnimatorStateInfo(0).ToString();
    }

    public void ChangeWeapon(AnimatorOverrideController newOverrideController)
	{
        animator.runtimeAnimatorController = newOverrideController;
        overrideController = newOverrideController;
	}

    public void SetCombatState(bool combat)
	{
        animator.SetBool("InCombat", combat);
	}

    public void TriggerAttack(BaseAttackType type)
	{
        animator.SetBool("Defending", false);
        string animationName = "";
		switch (type)
		{
            case BaseAttackType.Light:
                animationName = "LightAttack";
                break;
            case BaseAttackType.Medium:
                animationName = "MediumAttack";
                break;
            case BaseAttackType.Strong:
                animationName = "StrongAttack";
                break;

            case BaseAttackType.NULL:
            default:
                return;

		}
		animator.SetTrigger(animationName);
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

    public void PerformAbility(AnimationClip animation)
	{
        
        overrideController[defaultAbilityAnimation] = animation;
        animator.SetBool("PerformingAbility", true);
    }

    public void PerformAbility(ActionAnimationInfo animationInfo)
	{
        animator.SetBool("PerformingAbility", true);
        //animator.SetTrigger(abilityName);

        
        animator.SetFloat("ActionAnimationID", (int)animationInfo.source);
        animator.SetFloat("ActionAnimationID", (int)animationInfo.category);
        animator.SetFloat("ActionAnimationID", animationInfo.id);
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

    //Animation that loops is not playing but only after first loop
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
