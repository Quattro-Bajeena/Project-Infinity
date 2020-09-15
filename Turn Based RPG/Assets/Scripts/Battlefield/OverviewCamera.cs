using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverviewCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vector3 battlefieldCenterPosition = GameObject.Find("BattlefieldCenter").transform.position;
        transform.position = new Vector3(battlefieldCenterPosition.x, transform.position.y, battlefieldCenterPosition.z);

    }

    
}
