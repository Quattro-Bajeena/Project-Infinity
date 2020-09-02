using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityScript : MonoBehaviour
{
   

    public string entityName;

    public StatisticsScript stats;
    public CombatScript combat;
    public AnimationManager animations;


    void Awake()
    {
        stats = GetComponent<StatisticsScript>();
        combat = GetComponent<CombatScript>();
        animations = GetComponent<AnimationManager>();
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
