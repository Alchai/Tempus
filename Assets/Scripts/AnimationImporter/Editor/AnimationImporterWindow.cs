using UnityEditor;
using UnityEngine;

public class AnimationImporterWindow : EditorWindow
{
    int inFrames, outFrames, rate, numStates;


    [MenuItem("CustomGUI/Animation Importer")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AnimationImporterWindow));
    }

    void OnGUI()
    {
        GUILayout.Label("Animation Settings", EditorStyles.boldLabel);
        numStates = EditorGUILayout.IntField("Number of States: ", numStates);
        inFrames = EditorGUILayout.IntField("In Frames: ", inFrames);
        outFrames = EditorGUILayout.IntField("Out Frames: ", outFrames);
        rate = EditorGUILayout.IntField("Rate: ", rate);
    }
}