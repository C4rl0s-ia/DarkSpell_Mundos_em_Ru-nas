using UnityEngine;

/// <summary>
/// Responsável por rotacionar a arma e disparar projéteis.
/// </summary>
public class SistemaArma : MonoBehaviour
{
    // Alvo (posição do mouse)
    private Vector2 mousePosi;
    private Vector2 dirArma;
    private float angle;

    private bool podeAtirar = true;

    [Header("Sprites e Visual")]
    [SerializeField] private SpriteRenderer srGum;

    [Header("Configuração de Tiro")]
    [SerializeField] private float tempoEntreTiros = 0.3f;
    [SerializeField] private Transform pontoDeFogo; // Onde o projétil nasce
    [SerializeField] private GameObject prefabBala; // Prefab da bala

    [Header("Parâmetros da Bala")]
    [SerializeField] private float danoDoTiro = 15f;
    [SerializeField] private float velocidadeDoTiro = 5f;

    void Update()
    {
        // Pega posição do mouse em mundo
        mousePosi = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Bloqueia tiro se o jogo está pausado
        if (PauseMenu.instance != null && PauseMenu.instance.isPaused)
            return;

        // Atira se clicou e pode atirar
        if (Input.GetMouseButton(0) && podeAtirar)
        {
            podeAtirar = false;

            // Instancia a bala e configura os atributos
            GameObject novaBala = Instantiate(prefabBala, pontoDeFogo.position, pontoDeFogo.rotation);

            Bala balaScript = novaBala.GetComponent<Bala>();
            if (balaScript != null)
            {
                balaScript.speed = velocidadeDoTiro;
                balaScript.dano = Mathf.RoundToInt(danoDoTiro);
            }

            Invoke(nameof(RecarregarTiro), tempoEntreTiros);
        }
    }

    void FixedUpdate()
    {
        // Calcula direção e ângulo
        dirArma = mousePosi - new Vector2(transform.position.x, transform.position.y);
        angle = Mathf.Atan2(dirArma.y, dirArma.x) * Mathf.Rad2Deg - 90f;

        // Rotaciona arma
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    /// <summary>
    /// Libera o próximo tiro após o delay.
    /// </summary>
    void RecarregarTiro()
    {
        podeAtirar = true;
    }
}
