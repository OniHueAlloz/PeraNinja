using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyFollow : MonoBehaviour {
    public float speed = 3.0f;
    
    [Header("Áudio Inimigo")]
    public AudioClip crawlLoop;
    [Range(0f, 1f)]
    public float crawlVolume = 0.1f; 
    public AudioClip[] growlSounds;
    public float growlIntervalMin = 3f;
    public float growlIntervalMax = 7f;

    private Transform player;
    private AudioSource sfxSource;
    private AudioSource crawlSource;
    private float growlTimer;

    void Start(){
        GameObject playerObj = GameObject.FindWithTag("Player");
        sfxSource = GetComponent<AudioSource>();
        if(crawlLoop != null){
            crawlSource = gameObject.AddComponent<AudioSource>();
            crawlSource.clip = crawlLoop;
            crawlSource.loop = true;
            crawlSource.volume = crawlVolume;
            crawlSource.spatialBlend = 1.0f; 
            crawlSource.Play();
        }

        growlTimer = Random.Range(growlIntervalMin, growlIntervalMax);

        if (playerObj != null){
            player = playerObj.transform;
        }
    }

    void Update(){
        if (player == null) return;

        // Movimentação
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        Vector3 lookPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(lookPos);

        HandleGrowl(player.position);
    }

    void HandleGrowl(Vector3 playerPos){
        float distance = Vector3.Distance(transform.position, playerPos);
        if(distance < 10f){
            growlTimer -= Time.deltaTime;
            if(growlTimer <= 0){
                if(growlSounds.Length > 0){
                    sfxSource.PlayOneShot(growlSounds[Random.Range(0, growlSounds.Length)]);
                }
                growlTimer = Random.Range(growlIntervalMin, growlIntervalMax);
            }
        }
    }
}