using UnityEngine;
using LandersLegends.Extern;

namespace LandersLegends.DebugSystem
{
	public class DebugHandler : MonoBehaviour
	{
		private void Awake()
		{
			//if (!gameObject.CompareTag("EditorOnly"))
			//	Destroy(this);
		}

		public string landerDebug1 = "00 00 00 01 41 62 63 64 65 66 67 68 69 6A 6B 6C 6D 6E FF 51 00 01 00 0A 00 00 00 40 00 0A 00 14 00 01 00 02 00 03 00 04 01 02 03 04 05 06 01 02 03 04 05 06";
		public string landerDebug2 = "00 00 00 02 4D 61 72 73 68 6D 61 6C 6C 6F 77 20 20 20 20 33 00 03 00 1A 00 00 02 14 00 43 00 20 00 01 00 02 00 03 00 04 0A 14 1E 28 32 3C 46 50 5A 64 6E 78";

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Keypad1))
			{
				ExternLanderManager.instance.ProccessLanderDebug(landerDebug1);
			}
			if (Input.GetKeyDown(KeyCode.Keypad2))
			{
				ExternLanderManager.instance.ProccessLanderDebug(landerDebug2);
			}
			if (Input.GetKeyDown(KeyCode.Keypad3))
			{
				ExternLanderManager.instance.ProccessLanderDebug(null);
			}
		}
	}
}
