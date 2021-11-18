using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private bool transitOnStart = false;

    [SerializeField]
    private float transitionTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if (transitOnStart) Open();
    }

    public void Open() => animator.SetTrigger("Open");

    public void Close() => animator.SetTrigger("Close");

    public void LoadScene(int id)
    {
        StartCoroutine(Transition(id));
    }
    
    private IEnumerator Transition(int id)
    {
        animator.SetTrigger("Close");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(id);
    }
}
