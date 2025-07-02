using UnityEngine;

public class BalaEspecial : MonoBehaviour
{
    [SerializeField] public float speed = 5f;
    [SerializeField] public float tempoDeVida = 3f;
    [SerializeField] public int dano = 5;


    [HideInInspector] public float speedBase;  // Valor base da velocidade
    [HideInInspector] public int danoBase;     // Valor base do dano

    void Start()
    {
        speedBase = speed;   // Guarda valor original da velocidade
        danoBase = dano;     // Guarda valor original do dano

        Destroy(gameObject, tempoDeVida);
    }

    void FixedUpdate()
    {
        // Move a bala na direção que está "pra cima" do objeto
        transform.Translate(transform.up * speed * Time.fixedDeltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Dano em estátuas
        if (collision.TryGetComponent<EstatuaComVida>(out var estatua))
        {
            estatua.ReceberDano(dano);
            Destroy(gameObject);
            return;
        }

        // Dano em inimigos
        if (collision.CompareTag("Inimigo"))
        {
            Inimigo inimigo = collision.GetComponent<Inimigo>();
            if (inimigo != null)
            {
                Debug.Log("Atingiu o inimigo!"); // teste visual no Console
                inimigo.ReceberDano(dano);
                Destroy(gameObject);
            }
        }
    }
}