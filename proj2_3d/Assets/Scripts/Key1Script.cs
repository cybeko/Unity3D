using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Key1Script : MonoBehaviour
{
    [SerializeField]
    private int keyNumber;

    private GameObject content;
    private Image indicatorImage;
    private float timeout = 10.0f;
    private float leftTime;
    void Start()
    {
        content = transform.Find("Content").gameObject;
        indicatorImage = transform
            .Find("Indicator/Canvas/Foreground")
            .GetComponent<Image>();
        indicatorImage.fillAmount = 1;
        leftTime = timeout;
    }
    void Update()
    {
        content.transform.Rotate(0, Time.deltaTime * 30f, 0);
        if (leftTime >= 0)
        {
            indicatorImage.fillAmount = leftTime / timeout;
            indicatorImage.color = new Color(
                Mathf.Clamp01(2.0f * (1.0f - indicatorImage.fillAmount)),
                Mathf.Clamp01(2.0f* indicatorImage.fillAmount),
                0.0f
                );
            leftTime -= Time.deltaTime;
            if (leftTime <= 0) 
            {
                GameState.SetProperty($"isKey{keyNumber}InTime", false);

            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Player")
        {
            GameState.SetProperty($"isKey{keyNumber}Collected", true);
            Destroy(this.gameObject);
        }
    }   
}
