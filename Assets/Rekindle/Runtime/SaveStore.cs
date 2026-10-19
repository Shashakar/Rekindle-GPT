using System;
using System.IO;
using UnityEngine;
namespace Rekindle {
 public sealed class SaveStore {
  readonly string path;public string Path=>path;public bool Exists=>File.Exists(path)||File.Exists(path+".bak");public SaveStore(string p){path=p;}
  public void Write(SaveData data){var dir=System.IO.Path.GetDirectoryName(path);Directory.CreateDirectory(dir);string temp=path+".tmp";File.WriteAllText(temp,JsonUtility.ToJson(data,true));if(File.Exists(path)){File.Copy(path,path+".bak",true);File.Replace(temp,path,null);}else File.Move(temp,path);}
  public SaveData Read(Func<SaveData,bool> validate){foreach(var candidate in new[]{path,path+".bak"}){try{if(!File.Exists(candidate))continue;var s=JsonUtility.FromJson<SaveData>(File.ReadAllText(candidate));if(s!=null&&validate(s))return s;}catch(Exception e)when(e is IOException||e is ArgumentException){Debug.LogWarning("Save recovery: "+e.Message);}}return null;}
 }
}
