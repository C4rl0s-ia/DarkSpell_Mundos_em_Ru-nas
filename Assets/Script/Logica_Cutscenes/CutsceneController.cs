using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    public float tempoCutscene = 5f; // dura��o em segundos
    public string proximaCena; // nome da pr�xima cena

    void Start()
    {
        Invoke("CarregarProximaCena", tempoCutscene);
    }

    void CarregarProximaCena()
    {
        SceneManager.LoadScene(proximaCena);
    }
}