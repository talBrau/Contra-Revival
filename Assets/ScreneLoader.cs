using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene("Welcome");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Welcome")
        {
            if (Input.anyKey)
            {
                SceneManager.LoadScene("Contra");
            }
        }

        if (SceneManager.GetActiveScene().name == "GameOver")
        {
            StartCoroutine(WaitForRestart());
        }
    }

    private IEnumerator WaitForRestart()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Welcome");
    }
}
