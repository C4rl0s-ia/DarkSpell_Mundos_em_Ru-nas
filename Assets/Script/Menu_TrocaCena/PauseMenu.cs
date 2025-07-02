using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool isPaused;
    public static PauseMenu instance;

    [Header("Paineis e Menu")]
    public GameObject pausePanel;
    public string MenuIniciar;

    [Header("Opções")]
    public GameObject menuOpcoesCanvas; // Referência ao Canvas de opções

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        Time.timeScale = 1.0f;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Se o menu de opções estiver aberto, fecha ele antes de despausar
            if (menuOpcoesCanvas != null && menuOpcoesCanvas.activeSelf)
            {
                menuOpcoesCanvas.SetActive(false);
                return;
            }

            PauseScreen();
        }
    }

    void PauseScreen()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1.0f;
        pausePanel.SetActive(isPaused);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MenuInicial", LoadSceneMode.Single);
    }

    public void BackToMenuAndSave()
    {
        Salvar();

        foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (obj.scene.name == null || obj.scene.name == "")
            {
                Destroy(obj);
            }
        }

        BackToMenu();
    }

    public void MenuOption()
    {
        if (menuOpcoesCanvas != null)
        {
            menuOpcoesCanvas.SetActive(true);
        }
    }

    public void Salvar()
    {
        string cenaAtual = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("Cena", cenaAtual);

        GameObject jogador = GameObject.FindWithTag("Player");
        if (jogador != null)
        {
            Player playerScript = jogador.GetComponent<Player>();

            PlayerPrefs.SetInt("Vida", playerScript.vidaAtual);

            Vector3 pos = jogador.transform.position;
            PlayerPrefs.SetFloat("PlayerX", pos.x);
            PlayerPrefs.SetFloat("PlayerY", pos.y);
            PlayerPrefs.SetFloat("PlayerZ", pos.z);
        }
        else
        {
            Debug.LogWarning("Jogador não encontrado para salvar.");
        }

        PlayerPrefs.Save();
        Debug.Log("Jogo salvo!");
    }

    public void Carregar()
    {
        if (PlayerPrefs.HasKey("Cena"))
        {
            string cenaSalva = PlayerPrefs.GetString("Cena");

            SceneManager.sceneLoaded += OnSceneLoaded;

            SceneManager.LoadScene(cenaSalva);
        }
        else
        {
            Debug.Log("Nenhum save encontrado.");
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
}
