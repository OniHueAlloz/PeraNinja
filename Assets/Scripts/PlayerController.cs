using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour {
    [Header("Movimento")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float jumpForce = 5f;

    [Header("Status")]
    public int health = 10;
    public int killCount = 0;

    [Header("Áudio Geral")]
    public AudioClip[] footstepSounds;
    public AudioClip[] slashSounds;
    public AudioClip jumpSound; // Coloque o Oush aqui
    public AudioClip hurtSound; // Coloque o Oush aqui
    public AudioClip deathSound; // Coloque o Smashed aqui
    public float stepInterval = 0.4f;

    [Header("Áudio Grama (Terrain)")]
    public AudioClip[] grassSounds; // Coloque Grass 1, 2, 3 aqui
    public Terrain terrain;
    public int detailLayerIndex = 0;

    [Header("Referências")]
    public Animator animator;
    public Rigidbody rb;

    private bool isGrounded = true;
    private AudioSource audioSource;
    private float stepTimer;
    private float grassTimer;

    void Start(){
        audioSource = GetComponent<AudioSource>();
        if (terrain == null){
            terrain = Terrain.activeTerrain;
        }
    }

    void Update(){
        HandleMovement();
        HandleActions();
    }

    void HandleMovement(){
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(h, 0, v).normalized;

        if (direction.magnitude >= 0.1f){
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            animator.SetBool("Armature|Pera_Walk", true);

            if(isGrounded){
                stepTimer += Time.deltaTime;
                if(stepTimer >= stepInterval){
                    if(IsOnGrass()){
                        PlayRandomSound(grassSounds);
                    } else {
                        PlayRandomSound(footstepSounds);
                    }
                    stepTimer = 0f;
                }
            }
        } else {
            animator.SetBool("Armature|Pera_Walk", false);
            stepTimer = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded){
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            if(jumpSound != null){
                audioSource.PlayOneShot(jumpSound);
            }
        }
    }

    void HandleActions(){
        if (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)){
            animator.SetTrigger("Armature|Pera_Attack");
            PlayRandomSound(slashSounds);

            Ray ray = new Ray(transform.position + Vector3.up, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 2f)){
                if (hit.collider.CompareTag("enemy")){
                    Destroy(hit.collider.gameObject);
                    killCount++;
                    if (killCount >= 10){
                        MusicManager.instance.PlayGong();
                        SceneManager.LoadScene("End");
                    } 
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision){
        if (collision.collider.CompareTag("enemy")){
            health--;
            if(hurtSound != null){
                audioSource.PlayOneShot(hurtSound);
            }
            
            if (health <= 0){
                if(deathSound != null){
                    AudioSource.PlayClipAtPoint(deathSound, transform.position);
                }
                MusicManager.instance.PlayGong();
                SceneManager.LoadScene("Start");
            }
        } else if (collision.contacts[0].normal.y > 0.5f){
            isGrounded = true;
        }
    }

    bool IsOnGrass(){
        if(terrain == null) return false;

        Vector3 playerPos = transform.position - terrain.transform.position;
        Vector3 terrainSize = terrain.terrainData.size;

        float relativeX = playerPos.x / terrainSize.x;
        float relativeZ = playerPos.z / terrainSize.z;

        if (relativeX < 0 || relativeX > 1 || relativeZ < 0 || relativeZ > 1) return false;

        int detailX = (int)(relativeX * terrain.terrainData.detailWidth);
        int detailZ = (int)(relativeZ * terrain.terrainData.detailHeight);

        int[,] details = terrain.terrainData.GetDetailLayer(detailX, detailZ, 1, 1, detailLayerIndex);

        return details[0, 0] > 0;
    }

    void PlayRandomSound(AudioClip[] clips){
        if(clips.Length > 0){
            int index = Random.Range(0, clips.Length);
            audioSource.PlayOneShot(clips[index]);
        }
    }
}