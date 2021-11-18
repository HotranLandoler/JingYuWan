using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// 每个单位x尺
    /// </summary>
    public static readonly int ChiPerUnit = 3;

    [SerializeField]
    private Character player;
    public Character Player => player;

    [SerializeField]
    private Character enemy;
    public Character Enemy => enemy;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GameMain());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator GameMain()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("GameStart");
        player.CurrentHealth -= 15;
        yield return null;
    }
}
