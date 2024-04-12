using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityToolbarExtender;

namespace LitFramework
{
    public class CustomToolBarEditor : Editor
    {
    }
    
	static class ToolbarStyles
	{
		public static readonly GUIStyle commandButtonStyle;

		static ToolbarStyles()
		{
			commandButtonStyle = new GUIStyle("Command")
			{
				fontSize = 10,
				alignment = TextAnchor.MiddleCenter,
				imagePosition = ImagePosition.ImageAbove,
				fontStyle = FontStyle.Normal,
				stretchWidth = true,
				fixedWidth = 60,
			};
		}
	}

	[InitializeOnLoad]
	public class SceneSwitchLeftButton
	{
		static SceneSwitchLeftButton()
		{
			ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
		}

		static void OnToolbarGUI()
		{
			GUILayout.FlexibleSpace();

			if(GUILayout.Button(new GUIContent("Main", "打开启动场景"), ToolbarStyles.commandButtonStyle))
			{
				SceneHelper.StartScene("main");
			}

			if(GUILayout.Button(new GUIContent("Map", "打开星系场景"), ToolbarStyles.commandButtonStyle))
			{
				SceneHelper.StartScene("map");
            }
            //if (GUILayout.Button(new GUIContent("CM Editor", "打开CM村庄配置场景"), ToolbarStyles.commandButtonStyle))
            //{
            //    SceneHelper.StartScene("cmeditor");
            //}
            //if (GUILayout.Button(new GUIContent("3DTable", "打开3D牌桌"), ToolbarStyles.commandButtonStyle))
            //{
            //    SceneHelper.StartScene("3dtable");
            //}
        }
	}

	static class SceneHelper
	{
		static string sceneToOpen;

		public static void StartScene(string sceneName)
		{
			if(EditorApplication.isPlaying)
			{
				EditorApplication.isPlaying = false;
			}

			sceneToOpen = sceneName;
			EditorApplication.update += OnUpdate;
		}

		static void OnUpdate()
		{
			if (sceneToOpen == null ||
			    EditorApplication.isPlaying || EditorApplication.isPaused ||
			    EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
			{
				return;
			}
	
			EditorApplication.update -= OnUpdate;

			if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
			{
				// need to get scene via search because the path to the scene
				// file contains the package version so it'll change over time
				string[] guids = AssetDatabase.FindAssets("t:scene " + sceneToOpen, null);
				if (guids.Length == 0)
				{
					Debug.LogWarning("Couldn't find scene file");
				}
				else
				{
					string scenePath = AssetDatabase.GUIDToAssetPath(guids[0]);
					EditorSceneManager.OpenScene(scenePath);
					// EditorApplication.isPlaying = true;
				}
			}
			sceneToOpen = null;
		}
	}

}
