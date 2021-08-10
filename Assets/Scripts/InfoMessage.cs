using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InfoMessage : MonoBehaviour
{
    public GameObject gameCamera;
    public GameObject messagePrefab;
    public float messageStride;
    public float maxMessages;

    private List<TextMeshPro> messages = new List<TextMeshPro>();

    private void Start()
    {
        for (int i = 0; i < maxMessages; i++)
        {
            var message = Instantiate(messagePrefab, gameCamera.transform);
            message.transform.Translate(Vector3.down * messageStride * i);
            messages.Add(message.GetComponent<TextMeshPro>());
        }
    }

    public void Message( string newText)
    {
        for (int i = 1; i < messages.Count; i++)
        {
            messages[messages.Count - i].text = messages[messages.Count - i - 1].text;
        }

        messages[0].text = newText;
    }
}