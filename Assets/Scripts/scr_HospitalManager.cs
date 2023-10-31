using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class scr_HospitalManager : MonoBehaviour
{
    private Dictionary<int, string> patientsList; //������ ��������� �� �������
    [HideInInspector]
    public int needToHealToday;
    [HideInInspector]
    public int needToHealYesterday; //��� ���������� �� ��������� ����
    [HideInInspector]
    public int wasHealToday;
    [HideInInspector]
    public int wasHealYesterday; //��� ���������� �� ��������� ����
    [HideInInspector]
    public bool death;

    private string[] patientsNames;
    [HideInInspector]
    public string newPatientName; //��� ������������
    [HideInInspector]
    public int newPatientNumber; // ���������� ����� ������������
    [HideInInspector]
    public bool arrival;
    [HideInInspector]
    public GameObject chosenBed; // ���������� ��� ����������� �����

    private int bedsHospitalCount; //����� ����� �����-���� � ��������
    [HideInInspector]
    public int patientsHospitalCount;
    private int newPatientCondition; //������� ������� ��������� ����� ������������ ��������
    private GameObject[] greenBeds;
    private GameObject[] blueBeds;
    private GameObject[] redBeds;

    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject defeatPanel;
    private scr_Economic economicScript;

    void Start()
    {
        defeatPanel.SetActive(false);
        victoryPanel.SetActive(false);
        // ������� ��� ����� �������� �� ����� �� �����
        greenBeds = GameObject.FindGameObjectsWithTag("GreenBed") as GameObject[];
        blueBeds = GameObject.FindGameObjectsWithTag("BlueBed") as GameObject[];
        redBeds = GameObject.FindGameObjectsWithTag("RedBed") as GameObject[];
        bedsHospitalCount = greenBeds.Length + blueBeds.Length + redBeds.Length;
        patientsList = new Dictionary<int, string>();
        death = false;
        economicScript = GetComponent<scr_Economic>();
        NamesGenerator();
    }

    void Update()
    {
        ArrivalOfThePatient();
        if (death)
        {
            defeatPanel.SetActive(true);
            Time.timeScale = 0;
        }

        if(economicScript.currentCash >= economicScript.cashTarget)
        {
            victoryPanel.SetActive(true);
            Time.timeScale = 0;
        }

    }

    private void ArrivalOfThePatient()
    {
        if(patientsHospitalCount <= bedsHospitalCount)
        {
            if (arrival)
            {
                Random random = new Random();
                newPatientCondition = random.Next(1, 4);

                if (newPatientCondition == 1)
                    BedChoser(newPatientCondition, greenBeds);
                if (newPatientCondition == 2)
                    BedChoser(newPatientCondition, blueBeds);
                if (newPatientCondition == 3)
                    BedChoser(newPatientCondition, redBeds);

                foreach (var person in patientsList)
                {
                    Debug.Log($"key: {person.Key}   value: {person.Value}  summary: {patientsList.Count}");
                }

                needToHealToday += 1;
                arrival = false;
            }
        }
    }

    private void BedChoser(int condition, GameObject[] bedType)
    {
        bool stillSearching = true;
        for (int i = 0; i < bedType.Length && stillSearching; i++)
        {
            if (bedType[i].GetComponent<scr_BedStatus>().isNotEmpty == false)
            {
                bedType[i].GetComponent<scr_BedStatus>().patientCondition = condition;
                bedType[i].GetComponent<scr_BedStatus>().isNotEmpty = true;
                bedType[i].GetComponent<scr_BedStatus>().StateSwitcher(4);
                patientsHospitalCount += 1;
                newPatientNumber = patientsHospitalCount;
                Random randomName = new Random();
                newPatientName = patientsNames[randomName.Next(0, patientsNames.Length)];
                patientsList.Add(patientsHospitalCount, newPatientName);
                bedType[i].GetComponent<scr_BedStatus>().patientName = newPatientName;
                bedType[i].GetComponent<scr_BedStatus>().patientNumber = newPatientNumber;
                chosenBed = bedType[i];
                stillSearching = false;
            }
        }
    }

    private void NamesGenerator()
    { 
        patientsNames = new string []{
            "����� �������� ������",
            "���������� ������� �������", 
            "�������� ���������� �����", 
            "ϸ�� ���������� �������",
            "������� ������� ������",
            "���� ������� ��������",
            "������� ���������� ��������",
"������ ���������� ������",
"������� ��������� �������",
"������� Ը������� ������",
"������ �������� ��������",
"������� �������� ����������",
"��������� ���������� ��������",
"�������� ���������� ����������",
"������ ���������� ���������",
"������ ��������� ������",
"����� ���������� ���������",
"������� ���������� ��������",
"�������� ���������� ���������",
"�������� ��������� ���������",
"����� �������� ��������",
"������� ��������� ������",
"���� ���������� ����������",
"������� ������� �������",
"������ ���������� �������",
"����� ���������� �������",
"������� �������� ��������",
"������ ���������� ���������",
"������� ��������� �������",
"������� ��������� ��������",
"���� �������� ��������",
"��������� ������� ���������",
"����� ���������� �������",
"������� ��������� ���������",
"����� ������� ����������",
"������ ���������� ��������",
"����� ������� �������",
"����� ������� ��������",
"����� ���������� ����������",
"���� ���������� �������",
"���������� �������� ����������",
"������ ��������� �������",
"������� ���������� ����������",
"�������� ��������� ������",
"������� �������� �������",
"������ ���������� ���������",
"����� �������� ������",
"������ ���������� �������",
"������� ��������� ��������",
"������ ���������� ���������",
"������� �������� ���������",
"���� Ը������� ���������",
"����� �������� ��������",
"���� ��������� ������",
"������ ��������� �������",
"���� ��������� ����������",
"������� ���������� �������",
"����� ��������� ��������",
"����� ���������� ���������",
"����� ��������� ��������",
"����� �������� �������",
"����� ���������� ���������",
"����� �������� ����������",
"���� �������� ��������",
"������� ��������� ����������",
"���������� �������� ��������",
"����� ��������� ��������",
"������ �������� ���������",
"�������� ��������� �������",
"�������� ��������� �������",
"���������� ���������� �������",
"��������� ��������� ����������",
"�������� ���������� ���������",
"�������� ��������� ��������",
"������� �������� ��������",
"����� �������� ����������",
"�������� �������� ���������",
"������ ��������� ����������",
"���� �������� ���������",
"����� ���������� ��������",
"���� ��������� ��������",
"������ ���������� �������",
"����� ��������� ���������",
"�������� �������� ��������",
"������ ���������� ���������",
"����� ���������� ���������",
"�������� ��������� ���������",
"������ ���������� ���������",
"����� ���������� ��������",
"������ ��������� ���������",
"������ ���������� ��������",
"�������� ��������� ����������",
"���� ��������� ��������",
"����� ��������� ���������",
"�������� �������� �������",
"���� ���������� �������",
"������ ���������� ���������",
"������ ��������� ����������",
"����� ���������� �������",
"������ ��������� ����������"};
    }
}
