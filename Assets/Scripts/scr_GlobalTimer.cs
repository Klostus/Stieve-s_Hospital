using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using Random = System.Random;

public class scr_GlobalTimer : MonoBehaviour
{
    //Продолжительность дня в секундах:
    [SerializeField] private float dayTime;
    [HideInInspector]
    public float hour;
    private float hourMultiply;
    private float actionHour;
    [SerializeField] private SpriteRenderer backgroundMain;
    [SerializeField] private Sprite background_morning;
    [SerializeField] private Sprite background_day;
    [SerializeField] private Sprite background_evening;
    [SerializeField] private GameObject clock;
    private Image clockImage;
    [SerializeField] private GameObject homeImage;
    [SerializeField] private Text hospitalInfoText;
    private Animator clockImageAnimator;


    [SerializeField] private Text dayInfoText;
    [HideInInspector]
    public int dayCounter;
    private string dayOfTheWeekName;
    [SerializeField] private Text currentTimeText;

    private float currentTime;
    [HideInInspector]
    public bool dayTick;
    private Dictionary<string, int> calendar;

    private scr_HospitalManager hospitalManagerScript;
    [HideInInspector]
    public int prevArrivals;
    private int todayArrivals;
    private bool arrivalsRandomizer; //факт пройденного количества "проверок" на поступление; 
    private int[] hospitalisationsToday; //информация о времени поступающих сегодня;
    
    //отметки об успешном поступлении для предотвращения накладывающегося случая:
    private bool arrivalSuccess1;
    private bool arrivalSuccess2;
    private bool arrivalSuccess3;

    [SerializeField, Range(50, 100)] private int hospitalizationChance; 


    private void Awake()
    {
        calendar = new Dictionary<string, int>()
        {
            ["Понедельник"] = 1,
            ["Вторник"] = 2, 
            ["Среда"] = 3, 
            ["Четверг"] = 4, 
            ["Пятница"] = 5, 
            ["Суббота"] = 6, 
            ["Воскресенье"] = 7
        };
        currentTime = dayTime;
        hour = dayTime / 24;
        hourMultiply = hour;
        dayCounter = 1;
        DayOfTheWeekWrighter();
        dayInfoText.text = "День: " + dayCounter.ToString() + "            День Недели: " + dayOfTheWeekName;
        actionHour = 9;
        currentTimeText.text = actionHour.ToString() + " : 00";
        backgroundMain.sprite = background_morning;
        clockImage = clock.GetComponent<Image>();
        clockImageAnimator = clock.GetComponent<Animator>();
        hospitalManagerScript = GetComponent<scr_HospitalManager>();
        hospitalisationsToday = new int[3];
        arrivalsRandomizer = true;
    }

    void Update()
    {
        dayTick = false;
        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            dayTick = true;
            dayCounter += 1;
            //Debug.Log($"За прошедшие сутки поступило {todayArrivals} человек!");
            DayOfTheWeekWrighter();
            dayInfoText.text = "День: " + dayCounter.ToString() + "            День Недели: " + dayOfTheWeekName;
            currentTime = dayTime;
            hour = dayTime / 24;
            hourMultiply = hour;
            actionHour = 9;
            prevArrivals = todayArrivals;
            todayArrivals = 0;
            arrivalsRandomizer = true;
            backgroundMain.sprite = background_morning;
            clockImageAnimator.SetBool("TimeOut", false);
            homeImage.SetActive(false);
        }
        else 
        {
            if (actionHour >= 18 || actionHour <= 7)
            {
                backgroundMain.sprite = background_evening;
                clockImageAnimator.SetBool("TimeOut", true);
                homeImage.SetActive(true);
            }
            else
            {
                if(actionHour >= 12)
                {
                    backgroundMain.sprite = background_day;
                }
                else backgroundMain.sprite = background_morning;
            }            
        } 

        clockImage.fillAmount = currentTime / dayTime;
        TimeUpdater();

        PatientLauncher();
    }

    private void TimeUpdater()
    {
        if(currentTime <= dayTime - hourMultiply)
        {
            if(actionHour < 23)
            {
                actionHour += 1;
            }
            else
            {
                actionHour = 0;
            }

            hourMultiply += hour;
            currentTimeText.text = actionHour.ToString() + " : 00";
        }
        
    }

    private void DayOfTheWeekWrighter()
    {
        foreach (var dotw in calendar)
        {
            if ((dayCounter - dotw.Value) == 0)
            {
                dayOfTheWeekName = dotw.Key;
            }
            else if ((dayCounter - dotw.Value) % 7 == 0)
            {
                dayOfTheWeekName = dotw.Key;
            }
        }
    }

    private void PatientLauncher()
    {
        if(todayArrivals <= 3)
        {
            if (arrivalsRandomizer == true)
            {
                arrivalSuccess1 = false;
                arrivalSuccess2 = false;
                arrivalSuccess3 = false;

                for (int i = 0; i < hospitalisationsToday.Length; i++)
                    hospitalisationsToday[i] = 0;

                for (int i = 0; i < 3; i++)
                {
                    Random arrivalChance = new Random();
                    if (arrivalChance.Next(0, 101) >= (100 - hospitalizationChance))
                    {
                        Random arrivalTime = new Random();
                        hospitalisationsToday[i] = arrivalTime.Next(1, 24);
                    }
                }

                Debug.Log($"Сегодня запланированы поступления на: {hospitalisationsToday[0]} : 00, {hospitalisationsToday[1]} : 00 и {hospitalisationsToday[2]} : 00");
                arrivalsRandomizer = false;
            }

            if(arrivalSuccess1 == false)
            {
                if (actionHour == hospitalisationsToday[0] && hospitalisationsToday[0] != 0)
                {
                    hospitalManagerScript.arrival = true;
                    todayArrivals += 1;
                    hospitalInfoText.text = $"Поступление пациента в приемный покой в {hospitalisationsToday[0]} : 00!";
                    //Debug.Log($"Поступление пациента в приемный покой в {hospitalisationsToday[0]} : 00!");
                    arrivalSuccess1 = true;
                }
            }

            if (arrivalSuccess2 == false)
            {
                if (actionHour == hospitalisationsToday[1] && hospitalisationsToday[1] != 0)
                {
                    hospitalManagerScript.arrival = true;
                    todayArrivals += 1;
                    hospitalInfoText.text = $"Поступление пациента в приемный покой в {hospitalisationsToday[1]} : 00!";
                    //Debug.Log($"Поступление пациента в приемный покой в {hospitalisationsToday[1]} : 00!");
                    arrivalSuccess2 = true;
                }
            }

            if (arrivalSuccess3 == false)
            {
                if (actionHour == hospitalisationsToday[2] && hospitalisationsToday[2] != 0)
                {
                    hospitalManagerScript.arrival = true;
                    todayArrivals += 1;
                    hospitalInfoText.text = $"Поступление пациента в приемный покой в {hospitalisationsToday[2]} : 00!";
                    //Debug.Log($"Поступление пациента в приемный покой в {hospitalisationsToday[2]} : 00!");
                    arrivalSuccess3 = true;
                }
            }
        }
    }
}
