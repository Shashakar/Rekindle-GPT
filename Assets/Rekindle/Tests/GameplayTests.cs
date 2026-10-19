using NUnit.Framework;
namespace Rekindle.Tests {
 public class GameplayTests {
  [Test]public void OverspendingDoesNotMutatePool(){var p=new Pool(30);Assert.IsFalse(p.Spend(31));Assert.AreEqual(30,p.Value);}
  [Test]public void InventoryTransactionIsAtomic(){var b=new Inventory();b.Add("wood",4);Assert.IsFalse(b.Consume(new Stack("wood",3),new Stack("iron",1)));Assert.AreEqual(4,b.Count("wood"));}
  [Test]public void BossDefeatIsIdempotent(){var w=new WorldState();int events=0;w.Changed+=()=>events++;w.Defeat("warden");w.Defeat("warden");Assert.IsTrue(w.BossDefeated);Assert.AreEqual(1,events);}
  [Test]public void MissingRequirementsRejectRestoration(){var w=new WorldState();Assert.IsFalse(w.Restore("hearth",new Inventory()));Assert.AreEqual(0,w.Stage("hearth"));}
  [Test]public void SnapshotRestoresDungeonAndDepletion(){var w=new WorldState();w.Defeat("warden");w.Deplete("wood-1");var restored=new WorldState();Assert.IsTrue(restored.Load(w.Snapshot(new Inventory()),new Inventory()));Assert.IsTrue(restored.BossDefeated);Assert.IsTrue(restored.IsDepleted("wood-1"));}
 }
}
