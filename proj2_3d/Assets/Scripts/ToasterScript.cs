using System.Collections.Generic;
using UnityEngine;

public class ToasterScript : MonoBehaviour
{
    private static ToasterScript instance;
    private GameObject content;
    private TMPro.TextMeshProUGUI text;
    private CanvasGroup contentGroup;

    private float timeout;
    private float showtime = 3.0f;

    private Queue<ToastMessage> messageQueue = new Queue<ToastMessage>();
    private float deltaTime;
    void Start()
    {
        instance = this;
        deltaTime = 0.0f;
        Transform t = this.transform.Find("Content");
        content = t.gameObject;
        contentGroup = content.GetComponent<CanvasGroup>();
        text = t.Find("Text").GetComponent<TMPro.TextMeshProUGUI>();
        content.SetActive(false);
        timeout = 0f;
        GameState.AddListener(OnGameStateChanged);
        GameEventSystem.Subscribe(OnGameEvent);
        //log
        Debug.Log($"targetFrameRate: {Application.targetFrameRate}, vSyncCount: {QualitySettings.vSyncCount}, Screen: {Screen.currentResolution.refreshRateRatio}");
    }

    void Update()
    {
        if(deltaTime == 0f  || deltaTime > Time.deltaTime && Time.deltaTime > 0f) deltaTime = Time.deltaTime;
        if (timeout > 0)
        {
            float dt = Time.timeScale > 0f ? Time.deltaTime
                : this.deltaTime > 0f ? this.deltaTime  
                : QualitySettings.vSyncCount > 0 ? QualitySettings.vSyncCount / (float)Screen.currentResolution.refreshRateRatio.value
                : Application.targetFrameRate > 0 ? 1.0f / Application.targetFrameRate
                : 0.016f;
            Debug.Log(dt);
            timeout-= dt;

            contentGroup.alpha = Mathf.Clamp01(timeout * 2.0f);
            if(timeout < 0)
            {
                content.SetActive(false);
            }
        }
        else if (messageQueue.Count > 0) {
            var toast = messageQueue.Dequeue();
            content.SetActive(true);
            text.text = toast.message;
            timeout = toast.time;
        }
    }
    private void OnGameEvent(GameEvent gameEvent)
    {
        if (gameEvent.toast is string msg)
        {
           Toast(msg);
        }
    }
    private void OnGameStateChanged(string fieldName)
    {
        if (fieldName == nameof(GameState.isDay))
        {
            Toast(GameState.isDay ? "It is Day" : "It is Night");
        }
    }
    private void OnDestroy()
    {
        GameState.RemoveListener(OnGameStateChanged);
        GameEventSystem.Unsubscribe(OnGameEvent);
    }

    public static void Toast(string message, float time = 0.0f)
    {
        instance.messageQueue.Enqueue(
            new ToastMessage() { message = message, time = time > 0.0f ? time : instance.showtime});
    }

    private class ToastMessage
    {
        public string message {  get; set; }
        public float time { get; set; }
    }
}
