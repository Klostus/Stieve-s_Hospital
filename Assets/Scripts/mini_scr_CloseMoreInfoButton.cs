using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class mini_scr_CloseMoreInfoButton : MonoBehaviour
{
    private GameObject targetVCamera;
    [SerializeField] private GameObject statusPanel;

    public void CloseInfo()
    {
        targetVCamera = GameObject.FindGameObjectWithTag("InfoCamera") as GameObject;
        CinemachineVirtualCamera cVC = targetVCamera.GetComponent<CinemachineVirtualCamera>();
        statusPanel.SetActive(false);
        cVC.Priority = 9;
    }
}
