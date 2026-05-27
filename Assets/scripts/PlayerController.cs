using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Movimentação")]
    public float forwardSpeed = 10f;
    public float laneSpeed = 8f;
    public float limiteLateral = 4.5f;

    [Header("Pulo")]
    public float jumpForce = 7f;
    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        Time.timeScale = 1f;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        float movimentoHorizontal = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * movimentoHorizontal * laneSpeed * Time.deltaTime, Space.World);

        Vector3 posicaoClamped = transform.position;
        posicaoClamped.x = Mathf.Clamp(posicaoClamped.x, -limiteLateral, limiteLateral); 
        transform.position = posicaoClamped;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Cenario"))
        {
            isGrounded = true;
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Morrer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Projectile") || other.gameObject.CompareTag("Obstacle"))
        {
            Morrer();
        }
    }

    void Morrer()
    {
        Debug.Log("Player Morreu! Reiniciando a fase...");

        Scene cenaAtual = SceneManager.GetActiveScene();
        SceneManager.LoadScene(cenaAtual.buildIndex);
    }
}