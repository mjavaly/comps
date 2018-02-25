using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ComponentToggleAnimation))]
public class ComponentToggleAnimationEditor : Editor {

	ComponentToggleAnimation componentToggle;

	public void Awake()
	{
		this.componentToggle = (ComponentToggleAnimation)target;
	}

	public override void OnInspectorGUI ()
	{
		// Configuration:
		bool _repeatable = EditorGUILayout.Toggle ("Repeatable?", componentToggle.repeatable);
		GameObject[] _targets = PrairieGUI.drawObjectList<GameObject> ("Objects To Animate:", componentToggle.targets);
		int _animX = EditorGUILayout.IntField("X-axis transl amount:", componentToggle.animX);
		int _animY = EditorGUILayout.IntField("Y-axis transl amount:", componentToggle.animY);
		int _animZ = EditorGUILayout.IntField("Z-axis transl amount:", componentToggle.animZ);
		int _speed = EditorGUILayout.IntField("Speed:", componentToggle.speed);

		// Save:
		if (GUI.changed) {
			Undo.RecordObject(componentToggle, "Modify Component Translation");
			componentToggle.repeatable = _repeatable;
			componentToggle.targets = _targets;
			componentToggle.animX = _animX;
			componentToggle.animY = _animY;
			componentToggle.animZ = _animZ;
			componentToggle.speed = _speed;
		}

		// Warnings (after properties have been updated):
		this.DrawWarnings();
	}

	public void DrawWarnings()
	{
		foreach (GameObject obj in componentToggle.targets)
		{
			if (obj == null)
			{
				PrairieGUI.warningLabel ("You have one or more empty slots in your list of toggles.  Please fill these slots or remove them.");
				break;
			}
		}
	}
}
