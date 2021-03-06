using System.Collections;
using TMPro;
// using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class gameManager : MonoBehaviour
{
    private static gameManager _shared;

    [SerializeField] private UnityEvent playerHit;
    [SerializeField] private UnityEvent playerReturn;
    [SerializeField] private TextMeshProUGUI scoreText;

    [SerializeField] private GameObject[] lifes;
    [SerializeField] private AudioSource EnemyHit;
    private int _playerScore = 0;
    private int lifeIndex = 2;

    [SerializeField] private  GameObject potionPrefab;

    [SerializeField] private  GameObject potionParent;
    // public Animator PlayerAnim;

    // Start is called before the first frame update
    void Start()
    {
        _shared = this;
        _shared.scoreText.text = "Score: " + _shared._playerScore;

        

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();

        }
    }

    public static void playerWon()
    {
        // print("You won");
        SceneManager.LoadScene("GameOver");
        // _shared.StartCoroutine(waitForMenu());
    }
    public static void lossLife()
    {
        _shared.lifes[_shared.lifeIndex].SetActive(false);
        _shared.lifeIndex--;
        if (_shared.lifeIndex == -1)
        {
            _shared.StartCoroutine(waitForMenu());
            
        }

        else
        {
            _shared.StartCoroutine(respawn());
        }
    }

    private static IEnumerator waitForMenu()
    {
        _shared.playerHit.Invoke();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("GameOver");
    }

    private static IEnumerator respawn()
    {
        _shared.playerHit.Invoke();
        yield return new WaitForSeconds(3);
        _shared.playerReturn.Invoke();
    }

    public static void playSoundEnemyHit()
    {
        _shared.EnemyHit.Play();
    }

    public static void increaseScore()
    {
        _shared._playerScore += 1000;
        _shared.scoreText.text = "Score: " + _shared._playerScore;
    }

    public static int getPlayerScore()
    {
        return _shared._playerScore;
    }

    public static void instantiatePotion(Transform transform)
    {
        Instantiate(_shared.potionPrefab, transform.position, transform.rotation, _shared.potionParent.transform);

    }
}