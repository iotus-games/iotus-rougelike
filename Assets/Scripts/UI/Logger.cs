using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace UI
{
    public enum MessageType
    {
        GameAction,
        Debug,
        Warning,
        Error,
        Step,
    }

    struct MessageInfo
    {
        public MessageType Type;
        public string Text;
    }

    public class Logger : MonoBehaviour
    {
        public GameObject messagePrefab;
        public float messageStride;
        public int maxMessages;
        public List<MessageType> filterTypes;
        public MouseListener panelMouseListener;

        private List<Text> messages = new List<Text>();
        private List<MessageInfo> messageInfos = new List<MessageInfo>();
        private int historyPosition;

        private void Start()
        {
            for (int i = 0; i < maxMessages; i++)
            {
                var message = Instantiate(messagePrefab, panelMouseListener.transform);
                message.transform.Translate(Vector3.down * messageStride * i);
                messages.Add(message.GetComponent<Text>());
            }
        }

        private void FixedUpdate()
        {
            var scrollDelta = 0;
            if (panelMouseListener.isOverlapped)
            {
                scrollDelta = (int) Input.mouseScrollDelta.y;
            }

            if (Input.GetKey(KeyCode.PageUp) || scrollDelta > 0)
            {
                if (historyPosition > 0)
                {
                    historyPosition -= 1;
                }

                UpdateText();
            }

            if (Input.GetKey(KeyCode.PageDown) || scrollDelta < 0)
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
            if (type == MessageType.Step)
            {
                newText = messageInfos.Count + 1 + ") " + newText;
            }

            var info = new MessageInfo {Text = newText, Type = type};
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
                    return Color.white;
                case MessageType.Debug:
                    return Color.green;
                case MessageType.Warning:
                    return Color.yellow;
                case MessageType.Error:
                    return Color.red;
                case MessageType.Step:
                    return Color.black;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}