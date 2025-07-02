using UnityEngine;

public class SistemaArma : MonoBehaviour
{
    Vector2 mousePosi;
    Vector2 dirArma;

    float angle;

    bool podeAtirar = true;

    [SerializeField] SpriteRenderer srGum;
    [SerializeField] float tempoEntreTiros;
    [SerializeField] Transform pontoDeFogo;
    [SerializeField] GameObject tiro;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        mousePosi = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 1️⃣ Bloqueia tiro se jogo pausado
        if (PauseMenu.instance != null && PauseMenu.instance.isPaused)
        {
            return;
        }

        // 2️⃣ Se NÃO está pausado, permite atirar sempre, sem checar UI
        if (Input.GetMouseButton(0) && podeAtirar)
        {
            podeAtirar = false;
            Instantiate(tiro, pontoDeFogo.position, pontoDeFogo.rotation);
            Invoke(nameof(CDTiro), tempoEntreTiros);
        }

    }

    private void FixedUpdate()
    {
        dirArma = mousePosi - new Vector2(transform.position.x, transform.position.y);
        angle = Mathf.Atan2(dirArma.y, dirArma.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void CDTiro()
    {
        podeAtirar = true;
    }

}