using UnityEngine;

public class scr_CameraMovement : MonoBehaviour
{
    [SerializeField, Range(0, 10)] private float speed;
    [SerializeField] private Transform playerTransform;
    private Vector2 motion;

    private void Awake()
    {
        transform.position = playerTransform.position;
    }

    private void Update()
    {
        transform.Translate(motion * speed * Time.deltaTime);
    }

    public void CameraMovement(float x, float y)
    {
        motion = new Vector2(x, y);
    }

    public void CameraReset(bool b2p)
    {
        if (b2p)
        {
            transform.position = playerTransform.position;
        }
    }
}
