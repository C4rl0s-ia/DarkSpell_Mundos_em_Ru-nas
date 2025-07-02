using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
public class movefase : MonoBehaviour
{
    [SerializeField]
    private string nomeProximaFase;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IniciarProxFase();
    }
    private void IniciarProxFase()
    {
        SceneManager.LoadScene(this.nomeProximaFase);
    }
}
