using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace JYW.UI
{
    public class SectSelectMenu : MonoBehaviour
    {
        [SerializeField]
        private SectSelectorUI playerSelector;

        [SerializeField]
        private SectSelectorUI enemySelector;

        [SerializeField]
        private SectSelection selection;

        [SerializeField]
        private Ui selectMenu;

        [SerializeField]
        private Button submitButton;

        [SerializeField]
        private Button cancelButton;

        [SerializeField]
        private SceneTransition transition;

        private AsyncOperation loadSceneOpeartion;

        private void Start()
        {
            submitButton.onClick.AddListener(SubmitAndStartGame);
            cancelButton.onClick.AddListener(() => selectMenu.FadeOut());
            loadSceneOpeartion = SceneManager.LoadSceneAsync(1);
            loadSceneOpeartion.allowSceneActivation = false;
        }

        private void SubmitAndStartGame()
        {
            selection.SectA = playerSelector.Selected;
            selection.SectB = enemySelector.Selected;
            transition.LoadScene(loadSceneOpeartion);
        }
    }
}