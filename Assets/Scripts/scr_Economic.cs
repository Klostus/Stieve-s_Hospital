using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_Economic : MonoBehaviour
{
    public float dailyCashBonus; // стандартна€ прибавка за часы работы
    public float cashTarget; //итогова€ цель
    private float dailyCashCountH; // ежедневна€ сумма с учетом бонусов и штрафов за лечение
    private float dailyCashCountD; // ежедневна€ сумма с учетом бонусов и штрафов за ведение документации
    private float dailySummCount; // общий приток средств за день
    [HideInInspector]
    public float currentCash;
    [SerializeField] private Text cashText; // индикатор в левом углу экрана
    [SerializeField] private Image cashBar;


    private scr_GlobalTimer globalTimerScript;
    private scr_HospitalManager hospitalManagerScript;
    private scr_DocumentationManager docManagerScript;
    [SerializeField] private scr_PauseController pauseController;

    [Header("Statistic:")]
    [SerializeField] private GameObject receiptPanel;
    [SerializeField] private Text dayText;
    [SerializeField] private Text hospitalStatistic;
    [SerializeField] private Text dailyCashText;
    [SerializeField] private Text patientsStatistic;
    [SerializeField] private Text docsStatistic;
    [SerializeField] private Text cashHealStatistic;
    [SerializeField] private Text cashDocsStatistic;
    [SerializeField] private Text cashSummStatistic;

    [Header("Fines & Bonuses:")]
    [SerializeField] private float docsPenalty;
    [SerializeField] private float healPenalty;
    [SerializeField] private float docsBonus;
    [SerializeField] private float healBonus;
    [SerializeField] private float monthlyTaxes;

    private bool calculation;
    // Start is called before the first frame update
    void Start()
    {
        globalTimerScript = GetComponent<scr_GlobalTimer>();
        hospitalManagerScript = GetComponent<scr_HospitalManager>();
        docManagerScript = GetComponent<scr_DocumentationManager>();
        currentCash = 50;
        cashText.text = currentCash.ToString();
        cashBar.fillAmount = currentCash / cashTarget;
    }

    // Update is called once per frame
    void Update()
    {
        if (globalTimerScript.dayTick)
        {
            calculation = true;
        }

        if (calculation)
        {
            receiptPanel.SetActive(true);
            dayText.text = $"–асчетный лист за {globalTimerScript.dayCounter - 1} -й день";
            dailyCashText.color = Color.blue;
            dailyCashText.text = "+ " + dailyCashBonus.ToString() + " $";
            hospitalStatistic.text = $"«а прошедшие сутки поступило человек: {globalTimerScript.prevArrivals}. ¬сего в стационаре: {hospitalManagerScript.patientsHospitalCount} пациентов.";
            HealingCounter();
            DocumantationCounter();
            dailySummCount = dailyCashBonus + dailyCashCountH + dailyCashCountD;
            currentCash += dailySummCount;
            TextColorizer();
            patientsStatistic.text = $"{hospitalManagerScript.wasHealYesterday} из {hospitalManagerScript.needToHealYesterday}";
            docsStatistic.text = $"{docManagerScript.documentationDoneYesterday} из {docManagerScript.documentationSummaryCount}";
            cashText.text = currentCash.ToString() + " $";
            cashBar.fillAmount = currentCash / cashTarget;
            calculation = false;
            pauseController.PauseGame();
        }
    }

    private void HealingCounter() // насколько выполнены лечебные меропри€ти€
    {
        dailyCashCountH = 0;
        if (hospitalManagerScript.wasHealYesterday == hospitalManagerScript.needToHealYesterday)
        {
            dailyCashCountH = healBonus;
        }
        else
        {
            if((hospitalManagerScript.needToHealYesterday - hospitalManagerScript.wasHealYesterday) > 1)
            {
                dailyCashCountH -= healPenalty * (hospitalManagerScript.needToHealYesterday - hospitalManagerScript.wasHealYesterday);
            }            
        }
    }

    private void DocumantationCounter() // насколько выполнена работа с документами
    {
        dailyCashCountD = 0;
        if(docManagerScript.documentationDoneYesterday == docManagerScript.documentationSummaryCount)
        {
            dailyCashCountD = docsBonus;
        }
        else
        {
            if((docManagerScript.documentationSummaryCount - docManagerScript.documentationDoneYesterday) > 1)
            {
                dailyCashCountD -= docsPenalty * (docManagerScript.documentationSummaryCount - docManagerScript.documentationDoneYesterday);
            }
        }
    }

    private void TextColorizer()
    {
        if (dailyCashCountH <= 0)
        {
            cashHealStatistic.color = Color.red;
            cashHealStatistic.text = dailyCashCountH.ToString() + " $";
        }
        else 
        {
            cashHealStatistic.color = Color.blue;
            cashHealStatistic.text = "+ " + dailyCashCountH.ToString() + " $";
        }

        if (dailyCashCountD <= 0)
        {
            cashDocsStatistic.color = Color.red;
            cashDocsStatistic.text = dailyCashCountD.ToString() + " $";
        }
        else 
        {
            cashDocsStatistic.color = Color.blue;
            cashDocsStatistic.text = "+ " + dailyCashCountD.ToString() + " $";
        }

        if (hospitalManagerScript.wasHealYesterday == hospitalManagerScript.needToHealYesterday)
        {
            patientsStatistic.color = Color.blue;
        }
        else patientsStatistic.color = Color.red;

        if (docManagerScript.documentationDoneYesterday == docManagerScript.documentationSummaryCount)
        {
            docsStatistic.color = Color.blue;
        }
        else docsStatistic.color = Color.red;

        if (dailySummCount <= 0)
        {
            cashSummStatistic.color = Color.red;
            cashSummStatistic.text = dailySummCount.ToString() + " $";
        }
        else 
        {
            cashSummStatistic.color = Color.blue;
            cashSummStatistic.text = "+ " + dailySummCount.ToString() + " $";
        }
    }
}
