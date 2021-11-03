
using UnityEngine;
using UnityEngine.SceneManagement;
public class sceneLoader : MonoBehaviour
{
 
    public void LoadScene(int level)
        
    {
        SceneManager.LoadScene(level);
    }
}