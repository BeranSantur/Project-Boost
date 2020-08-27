using UnityEngine;
using UnityEngine.SceneManagement;


public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    bool isAlive;
    [SerializeField] float thrustVelocity;
    [SerializeField] float rotateVelocity;

    [SerializeField] AudioClip onThrust;
    [SerializeField] AudioClip onFinish;
    [SerializeField] AudioClip onDeath;

    [SerializeField] ParticleSystem onThrustParticiple;
    [SerializeField] ParticleSystem onFinishParticiple;
    [SerializeField] ParticleSystem onDeathParticiple;

    bool collisionDisabled;
    bool onStart;


    //MeshRenderer meshRenderer;



    AudioSource audioSource;

    // Start is called before the first frame update
     void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        isAlive = true;
        onStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        DebugKeys(); //if it is on debug mode
        if (isAlive == true)
        {
            if (Thrust())
            {
                Rotate();
            }

        }
    }

    private void DebugKeys()
    {
        if (Debug.isDebugBuild)
        {

            if (Input.GetKey(KeyCode.C))
            {
                collisionDisabled = !collisionDisabled;
                print("Debug");

            }
            if (Input.GetKey(KeyCode.L))
            {
                LoadNextScene();
            }

        }
    }

    private void OnCollisionEnter(Collision collision)
    {



        if (!isAlive || collisionDisabled) { return; }

        switch (collision.gameObject.tag)
        {
            case "Finish":
                FinishProcess();

                break;
            case "Start":
                isAlive = true;
                //if (!onStart) { DeathProcess(); }
                break;
            default:
                DeathProcess();
                break;
        }
  


    }
    private void OnCollisionExit(Collision collision)
    {
        onStart = false;
    }

    private void FinishProcess()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(onFinish);
        onThrustParticiple.Stop();
        onFinishParticiple.Play();
        Invoke("LoadNextScene", 1f);
        isAlive = false;
    }

    private void DeathProcess()
    {
        audioSource.Stop();
        onThrustParticiple.Stop();
        audioSource.PlayOneShot(onDeath);
        onDeathParticiple.Play();

        Invoke("LoadCurrentLevel", 1f);
        isAlive = false;
    }

    private void LoadNextScene()
    {
        onFinishParticiple.Stop();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings) { nextSceneIndex = 0; }
        SceneManager.LoadScene(nextSceneIndex);
        
    }

    private  void LoadCurrentLevel()
    {
        onDeathParticiple.Stop();
        int currentSceneIndex=SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void Rotate()
    {
        rigidBody.angularVelocity = Vector3.zero;
        
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward*(rotateVelocity * Time.deltaTime));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-(Vector3.forward * (rotateVelocity * Time.deltaTime)));

        }
        
    }

    private bool Thrust()
    {
        
        if (Input.GetKey(KeyCode.Space))
        {


            rigidBody.AddRelativeForce(new Vector3(0,5f *(thrustVelocity*Time.deltaTime),0));

            if (!audioSource.isPlaying)
            {
                
                    audioSource.PlayOneShot(onThrust);
                    onThrustParticiple.Play();
                    return true;

                

            }
    
            return true;

        }
        else
        {
            audioSource.Stop();
            onThrustParticiple.Stop();
            return false;
        }

    }

}
