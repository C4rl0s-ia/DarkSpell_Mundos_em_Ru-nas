using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{
    [Header("Movimentação")]
    public float Speed;
    [HideInInspector] public float SpeedBase;  // Guarda o valor original da velocidade
    private Vector2 ultimaDirecao = Vector2.down;

    [Header("Vida")]
    [SerializeField] public int vidaMaxima = 100;
    public int vidaAtual;

    [Header("Sprite ao morrer")]
    [SerializeField] public Sprite spriteMagoDerrotado;

    [Header("Tela de transição")]
    public Image telaDerrotaFade;
    public string nomeCenaDerrota;

    private bool morto = false;

    [Header("Controle de Jogo - Menu")]
    public string cena;

    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    void Start()
    {
        SpeedBase = Speed;  // Guarda o valor inicial
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        vidaAtual = vidaMaxima;

        // Garante que a tela preta comece invisível
        if (telaDerrotaFade != null)
        {
            Color cor = telaDerrotaFade.color;
            telaDerrotaFade.color = new Color(cor.r, cor.g, cor.b, 0f);
        }
    }

    void Update()
    {
        if (morto) return;

        Vector2 direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.linearVelocity = direction.normalized * Speed;

        if (direction != Vector2.zero)
            ultimaDirecao = direction;

        AtualizarAnimacao(direction);
        VerificarRegeneracao();

    }

    private void AtualizarAnimacao(Vector2 direction)
    {
        if (direction.x != 0)
        {
            ResetLayer();
            anim.SetLayerWeight(2, 1);
            sprite.flipX = direction.x < 0;
        }
        else if (direction.y > 0)
        {
            ResetLayer();
            anim.SetLayerWeight(1, 1);
        }
        else if (direction.y < 0)
        {
            ResetLayer();
            anim.SetLayerWeight(0, 1);
        }

        anim.SetBool("Move", direction != Vector2.zero);
    }

    private void ResetLayer()
    {
        anim.SetLayerWeight(0, 0);
        anim.SetLayerWeight(1, 0);
        anim.SetLayerWeight(2, 0);
    }

    public void ReceberDano(int dano)
    {
        if (morto) return;

        vidaAtual -= dano;

        if (vidaAtual <= 0)
        {
            vidaAtual = 0;
            morto = true;
            rb.linearVelocity = Vector2.zero;
            sprite.sprite = spriteMagoDerrotado;
            anim.enabled = false;

            StartCoroutine(TransicaoParaDerrota());
        }
    }

    private IEnumerator TransicaoParaDerrota()
    {
        yield return new WaitForSeconds(1f);

        if (telaDerrotaFade != null)
        {
            float tempo = 1.5f;
            float t = 0f;
            Color cor = telaDerrotaFade.color;

            while (t < tempo)
            {
                t += Time.deltaTime;
                float alpha = Mathf.Lerp(0f, 1f, t / tempo);
                telaDerrotaFade.color = new Color(cor.r, cor.g, cor.b, alpha);
                yield return null;
            }
        }

        SceneManager.LoadScene(this.nomeCenaDerrota);
    }

    // --- Regeneração de Vida ---
    private Vector2 ultimaPosicao;
    private float tempoParado = 0f;
    private float tempoParaRegenerar = 10f;

    private void VerificarRegeneracao()
    {
        if (rb.position == ultimaPosicao)
        {
            tempoParado += Time.deltaTime;

            if (tempoParado >= tempoParaRegenerar)
            {
                RegenerarVida();
                tempoParado = 0f;
            }
        }
        else
        {
            tempoParado = 0f;
            ultimaPosicao = rb.position;
        }
    }

    private void RegenerarVida()
    {
        int quantidade = 10;
        vidaAtual = Mathf.Min(vidaAtual + quantidade, vidaMaxima);
    }

    // --- Auxiliares ---
    public int GetVidaAtual() => vidaAtual;
    public int GetVidaMaxima() => vidaMaxima;

    public void AplicarKnockback(Vector2 forca)
    {
        rb.AddForce(forca, ForceMode2D.Impulse);
    }

}
