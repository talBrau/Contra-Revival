using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WelcomeManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource welcomeMusic;
    public Transform select;
    private bool _playOrQuit = true; // true if play
    private Vector3 _transformPosition;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown((KeyCode.DownArrow)) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            _playOrQuit = !_playOrQuit;
            if (_playOrQuit)
            {
                @select.position = new Vector2(select.position.x, -2.55f);
            }
            else
            {
                @select.position = new Vector2(select.position.x, -3.28f);
            }

        }

        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Return))
        {
            if(_playOrQuit){SceneManager.LoadScene("Contra");
            }
            else
            {
                Application.Quit();

                UnityEditor.EditorApplication.isPlaying = false;
            }
        }

    }
}
