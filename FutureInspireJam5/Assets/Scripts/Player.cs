using System.Collections.Generic;
using System.Linq;
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
    private int swordHeld = 0;
    private int shieldHeld = 0;
    private List<GameObject> ironHeldObjects;
    private List<GameObject> woodHeldObjects;
    private List<GameObject> swordHeldObjects;
    private List<GameObject> shieldHeldObjects;

    public GameObject iron;
    public GameObject wood;
    public GameObject sword;
    public GameObject shield;

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
    private GameObject swordHold;
    private GameObject shieldHold;
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

    private void OnTriggerEnter(Collider other) {
        if (other.isTrigger) {
            near = other.tag;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.isTrigger) {
            near = null;
        }
    }

    private void RemoveWoodHeld(int quantity = 1) {
        for (int i = 0; i < quantity; i++) {
            if(woodHeldObjects.Any()) {
                Destroy(woodHeldObjects.Last());
                woodHeldObjects.RemoveAt(woodHeldObjects.Count - 1);
            }
        }
    }

    private void RemoveIronHeld(int quantity = 1) {
        for (int i = 0; i < quantity; i++) {
            if(ironHeldObjects.Any()) {
                Destroy(ironHeldObjects.Last());
                ironHeldObjects.RemoveAt(ironHeldObjects.Count - 1);
            }
        }
    }

    private void RemoveSwordHeld(int quantity = 1) {
        for (int i = 0; i < quantity; i++) {
            if(swordHeldObjects.Any()) {
                Destroy(swordHeldObjects.Last());
                swordHeldObjects.RemoveAt(swordHeldObjects.Count - 1);
            }
        }
    }

    private void RemoveShieldHeld(int quantity = 1) {
        for (int i = 0; i < quantity; i++) {
            if(shieldHeldObjects.Any()) {
                Destroy(shieldHeldObjects.Last());
                shieldHeldObjects.RemoveAt(shieldHeldObjects.Count - 1);
            }
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
        switch(near) {
            case "Mine":
                ironHeldObjects.Add(Instantiate(iron, ironHold.transform.position + new Vector3(0, 0.4f * ironHeld, 0), transform.rotation, transform));
                ironHeld += 1;
                break;
            case "Tree":
                woodHeldObjects.Add(Instantiate(wood, woodHold.transform.position + new Vector3(0, 0.4f * woodHeld, 0), transform.rotation, transform));
                woodHeld += 1;
                break;
            case "Swordsmith":
                if (woodHeld >= 1 && ironHeld >= 3) {
                    woodHeld -= 1;
                    RemoveWoodHeld();
                    ironHeld -= 3;
                    RemoveIronHeld(3);
                    swordHeldObjects.Add(Instantiate(sword, swordHold.transform.position + new Vector3(0, 0.4f * swordHeld, 0), transform.rotation, transform));
                    swordHeld += 1;
                }
                break;
            case "Shieldsmith":
                if (woodHeld >= 3 && ironHeld >= 1) {
                    woodHeld -= 3;
                    RemoveWoodHeld(3);
                    ironHeld -= 1;
                    RemoveIronHeld();
                    shieldHeldObjects.Add(Instantiate(shield, shieldHold.transform.position + new Vector3(0, 0.4f * shieldHeld, 0), transform.rotation, transform));
                    shieldHeld += 1;
                }
                break;
            default:
                break;
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
        swordHold = transform.Find("SwordHold").gameObject;
        shieldHold = transform.Find("ShieldHold").gameObject;
        woodHeldObjects = new List<GameObject>();
        ironHeldObjects = new List<GameObject>();
        swordHeldObjects = new List<GameObject>();
        shieldHeldObjects = new List<GameObject>();

        playerControls = new PlayerControls();
    }

    void Update() {
        Controls();
    }

    void FixedUpdate() {
        Move();
    }
}
