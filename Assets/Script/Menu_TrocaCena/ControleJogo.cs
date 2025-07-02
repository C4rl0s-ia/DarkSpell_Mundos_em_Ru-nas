using UnityEngine;
using UnityEngine.SceneManagement;

public class ControleJogo : MonoBehaviour
{
    [Header("Configurações")]
    [SerializeField] private GameObject pauseMenu; // Arraste seu GameObject PauseMenu aqui no Inspector
    [SerializeField] private Player player;        // Referência ao player para salvar estado

    private bool jogoPausado = false;

    void Start()
    {
        // Garante que o pause menu comece desativado
        if (pauseMenu != null)
            pauseMenu.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!jogoPausado)
                PausarJogo();
            else
                VoltarJogo();
        }
    }

    private void PausarJogo()
    {
        Time.timeScale = 0f; // Pausa o jogo
        jogoPausado = true;

        if (pauseMenu != null)
            pauseMenu.SetActive(true);
        else
            Debug.LogWarning("PauseMenu não atribuído no Inspector.");
    }

    public void VoltarJogo()
    {
        Time.timeScale = 1f; // Retoma o jogo
        jogoPausado = false;

        if (pauseMenu != null)
            pauseMenu.SetActive(false);
    }

    public void SalvarEFechar()
    {
        SalvarProgresso();

        Time.timeScale = 1f; // Retoma caso esteja pausado

        AplicarQuit();
    }

    private void SalvarProgresso()
    {
        string cenaAtual = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("CenaSalva", cenaAtual);

        if (player != null)
        {
            PlayerPrefs.SetInt("VidaSalva", player.GetVidaAtual());

            Vector3 pos = player.transform.position;
            PlayerPrefs.SetFloat("PosX", pos.x);
            PlayerPrefs.SetFloat("PosY", pos.y);
            PlayerPrefs.SetFloat("PosZ", pos.z);
        }

        PlayerPrefs.Save();

        Debug.Log("Progresso salvo!");
    }

    private void AplicarQuit()
    {
        Debug.Log("Fechando o jogo...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
