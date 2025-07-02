using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDebugger : MonoBehaviour
{
    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Cena carregada: " + scene.name);
        Debug.Log("Root GameObjects na nova cena:");

        foreach (GameObject obj in scene.GetRootGameObjects())
        {
            Debug.Log(obj.name);
        }

        // Adicional: também logar todos que estão no DontDestroyOnLoad
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        Debug.Log("Todos os objetos ativos no projeto:");
        foreach (GameObject obj in allObjects)
        {
            Debug.Log(obj.name + " (Cena: " + obj.scene.name + ")");
        }
    }
}
