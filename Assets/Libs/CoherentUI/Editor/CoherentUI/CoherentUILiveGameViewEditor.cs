using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(CoherentUILiveGameView))]
public class CoherentUILiveGameViewEditor : Editor {

	private CoherentUILiveGameView m_Target;
	private CoherentFoldout[] m_Fields;

	public void OnEnable() {
		m_Target = target as CoherentUILiveGameView;
		m_Fields = CoherentExposeProperties.GetProperties(m_Target);
	}

	public override void OnInspectorGUI() {
		if(m_Target == null)
			return;
		this.DrawDefaultInspector();
		CoherentExposeProperties.Expose(m_Fields);
	}
}