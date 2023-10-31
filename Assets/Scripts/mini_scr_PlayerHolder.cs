using UnityEngine;

public class mini_scr_PlayerHolder : MonoBehaviour
{
    [SerializeField] private scr_PlayerMovement playerMovement;

    public void PlayerPlaceHolder()
    {
        playerMovement.isMoving = !playerMovement.isMoving;
    }
}
