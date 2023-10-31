using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class scr_PatientStatus : MonoBehaviour
{
    [SerializeField] private GameObject gameManager;
    private scr_GlobalTimer globalTimerScript;
    private scr_HospitalManager hospitalManagerScript;
    [SerializeField] private scr_PlayerInput playerInputScript;
    private scr_BedStatus bedScript;
    private scr_Registrator registratorScript;

    private bool registrationIsOpen; //���� ��������� ����������� ��������
    private int firstDay; //���� �����������
    private int lastDay; // ���� �������������� �������

    private int patientCondition; //1- ������, 2- �������, 3- �������
    private bool conditionChanged; //��������� ���������
    private string personalName;
    private int personalNumber;

    [HideInInspector]
    public bool interact;
    private bool interactionAvailable;
    [SerializeField] private GameObject tips;

    //������� �������:
    [HideInInspector]
    public bool isInspectedToday; // �������� �� ���������� ������
    private int daysWithoutCure; // ���� ��� �������
    private bool curationBegin;
    private float curationTime; //����� �� �������
    private float constCureTime;


    //������� �������:
    private bool firstInspection; // ������� �� ��������� ������
    private int firstInspectionDelay; // �� ������� ���� ��������� � ���������� ���������� �������
    private int dailyInspectionSummary; //������� ��������� ���������� �� �������������� (��� ����� ������� � ���������� ����)
    private int dailyInspectionCurrent; //������� ��������� ��������
    private bool epicrisis; // ������� �� �������� �������
    private int epicrisisDelay; //�� ������� ���� ��������� � ���������� ���������� �������

    void Start()
    {
        globalTimerScript = gameManager.GetComponent<scr_GlobalTimer>();
        hospitalManagerScript = gameManager.GetComponent<scr_HospitalManager>();
        bedScript = GetComponent<scr_BedStatus>();
        registratorScript = gameManager.GetComponent<scr_Registrator>();
        curationTime = 10;
        registrationIsOpen = true;
    }

    void Update()
    {
        if (bedScript.isNotEmpty == true)
        {
            if (registrationIsOpen)
            {
                patientCondition = bedScript.patientCondition;
                personalName = bedScript.patientName;
                personalNumber = bedScript.patientNumber;
                firstDay = globalTimerScript.dayCounter;
                conditionChanged = true;
                curationTime = curationTime * patientCondition;
                registrationIsOpen = false;
            }

            HealthStatusChecker();
            TreatmentProcess();
        }
        else registrationIsOpen = true;
    }

    private void HealthStatusChecker()
    {
        if (conditionChanged)
        {
            Random daysInHospital = new Random();

            if (patientCondition == 1)
            {
                lastDay = firstDay + daysInHospital.Next(7, 11);
                dailyInspectionSummary = lastDay - firstDay - 1;
            }

            if (patientCondition == 2)
            {
                lastDay = firstDay + daysInHospital.Next(11, 18);
                dailyInspectionSummary = lastDay - firstDay - 1;
            }

            if (patientCondition == 3)
            {
                lastDay = firstDay + daysInHospital.Next(18, 21);
                dailyInspectionSummary = lastDay - firstDay - 1;
            }

            registratorScript.RegistrationOfPatient(personalNumber, personalName, patientCondition, firstDay);
            conditionChanged = false;
        }
    }

    private void TreatmentProcess()
    {
        if(globalTimerScript.dayCounter > lastDay)
        {
            // ����� ���������� ����� ��������� ������ ��������

            //bedScript.StateSwitcher(0);
        }
        else
        {
            if (globalTimerScript.dayTick)
            {
                hospitalManagerScript.needToHealToday = 0;
                hospitalManagerScript.wasHealToday = 0;
                
                if (daysWithoutCure >= 1) //����������� � ������� �����������
                {
                    hospitalManagerScript.needToHealToday += 1;
                    if (daysWithoutCure == 3)
                    {
                        bedScript.StateSwitcher(3);
                    }
                    else if (daysWithoutCure > 3)
                    {
                        if (patientCondition != 3)
                        {
                            patientCondition += 1;
                            conditionChanged = true;
                        }
                        else hospitalManagerScript.death = true;
                    }

                }
                else
                {
                    Random rnd = new Random();
                    int stability = rnd.Next(0, 101);
                    if (stability <= patientCondition * 30) // ����������� ��������� ��������� � ����������� �� ������� �������
                    {
                        bedScript.StateSwitcher(4);
                        hospitalManagerScript.needToHealToday += 1;
                        if (isInspectedToday == false)
                        {
                            daysWithoutCure += 1;
                        }
                        else
                        {
                            daysWithoutCure = 0;
                            hospitalManagerScript.wasHealToday += 1;
                        }
                    }
                }
            }

            if (isInspectedToday == false && bedScript.currentState == 4)
            {
                daysWithoutCure += 1;
            }
            else if (isInspectedToday == true && bedScript.currentState == 4)
            {
                daysWithoutCure = 0;
            }

            hospitalManagerScript.needToHealYesterday = hospitalManagerScript.needToHealToday;

            Interaction();
        }
    }

    public void Papers(bool papersDone) // �� �����������
    {
        if (papersDone)
        {
            if(firstInspection == false)
            {
                firstInspectionDelay = globalTimerScript.dayCounter - firstDay;
                if (globalTimerScript.dayCounter == firstDay)
                {
                    firstInspection = true;
                }
            }
            else
            {
                if (dailyInspectionCurrent < dailyInspectionSummary)
                {
                    dailyInspectionCurrent += 1;
                }
                else if (epicrisis == false)
                {
                    epicrisisDelay = globalTimerScript.dayCounter - lastDay;
                    epicrisis = true;
                }
            }
        }
    }

    private void Interaction()
    {
         if(interact == true && interactionAvailable)
         {
            Debug.Log($"Tiggered with {gameObject.name}");
            curationBegin = true;
            interact = false;
         }

        if(curationBegin == true)
        {
            isInspectedToday = true;
            bedScript.StateSwitcher(2);
            CureTimer();
        }

    }

    private void CureTimer()
    {
        if (curationBegin)
        {
            if (curationTime > 0)
            {
                curationTime -= Time.deltaTime;
            }
            else if (curationTime < 0)
            {
                bedScript.StateSwitcher(1);
                curationTime = constCureTime;
                hospitalManagerScript.wasHealToday += 1;
                hospitalManagerScript.wasHealYesterday = hospitalManagerScript.wasHealToday;
                curationBegin = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(bedScript.currentState == 4)
            {
                interactionAvailable = true;
                tips.SetActive(true);
            }
            playerInputScript.interactiveObject = gameObject;
            Debug.Log($"�������� �������������� � {gameObject.name}");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            interactionAvailable = false;
            tips.SetActive(false);
            playerInputScript.interactiveObject = null;
        }
    }
}

