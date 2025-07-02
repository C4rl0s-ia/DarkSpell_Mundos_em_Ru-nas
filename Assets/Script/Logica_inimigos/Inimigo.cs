using UnityEngine;

public class Inimigo : MonoBehaviour
{
    [Header("Movimentação")]
    [SerializeField] public float velocidade = 3f;

    [Header("Combate")]
    [SerializeField] public int dano = 5;
    [SerializeField] public float intervaloDano = 1.5f;
    [SerializeField] public float forcaKnockback = 5f;
    [SerializeField] public int vida = 30;

    private Transform alvo;
    private float tempoUltimoDano;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;

    private EstatuaComVida estatuaPai;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Garante que há um AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;

        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
            alvo = playerGO.transform;
    }

    void FixedUpdate()
    {
        if (alvo != null)
        {
            Vector2 direcao = (alvo.position - transform.position).normalized;
            rb.linearVelocity = direcao * velocidade;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();

            if (player != null && Time.time >= tempoUltimoDano + intervaloDano)
            {
                player.ReceberDano(dano);

                Vector2 direcaoKnockback = (collision.transform.position - transform.position).normalized;
                player.AplicarKnockback(direcaoKnockback * forcaKnockback);

                tempoUltimoDano = Time.time;
            }
        }
    }

    public void ReceberDano(int danoRecebido)
    {
        vida -= danoRecebido;

        if (vida <= 0)
        {
            Morrer();
        }
    }

    public void SetEstatua(EstatuaComVida estatua)
    {
        estatuaPai = estatua;
    }

    private void Morrer()
    {


        if (estatuaPai != null)
        {
            estatuaPai.RemoverInimigo(gameObject);
        }

        Destroy(gameObject);

        if (GameManager.Instance != null)
            GameManager.Instance.AdicionarKill();

    }
}
