using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour, ICleanableObject
{
    [SerializeField] private Mob[] mobs;
    [SerializeField] private float spawnPeriod;
    public float timer;
    [SerializeField] private float spawnDistance;
    public Transform player;

    [SerializeField] private float maxDirtyness;
    private float currentDirtyness;

    
    // Start is called before the first frame update
    
    private void Start()
    {
        currentDirtyness = maxDirtyness;
        timer = 0;
        
    }
    
    // Update is called once per frame
    private void FixedUpdate()
    {
        
       
        
        if(player != null)
        {
            if (Vector2.Distance(transform.position, player.position) <= spawnDistance)
            {

                if (timer > 0)
                {
                    timer -= Time.fixedDeltaTime;
                }
                else
                {

                    SpawnMob();
                    timer = spawnPeriod + Random.Range(-spawnPeriod / 4, spawnPeriod / 4);
                }
            }
            
        }
        else
        {
            player = PlayerIdentity.instance.player.transform;
        }

    }
    private void SpawnMob()
    {

        float[] spawnGradient = GetSpawnGradient(GetnormalizedProbabilities());
        float rand = Random.Range(0, 1);
        int j = 0;
        for (int i = 0; i < spawnGradient.Length; i++)
        {
            if (rand >= spawnGradient[i])
            {
                j = i;

            }
        }

        GameObject instance = Instantiate(mobs[j].Prefab, transform.position, Quaternion.identity);
        instance.GetComponent<EnemyAI>().roomNavmesh = GetComponentInParent<Navmesh>();;
    }
    private float[] GetnormalizedProbabilities()
    {
        float sum = 0f;
        for (int i = 0; i < mobs.Length; i++)
        {
            sum += 1f / mobs[i].Rarity;
        }
        float[] normalizedProbabilities = new float[mobs.Length];
        for (int i = 0; i < mobs.Length; i++)
        {
            normalizedProbabilities[i] = (1f / mobs[i].Rarity) / sum;

        }
        return normalizedProbabilities;
    }
    private float[] GetSpawnGradient(float[] normalizedProbabilities)
    {
        float[] spawnGradient = new float[normalizedProbabilities.Length - 1];
        float currentKey = 0f;
        for (int i = 0; i < normalizedProbabilities.Length - 1; i++)
        {
            spawnGradient[i] = currentKey;
            currentKey += normalizedProbabilities[i];
        }
        return spawnGradient;
    }
    public void Interact()
    {

    }
    public void Clean(float amount)
    {

        currentDirtyness -= amount;
        Debug.Log("Cleaning :" + currentDirtyness);
        if (currentDirtyness <= 0f)
        {
            Destroy(gameObject);
        }
        

    }
    public float CurrentDirtyness()
    {
        return currentDirtyness;
    }
    public float MaxDirtyness()
    {
        return maxDirtyness;
    }
    public InteractionEventArgs Info()
    {
        return new InteractionEventArgs(InteractionType.CleanObject, this, transform.position);
    }
    public string Name()
    {
        return "Mob Spawner";
    }
    public KeyCode InteractionKey()
    {
        return KeyCode.C;
    }
    [System.Serializable]
    struct Mob
    {
        [SerializeField]
        GameObject prefab;
        [Range(1, 10), SerializeField]
        int rarity;
        public GameObject Prefab
        {
            get
            {
                return prefab;
            }
        }
        public int Rarity
        {
            get
            {
                return rarity;
            }
        }
    }

}
