using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] keys; //0 - left, 1-right, 2-up,3-down,4-space,5-x,6-z
    [SerializeField] private GameObject[] instructions; //state - index, 7-goodjob
    private int state = 0; // 0 - move, 1- jump, 2 - crouch, 3-shoot, 4 - shield, 5- decend

    [SerializeField] private GameObject curPlatform;
    // Start is called before the first frame update
    
    void Start()
    {
        instructions[0].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            SceneManager.LoadScene("Contra");
        }

        if (state == 0 && instructions[state].activeSelf) //move
        {
            instructions[0].SetActive((true));
            keys[0].GetComponent<SpriteRenderer>().color = new Color(200,0,0);
            keys[1].GetComponent<SpriteRenderer>().color = new Color(200,0,0);
            if (player.GetComponent<Rigidbody2D>().velocity.x != 0)
            {
                keys[0].GetComponent<SpriteRenderer>().color = Color.white;
                keys[1].GetComponent<SpriteRenderer>().color = Color.white;
                StartCoroutine(changeState(2));
            }
        }
        
        if (state == 1 && instructions[state].activeSelf) //jump
        {
            keys[4].GetComponent<SpriteRenderer>().color = new Color(200,0,0);
            if (player.GetComponent<Rigidbody2D>().velocity.y != 0)
            {
                keys[4].GetComponent<SpriteRenderer>().color = Color.white;
                StartCoroutine(changeState(2));
            }
        }
        if (state == 2&& instructions[state].activeSelf) //crouch
        {
            keys[3].GetComponent<SpriteRenderer>().color = new Color(200,0,0);
            if (player.GetComponent<Player>().isCrouching())
            {
                keys[3].GetComponent<SpriteRenderer>().color = Color.white;
                StartCoroutine(changeState(2));
            }
        }
        if (state == 3&& instructions[state].activeSelf) //shoot
        {
            keys[5].GetComponent<SpriteRenderer>().color = new Color(200,0,0);
            keys[0].GetComponent<SpriteRenderer>().color = new Color(200,0,0);
            keys[1].GetComponent<SpriteRenderer>().color = new Color(200,0,0);
            keys[2].GetComponent<SpriteRenderer>().color = new Color(200,0,0);
            keys[3].GetComponent<SpriteRenderer>().color = new Color(200,0,0);

            if (player.GetComponent<Player>().isShooting)
            {
                keys[5].GetComponent<SpriteRenderer>().color = Color.white;
                keys[0].GetComponent<SpriteRenderer>().color = Color.white;
                keys[1].GetComponent<SpriteRenderer>().color = Color.white;
                keys[2].GetComponent<SpriteRenderer>().color = Color.white;
                keys[3].GetComponent<SpriteRenderer>().color = Color.white;

                StartCoroutine(changeState(2));
            }
        }
        if (state == 4&& instructions[state].activeSelf) //shield
        {
            keys[6].GetComponent<SpriteRenderer>().color = new Color(200,0,0);
            if (Input.GetKey(KeyCode.Z))
            {
                keys[6].GetComponent<SpriteRenderer>().color = Color.white;
                StartCoroutine(changeState(2));
            }
        }
        if (state == 5&& instructions[state].activeSelf) //descend
        {
            keys[3].GetComponent<SpriteRenderer>().color = new Color(200,0,0);
            keys[4].GetComponent<SpriteRenderer>().color = new Color(200,0,0);

            if (curPlatform.GetComponent<PlatformEffector2D>().rotationalOffset != 0)
            {
                instructions[state].SetActive(false);
                keys[3].GetComponent<SpriteRenderer>().color = Color.white;
                keys[4].GetComponent<SpriteRenderer>().color = Color.white;
                StartCoroutine(startGame(2));
            }
        }
    }

    private IEnumerator changeState(float sec) // dont enter if at last state
    {
        instructions[state].SetActive(false);
        yield return new WaitForSeconds(sec);
        state++;
        instructions[state].SetActive(true);
    }

    private IEnumerator startGame(float sec)
    {
        instructions[state].SetActive(false);
        instructions[state+1].SetActive(true);
        yield return new WaitForSeconds(sec);
        SceneManager.LoadScene("Contra");
    }
}
