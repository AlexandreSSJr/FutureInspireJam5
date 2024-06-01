using UnityEngine;

public class Cam : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Vector3 diff = new (0f, 10f, -4f);
    private Camera cam;

    public void ChangeCameraDistance(int value) {
        cam.orthographicSize = value;
    }

    private void Awake() {
        cam = transform.GetComponent<Camera>();
    }

    private void Update() {
        transform.position = new Vector3(player.transform.position.x + diff.x, player.transform.position.y + diff.y, player.transform.position.z + diff.z);
    }
}
