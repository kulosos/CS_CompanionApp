using UnityEngine;
using UnityEditor;
using System.Collections;
using Wb.Companion.Core.WbAdministration;
using Wb.Companion.Core.UI;


namespace Wb.Companion.WbEditor {

    [CustomEditor(typeof(SceneManager))]
    public class SceneManagerEditorGUI : Editor {

        private SceneManager editorTarget;
        private string[] sceneList;
        private int selectedItem;

        //---------------------------------------------------------------------

        public void Awake() {
            this.editorTarget = ((SceneManager)target);
            this.sceneList = this.editorTarget.getSceneList();
			this.selectedItem = this.editorTarget.getDefaultStartSceneInt();
        }

        //---------------------------------------------------------------------
        
        public override void OnInspectorGUI() {

            DrawDefaultInspector();


            EditorGUI.BeginChangeCheck();

			this.selectedItem = EditorGUILayout.Popup("Default Start Scene", this.selectedItem, sceneList);
            if (EditorGUI.EndChangeCheck()) {
                this.editorTarget.setDefaultStartScene(this.selectedItem);
            }

        }

    }
}
