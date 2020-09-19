using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    public enum Type
	{
        Blaster,
        Rifle,
        Blade,
        Staff,
        Lightsaber
	}

    public Type type;

    [SerializeField] Transform secondHandHandle = null;
    Transform handTransform;

    WeaponStatistics statistics;
    IWeaponBehaviour weaponBehaviour;

    public AnimatorOverrideController animatorOverride;

    [Space(10)]
    [Header("Actions")]
    [SerializeField] CombatAction lightAttack;
    [SerializeField] CombatAction mediumAttack;
    [SerializeField] CombatAction strongAttack;

    [SerializeField] List<CombatAction> abilities = new List<CombatAction>();
    [SerializeField] List<CombatAction> combos = new List<CombatAction>();


    public Dictionary<BaseAttackType, CombatAction> Attacks
	{
        get
        {
            return new Dictionary<BaseAttackType, CombatAction>
            {
                {BaseAttackType.Light, lightAttack },
                {BaseAttackType.Medium, mediumAttack },
                {BaseAttackType.Strong, strongAttack }
            };

        }
	}
    public List<CombatAction> Abilities
	{
		get { return abilities; }
	}
    public List<CombatAction> Combos
	{
		get { return combos; }
	}
    

    void Awake()
    {
        statistics = GetComponent<WeaponStatistics>();
        weaponBehaviour = GetComponent<IWeaponBehaviour>();
           
    }

    public Transform SetHand(Transform hand)
	{
        handTransform = hand;
		transform.position = handTransform.position;
		transform.rotation = handTransform.rotation;
		transform.SetParent(handTransform);
        return secondHandHandle;
	}

    public void ActivateWeapon()
	{
        weaponBehaviour.Activate();
	}

}
