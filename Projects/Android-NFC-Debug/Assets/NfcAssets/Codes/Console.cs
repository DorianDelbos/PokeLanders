using TMPro;
using UnityEngine;

public class Console : MonoBehaviour
{
    private static Console instance;
    public static Console current => instance;

    [SerializeField] private TMP_Text tag_output_text;

    private void Awake() => instance = this;
    public void Clear() => tag_output_text.text = "<b>Debug console</b>";
    public void AppendText(string text, string color = "white") => tag_output_text.text += $"\r\n<color={color}>{text}</color>";
}
