using UnityEngine;
using UnityEngine.UI;

public class HUDVida : MonoBehaviour
{
    [SerializeField] private Image barraDeVida; // referência ao Image do HUD
    [SerializeField] private Player player;     // referência ao script do player

    [SerializeField] private float velocidadeAtualizacao = 5f; // velocidade da interpolação

    private float valorAtualBarra = 1f;

    void Update()
    {
        float vidaAtual = player.GetVidaAtual();
        float vidaMax = player.GetVidaMaxima();
        float porcentagem = vidaAtual / vidaMax;

        // Interpolação suave
        valorAtualBarra = Mathf.Lerp(valorAtualBarra, porcentagem, Time.deltaTime * velocidadeAtualizacao);
        barraDeVida.fillAmount = valorAtualBarra;
    }
}
