using UnityEditor;
using UnityEngine;

namespace SupplyChain.View.World
{
    [CustomEditor(typeof(UpgradeNode))]
    public class UpgradeNodeEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            UpgradeNode node = (UpgradeNode)target;

            GUILayout.Space(10);
            if (GUILayout.Button("Generate Edges", GUILayout.Height(30)))
            {
                // 실행 전 상태 기록 (삭제되는 객체들을 위해)
                Undo.RegisterFullObjectHierarchyUndo(node, "Generate Edges");
                
                node.UpdateLines();
                
                // 생성된 객체들을 Undo 시스템에 등록하기 위해 씬 전체를 더티로 설정하거나 
                // 개별적으로 RegisterCreatedObjectUndo를 호출해야 하지만, 
                // RegisterFullObjectHierarchyUndo가 이미 계층 구조를 잡고 있으므로 
                // EditorUtility.SetDirty로 충분합니다.
                EditorUtility.SetDirty(node);
                if (!Application.isPlaying)
                {
                    UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(node.gameObject.scene);
                }
            }
        }
    }
}
