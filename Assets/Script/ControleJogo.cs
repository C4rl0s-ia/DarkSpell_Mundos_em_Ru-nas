using UnityEngine;
using UnityEngine.SceneManagement;

public class ControleJogo : MonoBehaviour
{
    [Header("Configurações")]
    public string nomeCenaPause = "PauseMenu"; // Nome da cena de pausa

    public Player player; // Refer�ncia ao player para salvar estado

    private bool jogoPausado = false;
    private bool cenaPauseCarregada = false;

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

        // Carrega a cena de pause em modo additive
        if (!cenaPauseCarregada)
        {
            SceneManager.LoadSceneAsync(nomeCenaPause, LoadSceneMode.Additive).completed += (op) =>
            {
                cenaPauseCarregada = true;
            };
        }
    }

    public void VoltarJogo()
    {
        Time.timeScale = 1f; // Retoma o tempo normal
        jogoPausado = false;

        if (cenaPauseCarregada)
        {
            // Descarrega a cena de pause
            SceneManager.UnloadSceneAsync(nomeCenaPause).completed += (op) =>
            {
                cenaPauseCarregada = false;
            };
        }
    }

    public void SalvarEFechar()
    {
        SalvarProgresso();

        Time.timeScale = 1f; // Garantir que o jogo n�o fique pausado

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
