using UnityEngine;

public class InimigoBoss : MonoBehaviour
{
    [Header("Movimentação")]
    [SerializeField] public float velocidade = 3f;

    [Header("Combate")]
    [SerializeField] public int dano = 5;
    [SerializeField] public float intervaloDano = 1.5f;
    [SerializeField] public float forcaKnockback = 5f;
    [SerializeField] public int vida = 30;

    [Header("Sprites - Andando")]
    [SerializeField] private Sprite[] walkUpSprites;
    [SerializeField] private Sprite[] walkDownSprites;
    [SerializeField] private Sprite[] walkLeftSprites;
    [SerializeField] private Sprite[] walkRightSprites;

    [Header("Sprites - Dano")]
    [SerializeField] private Sprite[] hurtUpSprites;
    [SerializeField] private Sprite[] hurtDownSprites;
    [SerializeField] private Sprite[] hurtLeftSprites;
    [SerializeField] private Sprite[] hurtRightSprites;

    [Header("Sprites - Morte")]
    [SerializeField] private Sprite[] deathUpSprites;
    [SerializeField] private Sprite[] deathDownSprites;
    [SerializeField] private Sprite[] deathLeftSprites;
    [SerializeField] private Sprite[] deathRightSprites;

    [Header("FPS da Animação")]
    [SerializeField] private float animFPS = 6f;

    private enum EstadoAnimacao { Andando, Dano, Morte }
    private EstadoAnimacao estadoAtual = EstadoAnimacao.Andando;

    private Transform alvo;
    private float tempoUltimoDano;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private float tempoAnimacao;
    private int frameAtual;
    private Vector2 ultimaDirecao = Vector2.down;

    private EstatuaComVida estatuaPai;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
            alvo = playerGO.transform;
    }

    void FixedUpdate()
    {
        if (estadoAtual == EstadoAnimacao.Morte)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (alvo == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direcao = (alvo.position - transform.position).normalized;

        if (estadoAtual == EstadoAnimacao.Andando)
            rb.linearVelocity = direcao * velocidade;
        else
            rb.linearVelocity = Vector2.zero;

        if (direcao != Vector2.zero)
            ultimaDirecao = direcao;
    }

    void Update()
    {
        AtualizarAnimacao();
    }

    void AtualizarAnimacao()
    {
        tempoAnimacao += Time.deltaTime;

        if (tempoAnimacao >= 1f / animFPS)
        {
            tempoAnimacao = 0f;
            frameAtual++;

            Sprite[] spritesAtuais = ObterSpritesPorEstadoEDirecao();

            if (spritesAtuais.Length > 0)
            {
                frameAtual %= spritesAtuais.Length;
                spriteRenderer.sprite = spritesAtuais[frameAtual];
            }
        }
    }

    Sprite[] ObterSpritesPorEstadoEDirecao()
    {
        switch (estadoAtual)
        {
            case EstadoAnimacao.Andando:
                return ObterSpritesPorDirecao(walkDownSprites, walkUpSprites, walkLeftSprites, walkRightSprites);
            case EstadoAnimacao.Dano:
                return ObterSpritesPorDirecao(hurtDownSprites, hurtUpSprites, hurtLeftSprites, hurtRightSprites);
            case EstadoAnimacao.Morte:
                return ObterSpritesPorDirecao(deathDownSprites, deathUpSprites, deathLeftSprites, deathRightSprites);
            default:
                return new Sprite[0];
        }
    }

    Sprite[] ObterSpritesPorDirecao(Sprite[] down, Sprite[] up, Sprite[] left, Sprite[] right)
    {
        if (Mathf.Abs(ultimaDirecao.x) > Mathf.Abs(ultimaDirecao.y))
        {
            return ultimaDirecao.x < 0 ? left : right;
        }
        else
        {
            return ultimaDirecao.y < 0 ? down : up;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (estadoAtual == EstadoAnimacao.Morte)
            return;

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
        if (estadoAtual == EstadoAnimacao.Morte)
            return;

        vida -= danoRecebido;

        if (vida <= 0)
        {
            Morrer();
        }
        else
        {
            // Troca para animação de dano
            estadoAtual = EstadoAnimacao.Dano;
            frameAtual = 0;
            tempoAnimacao = 0f;
            // Volta para andar depois de 0.4s
            Invoke(nameof(VoltarAndar), 0.4f);
        }
    }

    private void VoltarAndar()
    {
        if (estadoAtual != EstadoAnimacao.Morte)
        {
            estadoAtual = EstadoAnimacao.Andando;
        }
    }

    public void SetEstatua(EstatuaComVida estatua)
    {
        estatuaPai = estatua;
    }

    private void Morrer()
    {
        estadoAtual = EstadoAnimacao.Morte;
        frameAtual = 0;
        tempoAnimacao = 0f;

        if (estatuaPai != null)
        {
            estatuaPai.RemoverInimigo(gameObject);
        }

        // Destrói depois de 1s (ou ajuste conforme duração do sprite)
        Destroy(gameObject, 1f);
    }
}
