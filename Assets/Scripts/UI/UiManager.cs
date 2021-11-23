using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

namespace JYW.UI
{
    public class UiManager : MonoBehaviour
    {

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

        public event UnityAction<Vector2> PosSubmited;

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
        private GameObject posSelectPrefab;

        [SerializeField]
        private UiButton posSubmitButton;

        private Canvas canvas;

        private bool selectingPos = false;

        private GameObject posSelector;

        private void Awake()
        {
            canvas = GetComponent<Canvas>();            
        }

        private void Start()
        {
            cardNameText.gameObject.SetActive(false);
            playCardButton.button.onClick.AddListener(() => PlayButtonClicked?.Invoke());
            nextRoundButton.button.onClick.AddListener(() => NextButtonClicked?.Invoke());
            posSubmitButton.button.onClick.AddListener(SubmitPos);
        }

        private void Update()
        {
            if (selectingPos)
            {
                if (Input.GetMouseButton(0))
                {
                    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                    if (hit.collider != null)
                    {
                        if (hit.collider.CompareTag("Mouse"))
                        {
                            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            //Debug.Log(pos.x);
                            float posX = pos.x;
                            if (posSelector == null)
                            {
                                posSelector = Instantiate(posSelectPrefab, new Vector3(pos.x, 0, 0), Quaternion.identity);
                                posSubmitButton.FadeIn();
                            }
                            else
                                posSelector.transform.position = new Vector3(pos.x, 0, 0);
                        }
                    }
                }
            }
        }

        public IEnumerator ShowCardText(CardData card)
        {
            cardNameText.text = card.Title;
            cardNameText.color = new Color(1f, 1f, 1f, 0f);
            cardNameText.transform.localScale = Vector3.one * cardTextScale;
            cardNameText.gameObject.SetActive(true);
            cardNameText.DOFade(1f, cardTextScaleTime);
            yield return cardNameText.transform.DOScale(Vector3.one, cardTextScaleTime);
            yield return new WaitForSeconds(cardTextShowTime);
            cardNameText.DOFade(0f, cardTextScaleTime);
            cardNameText.transform.DOScale(cardTextScale, cardTextScaleTime);
        }

        public void ShowWarning(string warning)
        {
            var text = Instantiate(warningTextPrefab, canvas.transform);
            text.text = warning; 
            text.rectTransform.anchoredPosition = warningTextPos.anchoredPosition;
            text.rectTransform.DOAnchorPosY(warningTextPos.anchoredPosition.y + 100, 1f);
            text.DOFade(0f, 1f);
        }

        public void StartPosSelect()
        {
            stateText.text = "点击地面选择位置";
            nextRoundButton.FadeOut();
            selectingPos = true;
        }

        private void SubmitPos()
        {
            stateText.text = string.Empty;
            selectingPos = false;
            posSubmitButton.FadeOut();
            Vector2 pos = new Vector2(posSelector.transform.position.x, 0f);
            PosSubmited?.Invoke(pos);
            Destroy(posSelector.gameObject);
            nextRoundButton.FadeIn();
        }
    }
}