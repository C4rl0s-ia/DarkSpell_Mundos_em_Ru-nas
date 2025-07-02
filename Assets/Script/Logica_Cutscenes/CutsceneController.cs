using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    public float tempoCutscene = 5f; // duração em segundos
    public string proximaCena; // nome da próxima cena

    void Start()
    {
        Invoke("CarregarProximaCena", tempoCutscene);
    }

    void CarregarProximaCena()
    {
        SceneManager.LoadScene(proximaCena);
    }
}