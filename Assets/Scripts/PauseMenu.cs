using UnityEngine.InputSystem; // Bu satýrý ekleyin
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    // Duraklatma menüsü paneliniz
    public GameObject pausePanel;

    // Menünün açýk olup olmadýðýný kontrol eden deðiþken
    public static bool isPaused = false;

    // Girdi eylemlerine eriþmek için referans
    private PlayerInputActions playerControls;

    void Awake()
    {
        // Girdi eylemleri sýnýfýnýn bir örneðini oluþturun
        playerControls = new PlayerInputActions();
    }

    void OnEnable()
    {
        // Girdi eylemlerini etkinleþtir
        playerControls.Enable();
        // Pause eylemi gerçekleþtirildiðinde TogglePause fonksiyonunu çaðýr
        playerControls.UI.Pause.performed += context => TogglePause();
    }

    void OnDisable()
    {
        // Girdi eylemlerini devre dýþý býrak
        playerControls.UI.Pause.performed -= context => TogglePause();
        playerControls.Disable();
    }

    // Oyunu duraklatma veya devam ettirme fonksiyonu
    private void TogglePause()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    // Oyunu duraklatma fonksiyonu
    public void PauseGame()
    {
        pausePanel.SetActive(true); // Menü panelini aktif hale getir
        Time.timeScale = 0f;       // Oyunun zaman akýþýný durdur
        isPaused = true;           // Oyunun duraklatýldýðýný belirt
    }

    // Oyuna devam etme fonksiyonu
    public void ResumeGame()
    {
        pausePanel.SetActive(false); // Menü panelini kapat
        Time.timeScale = 1f;        // Oyunun zaman akýþýný normale döndür
        isPaused = false;          // Oyunun duraklatýlmadýðýný belirt
    }

    // Ana menüye dönme fonksiyonu
    public void GoToMainMenu()
    {
        // Zamaný normale döndür, aksi halde ana menü de duraklatýlmýþ kalýr
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Ana menü sahnesini yükle
    }
}