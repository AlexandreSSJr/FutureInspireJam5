using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Threading;

public class Player : MonoBehaviour
{
    private readonly float speed = 500f;
    private float maxHealth = 10f;
    private float attackRate = 0.5f;
    private float attackDmg = 1f;
    private bool shouldAttack = false;
    private float health;
    private Vector2 movement;
    private readonly float boundary = 50f;
    private readonly float attackBoundary = 10f;
    private string near;
    private GameObject nearObj;
    private int ironHeld = 0;
    private const int maxIronHeld = 6;
    private int woodHeld = 0;
    private const int maxWoodHeld = 6;
    private int bowLevel = 0;
    private const int bowMaxLevel = 5;
    private int shieldLevel = 0;
    private const int shieldMaxLevel = 5;
    private List<GameObject> ironHeldObjects;
    private List<GameObject> woodHeldObjects;

    public GameObject iron;
    public GameObject wood;
    public GameObject projectile;
    public Slider healthSlider;
    public List<Material> bowMaterials;
    public AudioSource audioWoodGather;
    public AudioSource audioMetalMine;
    public AudioSource audioBowUpgrade;
    public AudioSource audioShieldUpgrade;
    public AudioSource audioAttack;
    public AudioSource audioBarrierRepair;

    private Rigidbody rb;
    private Collider col;
    private GameObject mesh;
    private GameObject bow;
    private GameObject shield;
    private GameObject woodHold;
    private GameObject ironHold;
    private GameObject shieldHold;
    private PlayerControls playerControls;
    private InputAction move;
    private InputAction interact;
    private readonly string gameOverScene = "Defeat";

    public void Damage(float dmg) {
        health -= dmg;

        if (health <= 0) {
            SceneManager.LoadScene(gameOverScene, LoadSceneMode.Single);
        }
        
        healthSlider.value = health/maxHealth;
    }

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
            nearObj = other.gameObject;
            if (other.CompareTag("Tree")) {
                InvokeRepeating(nameof(GatherWood), 1f, 1f);
            } else if (other.CompareTag("Mine")) {
                InvokeRepeating(nameof(GatherIron), 1f, 1f);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.isTrigger) {
            near = null;
            nearObj = null;
            if (other.CompareTag("Tree")) {
                CancelInvoke(nameof(GatherWood));
            } else if (other.CompareTag("Mine")) {
                CancelInvoke(nameof(GatherIron));
            }
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

        if (shouldAttack && (transform.position.x > attackBoundary || transform.position.x < -attackBoundary)) {
            shouldAttack = false;
            // bow.GetComponent<Renderer>().enabled = false;
        } else if (!shouldAttack && transform.position.x < attackBoundary && transform.position.x > -attackBoundary) {
            shouldAttack = true;
            // bow.GetComponent<Renderer>().enabled = true;
        }
    }

    private void GatherIron() {
        if (ironHeld < maxIronHeld) {
            ironHeldObjects.Add(Instantiate(iron, ironHold.transform.position + new Vector3(0, 0.4f * ironHeld, 0), transform.rotation, transform));
            ironHeld += 1;
            audioMetalMine.Play();
        }
    }

    private void GatherWood() {
        if (woodHeld < maxWoodHeld) {
            woodHeldObjects.Add(Instantiate(wood, woodHold.transform.position + new Vector3(0, 0.4f * woodHeld, 0), transform.rotation, transform));
            woodHeld += 1;
            audioWoodGather.Play();
        }
    }

    private void Interact() {
        int woodCost;
        int ironCost;

        switch(near) {
            case "Mine":
                GatherIron();
                break;
            case "Tree":
                GatherWood();
                break;
            case "Swordsmith":
                woodCost = 1;
                ironCost = 3;

                if (bowLevel < bowMaxLevel && woodHeld >= woodCost && ironHeld >= ironCost) {
                    woodHeld -= woodCost;
                    RemoveWoodHeld(woodCost);
                    ironHeld -= ironCost;
                    RemoveIronHeld(ironCost);

                    bow.GetComponent<Renderer>().material = bowMaterials[Mathf.Min(bowLevel, bowMaterials.Count)];
                    bowLevel += 1;
                    attackDmg += 1f;

                    audioBowUpgrade.Play();
                }
                break;
            case "Shieldsmith":
                woodCost = 3;
                ironCost = 1;
                
                if (shieldLevel < shieldMaxLevel && woodHeld >= woodCost && ironHeld >= ironCost) {
                    woodHeld -= woodCost;
                    RemoveWoodHeld(woodCost);
                    ironHeld -= ironCost;
                    RemoveIronHeld(ironCost);

                    shield.GetComponent<Renderer>().material = bowMaterials[Mathf.Min(shieldLevel, bowMaterials.Count)];
                    shieldLevel += 1;
                    maxHealth += 5f;
                    health += 5f;
                    healthSlider.value = health/maxHealth;

                    audioShieldUpgrade.Play();
                }
                break;
            case "Barrier":
                woodCost = 5;
                ironCost = 5;

                Barrier barrier = nearObj.GetComponent<Barrier>();
                if (woodHeld >= woodCost && ironHeld >= ironCost && barrier != null) {
                    woodHeld -= woodCost;
                    RemoveWoodHeld(woodCost);
                    ironHeld -= ironCost;
                    RemoveIronHeld(ironCost);
                    barrier.Rebuild();
                    
                    audioBarrierRepair.Play();
                }
                break;
            default:
                break;
        }
    }

    public void Interact(InputAction.CallbackContext context) {
        Interact();
    }

    private void Attack() {
        if (shouldAttack) {
            GameObject newProjectile = Instantiate(projectile, transform.position + new Vector3(0, 0.7f, 3f), Quaternion.identity);
            newProjectile.GetComponent<Projectile>().Point("Player", attackDmg, Vector3.forward);
            audioAttack.Play();
        }
    }

    private void Reset() {
        transform.position = Vector3.zero;
    }

    void Awake() {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        mesh = transform.Find("Mesh").gameObject;

        bow = transform.Find("Bow").gameObject;
        shield = transform.Find("ShieldHold").Find("Shield").Find("Mesh").gameObject;
        woodHold = transform.Find("WoodHold").gameObject;
        ironHold = transform.Find("IronHold").gameObject;
        shieldHold = transform.Find("ShieldHold").gameObject;
        woodHeldObjects = new List<GameObject>();
        ironHeldObjects = new List<GameObject>();

        playerControls = new PlayerControls();

        health = maxHealth;
        healthSlider.value = health/maxHealth;

        InvokeRepeating(nameof(Attack), 1f, attackRate);
    }

    void FixedUpdate() {
        Move();
    }
}
