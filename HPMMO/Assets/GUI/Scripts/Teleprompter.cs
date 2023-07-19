using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Teleprompter : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI textPrefab;
    [SerializeField] private float displayTime;

    public static Teleprompter Instance;

    public void Awake()
    {
        Instance = this;
    }

    public void RunMessage(string message, MessageType messageType) {

        TextMeshProUGUI tmp = Instantiate(textPrefab.gameObject, transform).GetComponent<TextMeshProUGUI>();
        tmp.text = message;


        switch (messageType) {

            case MessageType.WARNING:
                tmp.color = Color.yellow;
                break;

            case MessageType.ERROR:
                tmp.color = Color.red;
                break;

            case MessageType.MESSAGE:
                tmp.color = Color.white;
                break;
        }

        StartCoroutine(KillTimer(tmp.gameObject));
    }

    public IEnumerator KillTimer(GameObject go) {

        yield return new WaitForSeconds(displayTime);
        Destroy(go);

    }

}


public enum MessageType {

    WARNING, ERROR, MESSAGE

}