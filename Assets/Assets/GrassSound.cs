using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GrassSound : MonoBehaviour {
    public AudioClip[] grassSounds;
    private AudioSource audioSource;

    void Start(){
        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1.0f;
    }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player") || other.CompareTag("enemy")){
            if(grassSounds.Length > 0){
                int index = Random.Range(0, grassSounds.Length);
                audioSource.PlayOneShot(grassSounds[index]);
            }
        }
    }
}