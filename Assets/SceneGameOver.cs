using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneGameOver : MonoBehaviour
{
    public AudioSource GOmusic;

    public TextMeshPro scoreTextMeshPro;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadWelcome());
        scoreTextMeshPro.SetText(gameManager.getPlayerScore().ToString());
    }

    private IEnumerator LoadWelcome()
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("Welcome");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
