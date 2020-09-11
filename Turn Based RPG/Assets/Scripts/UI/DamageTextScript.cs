using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DamageTextScript : MonoBehaviour
{
    public float timeVisible;
    public float speed;
    Color textColor;
    Camera mainCamera;

    

    public Vector2 direction;
    public Vector2 directionWorld;


    public void Initialize(float amount)
    {
        
        if(amount == 0)
        {
            Debug.Log("DamageTextScript - no damage, object destroyed");
            Destroy(this.gameObject);
        }
        else if (amount > 0)
        {
            //Heal
            textColor = Color.green;
        }
        else if (amount < 0)
        {
            //Damage
            textColor = Color.red;
        }

        //Text text = GetComponent<Text>();
        TextMeshPro text = GetComponent<TextMeshPro>();
        
        
        text.text = Math.Abs(amount).ToString();
        text.color = textColor;

    }

    void Start()
    {
        mainCamera = Camera.main;
        Destroy(this.gameObject, timeVisible);

        Vector2 worldPosition = mainCamera.WorldToViewportPoint(transform.position);
        direction = new Vector2(worldPosition.x, 1);
        directionWorld = mainCamera.ViewportToWorldPoint(direction);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = mainCamera.transform.rotation;

        //transform.position = Vector3.MoveTowards(transform.position, directionWorld, speed);
        transform.position += (Vector3.up * speed * Time.deltaTime);

        //transform.position = Vector3.MoveTowards(transform.position, camera.transform.position, speed);
        if(speed > 0)
        {
            speed -= speed * 0.95f * Time.deltaTime;
        }
    }
}
