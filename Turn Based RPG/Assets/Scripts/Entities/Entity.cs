using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
   

    public string entityName;

    public StatisticsModule stats;
    public CombatModule combat;
    public AnimationModuleOld_ animationsOld_;
    public AnimationModule animations;


    void Awake()
    {
        stats = GetComponent<StatisticsModule>();
        combat = GetComponent<CombatModule>();
        animationsOld_ = GetComponent<AnimationModuleOld_>();
        animations = GetComponent<AnimationModule>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
}
