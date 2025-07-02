using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class HUDController : MonoBehaviour
{
    [Header("Ícones de Ataques")]
    public GameObject iconeAtaqueBasico;
    public GameObject iconeAtaqueEspecial;

    [Header("Status do Player")]
    public TextMeshProUGUI valorDano;           // Dano da bala
    public TextMeshProUGUI valorVelocidade;     // Velocidade do player
    public TextMeshProUGUI valorVelocidadeAtaque; // Velocidade da bala
    public TextMeshProUGUI valorVida;           // Vida atual/maxima

    [Header("Referências")]
    public Player player;
    public Bala balaPrefab; // Prefab da bala que tem o dano e velocidade

    [Header("Barra de Vida")]
    public Image barraVida;  // imagem com tipo Fill

    private string[] cenasComAtaqueEspecial = { "lobby3", "fase3", "lobby4" };

    private void Start()
    {
        VerificarAtaqueEspecial();
    }

    private void Update()
    {
        AtualizarStatus();
    }

    private void VerificarAtaqueEspecial()
    {
        string cenaAtual = SceneManager.GetActiveScene().name.ToLower();
        bool mostrarEspecial = false;

        foreach (string cena in cenasComAtaqueEspecial)
        {
            if (cenaAtual == cena)
            {
                mostrarEspecial = true;
                break;
            }
        }

        if (iconeAtaqueEspecial != null)
            iconeAtaqueEspecial.SetActive(mostrarEspecial);
    }

    private void AtualizarStatus()
    {
        if (player == null || balaPrefab == null)
            return;

        // Dano vem da bala
        if (valorDano != null)
            valorDano.text = balaPrefab.dano.ToString();

        // Velocidade do player
        if (valorVelocidade != null)
            valorVelocidade.text = player.Speed.ToString("F1");

        // Velocidade da bala
        if (valorVelocidadeAtaque != null)
            valorVelocidadeAtaque.text = balaPrefab.speed.ToString("F1");

        // Vida atual / máxima
        if (valorVida != null)
            valorVida.text = $"{player.vidaAtual}/{player.vidaMaxima}";

        // Atualiza barra de vida
        if (barraVida != null)
        {
            barraVida.fillAmount = (float)player.vidaAtual / player.vidaMaxima;
        }
    }
}
