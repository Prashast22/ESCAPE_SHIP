using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float mainRotate = 100f;
    [SerializeField] AudioClip audioClip;

    [SerializeField] ParticleSystem mainParticles;
    [SerializeField] ParticleSystem leftParticles;
    [SerializeField] ParticleSystem rightParticles;
    
    Rigidbody rb;
    AudioSource audioSource;
    CollisionHandler collisionHandler;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        collisionHandler = GetComponent<CollisionHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotate();
        LoadNextLevel();
        DisableCollisions();
    }

    void ProcessThrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(audioClip);
            }
            if (!mainParticles.isPlaying)
            {
                mainParticles.Play();
            }
        }
        else
        {
            audioSource.Stop();
            mainParticles.Stop();
        }
    }

    void ProcessRotate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            ApplyRotation(mainRotate);
            if (!rightParticles.isPlaying)
            {
                rightParticles.Play();
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            ApplyRotation(-mainRotate);
            if (!leftParticles.isPlaying)
            {
                leftParticles.Play();
            }
        }
        else
        {
            rightParticles.Stop();
            leftParticles.Stop();
        }
    }

    void LoadNextLevel()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;
            if (nextSceneIndex == SceneManager.sceneCountInBuildSettings){
                nextSceneIndex = 0;
            }
            SceneManager.LoadScene(nextSceneIndex);
        }
        
    }

    void DisableCollisions()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            collisionHandler.enabled = false;
        }
    }

    void ApplyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false;
    }
}
