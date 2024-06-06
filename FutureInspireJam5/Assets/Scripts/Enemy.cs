using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private float speed = 100f;
    private float maxHealth = 3f;
    private float attackRate = 2f;
    private float attackDmg = 1f;
    private bool alive = true;
    private float health;
    private Vector3 movement;
    private Rigidbody rb;
    private GameObject mesh;
    private GameObject barrier;
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

    private void DamageBarrier() {
        if (barrier) {
            barrier.GetComponent<Barrier>().Damage(attackDmg);
        }
    }

    void OnTriggerEnter(Collider other) {
        if (other) {
            if (other.tag == "Barrier") {
                barrier = other.gameObject;
                speed = 0f;
            }
            CancelInvoke(nameof(Attack));
            InvokeRepeating(nameof(DamageBarrier), 0f, attackRate);
        }
    }

    private void Attack() {
        GameObject newProjectile = Instantiate(projectile, transform.position + new Vector3(0, 0.5f, -2f), Quaternion.identity);
        newProjectile.GetComponent<Projectile>().Point("Enemy", attackDmg, Vector3.back.normalized);
    }

    private void Move() {
        rb.velocity = movement * speed * Time.fixedDeltaTime;
    }

    void Awake() {
        rb = GetComponent<Rigidbody>();
        mesh = transform.Find("Mesh").gameObject;
        
        health = maxHealth;
        healthSlider.value = health/maxHealth;
        alive = true;

        transform.LookAt(Vector3.back.normalized);
        movement = Vector3.back;

        InvokeRepeating(nameof(Attack), 0f, attackRate);
    }

    void FixedUpdate() {
        if (alive) {
            Move();
        }
    }
}
