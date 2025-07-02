using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EstatuaComVida : MonoBehaviour
{
    public int vidaMaxima = 20;
    private int vidaAtual;

    [Header("Sprites de Estátua Quebrada")]
    public Sprite[] spritesQuebrados;

    private SpriteRenderer spriteRenderer;

    private static int estatuasDestruidas = 0;
    private static bool contadorResetado = false; // Garante reset uma vez por cena

    public static int totalEstatuas = 3;
    public static Transform pontoDoPortal;

    public GameObject portalPrefab;

    [Header("Summon de Inimigos")]
    public GameObject inimigoPrefab;
    public int limiteMaximoInimigos = 20;
    public float intervaloChecagem = 1f;

    private bool destruida = false;
    private List<GameObject> inimigosAtivos = new List<GameObject>();
    private Coroutine coroutineSummon;

    void Start()
    {
        // Resetar contador de estátuas destruídas só uma vez por cena
        if (!contadorResetado)
        {
            estatuasDestruidas = 0;
            contadorResetado = true;

            // Atualiza o total de estátuas na cena automaticamente
            totalEstatuas = FindObjectsByType<EstatuaComVida>(FindObjectsSortMode.None).Length;
        }

        vidaAtual = vidaMaxima;
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (inimigoPrefab != null && !destruida)
        {
            coroutineSummon = StartCoroutine(SummonInimigos());
        }
    }

    public void ReceberDano(int dano)
    {
        if (destruida) return;

        vidaAtual -= dano;

        if (vidaAtual <= 0)
        {
            vidaAtual = 0;
            destruida = true;

            if (coroutineSummon != null)
                StopCoroutine(coroutineSummon);

            // Destroi todos os inimigos ativos
            foreach (GameObject inimigo in inimigosAtivos)
            {
                if (inimigo != null)
                    Destroy(inimigo);
            }
            inimigosAtivos.Clear();

            // Muda sprite da estátua
            if (spritesQuebrados.Length > 0)
            {
                int sorteio = Random.Range(0, spritesQuebrados.Length);
                spriteRenderer.sprite = spritesQuebrados[sorteio];
            }

            estatuasDestruidas++;

            // Verifica se todas as estátuas foram destruídas para invocar o portal
            if (estatuasDestruidas >= totalEstatuas && portalPrefab != null)
            {
                Vector3 posicao = pontoDoPortal != null ? pontoDoPortal.position : transform.position;
                Instantiate(portalPrefab, posicao, Quaternion.identity);

                // Reseta flag para a próxima cena
                contadorResetado = false;
            }
        }
    }

    private IEnumerator SummonInimigos()
    {
        while (!destruida)
        {
            // Limpa referências nulas (inimigos mortos)
            inimigosAtivos.RemoveAll(item => item == null);

            int inimigosFaltando = limiteMaximoInimigos - inimigosAtivos.Count;

            for (int i = 0; i < inimigosFaltando; i++)
            {
                Vector3 posSpawn = transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
                GameObject novoInimigo = Instantiate(inimigoPrefab, posSpawn, Quaternion.identity);
                inimigosAtivos.Add(novoInimigo);

                // Informa ao inimigo de qual estátua ele veio
                Inimigo inimigoScript = novoInimigo.GetComponent<Inimigo>();
                if (inimigoScript != null)
                {
                    inimigoScript.SetEstatua(this);
                }
            }

            yield return new WaitForSeconds(intervaloChecagem);
        }
    }

    public void RemoverInimigo(GameObject inimigo)
    {
        inimigosAtivos.Remove(inimigo);
    }
}