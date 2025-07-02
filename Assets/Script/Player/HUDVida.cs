using UnityEngine;
using UnityEngine.UI;

public class HUDVida : MonoBehaviour
{
    [SerializeField] private Image barraDeVida; // refer�ncia ao Image do HUD
    [SerializeField] private Player player;     // refer�ncia ao script do player

    [SerializeField] private float velocidadeAtualizacao = 5f; // velocidade da interpola��o

    private float valorAtualBarra = 1f;

    void Update()
    {
        float vidaAtual = player.GetVidaAtual();
        float vidaMax = player.GetVidaMaxima();
        float porcentagem = vidaAtual / vidaMax;

        // Interpola��o suave
        valorAtualBarra = Mathf.Lerp(valorAtualBarra, porcentagem, Time.deltaTime * velocidadeAtualizacao);
        barraDeVida.fillAmount = valorAtualBarra;
    }
}
