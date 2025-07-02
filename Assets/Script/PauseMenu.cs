using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private ControleJogo controle;

    void Start()
    {
        // Tenta encontrar o ControleJogo com segurança ao iniciar
        controle = FindFirstObjectByType<ControleJogo>();

        if (controle == null)
            Debug.LogWarning("ControleJogo não encontrado na cena. PauseMenu não funcionará corretamente.");
    }

    public void BotaoVoltar()
    {
        if (controle != null)
            controle.VoltarJogo();
        else
            Debug.LogError("Não foi possível voltar ao jogo: ControleJogo não encontrado.");
    }

    public void BotaoSalvarSair()
    {
        if (controle != null)
            controle.SalvarEFechar();
        else
            Debug.LogError("Não foi possível salvar e sair: ControleJogo não encontrado.");
    }
}
