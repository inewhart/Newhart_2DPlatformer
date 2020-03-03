using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public void changeScene()
    {
        SceneManager.LoadScene("Game");
    }
    public void exitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
