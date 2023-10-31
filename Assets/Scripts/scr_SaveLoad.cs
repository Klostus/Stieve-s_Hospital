using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_SaveLoad : MonoBehaviour
{
    private scr_GlobalTimer globalTimerScr;
    private scr_HospitalManager hospitalManagerScr;
    private scr_DocumentationManager documentationManagerScr;
    private scr_Economic economicScr;

    private float cash;


    void Start()
    {
        globalTimerScr = GetComponent<scr_GlobalTimer>();
        hospitalManagerScr = GetComponent<scr_HospitalManager>();
        documentationManagerScr = GetComponent<scr_DocumentationManager>();
        economicScr = GetComponent<scr_Economic>();
    }

    
    // не реализованы до конца
    public void Save()
    {
        PlayerPrefs.SetInt("Day", globalTimerScr.dayCounter);
        PlayerPrefs.SetFloat("Cash", economicScr.currentCash);
    }

    public void Load()
    {
        globalTimerScr.dayCounter = PlayerPrefs.GetInt("Day");
        economicScr.currentCash = PlayerPrefs.GetFloat("Cash");
    }
}
