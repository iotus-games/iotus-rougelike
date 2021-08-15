using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public enum MessageType
{
    GameAction,
    Log,
    Warning,
    Error,
    Step,
}

public struct MessageInfo
{
    public MessageInfo(string text, MessageType type)
    {
        Text = text;
        Type = type;
    }

    public MessageType Type;
    public string Text;
}

public class InfoMessage : MonoBehaviour
{
    public GameObject gameCamera;
    public GameObject messagePrefab;
    public float messageStride;
    public int maxMessages;
    public List<MessageType> filterTypes;

    private List<TextMeshPro> messages = new List<TextMeshPro>();
    private List<MessageInfo> messageInfos = new List<MessageInfo>();
    private int historyPosition = 0;

    private void Start()
    {
        for (int i = 0; i < maxMessages; i++)
        {
            var message = Instantiate(messagePrefab, gameCamera.transform);
            message.transform.Translate(Vector3.down * messageStride * i);
            messages.Add(message.GetComponent<TextMeshPro>());
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.PageUp))
        {
            if (historyPosition > 0)
            {
                historyPosition -= 1;
            }

            UpdateText();
        }

        if (Input.GetKey(KeyCode.PageDown))
        {
            if (historyPosition < messageInfos.Count - maxMessages)
            {
                historyPosition += 1;
            }

            UpdateText();
        }
    }

    public void Message(string newText, MessageType type)
    {
        var info = new MessageInfo {Text = messageInfos.Count + 1 + ") " + newText, Type = type};
        messageInfos.Add(info);
        UpdateText();
    }

    public void EnableMessageType(MessageType type)
    {
        if (!filterTypes.Contains(type))
        {
            filterTypes.Add(type);
        }

        UpdateText();
    }

    public void DisableMessageType(MessageType type)
    {
        filterTypes.Remove(type);
        UpdateText();
    }

    private void UpdateText()
    {
        var cnt = 0;
        for (int i = historyPosition; i < messageInfos.Count && cnt < maxMessages; i++)
        {
            var info = messageInfos[messageInfos.Count - i - 1];
            if (filterTypes.Contains(info.Type))
            {
                messages[cnt].text = info.Text;
                messages[cnt].color = MessageColor(info.Type);
                cnt += 1;
            }
        }
    }

    private static Color MessageColor(MessageType type)
    {
        switch (type)
        {
            case MessageType.GameAction:
                return Color.green;
            case MessageType.Log:
                return Color.white;
            case MessageType.Warning:
                return Color.yellow;
            case MessageType.Error:
                return Color.red;
            case MessageType.Step:
                return Color.blue;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}