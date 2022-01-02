using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class shieldBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    [SerializeField] private GameObject player;
    private float shieldTime;
    void Start()
    {
        shieldTime = player.GetComponent<Player>().shieldTime;
        slider.maxValue = shieldTime;
        slider.minValue = 0;
        slider.value = shieldTime;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = player.GetComponent<Player>().shieldTime;
    }

    public void setShieldTime(float time)
    {
        slider.value = time;
    }
}
