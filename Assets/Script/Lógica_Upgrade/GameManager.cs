using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int inimigosDerrotados = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AdicionarKill()
    {
        inimigosDerrotados++;
        Debug.Log("Inimigos derrotados: " + inimigosDerrotados);
    }

    public int GetKills()
    {
        return inimigosDerrotados;
    }
}
