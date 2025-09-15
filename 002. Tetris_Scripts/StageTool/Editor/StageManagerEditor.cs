using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StageManager))]
public class StageManagerEditor : Editor
{
    private int spawnX = 0;
    private int spawnY = 0;

    public override void OnInspectorGUI()
    {

        DrawDefaultInspector();
        GUILayout.Space(10);

        StageManager manager = (StageManager)target;

        if (GUILayout.Button("Spawn Random Obstacles"))
        {
            manager.SpawnRandomObstacle();
        }

        if (GUILayout.Button("Remove ALL Obstacles"))
        {
            manager.RemoveAllObstacle();
        }

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Target Spawn Obstacle", EditorStyles.boldLabel);
        spawnX = EditorGUILayout.IntField("Position X", spawnX);
        spawnY = EditorGUILayout.IntField("Position Y", spawnY);

        if (GUILayout.Button("Spawn Target Obstacle"))
        {
            // z���� 0���� ����
            Vector3Int tilePosition = new Vector3Int(spawnX, spawnY, 0);
            manager.TargetSpawnObstacle(tilePosition);
        }

        if (GUILayout.Button("Remove Target Obstacle"))
        {
            Vector3Int tilePosition = new Vector3Int(spawnX, spawnY, 0);
            manager.RemoveTargetObstacle(tilePosition);
        }

        GUILayout.Space(10);

        if (GUILayout.Button("Save Stage"))
        {
            manager.SaveStage();
        }

        if (GUILayout.Button("Save Stage As..."))
        {
            string path = EditorUtility.SaveFilePanel("Save Stage As", "", $"Stage_{manager.stageNumber}.json", "json");
            if (!string.IsNullOrEmpty(path))
            {
                manager.SaveStageAs(path);
            }
        }
        GUILayout.Space(10);

        if (GUILayout.Button("Load Stage"))
        {
            manager.LoadStage();
        }

        if (GUILayout.Button("Load Stage As..."))
        {
            string path = EditorUtility.SaveFilePanel("Save Stage As", "", $"Stage_{manager.stageNumber}.json", "json");
            if (!string.IsNullOrEmpty(path))
            {
                manager.LoadStageAs(path);
            }
        }
    }

    private void OnSceneGUI()
    {
        StageManager manager = (StageManager)target;

        Event e = Event.current;
        // ���콺 ���� ��ư Ŭ�� �̺�Ʈ�� ����
        if (e.type == EventType.MouseDown && e.button == 0)
        {
            // Scene ���� ���콺 ������ ��ġ�� ���� ��ǥ�� ��ȯ
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            // Ÿ�ϸ� ����� z=0��� ����
            Vector3 worldPoint = ray.origin;

            if (manager.tilemap != null)
            {
                // ���� ��ǥ�� Ÿ�ϸ� �� ��ǥ�� ��ȯ
                Vector3Int cellPos = manager.tilemap.WorldToCell(worldPoint);
                spawnX = cellPos.x;
                spawnY = cellPos.y;

                // �ν����� ���� (����� ��ǥ�� �ݿ�)
                Repaint();
            }
            // �̺�Ʈ ��� ó��
            e.Use();
        }
    }
}