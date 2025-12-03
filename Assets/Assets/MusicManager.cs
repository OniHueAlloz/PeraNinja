using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour {
    private AudioSource audioSource;
    public AudioSource sfxSource;
    
    [Header("Músicas")]
    public AudioClip menu;
    public AudioClip gameplay;
    
    [Header("UI e Eventos")]
    public AudioClip uiTaiko;
    public AudioClip eventGong;

    private string currentSceneName;
    private string previousSceneName;
    public static MusicManager instance;

    void Awake(){
        if (instance != null && instance != this){
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = true;

        previousSceneName = SceneManager.GetActiveScene().name;
    }

    void Start(){
        Debug.Log("Number of MusicManager instances: " + FindObjectsOfType<MusicManager>().Length);
        MusicPlay(SceneManager.GetActiveScene().name);
    }

    void Update(){
        currentSceneName = SceneManager.GetActiveScene().name;
        if (currentSceneName != previousSceneName){
            previousSceneName = currentSceneName;
            MusicPlay(currentSceneName);
        }

        if (Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
    }

    void MusicPlay (string sceneName){
        if(sceneName == "Game"){
            if (audioSource.clip != gameplay){
                audioSource.clip = gameplay;
                audioSource.Play();
            }
        } else {
            if (audioSource.clip != menu){
                audioSource.clip = menu;
                audioSource.Play();
            }
        }
    }

    public void PlayTaiko(){
        if(sfxSource != null && uiTaiko != null){
            sfxSource.PlayOneShot(uiTaiko);
        }
    }

    public void PlayGong(){
        if(sfxSource != null && eventGong != null){
            sfxSource.PlayOneShot(eventGong);
        }
    }
}