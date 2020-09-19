using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementModule : MonoBehaviour
{
    Entity entity;

    [SerializeField] float speed = 5f;
    [SerializeField] float turnTime = 0.3f;
    [SerializeField] float gravity = 1;

    CharacterController controller;

    Vector3 orgPosition;
    Quaternion orgRotation;

    public bool IsMoving
	{
		get { return controller.velocity != Vector3.zero; }
	}
    
    void Awake()
    {
        entity = GetComponent<Entity>();
        controller = GetComponent<CharacterController>();
    }

	private void Start()
	{
        orgPosition = transform.position;
        orgRotation = transform.rotation;
	}


	void Update()
    {
        Gravity();
    }

    void Gravity()
	{
        if (controller.isGrounded == false)
		{
            controller.Move(new Vector3(0, -gravity * Time.deltaTime, 0));
        }
        
	}

    public void ReturnToDefault(float speedModifier = 1, float turnTimeModifier = 1)
	{
        StartCoroutine(ReturnToDefaultCoroutine(speedModifier, turnTimeModifier));
	}

    public void TurnToDefault(float timeMultiplier = 1)
    {
        StartCoroutine(TurnInTimeCoroutine(orgRotation, timeMultiplier));
    }

    public void TurnInTime(Quaternion targetRotation, float timeMultiplier = 1)
    {
        StartCoroutine(TurnInTimeCoroutine(targetRotation, timeMultiplier));
    }

    public void MoveToTarget(Vector3 moveTarget, Vector3 lookTarget, float speedModifier = 1)
    {
        Vector3 targetGroundPosition = new Vector3(moveTarget.x, transform.position.y, moveTarget.z);
        Vector3 lookTargetGroundPosition = new Vector3(lookTarget.x, transform.position.y, lookTarget.z);
        StartCoroutine(MoveToTargetCoroutine(targetGroundPosition, lookTargetGroundPosition, speedModifier));
    }

    IEnumerator TurnInTimeCoroutine(Quaternion targetRotation, float timeMultiplier)
    {
        Quaternion startRotation = transform.rotation;
        float percent = 0;
        while (percent < 1)
        {
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, percent);
            percent += Time.deltaTime / (turnTime * timeMultiplier);
            yield return null;

        }
        transform.rotation = targetRotation;
    }

    IEnumerator MoveToTargetCoroutine(Vector3 moveTarget, Vector3 lookTarget, float speedModifier)
	{
        entity.animations.SetWalking(true, speedModifier);

        float distance = Vector3.Distance(transform.position, moveTarget);
        Vector3 direction;
        transform.LookAt(moveTarget);

        while (distance > 0.1f)
		{
            direction = Vector3.Normalize(moveTarget - transform.position);
            controller.Move(direction * speed * speedModifier * Time.deltaTime);

            distance = Vector3.Distance(transform.position, moveTarget);
            

            yield return null;
        }

        transform.LookAt(lookTarget);
        controller.Move(moveTarget - transform.position);

        entity.animations.SetWalking(false);
    }

    IEnumerator ReturnToDefaultCoroutine(float speedModifier, float turnTimeModifier)
	{
        Vector3 targetGroundPosition = new Vector3(orgPosition.x, transform.position.y, orgPosition.z);

        yield return StartCoroutine(MoveToTargetCoroutine(targetGroundPosition, targetGroundPosition, speedModifier));
        yield return StartCoroutine(TurnInTimeCoroutine(orgRotation, turnTimeModifier));
	}
}
