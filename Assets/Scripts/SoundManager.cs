using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Sonidos")]
    [SerializeField] private AudioClip sonidoVictoria;
    [SerializeField] private AudioClip sonidoDerrota;
    [SerializeField] private AudioClip sonidoDisparoPlayer;
    [SerializeField] private AudioClip sonidoDisparoTorreta;

    private AudioSource audioSource;

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

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayVictoria() => audioSource.PlayOneShot(sonidoVictoria);
    public void PlayDerrota() => audioSource.PlayOneShot(sonidoDerrota);
    public void PlayDisparoPlayer() => audioSource.PlayOneShot(sonidoDisparoPlayer);
    public void PlayDisparoTorreta() => audioSource.PlayOneShot(sonidoDisparoTorreta);
}