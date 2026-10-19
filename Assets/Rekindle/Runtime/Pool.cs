using System;
using System.Collections.Generic;
using System.Linq;
namespace Rekindle {
 [Serializable] public class Pool {
  public float Max {get;private set;} public float Value {get;private set;} public event Action Changed;
  public Pool(float max){if(max<=0)throw new ArgumentException(); Max=max;Value=max;}
  public bool Spend(float n){if(n<0||float.IsNaN(n)||Value<n)return false;Value-=n;Changed?.Invoke();return true;}
  public void Add(float n){if(n<0||float.IsNaN(n))return;Value=Math.Min(Max,Value+n);Changed?.Invoke();}
  public void Restore(float n){if(float.IsNaN(n))n=Max;Value=Math.Max(0,Math.Min(Max,n));Changed?.Invoke();}
 }
}
