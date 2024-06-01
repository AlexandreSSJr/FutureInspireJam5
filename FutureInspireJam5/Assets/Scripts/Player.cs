using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 500f;
    private Vector2 movement;
    private readonly float boundary = 25f;

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

    private void Move() {
        movement = move.ReadValue<Vector2>().normalized;

        var direction = new Vector3(movement.x, 0, movement.y);

        if (direction != Vector3.zero) {
            mesh.transform.forward = direction;
        }
        
        rb.velocity = speed * Time.fixedDeltaTime * direction;

        if (transform.position.x > boundary || transform.position.x < -boundary || transform.position.z > boundary || transform.position.z < -boundary) {
            Reset();
        }
    }

    private void Interact() {
        Debug.Log("Interact");
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

        playerControls = new PlayerControls();
    }

    void Update() {
        Controls();
    }

    void FixedUpdate() {
        Move();
    }
}
