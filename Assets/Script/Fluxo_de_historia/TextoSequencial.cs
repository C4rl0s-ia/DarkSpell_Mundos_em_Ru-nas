using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class TextoSequencialFaseado : MonoBehaviour
{
    public TextMeshProUGUI texto1;
    public TextMeshProUGUI texto2;
    public string texto1Conteudo;
    public string texto2Conteudo;

    [Tooltip("Duração do fade in/out em segundos")]
    public float fadeDuration = 2f;

    [Tooltip("Tempo que o texto 1 fica totalmente visível antes do fade out")]
    public float tempoVisivelTexto1 = 6f;

    [Tooltip("Tempo que o texto 2 fica totalmente visível após o fade in")]
    public float tempoVisivelTexto2 = 10f;

    public string cenaParaCarregar;

    private void Start()
    {
        // Inicia os textos invisíveis
        SetAlpha(texto1, 0);
        SetAlpha(texto2, 0);

        // Define o texto
        texto1.text = texto1Conteudo;
        texto2.text = texto2Conteudo;

        StartCoroutine(RotinaFaseada());
    }

    IEnumerator RotinaFaseada()
    {
        // Texto 1 aparece
        yield return StartCoroutine(FadeIn(texto1));
        // Texto 1 visível por um tempo
        yield return new WaitForSeconds(tempoVisivelTexto1);

        // Começa o fade out do texto 1 E fade in do texto 2 simultaneamente
        Coroutine fadeOutTexto1 = StartCoroutine(FadeOut(texto1));
        Coroutine fadeInTexto2 = StartCoroutine(FadeIn(texto2));

        // Espera os dois finishes (fade in do texto 2 e fade out do texto 1)
        yield return fadeOutTexto1;
        yield return fadeInTexto2;

        // Texto 2 fica visível por um tempo
        yield return new WaitForSeconds(tempoVisivelTexto2);

        // Texto 2 desaparece
        yield return StartCoroutine(FadeOut(texto2));

        // Ambos invisíveis, agora troca a cena
        SceneManager.LoadScene(cenaParaCarregar);
    }

    IEnumerator FadeIn(TextMeshProUGUI texto)
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            SetAlpha(texto, Mathf.Clamp01(timer / fadeDuration));
            yield return null;
        }
        SetAlpha(texto, 1);
    }

    IEnumerator FadeOut(TextMeshProUGUI texto)
    {
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            SetAlpha(texto, 1 - Mathf.Clamp01(timer / fadeDuration));
            yield return null;
        }
        SetAlpha(texto, 0);
    }

    void SetAlpha(TextMeshProUGUI texto, float alpha)
    {
        Color c = texto.color;
        c.a = alpha;
        texto.color = c;
    }
}
