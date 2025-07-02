using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private ControleJogo controle;

    void Start()
    {
        // Tenta encontrar o ControleJogo com seguran�a ao iniciar
        controle = FindFirstObjectByType<ControleJogo>();

        if (controle == null)
            Debug.LogWarning("ControleJogo n�o encontrado na cena. PauseMenu n�o funcionar� corretamente.");
    }

    public void BotaoVoltar()
    {
        if (controle != null)
            controle.VoltarJogo();
        else
            Debug.LogError("N�o foi poss�vel voltar ao jogo: ControleJogo n�o encontrado.");
    }

    public void BotaoSalvarSair()
    {
        if (controle != null)
            controle.SalvarEFechar();
        else
            Debug.LogError("N�o foi poss�vel salvar e sair: ControleJogo n�o encontrado.");
    }
}
