using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    private Button startBtn;

    [SerializeField]
    private Button exitBtn;

    [SerializeField]
    private SceneTransition transition;

    private AsyncOperation loadSceneOpeartion;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    // Start is called before the first frame update
    void Start()
    {
        startBtn.onClick.AddListener(StartGame);
        exitBtn.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        });
        loadSceneOpeartion = SceneManager.LoadSceneAsync(1);
        loadSceneOpeartion.allowSceneActivation = false;
    }

    private void StartGame()
    {
        transition.LoadScene(loadSceneOpeartion);
    }
}
