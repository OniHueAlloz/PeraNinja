using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movimento")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float jumpForce = 5f;

    [Header("Status")]
    public int health = 10;
    public int killCount = 0;

    [Header("Referências")]
    public Animator animator;
    public Rigidbody rb;

    private bool isGrounded = true;

    void Update()
    {
        HandleMovement();
        HandleActions();
    }

    void HandleMovement()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(h, 0, v).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Gira o personagem na direção do movimento
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Move o personagem
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

            // Ativa animação de andar
            animator.SetBool("Armature|Pera_Walk", true);
        }
        else
        {
            animator.SetBool("Armature|Pera_Walk", false);
        }

        // Pulo
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void HandleActions()
    {
        if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            animator.SetTrigger("Armature|Pera_Attack");

            Ray ray = new Ray(transform.position + Vector3.up, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 2f))
            {
                if (hit.collider.CompareTag("enemy"))
                {
                    Destroy(hit.collider.gameObject);
                    killCount++;
                    if (killCount >= 30)
                    {
                        SceneManager.LoadScene("Victory");
                        // Aqui você pode adicionar lógica para avançar de nível ou recompensas
                    } 
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("enemy"))
        {
            health--;
            Debug.Log("Player levou dano! Vida restante: " + health);

            if (health <= 0)
            {
                SceneManager.LoadScene("Start");
            }
        }
        else if (collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
        }
    }
}
