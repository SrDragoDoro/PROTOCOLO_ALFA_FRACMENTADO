using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SkipScene : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private KeyCode skipKey = KeyCode.X;
    [SerializeField] private float holdTimeRequired = 4f;
    [SerializeField] private string nextSceneName = "Game";

    [Header("UI de salto")]
    [SerializeField] private Image skipImage;
    [SerializeField] private Sprite[] loadingFrames;

    private float holdTimer = 0f;
    private bool isSkipping = false;

    private void Start() // Inicializa la imagen de carga al estado inicial
    {
        holdTimer = 0f;

        if (skipImage != null && loadingFrames.Length > 0)
        {
            skipImage.sprite = loadingFrames[0];
        }
    }

    private void Update() // Maneja la lógica de salto de escena y la animación de carga
    {
        if (Input.GetKey(skipKey))
        {
            holdTimer += Time.deltaTime;
            UpdateLoadingAnimation();

            if (holdTimer >= holdTimeRequired && !isSkipping)
            {
                isSkipping = true;
                GoToNextScene();
            }
        }
        else
        {
            holdTimer = 0f;
            ResetLoadingAnimation();
        }
    }
    private void UpdateLoadingAnimation() // Actualiza la imagen de carga según el progreso de carga pero no funciona correctamente
    {
        if (skipImage == null || loadingFrames.Length == 0) return;

        float progress = holdTimer / holdTimeRequired;
        progress = Mathf.Clamp01(progress);

        int frameIndex = Mathf.FloorToInt(progress * loadingFrames.Length);

        if (frameIndex >= loadingFrames.Length)
            frameIndex = loadingFrames.Length - 1;

        skipImage.sprite = loadingFrames[frameIndex];
    }
    private void ResetLoadingAnimation() // Reinicia la imagen de carga al estado inicial pero naaaa esta fallando tmb
    {
        if (skipImage != null && loadingFrames.Length > 0)
        {
            skipImage.sprite = loadingFrames[0];
        }
    }
    private void GoToNextScene() // Carga la siguiente escena cuando se completa el tiempo de espera lo unico que me funciona bien
    {
        SceneManager.LoadScene(nextSceneName);
    }
}