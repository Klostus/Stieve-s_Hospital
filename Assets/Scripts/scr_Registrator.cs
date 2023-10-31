using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_Registrator : MonoBehaviour
{
    public GameObject[] patientInfoButtons;

    [SerializeField] private Sprite statusLight;
    [SerializeField] private Sprite statusMedium;
    [SerializeField] private Sprite statusHard;

    public void RegistrationOfPatient(int number, string name, int status, int firstDay)
    {
        GameObject chosenButton = patientInfoButtons[number - 1];
        chosenButton.GetComponent<Button>().interactable = true;

        Transform child0 = chosenButton.transform.GetChild(0);
        Text patientInfoNumber = child0.GetComponent<Text>();

        Transform child1 = chosenButton.transform.GetChild(1);
        Text patientInfoName = child1.GetComponent<Text>();

        Transform child3 = chosenButton.transform.GetChild(3);
        Image patientInfoStatus = child3.GetComponent<Image>();

        patientInfoNumber.text = number.ToString();
        patientInfoName.text = name;

        if (status == 1)
            patientInfoStatus.sprite = statusLight;

        if (status == 2)
            patientInfoStatus.sprite = statusMedium;

        if (status == 3)
            patientInfoStatus.sprite = statusHard;

        chosenButton.GetComponent<scr_InfoButton>().ButtonToBedConnector(name, status, firstDay);
    }
}
