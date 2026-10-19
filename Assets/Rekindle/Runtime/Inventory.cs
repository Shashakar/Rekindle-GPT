using System;
using System.Collections.Generic;
using System.Linq;
namespace Rekindle {
 public class Inventory {
  readonly List<Stack> slots=new List<Stack>();public IReadOnlyList<Stack> Slots=>slots;public int Capacity{get;}public event Action Changed;
  public Inventory(int capacity=16){Capacity=capacity;}
  public int Count(string id)=>slots.Where(x=>x.id==id).Sum(x=>x.quantity);
  public bool Add(string id,int n){if(string.IsNullOrEmpty(id)||n<=0)return false;var s=slots.Find(x=>x.id==id);if(s==null){if(slots.Count>=Capacity)return false;slots.Add(new Stack(id,n));}else s.quantity+=n;Changed?.Invoke();return true;}
  public bool Consume(params Stack[] cost){if(cost.Any(x=>x.quantity<=0)||cost.GroupBy(x=>x.id).Any(g=>Count(g.Key)<g.Sum(x=>x.quantity)))return false;foreach(var c in cost){var s=slots.Find(x=>x.id==c.id);s.quantity-=c.quantity;if(s.quantity==0)slots.Remove(s);}Changed?.Invoke();return true;}
  public bool Swap(int a,int b){if(a<0||b<0||a>=slots.Count||b>=slots.Count)return false;var s=slots[a];slots[a]=slots[b];slots[b]=s;Changed?.Invoke();return true;}
  public bool Load(IEnumerable<Stack> source){var list=source.ToList();if(list.Count>Capacity||list.Any(x=>x==null||string.IsNullOrEmpty(x.id)||x.quantity<=0)||list.Select(x=>x.id).Distinct().Count()!=list.Count)return false;slots.Clear();slots.AddRange(list.Select(x=>new Stack(x.id,x.quantity)));Changed?.Invoke();return true;}
 }
}
