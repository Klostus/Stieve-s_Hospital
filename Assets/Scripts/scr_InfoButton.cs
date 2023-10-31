using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class scr_InfoButton : MonoBehaviour
{
    [SerializeField] private scr_HospitalManager hospitalManagerScript;
    private GameObject targetVCamera;
    private Transform target; 

    [Header("StatusPanel:")]
    [SerializeField] private GameObject statusPanel;
    [SerializeField] private Text patientName;
    [SerializeField] private Text patientStatus;
    [SerializeField] private Text patientFirstDay;
    [SerializeField] private Image statusInfoBG;
    [SerializeField] private Text inspectedText;

    private string patientNameText;
    private string patientStatusText;
    private string firstDayText;
    private Sprite infoBgSprite;
    private GameObject bed;

    [SerializeField] private Sprite lightStatus;
    [SerializeField] private Sprite mediumStatus;
    [SerializeField] private Sprite hardStatus;

    private scr_PatientStatus patientStatusScript;
    private bool notEmpty;

    private void Update()
    {
        if (notEmpty)
        {
            if (patientStatusScript.isInspectedToday)
            {
                inspectedText.text = "Сегодня осмотрен!";
            }
            else inspectedText.text = "";
        }
    }

    public void ButtonToBedConnector(string patientInfoName, int status, int firstDay)
    {
        bed = hospitalManagerScript.chosenBed;
        patientStatusScript = bed.GetComponent<scr_PatientStatus>();
        target = bed.transform;
        patientNameText = "Ф.И.О.: " + patientInfoName;
        firstDayText = "День поступления: " + firstDay.ToString() + "-й";

        if(status == 1)
        {
            patientStatusText = "Состояние: удовлетворительное";
            infoBgSprite = lightStatus;
        }

        if (status == 2)
        {
            patientStatusText = "Состояние: средней тяжести";
            infoBgSprite = mediumStatus;
        }

        if (status == 3)
        {
            patientStatusText = "Состояние: тяжелое";
            infoBgSprite = hardStatus;
        }

        notEmpty = true;
    }

    public void LookForMoreInfo()
    {
        targetVCamera = GameObject.FindGameObjectWithTag("InfoCamera") as GameObject;
        CinemachineVirtualCamera cVC = targetVCamera.GetComponent<CinemachineVirtualCamera>();
        cVC.Priority = 12;
        cVC.Follow = target;
        StatusInfoWrighter();
        statusPanel.SetActive(true);
    }

    private void StatusInfoWrighter()
    {
        patientName.text = patientNameText;
        patientFirstDay.text = firstDayText;
        patientStatus.text = patientStatusText;
        statusInfoBG.sprite = infoBgSprite;
    }


}
