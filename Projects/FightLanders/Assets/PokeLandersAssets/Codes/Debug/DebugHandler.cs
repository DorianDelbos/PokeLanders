using UnityEngine;
using LandersLegends.Gameplay;

#if UNITY_EDITOR
using UnityEditor;
using LandersLegends.Extern;
#endif

namespace LandersLegends.DebugSystem
{
	public class DebugHandler : MonoBehaviour
	{
		private void Awake()
		{
			if (!gameObject.CompareTag("EditorOnly"))
				Destroy(this);
		}

#if UNITY_EDITOR
		public string dataDebug = "00 00 00 01 41 62 63 64 65 66 67 68 69 6A 6B 6C 6D 6E FF 51 00 01 00 0A 00 00 00 40 00 0A 00 14 00 01 00 02 00 03 00 04 01 02 03 04 05 06 01 02 03 04 05 06";

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.L))
			{
				ExternLanderManager.instance.ProccessLanderDebug(dataDebug);
			}
			if (Input.GetKeyDown(KeyCode.K))
			{
				Debug.Log(GameManager.instance.Landers[0].ToDataNFC().ToBytes().MakeString());
			}
		}
#endif
	}
}
