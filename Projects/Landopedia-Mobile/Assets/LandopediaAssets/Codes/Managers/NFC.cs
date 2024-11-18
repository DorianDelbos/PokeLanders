using Landopedia;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NFC : MonoBehaviour
{
	public static Action<List<byte[]>> onBlocksRead;

	private Coroutine routine = null;

	public void ReadTagProccess()
	{
		if (routine == null)
			routine = StartCoroutine(ReadTagCoroutine());
	}

	private IEnumerator ReadTagCoroutine()
	{
#if PLATFORM_ANDROID && !UNITY_EDITOR
		InitializeNfcReader();

		bool isRead = false;

		// Message panel
		DataPanelSystem.Instance.ClearDataPanel();
		DataPanelSystem.Instance.CreateDataPanel(new DataPanelStruct()
			{
				text = DataPanelSystem.ErrorMessageHandler.waitTagNfc,
				buttons = new List<DataPanelStruct.Button>
						{
							new DataPanelStruct.Button
							{
								text = "Cancel",
								action = () => {
									DataPanelSystem.Instance.ClearDataPanel();
									isRead = true;
								}
							},
						}
			}
		);

		while (!isRead)
		{
			try
			{
				AndroidJavaObject tag = GetNfcTag();
				if (tag != null)
				{
					string[] techList = GetTechList(tag);

					if (techList.Contains("android.nfc.tech.MifareClassic"))
					{
						List<byte[]> resultData = new List<byte[]>();
						resultData.Add(HandleMifareClassicTag(tag, 1, 0));
						resultData.Add(HandleMifareClassicTag(tag, 2, 0));
						resultData.Add(HandleMifareClassicTag(tag, 4, 1));
						onBlocksRead?.Invoke(resultData);

						isRead = true;
						DataPanelSystem.Instance.ClearDataPanel();
					}
					else
					{
						DataPanelSystem.Instance.ClearDataPanel();
						DataPanelSystem.Instance.CreateDataPanel(new DataPanelStruct()
							{
								text = DataPanelSystem.ErrorMessageHandler.mifareTagError,
								buttons = new List<DataPanelStruct.Button>
									{
										new DataPanelStruct.Button
										{
											text = "Cancel",
											action = () => DataPanelSystem.Instance.ClearDataPanel()
										},
									}
							}
						);
					}

					PauseNfcReader();
					ClearNfcIntent();
				}
			}
			catch (System.Exception e)
			{
				DataPanelSystem.Instance.ClearDataPanel();
				DataPanelSystem.Instance.CreateDataPanel(new DataPanelStruct()
					{
						text = e.Message,
						buttons = new List<DataPanelStruct.Button>
							{
								new DataPanelStruct.Button
								{
									text = "Cancel",
									action = () => DataPanelSystem.Instance.ClearDataPanel()
								},
							}
					}
				);

				PauseNfcReader();
				ClearNfcIntent();

				break;
			}

			yield return new WaitForSeconds(1.0f);
		}
#endif

		routine = null;
		yield return null;
	}

#if PLATFORM_ANDROID && !UNITY_EDITOR
	private void InitializeNfcReader()
	{
		AndroidJavaObject mActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaObject nfcReader = new AndroidJavaObject("com.dgames.nfchandlerlib.NfcHandler");
		nfcReader.Call("setContext", mActivity);
		nfcReader.Call("onResume");
	}

	private AndroidJavaObject GetNfcTag()
	{
		AndroidJavaObject mActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaObject intent = mActivity.Call<AndroidJavaObject>("getIntent");
		return intent.Call<AndroidJavaObject>("getParcelableExtra", "android.nfc.extra.TAG");
	}

	private string[] GetTechList(AndroidJavaObject tag)
	{
		return tag.Call<string[]>("getTechList");
	}

	private byte[] HandleMifareClassicTag(AndroidJavaObject tag, int blockToRead, int sector)
	{
		AndroidJavaClass mifareClassicClass = new AndroidJavaClass("android.nfc.tech.MifareClassic");
		AndroidJavaObject mifareClassic = mifareClassicClass.CallStatic<AndroidJavaObject>("get", tag);

		mifareClassic.Call("connect");

		byte[] defaultKey = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
		bool auth = mifareClassic.Call<bool>("authenticateSectorWithKeyA", sector, defaultKey);

		if (auth)
		{
			byte[] blockData = mifareClassic.Call<byte[]>("readBlock", blockToRead);

			mifareClassic.Call("close");
			return blockData;
		}
		else
		{
			DataPanelSystem.Instance.ClearDataPanel();
			DataPanelSystem.Instance.CreateDataPanel(new DataPanelStruct()
				{
					text = DataPanelSystem.ErrorMessageHandler.mifareTagAuthenticationError,
					buttons = new List<DataPanelStruct.Button>
								{
									new DataPanelStruct.Button
									{
										text = "Cancel",
										action = () => DataPanelSystem.Instance.ClearDataPanel()
									},
								}
				}
			);
		}

		mifareClassic.Call("close");
		return null;
	}

	private void PauseNfcReader()
	{
		AndroidJavaObject nfcReader = new AndroidJavaObject("com.dgames.nfchandlerlib.NfcHandler");
		nfcReader.Call("onPause");
	}

	private void ClearNfcIntent()
	{
		AndroidJavaObject mActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
		mActivity.Call("setIntent", new AndroidJavaObject("android.content.Intent"));
	}
#endif
}