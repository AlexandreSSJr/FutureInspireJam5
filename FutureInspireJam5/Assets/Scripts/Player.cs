using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 500f;
    private Vector2 movement;
    private readonly float boundary = 25f;
    private string near;
    private int ironHeld = 0;
    private int woodHeld = 0;

    public GameObject iron;
    public GameObject wood;

    // TODO: Move to Global
    public KeyCode moveUpKey = KeyCode.W;
    public KeyCode moveDownKey = KeyCode.S;
    public KeyCode moveLeftKey = KeyCode.A;
    public KeyCode moveRightKey = KeyCode.D;
    public KeyCode primaryActionKey = KeyCode.Mouse0;
    public KeyCode secondaryActionKey = KeyCode.Mouse1;
    public KeyCode dashKey = KeyCode.LeftShift;
    public KeyCode resetKey = KeyCode.R;

    private Rigidbody rb;
    private Collider col;
    private GameObject mesh;
    private GameObject woodHold;
    private GameObject ironHold;
    private PlayerControls playerControls;
    private InputAction move;
    private InputAction interact;

    private void OnEnable() {
        move = playerControls.Player.Move;
        move.Enable();

        interact = playerControls.Player.Interact;
        interact.Enable();
        interact.performed += Interact;
    }
    private void OnDisable() {
        move.Disable();
        interact.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) {
            near = other.tag;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger) {
            near = null;
        }
    }

    private void Move() {
        movement = move.ReadValue<Vector2>().normalized;

        var direction = new Vector3(movement.x, 0, movement.y);

        if (direction != Vector3.zero) {
            transform.forward = direction;
        }
        
        rb.velocity = speed * Time.fixedDeltaTime * direction;

        if (transform.position.x > boundary || transform.position.x < -boundary || transform.position.z > boundary || transform.position.z < -boundary) {
            Reset();
        }
    }

    private void Interact() {
        if (string.IsNullOrEmpty(near)) {
            Debug.Log("Interact");
        } else {
            Debug.Log("Interacting with " + near);
            if (near == "Mine") {
                ironHeld += 1;
                Instantiate(iron, ironHold.transform.position + new Vector3(0, 0.4f * ironHeld, 0), transform.rotation, transform);
            } else if (near == "Tree") {
                woodHeld += 1;
                Instantiate(wood, woodHold.transform.position + new Vector3(0, 0.4f * woodHeld, 0), transform.rotation, transform);
            }
        }
    }

    public void Interact(InputAction.CallbackContext context) {
        Interact();
    }

    private void Controls() {
        if (Input.GetKeyDown(resetKey)) {
            Reset();
        }
    }

    private void Reset() {
        transform.position = Vector3.zero;
    }

    void Awake() {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        mesh = transform.Find("Mesh").gameObject;
        woodHold = transform.Find("WoodHold").gameObject;
        ironHold = transform.Find("IronHold").gameObject;

        playerControls = new PlayerControls();
    }

    void Update() {
        Controls();
    }

    void FixedUpdate() {
        Move();
    }
}
