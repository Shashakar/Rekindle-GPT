#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using Unity.AI.Navigation;
using UnityEditor.TestTools.TestRunner.Api;
namespace Rekindle.Editor {
 [InitializeOnLoad]public static class LocalEditorActions {
  static double next;static TestRunnerApi runner;static LocalEditorActions(){EditorApplication.update+=Poll;Application.logMessageReceived+=OnLog;}
  static void OnLog(string msg,string trace,LogType type){if(type==LogType.Error||type==LogType.Exception)File.AppendAllText("Artifacts/editor-errors.txt",DateTime.UtcNow+" "+type+" "+msg+"\n"+trace+"\n");}
  static void Poll(){if(EditorApplication.timeSinceStartup<next||EditorApplication.isCompiling||EditorApplication.isUpdating)return;next=EditorApplication.timeSinceStartup+2;
   if(File.Exists("Artifacts/state.request")){File.Delete("Artifacts/state.request");File.WriteAllText("Artifacts/editor-state.txt",Application.dataPath+"\nPlaying="+EditorApplication.isPlaying+"\nScene="+SceneManager.GetActiveScene().path);}
   if(EditorApplication.isPlaying)return;
   if(File.Exists("Artifacts/persist.request")){File.Delete("Artifacts/persist.request");Persist();}
   if(File.Exists("Artifacts/nunit.request")){File.Delete("Artifacts/nunit.request");runner=ScriptableObject.CreateInstance<TestRunnerApi>();runner.RegisterCallbacks(new Results());runner.Execute(new ExecutionSettings(new Filter{testMode=TestMode.EditMode,assemblyNames=new[]{"Rekindle.Tests"}}){runSynchronously=true});}
   if(File.Exists("Artifacts/reload.request")){File.Delete("Artifacts/reload.request");EditorSceneManager.OpenScene("Assets/Rekindle/Content/Scenes/Cinderhollow.unity");RuleValidation.Run();File.WriteAllText("Artifacts/reload-result.txt","Scene reopened from disk; navigation triangles="+UnityEngine.AI.NavMesh.CalculateTriangulation().vertices.Length);}
  }
  static void Persist(){var surface=UnityEngine.Object.FindFirstObjectByType<NavMeshSurface>();if(surface&&surface.navMeshData&&!AssetDatabase.Contains(surface.navMeshData))AssetDatabase.CreateAsset(surface.navMeshData,"Assets/Rekindle/Content/Navigation.asset");var terrain=UnityEngine.Object.FindFirstObjectByType<Terrain>();if(terrain&&terrain.materialTemplate&&!AssetDatabase.Contains(terrain.materialTemplate))AssetDatabase.CreateAsset(terrain.materialTemplate,"Assets/Rekindle/Content/Materials/Terrain.mat");foreach(var text in UnityEngine.Object.FindObjectsByType<TextMesh>(FindObjectsSortMode.None))text.transform.localRotation=Quaternion.identity;EditorSceneManager.SaveScene(SceneManager.GetActiveScene());AssetDatabase.SaveAssets();File.WriteAllText("Artifacts/persistence-assets.txt","NavMesh and terrain material persisted through AssetDatabase; sign text corrected. "+Application.dataPath);}
  class Results:ICallbacks {public void RunStarted(ITestAdaptor tests){}public void TestStarted(ITestAdaptor test){}public void TestFinished(ITestResultAdaptor result){}public void RunFinished(ITestResultAdaptor result){TestRunnerApi.SaveResultToFile(result,"Artifacts/nunit-results.xml");File.WriteAllText("Artifacts/nunit-summary.txt","Passed="+result.PassCount+"; Failed="+result.FailCount+"; Skipped="+result.SkipCount);}}
 }
}
#endif
