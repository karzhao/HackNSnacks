using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    

    public void LoadMainSceneApple()
    {
        LoadSceneWithData("Apple");
        SceneManager.LoadScene("main"); // Change "GameScene" to your actual scene name
    }
    public void LoadMainSceneBread()
    {
        LoadSceneWithData("Bread");
        SceneManager.LoadScene("main"); // Change "GameScene" to your actual scene name
    }
    public void LoadMainSceneChicken()
    {
        LoadSceneWithData("Chicken");
        SceneManager.LoadScene("main"); // Change "GameScene" to your actual scene name
    }

    public void LoadSceneWithData(string sprite)
    {
        string dataString = string.Join(",", sprite);
        PlayerPrefs.SetString("Character", dataString);
        PlayerPrefs.Save();
    }
}
