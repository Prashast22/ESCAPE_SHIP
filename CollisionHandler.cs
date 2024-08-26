using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float delayScene = 1f;
    [SerializeField] AudioClip crashSound;
    [SerializeField] AudioClip finishSound;

    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem finishParticles;

    static int lifeCount = 5;

    Movement movement;
    AudioSource audioSource;
    bool isTransitioning = false;

    void Start() 
    {
        movement = GetComponent<Movement>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision other) 
    {
        if (isTransitioning)
        {
            return;
        }

        switch (other.gameObject.tag)
        {
            case "Friendly":
               Debug.Log("Launch Pad");
               break;
            case "Finish":
               StartNextSequence();
               break;
            default:
               StartCrashSequence();
               break;
        }        
    }

    void StartCrashSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(crashSound);
        movement.enabled = false;
        crashParticles.Play();
        Invoke("ReloadScene", delayScene);
    }

    void StartNextSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(finishSound);
        movement.enabled = false;
        finishParticles.Play();
        Invoke("LoadNextScene", delayScene);
    }

    void ReloadScene()
        {
            ReduceLife();

            int currentSceneIndex;
            if (lifeCount == 0)
            {
                currentSceneIndex = 0;
            }
            else 
            {
                currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            }
            SceneManager.LoadScene(currentSceneIndex);
        }
    
    void LoadNextScene()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;
            if (nextSceneIndex == SceneManager.sceneCountInBuildSettings){
                nextSceneIndex = 0;
            }
            SceneManager.LoadScene(nextSceneIndex);
        }

    void ReduceLife()
    {
        if (lifeCount > 0)
        {
            lifeCount--;
        }
    }
}
