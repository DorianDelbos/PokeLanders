using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleSystem : MonoBehaviour
{
	private static ConsoleSystem _i;
	public static ConsoleSystem instance => _i;

	[SerializeField] private Transform container;
	[SerializeField] private TMP_Text textPrefab;

	private void Awake()
	{
		_i = this;
		AppendText("Console ...", Color.green);
	}

	public void Clear()
	{
		foreach (Transform child in container)
			Destroy(child.gameObject);

		AppendText("Console ...", Color.green);
	}

	public void AppendText(string text)
	{
		AppendText(text, Color.white);
	}

	public void AppendText(string text, Color color)
	{
		TMP_Text instance = Instantiate(textPrefab, container);
		instance.text = text;
		instance.color = color;

		LayoutRebuilder.ForceRebuildLayoutImmediate(container.GetComponent<RectTransform>());
	}
}
