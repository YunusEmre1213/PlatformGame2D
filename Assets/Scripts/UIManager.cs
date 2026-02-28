using UnityEngine;
using TMPro; 

public class UIManager : MonoBehaviour
{
    
    public TMP_Text knifeCountText;

  
    public static UIManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return; 
        }
        UpdateKnifeCount(0);
    }

    /// <summary>
    /// Býçak sayýsýný ekranda gösteren ana metot.
    /// </summary>
    /// <param name="newCount">Karakterden gelen güncel býçak sayýsý.</param>
    public void UpdateKnifeCount(int newCount)
    {
        if (knifeCountText != null)
        {
 
            knifeCountText.text = "Knife: " + newCount.ToString();
        }
    }
}