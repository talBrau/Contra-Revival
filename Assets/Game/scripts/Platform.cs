using System.Collections;
using UnityEngine;


public class Platform : MonoBehaviour
{
    [SerializeField] private BoxCollider2D _boxCollider2D;
    [SerializeField] private PlatformEffector2D _platformEffector2D;
    private bool _isPlayerOn = false;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _platformEffector2D = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlayerOn)
        {
            if (player.GetComponent<Player>().isCrouching())
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    _platformEffector2D.rotationalOffset = 180;
                }
            }
        }
        else
        {
            StartCoroutine(initRotationalOffest());
        }
    }

    private IEnumerator initRotationalOffest()
    {
        yield return new WaitForSeconds(.1f);
        if (!_isPlayerOn)
        {
            _platformEffector2D.rotationalOffset = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _isPlayerOn = true;
        }
    }


    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _isPlayerOn = false;
        }
    }

    public bool getIsPlaterOn()
    {
        return _isPlayerOn;
    }
}