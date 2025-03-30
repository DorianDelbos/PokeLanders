using System;
using System.Collections;
using UnityEngine;

namespace TMPro
{
    public class TMP_Dialogue : TextMeshProUGUI
    {
        public float multSpeed = 1;
        [SerializeField] private float speed = 10;
        private Coroutine readRoutine;

        public event Action OnDialogueEnd;

        public bool IsReading => readRoutine != null;

        private bool isCustomTag(string tag) => tag.StartsWith("speed=") || tag.StartsWith("pause=");

        public void ReadText(string newText)
        {
            text = string.Empty;
            string[] subTexts = newText.Split('<', '>');

            string displayText = "";
            for (int i = 0; i < subTexts.Length; i++)
            {
                if (i % 2 == 0)
                    displayText += subTexts[i];
                else if (!isCustomTag(subTexts[i].Replace(" ", "")))
                    displayText += $"<{subTexts[i]}>";
            }

            text = displayText;
            maxVisibleCharacters = 0;
            readRoutine = StartCoroutine(Read(subTexts));
        }

        private IEnumerator Read(string[] subTexts)
        {
            int subCounter = 0;
            int visibleCounter = 0;
            while (subCounter < subTexts.Length)
            {
                if (subCounter % 2 == 1)
                {
                    yield return EvaluateTag(subTexts[subCounter].Replace(" ", ""));
                }
                else
                {
                    while (visibleCounter < subTexts[subCounter].Length)
                    {
                        visibleCounter++;
                        maxVisibleCharacters++;
                        yield return new WaitForSeconds(1f / (speed * multSpeed));
                    }
                    visibleCounter = 0;
                }
                subCounter++;
            }

            readRoutine = null;
            OnDialogueEnd?.Invoke();

            yield return null;
        }

        private WaitForSeconds EvaluateTag(string tag)
        {
            if (tag.Length > 0)
            {
                if (tag.StartsWith("speed="))
                {
                    speed = float.Parse(tag.Split('=')[1]);
                }
                else if (tag.StartsWith("pause="))
                {
                    return new WaitForSeconds(float.Parse(tag.Split('=')[1]) / multSpeed);
                }
            }
            return null;
        }
    }
}
