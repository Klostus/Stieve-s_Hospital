using UnityEngine;
using UnityEngine.UI;

public class scr_DocumentationManager : MonoBehaviour
{
    [HideInInspector]
    public int documentationSummaryCount;
    private scr_HospitalManager hospitalManagerScript;
    private scr_GlobalTimer globalTimerScript;
    [SerializeField] private Text documentationLeftText;

    [HideInInspector]
    public int documentationDone;
    [HideInInspector]
    public int documentationDoneYesterday; //для статистики за прошедший день

    void Start()
    {
        documentationDone = 0;
        hospitalManagerScript = GetComponent<scr_HospitalManager>();
        globalTimerScript = GetComponent<scr_GlobalTimer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (globalTimerScript.dayTick)
        {
            documentationDoneYesterday = documentationDone;
            documentationDone = 0;
        }

        documentationSummaryCount = hospitalManagerScript.patientsHospitalCount;
        documentationLeftText.text = $"{documentationDone} / {documentationSummaryCount}";       
    }

    public void DocumentationProgress()
    {
        documentationDone += 1;
    }
}
