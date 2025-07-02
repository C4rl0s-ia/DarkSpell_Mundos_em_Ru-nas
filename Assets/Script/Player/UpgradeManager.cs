using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeManager : MonoBehaviour
{
    public Player player;         
    public Bala balaPrefab;

    public int vidaBase = 100;  // Valor base da vida do player, deve ser igual ao valor inicial de vidaMaxima

    private void Start()
    {
        string cena = SceneManager.GetActiveScene().name.ToLower();

        if (cena == "lobby2" || cena == "lobby3")
        {
            AplicarUpgrade();
        }
    }

    private void AplicarUpgrade()
    {
        int kills = GameManager.Instance != null ? GameManager.Instance.GetKills() : 0;

        Debug.Log($"Aplicando upgrade com base em {kills} kills.");

        // Atualiza vida máximo do player baseado no valor base + upgrade
        player.vidaMaxima = vidaBase + kills * 2;
        player.vidaAtual = player.vidaMaxima;

        // Atualiza velocidade do player baseado na velocidade base + upgrade
        player.Speed = player.SpeedBase + kills * 0.02f;

        // Atualiza dano e velocidade da bala baseado nos valores base + upgrade
        balaPrefab.dano = balaPrefab.danoBase + Mathf.RoundToInt(kills * 0.5f);
        balaPrefab.speed = balaPrefab.speedBase + kills * 0.02f;

        Debug.Log($"Upgrade aplicado: Vida={player.vidaMaxima}, Dano={balaPrefab.dano}, Velocidade Player={player.Speed}, Velocidade Bala={balaPrefab.speed}");
    }
}
