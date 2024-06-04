using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 100f;
    [SerializeField] private float maxHealth = 3f;
    [SerializeField] private float attackRate = 1f;
    private bool alive = true;
    private float health;
    private float attackCooldown;
    private Vector2 movement;
    private Rigidbody rb;
    private GameObject mesh;
    private GameObject armR;
    public GameObject player;
    public GameObject projectile;
    public Slider healthSlider;

    public void Damage(float dmg) {
        health -= dmg;

        if (health <= 0 && alive) {
            alive = false;
            Destroy(mesh);
            Destroy(gameObject, 0.2f);
        }
        
        healthSlider.value = health/maxHealth;
    }

    private void Attack() {
        if (attackCooldown <= 0) {
            GameObject newProjectile = Instantiate(projectile, armR.transform.position, Quaternion.identity);
            newProjectile.GetComponent<Projectile>().Point("Enemy", mesh.transform.forward.normalized);

            attackCooldown = attackRate;
        }
    }

    private void UpdateCooldowns() {
        if (attackCooldown > 0) {
            attackCooldown -= 0.05f;
        }
    }

    private void Move() {
        rb.velocity = new Vector3(movement.x, 0, movement.y) * speed * Time.fixedDeltaTime;

    }

    void Awake() {
        rb = GetComponent<Rigidbody>();
        mesh = transform.Find("Mesh").gameObject;
        armR = transform.Find("SwordHold").gameObject;
        
        health = maxHealth;
        healthSlider.value = health/maxHealth;
        alive = true;
    }

    void Act() {
        if (mesh && player) {
            mesh.transform.LookAt(player.transform);
            Attack();

            movement = new Vector2(mesh.transform.forward.x, mesh.transform.forward.z).normalized;

            Move();
        }
    }

    void FixedUpdate() {
        if (alive) {
            Act();
            UpdateCooldowns();
        }
    }
}
