using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private bool noTeto;
    public float alcanceRaycast = 20f;
    public float taxaDeTiro = 2f;
    private float proximoTiro;

    public Transform canhao;

    public void ConfigurarInimigo(bool estaNoTeto)
    {
        noTeto = estaNoTeto;
    }

    void Update()
    {
        Vector3 direcaoRay = noTeto ? Vector3.down : Vector3.back;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direcaoRay, out hit, alcanceRaycast))
        {
            if (hit.collider.CompareTag("Player"))
            {
                if (Time.time > proximoTiro)
                {
                    proximoTiro = Time.time + taxaDeTiro;
                    AtirarRajada(hit.collider.transform);
                }
            }
        }

        Debug.DrawRay(transform.position, direcaoRay * alcanceRaycast, Color.red);
    }

    void AtirarRajada(Transform playerTarget)
    {
        int qtdProjeteis = GameManager.Instance.quantidadeProjeteisInimigo;
        float velProjetil = GameManager.Instance.velocidadeProjetilInimigo;

        for (int i = 0; i < qtdProjeteis; i++)
        {
            GameObject projetilObj = ObjectPooler.Instance.SpawnFromPool("Projectile", canhao.position, Quaternion.identity);

            if (projetilObj != null)
            {
                Vector3 direcaoDoTiro = (playerTarget.position - canhao.position).normalized;

                projetilObj.GetComponent<EnemyProjectile>().IniciarMovimento(direcaoDoTiro, velProjetil);
            }
        }
    }
}