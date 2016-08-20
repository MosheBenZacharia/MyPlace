
using UnityEngine;
using UnityEditor;
using System.Collections;

namespace App
{
	[ExecuteInEditMode]
	[CustomEditor (typeof(ChildSwitcherUtility))]
	class ChildSwitcherUtilityEditor : Editor {
		
		Transform[] children ;
		void OnEnable(){
			Transform myTransform = ((ChildSwitcherUtility) target).transform;
			children = new Transform[myTransform.childCount];
			for (int i=0; i<children.Length; ++i) {
				children [i] = myTransform.GetChild (i);
			}
		}
		static readonly float disableColorValue=.85f;
		Color disabledColor = new Color(disableColorValue,disableColorValue,disableColorValue);
		public override void OnInspectorGUI ()
		{
			GUILayout.Space(10);
			GUILayout.BeginHorizontal ();

			ChildSwitcherUtility tracker = (ChildSwitcherUtility) target;

			for(int i=0;i<children.Length;++i)
			{
				GUIStyle currentStyle = EditorStyles.miniButtonMid;
				if(i==0)
					currentStyle=EditorStyles.miniButtonLeft;
				if(i==children.Length-1)
					currentStyle=EditorStyles.miniButtonRight;
				if(!children[i].gameObject.activeSelf){
					GUI.backgroundColor=disabledColor;
						GUI.color=disabledColor;
				}
				if(GUILayout.Button(children[i].gameObject.name,currentStyle)){
					tracker.SetActive(i);
				}
				GUI.backgroundColor=Color.white;
				GUI.color=Color.white;
			}

			GUILayout.EndHorizontal ();
			GUILayout.Space(10);
		}


		[MenuItem("GameObject/Select Child Switcher Child #t", false, 0)]
		static void SelectTrackerCamera ()
		{
			Transform tracker = GameObject.Find("Cameras").transform;
			for(int i=0;i<tracker.childCount;++i)
			{
				if(tracker.GetChild(i).gameObject.activeSelf)
					Selection.activeTransform = tracker.GetChild(i);
			}
		}

		[MenuItem("GameObject/Child Switcher/Select Child Switcher #%t", false, 0)]
		static void SelectTracker ()
		{
			Transform tracker = GameObject.Find("Cameras").transform;

			Selection.activeTransform = tracker;
		}
	}
}