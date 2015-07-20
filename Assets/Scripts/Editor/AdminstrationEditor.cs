using UnityEngine;
using UnityEditor;
using System.Collections;
using Wb.Companion.Core.WbAdministration;
using Wb.Companion.Core.UI;


namespace Wb.Companion.WbEditor {

    [CustomEditor(typeof(SceneManager))]
    public class AdminstrationEditor : Editor {

        private SceneManager editorTarget;
        private string[] sceneList;
        private int selectedItem = 0;

        //---------------------------------------------------------------------

        public void Awake() {
            this.editorTarget = ((SceneManager)target);
            this.sceneList = this.editorTarget.getSceneList();
        }

        //---------------------------------------------------------------------
        
        public override void OnInspectorGUI() {

            DrawDefaultInspector();

            this.selectedItem = EditorGUILayout.Popup("Default Start Scene", selectedItem, sceneList);
            this.editorTarget.setDefaultStartScene(this.selectedItem);

           
        }

    }
}
