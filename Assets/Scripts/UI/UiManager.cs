using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Pool;
using Cinemachine;
using UnityEngine.SceneManagement;

namespace JYW.UI
{
    public class UiManager : MonoBehaviour
    {
        public static WaitForSeconds waitForCardText;

        public static readonly string PlayerRound = "玩家回合\n移动一步/打出普通牌x1/打出附加牌xN";

        public static readonly string Win = "胜利";

        public static readonly string Lose = "失败";

        [SerializeField]
        private CinemachineVirtualCamera followCamera;

        [Header("Buttons")]
        [SerializeField]
        private UiButton menuButton;
        public UiButton MenuButton => menuButton;

        [SerializeField]
        private UiButton playCardButton;
        public UiButton PlayCardButton => playCardButton;

        [SerializeField]
        private UiButton nextRoundButton;
        public UiButton NextRoundButton => nextRoundButton;

        public UnityEvent PlayButtonClicked;
        public UnityEvent NextButtonClicked;

        //public event UnityAction<Vector2> PosSubmited;

        [Header("Effects")]
        [SerializeField]
        private Text cardNameText;

        [SerializeField]
        private float cardTextScale = 2f;

        [SerializeField]
        private float cardTextScaleTime = 0.4f;

        [SerializeField]
        private float cardTextShowTime = 0.5f;

        [Header("Tips")]
        [SerializeField]
        private Text warningTextPrefab;

        [SerializeField]
        private RectTransform warningTextPos;

        [SerializeField]
        private Text stateText;

        [Header("PosSelect")]
        [SerializeField]
        private UiButton posSubmitButton;

        [SerializeField]
        private FadableSprite posSelector;

        [SerializeField]
        private Transform selectorCameraRoot;

        [Header("GameOver")]
        [SerializeField]
        private RectTransform gameOverScreen;

        [SerializeField]
        private Text gameOverText;

        [SerializeField]
        private Button restartButton;

        [SerializeField]
        private SceneTransition transition;

        private Canvas canvas;

        private ObjectPool<Text> warningTextPool;

        public Vector2? SelectedPos { get; private set; } = null;


        private void Awake()
        {
            canvas = GetComponent<Canvas>();
            warningTextPool = new ObjectPool<Text>(
                createFunc: () => Instantiate(warningTextPrefab, canvas.transform),
                actionOnGet: text => text.gameObject.SetActive(true),
                actionOnRelease: text => text.gameObject.SetActive(false),
                actionOnDestroy: text => Destroy(text.gameObject),
                defaultCapacity: 3,
                maxSize: 10);
            waitForCardText = new WaitForSeconds(cardTextShowTime);
        }

        private void Start()
        {
            cardNameText.gameObject.SetActive(false);
            playCardButton.button.onClick.AddListener(() => PlayButtonClicked?.Invoke());
            nextRoundButton.button.onClick.AddListener(() => NextButtonClicked?.Invoke());
            posSubmitButton.button.onClick.AddListener(SubmitPos);
            restartButton.onClick.AddListener(Restart);
        }

        private void OnDestroy()
        {
            warningTextPool.Clear();
        }

        public void OnPlayerRound()
        {
            stateText.text = PlayerRound;
        }

        public void OnEnemyRound()
        {
            stateText.text = string.Empty;
        }

        public IEnumerator ShowCardText(CardData card)
        {
            cardNameText.text = card.Title;
            cardNameText.color = new Color(1f, 1f, 1f, 0f);
            cardNameText.transform.localScale = Vector3.one * cardTextScale;
            cardNameText.gameObject.SetActive(true);
            cardNameText.DOFade(1f, cardTextScaleTime);
            yield return cardNameText.transform.DOScale(Vector3.one, cardTextScaleTime);
            yield return waitForCardText;
            cardNameText.DOFade(0f, cardTextScaleTime);
            cardNameText.transform.DOScale(cardTextScale, cardTextScaleTime);
        }

        public void ShowWarning(string warning)
        {
            var text = warningTextPool.Get();
            text.text = warning;
            text.color = Color.white;
            text.rectTransform.anchoredPosition = warningTextPos.anchoredPosition;
            text.rectTransform.DOAnchorPosY(warningTextPos.anchoredPosition.y + 100, 1f);
            text.DOFade(0f, 1f).OnComplete(() => warningTextPool.Release(text));
        }

        public IEnumerator WaitForPosSelecting()
        {
            stateText.text = "点击地面选择位置";
            nextRoundButton.FadeOut();
            SelectedPos = null;
            //posSelect = true;
            while (SelectedPos == null)
            {
                if (Input.GetMouseButton(0))
                {
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                    if (hit.collider != null)
                    {
                        if (hit.collider.CompareTag("Mouse"))
                        {
                            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            posSelector.transform.position = new Vector3(pos.x, 0, 0);
                            if (!posSelector.Display)
                            {
                                //相机跟随选择
                                followCamera.Follow = selectorCameraRoot;
                                posSelector.Show();
                                posSubmitButton.FadeIn();
                            }
                        }
                    }
                }
                yield return null;
            }
            stateText.text = string.Empty;
            posSelector.Hide();
            posSubmitButton.FadeOut();
            nextRoundButton.FadeIn();
            //posSelect = false;
        }

        private void SubmitPos()
        {
            if (posSelector)
            {
                Vector2 pos = new Vector2(posSelector.transform.position.x, 0f);
                SelectedPos = pos;
            }
            //stateText.text = string.Empty;
            //selectingPos = false;
            //posSubmitButton.FadeOut();
            //Vector2 pos = new Vector2(posSelector.transform.position.x, 0f);
            //PosSubmited?.Invoke(pos);
            //Destroy(posSelector.gameObject);
            //nextRoundButton.FadeIn();
        }

        public void ShowGameOver(bool win)
        {
            gameOverText.text = win ? Win : Lose;
            gameOverScreen.gameObject.SetActive(true);
        }

        private void Restart()
        {
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
            transition.LoadScene(asyncOperation);            
        }
    }
}