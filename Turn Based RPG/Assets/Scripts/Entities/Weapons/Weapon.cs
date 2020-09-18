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

    [SerializeField] List<GameObject> abilitiesPrefab = new List<GameObject>();
    [SerializeField] List<GameObject> combosPrefab = new List<GameObject>();

    [SerializeField] GameObject lightAttack;
    [SerializeField] GameObject mediumAttack;
    [SerializeField] GameObject strongAttack;

    public List<CombatAction> abilities { get; private set; } = new List<CombatAction>();
    public Dictionary<BaseAttackType, CombatAction> attacks { get; private set; } = new Dictionary<BaseAttackType, CombatAction>();
    public List<CombatAction> combos { get; private set; } = new List<CombatAction>();

    void Awake()
    {
        statistics = GetComponent<WeaponStatistics>();
        weaponBehaviour = GetComponent<IWeaponBehaviour>();

		foreach (GameObject ability in abilitiesPrefab)
		{
            GameObject newAbility = Instantiate(ability);
            newAbility.transform.SetParent(transform.Find("Abilities"));
            abilities.Add(newAbility.GetComponent<CombatAction>());
		}

        GameObject lightAttackGO =  Instantiate(lightAttack);
        lightAttackGO.transform.SetParent(transform.Find("Attacks"));
        attacks.Add(BaseAttackType.Light, lightAttackGO.GetComponent<CombatAction>());

        GameObject mediumAttackGO = Instantiate(mediumAttack);
        mediumAttackGO.transform.SetParent(transform.Find("Attacks"));
        attacks.Add(BaseAttackType.Medium, mediumAttackGO.GetComponent<CombatAction>());

        GameObject strongAttackGO = Instantiate(strongAttack);
        strongAttackGO.transform.SetParent(transform.Find("Attacks"));
        attacks.Add(BaseAttackType.Strong, strongAttackGO.GetComponent<CombatAction>());


        foreach (GameObject combo in combosPrefab)
        {
            GameObject newCombo = Instantiate(combo);
            newCombo.transform.SetParent(transform.Find("Combos"));
            combos.Add(newCombo.GetComponent<CombatAction>());
        }
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
