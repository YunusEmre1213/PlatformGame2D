using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public SceneFader sceneFader; // SceneFader scriptine referans oluþturun

    public void StartGame()
    {
        sceneFader.FadeToScene("TutoriolScene"); // Sahne adýný kendi sahnenizin adýyla deðiþtirin
    }

    public void QuitGame()
    {
        Application.Quit();

        Debug.Log("Oyun kapatýldý.");
    }
}