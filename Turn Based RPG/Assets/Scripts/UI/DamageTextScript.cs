using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DamageTextScript : MonoBehaviour
{
    [SerializeField] float timeVisible;
    [SerializeField] float speed;
    [SerializeField] Vector2 offset;
    



    Color textColor;
    Camera mainCamera;
    TextMeshPro text;


    
   // public Vector2 direction;
   // public Vector2 directionWorld;


    public void SetPosition(Vector3 characterPosition)
	{
        Vector3 direction = mainCamera.transform.position - characterPosition;

        transform.position = characterPosition + direction.normalized * offset.x + Vector3.up * offset.y;
        
	}

    public void Initialize(float amount)
    {
        text = GetComponent<TextMeshPro>();

        if (amount == 0)
        {
            //Attack Miss
            textColor = Color.white;
            text.text = "miss";

        }
        else if (amount > 0)
        {
            //Heal
            textColor = Color.green;
            text.text = Math.Abs(amount).ToString();
        }
        else if (amount < 0)
        {
            //Damage
            textColor = Color.red;
            text.text = Math.Abs(amount).ToString();
        }
        
        text.color = textColor;

    }

    public void InitializeBlocked()
	{
        text = GetComponent<TextMeshPro>();
        text.text = "blocked";
        text.color = Color.white;

        speed *= 1.05f;
        offset.y += 0.4f;
    }

	private void Awake()
	{
        mainCamera = Camera.main;
    }

	void Start()
    {
        
        Destroy(this.gameObject, timeVisible);

        //Vector2 worldPosition = mainCamera.WorldToViewportPoint(transform.position);
        //direction = new Vector2(worldPosition.x, 1);
        //directionWorld = mainCamera.ViewportToWorldPoint(direction);
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
