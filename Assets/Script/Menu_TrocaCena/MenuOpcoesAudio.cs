using UnityEngine;
using UnityEngine.UI;

public class MenuOpcoesAudio : MonoBehaviour
{
    public Slider sliderMusica;
    public Slider sliderEfeitos;

    public GameObject menuOpcoesCanvas;

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            sliderMusica.value = AudioManager.Instance.volumeMusica;
            sliderEfeitos.value = AudioManager.Instance.volumeEfeitos;
        }

        sliderMusica.onValueChanged.AddListener(OnMusicaVolumeChanged);
        sliderEfeitos.onValueChanged.AddListener(OnEfeitosVolumeChanged);
    }

    private void OnMusicaVolumeChanged(float value)
    {
        AudioManager.Instance.AjustarVolumeMusica(value);
    }

    private void OnEfeitosVolumeChanged(float value)
    {
        AudioManager.Instance.AjustarVolumeEfeitos(value);
    }

    public void FecharMenuOpcoes()
    {
        if (menuOpcoesCanvas != null)
            menuOpcoesCanvas.SetActive(false);
    }

    private void OnDestroy()
    {
        sliderMusica.onValueChanged.RemoveListener(OnMusicaVolumeChanged);
        sliderEfeitos.onValueChanged.RemoveListener(OnEfeitosVolumeChanged);
    }
}
