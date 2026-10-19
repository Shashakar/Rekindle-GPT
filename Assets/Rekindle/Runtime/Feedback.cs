using UnityEngine;
using System.Collections;
namespace Rekindle {
 public static class Feedback {
  static AudioClip tone;static Material sparks;public static void Tone(float pitch,float duration){if(!Application.isPlaying)return;if(!tone){tone=AudioClip.Create("Forged chime",2205,1,22050,false);var data=new float[2205];for(int i=0;i<data.Length;i++)data[i]=Mathf.Sin(i*.08f)*.1f*(1-i/(float)data.Length);tone.SetData(data,0);}var g=new GameObject("Chime");var s=g.AddComponent<AudioSource>();s.clip=tone;s.pitch=pitch/280;s.volume=.35f;s.Play();Object.Destroy(g,1);}
  public static void Burst(Vector3 p,Color c,int count){if(!Application.isPlaying)return;var g=new GameObject("Restoration sparks");g.transform.position=p;var ps=g.AddComponent<ParticleSystem>();ps.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);var main=ps.main;main.startColor=c;main.startLifetime=.8f;main.startSpeed=2;main.startSize=.15f;main.loop=false;main.duration=.5f;var emission=ps.emission;emission.rateOverTime=0;var r=ps.GetComponent<ParticleSystemRenderer>();if(!sparks)sparks=Resources.Load<Material>("RekindleSparks");if(!sparks)sparks=new Material(Shader.Find("Universal Render Pipeline/Particles/Unlit"));r.sharedMaterial=sparks;ps.Play();ps.Emit(count);Object.Destroy(g,2);}
 }
}
