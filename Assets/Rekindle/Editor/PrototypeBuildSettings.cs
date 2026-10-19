#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
namespace Rekindle.Editor {
 [InitializeOnLoad]public static class PrototypeBuildSettings {
  static double next;static PrototypeBuildSettings(){EditorApplication.update+=()=>{if(EditorApplication.timeSinceStartup<next||EditorApplication.isCompiling||EditorApplication.isPlayingOrWillChangePlaymode)return;next=EditorApplication.timeSinceStartup+1;if(File.Exists("Artifacts/managed-build.request")){File.Delete("Artifacts/managed-build.request");Apply();}};}
  [MenuItem("Rekindle/Windows prototype managed optimization profile")]public static void Apply(){try{var assembly=AppDomain.CurrentDomain.GetAssemblies().First(a=>a.GetName().Name=="Unity.Burst.Editor");var type=assembly.GetType("Unity.Burst.Editor.BurstPlatformAotSettings",true);var get=type.GetMethod("GetSerializedSettings",BindingFlags.Static|BindingFlags.NonPublic);var settings=(SerializedObject)get.Invoke(null,new object[]{BuildTarget.StandaloneWindows64});settings.FindProperty("EnableBurstCompilation").boolValue=false;settings.ApplyModifiedPropertiesWithoutUndo();BuildTarget? target=BuildTarget.StandaloneWindows64;type.GetMethod("Save",BindingFlags.Instance|BindingFlags.NonPublic).Invoke(settings.targetObject,new object[]{target});File.WriteAllText("Artifacts/windows-optimization-profile.txt","Windows prototype: Mono scripting backend; Burst AOT disabled through the package's platform settings. Managed fallback retains gameplay. Native optimization and performance profiling are deferred.");}catch(Exception e){File.WriteAllText("Artifacts/windows-optimization-profile.txt",e.ToString());Debug.LogException(e);}}
 }
}
#endif
