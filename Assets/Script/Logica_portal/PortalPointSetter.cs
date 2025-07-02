using UnityEngine;

public class PortalPointSetter : MonoBehaviour
{
    void Awake()
    {
        EstatuaComVida.pontoDoPortal = transform;
    }
}