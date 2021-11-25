using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Launches scene loading between two levels
public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;
    public GameObject loadingScreen;
    public Slider slider;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        GameEvents.current.onLevelEnded += LoadLevel;
        
    }
    public void LoadLevel(bool nextLevel){

       
        StartCoroutine(LoadAsynchronously(0));
        
    }
    IEnumerator LoadAsynchronously(int sceneIndex){
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);
        while(!operation.isDone){
            float progress = Mathf.Clamp01(operation.progress/.9f);
            slider.value = progress;
            yield return null;
        }
        loadingScreen.SetActive(false);
    }
}
