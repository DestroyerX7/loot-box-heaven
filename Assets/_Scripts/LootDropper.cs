using System.Linq;
using UnityEngine;

public class LootDropper : MonoBehaviour
{
    [System.Serializable]
    private struct LootDropData
    {
        public Loot LootPrefab;
        public int MinInclusive;
        public int MaxExclusive;
    }

    [SerializeField] private LootDropData[] _lootDropDatas;
    [SerializeField] private int _numDrops = 3;

    public void DropLoot()
    {
        for (int i = 0; i < _numDrops; i++)
        {
            int randNum = Random.Range(0, 100);
            Loot lootPrefab = _lootDropDatas.FirstOrDefault(l => randNum >= l.MinInclusive && randNum < l.MaxExclusive).LootPrefab;

            if (lootPrefab != null)
            {
                Loot loot = Instantiate(lootPrefab, transform.position, Quaternion.identity);
                Vector2 velocity = new(Random.Range(-5f, 5f), Random.Range(10, 15));
                loot.Bounce(velocity, 0.5f);
            }
        }
    }

    
    public void SetNumDrops(int numDrops)
    {
        _numDrops = numDrops;
    }
}
