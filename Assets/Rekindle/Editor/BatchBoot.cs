#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
namespace Rekindle.Editor {
 public static class BatchBoot {
  public static void Run(){PrototypeBuildSettings.Apply();if(!File.ReadAllText("Artifacts/windows-optimization-profile.txt").StartsWith("Windows prototype:"))throw new System.InvalidOperationException("Managed Windows profile was not applied");if(File.Exists("Artifacts/managed-build.request"))File.Delete("Artifacts/managed-build.request");EditorSceneManager.OpenScene("Assets/Rekindle/Content/Scenes/Cinderhollow.unity");ScenePolish.Run();RuleValidation.Run();File.WriteAllText("Artifacts/restart-result.txt","Fresh editor loaded saved Cinderhollow and ran validation.");File.WriteAllText("Artifacts/nunit.request","run");File.WriteAllText("Artifacts/smoke.request","run");}
 }
}
#endif
