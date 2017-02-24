using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(AssociatedTwineNodes))]
public class NextTwineNodeInspector : Editor
{

	AssociatedTwineNodes associatedTwineNodes;

	public void Awake()
	{
		this.associatedTwineNodes = (AssociatedTwineNodes)target;
	}

	public override void OnInspectorGUI()
	{
		TwineNode[] nodes = FindObjectsOfType(typeof(TwineNode)) as TwineNode[];

		if (nodes.Length == 0) {
			PrairieGUI.warningLabel ("No Twine Node objects found. Have you imported your story and dragged it into the object hierarchy?");
		} else {

			if (associatedTwineNodes.selectedTwineNodeIndices == null) {
				// If the twine node has no associated indices set yet, try to auto-select
				//	a node with the same name as this object.
				int suggestedIndex = this.GetSuggestedTwineNodeIndex (nodes);
				associatedTwineNodes.selectedTwineNodeIndices = new List<int> (){ suggestedIndex };
			}

			associatedTwineNodes.selectedTwineNodeIndices = PrairieGUI.drawTwineNodeDropdownList ("Associated Twine Nodes", "Twine Node Object",
				nodes, associatedTwineNodes.selectedTwineNodeIndices);

			associatedTwineNodes.UpdateTwineNodeObjectsFromIndices (nodes);
		}

		if (GUI.changed) {
			Undo.RecordObject(associatedTwineNodes, "Modify Associated Twine Nodes");
			// BLOCKING:
			// Because Graham is currently refactoring this editor,
			// I found it best NOT to shuffle around the code too much from under him. 
			// Thus, this method is incomplete.
		}
	}

	/// <summary>
	/// Checks if any of the Twine Nodes (from the list of options for the dropdown)
	/// has the same name as the object this component is attached to.
	/// 
	/// Suggests the first item in the list if there is no name match.
	/// </summary>
	/// <returns>The suggested twine node index.</returns>
	/// <param name="nodes">Nodes.</param>
	private int GetSuggestedTwineNodeIndex(TwineNode[] nodes)
	{
		GameObject attachedGameObject = ((AssociatedTwineNodes)target).gameObject;
		for (int i = 0; i < nodes.Length; i++) {
			TwineNode node = nodes [i];
			if (node.name.Equals (attachedGameObject.name)) {
				return i;
			}
		}

		// If no name match found, then choose the first node in the list:
		return 0;
	}

}