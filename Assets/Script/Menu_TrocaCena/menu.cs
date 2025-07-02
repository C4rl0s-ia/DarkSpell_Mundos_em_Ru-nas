using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class Menu : MonoBehaviour
{
    [Header("Cenas")]
    public string nomeCenaJogoNovo = "Lobby1";

    [Header("UI Mensagens")]
    public TextMeshProUGUI mensagemErroTexto;
    public CanvasGroup grupoErroTexto;

    public TextMeshProUGUI mensagemCarregandoTexto;
    public CanvasGroup grupoCarregandoTexto;

    [Header("Configurações")]
    [SerializeField] AudioSource efeitoAudio;
    public float duracaoMensagem = 3f;
    public float fadeDuration = 1f;

    private Coroutine fadeCoroutine;

    public GameObject menuOpcoesCanvas;
    public void IniciarJogo()
    {
        if (efeitoAudio != null)
            efeitoAudio.Play();
        else
            Debug.LogWarning("AudioSource 'efeitoAudio' não está atribuído.");

        ExibirMensagemCarregando();
        PlayerPrefs.DeleteKey("Cena");
        PlayerPrefs.DeleteKey("Vida");
        PlayerPrefs.DeleteKey("PlayerX");
        PlayerPrefs.DeleteKey("PlayerY");
        PlayerPrefs.DeleteKey("PlayerZ");
        SceneManager.LoadScene(nomeCenaJogoNovo);
    }

    public void CarregarUltimoJogo()
    {
        if (PlayerPrefs.HasKey("Cena"))
        {
            ExibirMensagemCarregando();

            string cenaSalva = PlayerPrefs.GetString("Cena");
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(cenaSalva);
        }
        else
        {
            MostrarMensagem("Não há jogo salvo. Inicie um novo jogo.");
            Debug.LogWarning("Nenhum jogo salvo encontrado.");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject jogador = GameObject.FindWithTag("Player");
        if (jogador != null)
        {
            Vector3 pos = new Vector3(
                PlayerPrefs.GetFloat("PlayerX"),
                PlayerPrefs.GetFloat("PlayerY"),
                PlayerPrefs.GetFloat("PlayerZ")
            );

            jogador.transform.position = pos;

            Player playerScript = jogador.GetComponent<Player>();
            playerScript.vidaAtual = PlayerPrefs.GetInt("Vida");
        }
        else
        {
            Debug.LogWarning("Jogador não encontrado na cena carregada.");
        }

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void AbrirOpcoes()
    {
        if (menuOpcoesCanvas != null)
        {
            menuOpcoesCanvas.SetActive(true);
        }
    }
    public void SairDoJogo()
    {
        Debug.Log("Fechando o jogo...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void MostrarMensagem(string mensagem)
    {
        if (mensagemErroTexto != null && grupoErroTexto != null)
        {
            mensagemErroTexto.text = mensagem;
            grupoErroTexto.alpha = 1f;

            if (fadeCoroutine != null)
                StopCoroutine(fadeCoroutine);

            fadeCoroutine = StartCoroutine(FadeOutMensagemErro(duracaoMensagem, fadeDuration));
        }
    }

    private IEnumerator FadeOutMensagemErro(float delay, float fadeTime)
    {
        yield return new WaitForSeconds(delay);

        float tempo = 0f;
        float alphaInicial = grupoErroTexto.alpha;

        while (tempo < fadeTime)
        {
            tempo += Time.deltaTime;
            float novoAlpha = Mathf.Lerp(alphaInicial, 0f, tempo / fadeTime);
            grupoErroTexto.alpha = novoAlpha;
            yield return null;
        }

        grupoErroTexto.alpha = 0f;
    }

    private void ExibirMensagemCarregando()
    {
        if (mensagemCarregandoTexto != null && grupoCarregandoTexto != null)
        {
            mensagemCarregandoTexto.text = "Carregando...";
            grupoCarregandoTexto.alpha = 1;
        }
    }
}
