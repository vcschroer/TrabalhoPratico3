using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Referências")]
    public Transform playerTransform;

    [Header("Spawn de Cenário (ProBuilder)")]
    public float tamanhoDoBloco = 30f;
    private float proximoZ_Cenario = 0f;
    private int blocosIniciais = 5;

    [Header("Propriedades dos Obstáculos (Dificuldade)")]
    public float obstacleSpawnInterval = 3f; 
    private float obstacleTimer;

    [Header("Propriedades dos Inimigos (Dificuldade)")]
    public float enemySpawnInterval = 5f;
    private float enemyTimer;
    [HideInInspector] public float velocidadeProjetilInimigo = 10f;
    [HideInInspector] public int quantidadeProjeteisInimigo = 1;    

    [Header("Configurações da Curva de Dificuldade")]
    public float tempoParaAumentarDificuldade = 10f;
    private float dificuldadeTimer;

    void Awake() => Instance = this;

    void Start()
    {
        for (int i = 0; i < blocosIniciais; i++)
        {
            SpawnCenarioProcedural();
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        if (playerTransform.position.z > proximoZ_Cenario - (tamanhoDoBloco * 3))
        {
            SpawnCenarioProcedural();
        }

        obstacleTimer += Time.deltaTime;
        if (obstacleTimer >= obstacleSpawnInterval)
        {
            SpawnObstaculoProcedural();
            obstacleTimer = 0f;
        }

        enemyTimer += Time.deltaTime;
        if (enemyTimer >= enemySpawnInterval)
        {
            SpawnInimigoProcedural();
            enemyTimer = 0f;
        }

        AumentarDificuldadeGradual();
    }

    void SpawnCenarioProcedural()
    {
        ObjectPooler.Instance.SpawnFromPool("Cenario", new Vector3(0, 0, proximoZ_Cenario), Quaternion.identity);
        proximoZ_Cenario += tamanhoDoBloco;
    }

    void SpawnObstaculoProcedural()
    {
        Vector3 posicaoSpawn = new Vector3(Random.Range(-2f, 2f), 1f, playerTransform.position.z + 40f);
        ObjectPooler.Instance.SpawnFromPool("Obstacle", posicaoSpawn, Quaternion.identity);
    }

    void SpawnInimigoProcedural()
    {
        bool noTeto = Random.value > 0.5f;
        float posY = noTeto ? 6f : 1f; 

        Vector3 posicaoSpawn = new Vector3(Random.Range(-2f, 2f), posY, playerTransform.position.z + 45f);
        GameObject inimigo = ObjectPooler.Instance.SpawnFromPool("Enemy", posicaoSpawn, Quaternion.identity);

        if (inimigo != null)
        {
            inimigo.GetComponent<EnemyController>().ConfigurarInimigo(noTeto);
        }
    }

    void AumentarDificuldadeGradual()
    {
        dificuldadeTimer += Time.deltaTime;

        if (dificuldadeTimer >= tempoParaAumentarDificuldade)
        {
            obstacleSpawnInterval = Mathf.Max(0.8f, obstacleSpawnInterval - 0.3f);

            velocidadeProjetilInimigo += 3f;

            if (quantidadeProjeteisInimigo < 5)
                quantidadeProjeteisInimigo++;

            dificuldadeTimer = 0f;
            Debug.Log($"Dificuldade Aumentou! Spawn Obstáculo: {obstacleSpawnInterval}s | Vel Tiro: {velocidadeProjetilInimigo} | Qtd Tiros: {quantidadeProjeteisInimigo}");
        }
    }
}