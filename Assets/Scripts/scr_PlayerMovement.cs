using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_PlayerMovement : MonoBehaviour
{
    [SerializeField, Range(0, 10)] private float speed;
    private Vector2 targetPosition;
    [HideInInspector]
    public bool isMoving;
    private Animator animator;
    private SpriteRenderer sr;

    [Header("Elevators:")]
    //Определение этажа пребывания персонажа:
    private float currentY;
    private float floor1_Y = -4.19f;
    private float floor2_Y = -1.81f;
    private float floor3_Y = 0.58f;
    private float floor4_Y = 3.01f;

    private const float level_1 = 0;
    [SerializeField] private GameObject elevator_1;
    private const float level_2 = 1;
    [SerializeField] private GameObject elevator_2;
    private const float level_3 = 2;
    [SerializeField] private GameObject elevator_3;
    private const float level_4 = 3;
    [SerializeField] private GameObject elevator_4;
    private GameObject currentElevetor;
    private float currentLevel;
    private float targetLevel;
    private bool Up;

    [Header("HidingFloors:")]
    [SerializeField] private GameObject vignette1;
    [SerializeField] private GameObject vignette2;
    [SerializeField] private GameObject vignette3;
    [SerializeField] private GameObject vignette4;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        currentLevel = level_1;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentLevel)
        {
            case level_1:
                currentElevetor = elevator_1;
                currentY = floor1_Y;
                vignette1.SetActive(false);
                vignette2.SetActive(true);
                vignette3.SetActive(true);
                vignette4.SetActive(true);
                break;
            case level_2:
                currentY = floor2_Y;
                currentElevetor = elevator_2;
                vignette1.SetActive(true);
                vignette2.SetActive(false);
                vignette3.SetActive(true);
                vignette4.SetActive(true);
                break;
            case level_3:
                currentY = floor3_Y;
                currentElevetor = elevator_3;
                vignette1.SetActive(true);
                vignette2.SetActive(true);
                vignette3.SetActive(false);
                vignette4.SetActive(true);
                break;
            case level_4:
                currentY = floor4_Y;
                currentElevetor = elevator_4;
                vignette1.SetActive(true);
                vignette2.SetActive(true);
                vignette3.SetActive(true);
                vignette4.SetActive(false);
                break;
        }

        if (isMoving)
        {
            Move();
            animator.SetBool("Moving", true);
        }
        else animator.SetBool("Moving", false);
    }

    public void SetTargetPosition(bool click, Vector2 mousePos)
    {
        if (click)
        {
            targetPosition = Camera.main.ScreenToWorldPoint(mousePos);
            FloorChecker();         
        }
    }

    private void Move()
    {
        if (Up)
        {
            Vector2 elevatorPosition = new Vector2(currentElevetor.transform.position.x, currentY);
            transform.position = Vector2.MoveTowards(transform.position, elevatorPosition, speed * Time.deltaTime);

            if (elevatorPosition.x > transform.position.x)
            {
                sr.flipX = true;
            }
            else sr.flipX = false;

            if (Mathf.Abs(transform.position.x - elevatorPosition.x) <= 0.05f)
            {
                animator.SetBool("Elevator", true);
                currentLevel = targetLevel;
                transform.position = new Vector2(transform.position.x, currentElevetor.transform.position.y);
                if(Vector2.Distance(transform.position, currentElevetor.transform.position) <= 0.005f)
                {
                    Up = false;
                }
            }
        }
        else 
        {
            targetPosition.y = currentY;
            animator.SetBool("Elevator", false);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if (targetPosition.x > transform.position.x)
            {
                sr.flipX = true;
            }
            else sr.flipX = false;

            if (Mathf.Abs(transform.position.x - targetPosition.x) <= 0.05f)
            {
                isMoving = false;
            }
        }
    }

    private void FloorChecker()
    {
        if (targetPosition.y >= floor2_Y)
        {
            if (targetPosition.y >= floor3_Y)
            {
                if (targetPosition.y >= floor4_Y)
                {
                    targetLevel = level_4;
                }
                else targetLevel = level_3;
            }
            else targetLevel = level_2;
        }
        else targetLevel = level_1;

        if(currentLevel != targetLevel)
        {
            isMoving = true;
            Up = true;
        }
        else
        {
            targetPosition.y = currentY;
            isMoving = true;
        }
    }
}
