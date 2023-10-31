using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_BedStatus : MonoBehaviour
{
    private Animator animator;
    private bool animationSwitch;

    //статус кровати
    private const float EMPTY = 0;
    private const float IDLE = 1;
    private const float CURE = 2;
    private const float CRITICAL = 3;
    private const float NEED = 4;
    [HideInInspector]
    public float currentState;
    [HideInInspector]
    public bool isNotEmpty;

    //степень т€жести состо€ни€ пациента:
    [HideInInspector]
    public int patientCondition; //1- легкое, 2- среднее, 3- т€желое
    [HideInInspector]
    public int patientNumber;
    [HideInInspector]
    public string patientName;


    //не забыть удал€ть пациента из списка в "ћенеджере √оспитал€" после выписки, но при этом сохран€ть данные о недописанных »стори€х болезни.

    void Start()
    {
        animator = GetComponent<Animator>();
        currentState = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case EMPTY:
                if (animationSwitch)
                {
                    animator.SetBool("Empty", true);
                    isNotEmpty = false;
                    animationSwitch = false;
                }
                break;
            case IDLE:
                if (animationSwitch)
                {
                    animator.SetBool("Empty", false);
                    animator.SetBool("Curation", false);
                    isNotEmpty = true;
                    animationSwitch = false;
                }
                break;
            case CURE:
                if (animationSwitch)
                {
                    animator.SetBool("Curation", true);
                    animator.SetBool("NeedToHeal", false);
                    animator.SetBool("Critical", false);
                    animationSwitch = false;
                }
                break;
            case CRITICAL:
                if (animationSwitch)
                {
                    animator.SetBool("Critical", true);
                    animationSwitch = false;
                }
                break;
            case NEED:
                if (animationSwitch)
                {
                    animator.SetBool("Empty", false);
                    animator.SetBool("NeedToHeal", true);
                    animationSwitch = false;
                }
                break;
        }
    }

    public void StateSwitcher(int stateIndex)
    {
        currentState = stateIndex;
        animationSwitch = true;
    }
}
