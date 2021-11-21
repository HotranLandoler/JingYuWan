using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//namespace JYW.EditorScript
//{
//    [CustomEditor(typeof(CardData))]
//    public class CardEditor : Editor
//    {
//        public override void OnInspectorGUI()
//        {
//            DrawDefaultInspector();
//            CardData data = target as CardData;
//            EditorGUI.indentLevel++;
//            foreach (var effect in data.Effects)
//            {
//                Editor effectEditor = Editor.CreateEditor(effect);
//                effectEditor?.OnInspectorGUI();
//            }
//            EditorGUI.indentLevel--;
//        }
//    }
//}