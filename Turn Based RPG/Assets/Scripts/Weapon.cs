using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Transform weaponTransform;
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = weaponTransform.position;
        gameObject.transform.rotation = weaponTransform.rotation;
    }
}
