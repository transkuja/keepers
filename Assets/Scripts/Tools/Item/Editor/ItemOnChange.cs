using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;


[CustomEditor(typeof(ItemInstance))]
public class ItemOnChange : Editor {
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();

        if((this.target as ItemInstance).typeItem == TypeItem.Equipement)
        {
            EditorGUILayout.LabelField("Localisation", EditorStyles.boldLabel);
            (this.target as ItemInstance).equipType = (TypeEquipement)EditorGUILayout.EnumPopup("Equip", (this.target as ItemInstance).equipType);
            EditorGUILayout.LabelField("Bonus Stats", EditorStyles.boldLabel);
        }
        if ((this.target as ItemInstance).typeItem == TypeItem.Consummable)
        {
            EditorGUILayout.LabelField("Quantite", EditorStyles.boldLabel);
            (this.target as ItemInstance).quantite = EditorGUILayout.IntSlider((this.target as ItemInstance).quantite, 1, 99);

        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(this.target);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

    }
}
