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

    void Start()
    {
        instance = this;
        Transform t = this.transform.Find("Content");
        content = t.gameObject;
        contentGroup = content.GetComponent<CanvasGroup>();
        text = t.Find("Text").GetComponent<TMPro.TextMeshProUGUI>();
        content.SetActive(false);
        timeout = 0f;
        GameState.AddListener(OnGameStateChanged);
        GameEventSystem.Subscribe(OnGameEvent);
    }

    void Update()
    {
        if (timeout > 0)
        {
            timeout-= Time.deltaTime;
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
