using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

[System.Serializable]
public class Chain
{
    public List<Enemy> Enemies;

    public Chain() {
        Enemies = new List<Enemy>();
    }

    public void Sort(Vector3 position) {
        Enemies = Enemies.OrderBy(x => Vector3.Distance(position, x.transform.position)).ToList();
    }

    public void Add(Enemy enemy) {
        Enemies.Add(enemy);
    }
}

public class ChainManager : MonoBehaviour
{
    private List<Chain> Chains = new List<Chain>();

    public Chain NewChain() {
        var chain = new Chain();
        AddChain(chain);
        return chain;
    }

    private void AddChain(Chain chain) {
        Chains.Add(chain);
    }

    public void RemoveChain(Chain chain) {
        Chains.Remove(chain);
    }

    public void RemoveAt(int index) {
        Chains.RemoveAt(index);
    }
}
