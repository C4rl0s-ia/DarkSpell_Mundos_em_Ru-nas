using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicaSource;
    public AudioSource efeitosSource;

    [Range(0f, 1f)]
    public float volumeMusica = 1f;

    [Range(0f, 1f)]
    public float volumeEfeitos = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Carrega volumes salvos (ou usa 1 caso não exista)
        volumeMusica = PlayerPrefs.GetFloat("VolumeMusica", 1f);
        volumeEfeitos = PlayerPrefs.GetFloat("VolumeEfeitos", 1f);

        AtualizarVolumes();
    }

    public void AjustarVolumeMusica(float volume)
    {
        volumeMusica = volume;

        if (musicaSource != null)
            musicaSource.volume = volumeMusica;

        // Salvar volume da música
        PlayerPrefs.SetFloat("VolumeMusica", volumeMusica);
        PlayerPrefs.Save();
    }

    public void AjustarVolumeEfeitos(float volume)
    {
        volumeEfeitos = volume;

        if (efeitosSource != null)
            efeitosSource.volume = volumeEfeitos;

        // Salvar volume dos efeitos
        PlayerPrefs.SetFloat("VolumeEfeitos", volumeEfeitos);
        PlayerPrefs.Save();
    }

    public void AtualizarVolumes()
    {
        if (musicaSource != null)
            musicaSource.volume = volumeMusica;

        if (efeitosSource != null)
            efeitosSource.volume = volumeEfeitos;
    }
}
