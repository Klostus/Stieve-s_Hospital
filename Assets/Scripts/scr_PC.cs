using UnityEngine;

public class scr_PC : MonoBehaviour
{
    [HideInInspector]
    public bool interact;
    [SerializeField] private scr_PlayerInput playerInputScript;
    [SerializeField] private scr_DocumentationManager docManager;
    [SerializeField] private GameObject tips;

    [SerializeField] private Transform pointAfterWork; //точка, куда перемещается игрок, после завершенной работы
    private Transform playerTransform;

    private Animator animator;

    private bool wrightingBegin;
    private float constTime;
    private float currentTime = 5;

    private void Start()
    {
        constTime = currentTime;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (interact)
        {
            if(docManager.documentationDone < docManager.documentationSummaryCount)
            {
                Debug.Log("Working with papers!");
                playerTransform.position = new Vector2(999, 999);
                animator.SetBool("Work", true);
                wrightingBegin = true;
                interact = false;
            }
        }

        WorkTimer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerTransform = collision.gameObject.transform;
            tips.SetActive(true);
            playerInputScript.interactiveObject = gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            tips.SetActive(false);
            playerInputScript.interactiveObject = null;
        }
    }

    private void WorkTimer()
    {
        if (wrightingBegin)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
            }
            else if (currentTime < 0)
            {
                docManager.DocumentationProgress();
                currentTime = constTime;
                animator.SetBool("Work", false);
                playerTransform.position = pointAfterWork.position;
                wrightingBegin = false;
            }
        }
    }
}
