using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;       // ??i t??ng Player ?? theo dõi
    public float followSpeed = 5f; // T?c ?? theo
    public float zoomSpeed = 2f;   // T?c ?? zoom
    public float minZoom = 3f;     // Gi?i h?n zoom g?n nh?t
    public float maxZoom = 10f;    // Gi?i h?n zoom xa nh?t

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

        if (player == null)
        {
            Debug.LogError("Ch?a gán Player cho CameraController!");
        }
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Camera m??t mà theo Player
            Vector3 targetPos = new Vector3(player.position.x, player.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
        }

        // X? lý zoom b?ng cu?n chu?t
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
    }
}
