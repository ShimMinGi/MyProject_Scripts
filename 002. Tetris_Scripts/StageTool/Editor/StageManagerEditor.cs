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
            // z축은 0으로 고정
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
        // 마우스 왼쪽 버튼 클릭 이벤트를 감지
        if (e.type == EventType.MouseDown && e.button == 0)
        {
            // Scene 뷰의 마우스 포인터 위치를 월드 좌표로 변환
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            // 타일맵 평면이 z=0라고 가정
            Vector3 worldPoint = ray.origin;

            if (manager.tilemap != null)
            {
                // 월드 좌표를 타일맵 셀 좌표로 변환
                Vector3Int cellPos = manager.tilemap.WorldToCell(worldPoint);
                spawnX = cellPos.x;
                spawnY = cellPos.y;

                // 인스펙터 갱신 (변경된 좌표를 반영)
                Repaint();
            }
            // 이벤트 사용 처리
            e.Use();
        }
    }
}