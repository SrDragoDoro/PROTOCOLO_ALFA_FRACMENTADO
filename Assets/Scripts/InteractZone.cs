using UnityEngine;
using UnityEngine.Events;
public class InteractZone : MonoBehaviour
{
    [Header("Referencias de UI")]
    [SerializeField] private GameObject toolTip;
    [SerializeField] private GameObject canvas;

    [Header("Configuración")]
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    [Header("Eventos al elegir cada botón (opcional)")]
    public UnityEvent onOption1;
    public UnityEvent onOption2;
    public UnityEvent onOption3;

    private bool playerInRange = false;
    private bool menuOpen = false;

    private void Start()
    {
        if (toolTip != null) toolTip.SetActive(false);
        if (canvas != null) canvas.SetActive(false);
    }

    private void Update()
    {
        if (!playerInRange) return;

        if (!menuOpen && Input.GetKeyDown(interactKey))
        {
            OpenMenu();
        }

        if (menuOpen)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            {
                SelectOption(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            {
                SelectOption(2);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            {
                SelectOption(3);
            }
        }
    }

    private void OpenMenu()
    {
        menuOpen = true;
        if (toolTip != null) toolTip.SetActive(false);
        if (canvas != null) canvas.SetActive(true);
    }

    private void CloseMenu()
    {
        menuOpen = false;
        if (canvas != null) canvas.SetActive(false);
        if (playerInRange && toolTip != null) toolTip.SetActive(true);
    }

    private void SelectOption(int option)
    {
        switch (option)
        {
            case 1: onOption1?.Invoke(); break;
            case 2: onOption2?.Invoke(); break;
            case 3: onOption3?.Invoke(); break;
        }

        // Si NO quieres que el menú se cierre automáticamente al elegir, borra la línea de abajo.
        CloseMenu();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = true;
            if (toolTip != null) toolTip.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = false;
            menuOpen = false;
            if (toolTip != null) toolTip.SetActive(false);
            if (canvas != null) canvas.SetActive(false);
        }
    }
}