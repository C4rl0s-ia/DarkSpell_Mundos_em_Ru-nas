using UnityEngine;

public class Especial : MonoBehaviour
{
    Vector2 mousePosi;
    Vector2 dirArma;
    float angle;

    bool podeAtirar = true;
    bool emRecarga = false;

    [SerializeField] SpriteRenderer srGum;
    [SerializeField] float tempoEntreTiros = 0.2f; // mais rápido
    [SerializeField] float tempoRecarga = 5f; // cooldown
    [SerializeField] Transform pontoDeFogo;
    [SerializeField] GameObject tiroEspecial;

    void Update()
    {
        mousePosi = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(1) && podeAtirar && !emRecarga)
        {
            podeAtirar = false;
            emRecarga = true;

            // Calcula direção
            dirArma = (mousePosi - (Vector2)pontoDeFogo.position).normalized;

            // Instancia o tiro e aplica velocidade
            GameObject tiro = Instantiate(tiroEspecial, pontoDeFogo.position, pontoDeFogo.rotation);
            Rigidbody2D rb = tiro.GetComponent<Rigidbody2D>();
            if (rb != null)
                rb.linearVelocity = dirArma * 12f; // mais rápido que o tiro normal

            // Dano pode ser ajustado no script do prefab (TiroEspecial.cs)

            Invoke(nameof(CDTiro), tempoEntreTiros);
            Invoke(nameof(FimRecarga), tempoRecarga);

            Debug.Log(" Habilidade especial usada. Aguardando recarga...");
        }
    }

    private void FixedUpdate()
    {
        dirArma = mousePosi - new Vector2(transform.position.x, transform.position.y);
        angle = Mathf.Atan2(dirArma.y, dirArma.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void CDTiro() => podeAtirar = true;
    void FimRecarga()
    {
        emRecarga = false;
        Debug.Log(" Habilidade especial pronta!");
    }
}