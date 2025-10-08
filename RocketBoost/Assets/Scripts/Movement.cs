using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] InputAction thrust;
    [SerializeField] InputAction rotation;
    [SerializeField] float thrustStrength = 10f;
    [SerializeField] float rotationStrength = 10f;
    [SerializeField] AudioClip mainEngineSFX;
    [SerializeField] ParticleSystem mainEnginePFX;
    [SerializeField] ParticleSystem leftThrusterPFX;
    [SerializeField] ParticleSystem rightThrusterPFX;

    Rigidbody rb;
    AudioSource audioSource;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        thrust.Enable();
        rotation.Enable();
    }

    void FixedUpdate()
    {
        ProcessThrust();
        ProcessRotation();
        // This line enforces the rigidbody contraints that prevent unwanted movement. It's
        // required because of a later bit of code that unintentially unfreezes all constraints
        rb.constraints = RigidbodyConstraints.FreezeRotationY |
        RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezePositionZ;
    }

    // This handles vertical thrust generation and the sound effect for thrust
    private void ProcessThrust()
    {
        if (thrust.IsPressed())
        {
            StartThrusting();
        }
        else
        {
            StopThrusting();
        }
    }

    private void StartThrusting()
    {
        rb.AddRelativeForce(Vector3.up * thrustStrength * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngineSFX);
        }
        if (!mainEnginePFX.isPlaying)
        {
            mainEnginePFX.Play();
        }
    }

    private void StopThrusting()
    {
        audioSource.Stop();
        mainEnginePFX.Stop();
    }

    // This method handles player rotation inputs
    private void ProcessRotation()
    {
        float rotationInput = rotation.ReadValue<float>();
        if (rotationInput < 0)
        {
            RotateLeft();
        }
        else if (rotationInput > 0)
        {
            RoatateRight();
        }
        else
        {
            StopRotating();
        }
    }

    private void StopRotating()
    {
        leftThrusterPFX.Stop();
        rightThrusterPFX.Stop();
    }

    private void RotateLeft()
    {
        ApplyRotation(rotationStrength);
        if (!rightThrusterPFX.isPlaying)
        {
            leftThrusterPFX.Stop();
            rightThrusterPFX.Play();
        }
    }

    private void RoatateRight()
    {
        ApplyRotation(-rotationStrength);
        if (!leftThrusterPFX.isPlaying)
        {
            rightThrusterPFX.Stop();
            leftThrusterPFX.Play();
        }
    }

    // This method handles rotating the player rocket
    private void ApplyRotation(float rotationStrength)
    {
        rb.freezeRotation = true;
        transform.Rotate(Vector3.forward * rotationStrength * Time.deltaTime);
        rb.freezeRotation = false; // This is the line that unintentionally unfreezes all contraints
    }
}
