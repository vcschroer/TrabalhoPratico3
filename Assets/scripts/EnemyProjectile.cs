using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Vector3 direcaoTiro;
    private float velocidadeTiro;
    private float timerDesativar;

    public void IniciarMovimento(Vector3 dir, float vel)
    {
        direcaoTiro = dir;
        velocidadeTiro = vel;
        timerDesativar = 0f;
    }

    void Update()
    {
        transform.Translate(direcaoTiro * velocidadeTiro * Time.deltaTime, Space.World);

        timerDesativar += Time.deltaTime;
        if (timerDesativar >= 4f)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Ground"))
        {
            gameObject.SetActive(false);
        }
    }
}