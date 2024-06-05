using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed = 800f;
    private float lifetime = 5f;
    private float damage = 1f;

    private Rigidbody rb;
    private string origin;
    private Vector3 direction;
    private Transform mesh;

    private static float GetAngleFromVectorFloat(Vector3 dir) {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;

        return n;
    }

    public void Point(string originTag, Vector3 aim) {
        origin = originTag;
        direction = aim;
        mesh.transform.eulerAngles = new Vector3(-GetAngleFromVectorFloat(aim), 0, 0);
    }

    private void Move() {
        rb.velocity = new Vector3(direction.x, 0, direction.z).normalized * speed * Time.fixedDeltaTime;
    }

    private void Hit() {
        Destroy(mesh.gameObject);
        Destroy(gameObject, 0.2f);
    }

    void OnTriggerEnter(Collider other) {
        if (mesh) {
            if (other) {
                if (origin == "Player" && other.tag == "Enemy") {
                    other.GetComponent<Enemy>().Damage(damage);
                    Hit();
                }
                if (origin == "Enemy" && other.tag == "Player") {
                    other.GetComponent<Player>().Damage(damage);
                    Hit();
                }
                if (other.tag == "Wall" || other.tag == "Projectile") {
                    Hit();
                } else {
                    Hit();
                }
            }
        }
    }

    void Awake() {
        rb = GetComponent<Rigidbody>();
        mesh = transform.Find("Mesh");
        Destroy(gameObject, lifetime);
    }

    void FixedUpdate() {
        Move();
    }
}
