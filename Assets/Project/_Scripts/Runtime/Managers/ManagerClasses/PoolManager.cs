using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project._Scripts.Runtime.Managers.ManagerClasses
{
   [DefaultExecutionOrder(560)]
   public class PoolManager : MonoBehaviour
   {
   #region Fields
      [System.Serializable]
      public class Pool
      {

         public string ObjectName;
         public GameObject Object;
         public List<GameObject> Objects = new List<GameObject>();
         public Transform ObjectHolder;
         public int PoolSize;
      }

      public List<Pool> Pools;
      public static Dictionary<string, Queue<GameObject>> PoolDictionary;


   #endregion

      public void Start()
      {
         //Initializer
         PoolDictionary = new Dictionary<string, Queue<GameObject>>();

         foreach(Pool pool in Pools)
         {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for(int i = 0; i < pool.PoolSize; i++)
            {
               GameObject poolObject = Instantiate(pool.Object, pool.ObjectHolder);
               poolObject.name = pool.ObjectName;
            
               pool.Objects.Add(poolObject);
            
               poolObject.SetActive(false);
            
               //Mark the object in the pool
               objectPool.Enqueue(poolObject);
            }

            PoolDictionary.Add(pool.ObjectName, objectPool);
         }
      }

      /// <summary>
      /// Spawning the object as type of GameObject with given Transform data
      /// </summary>
      /// <param name="type"></param>
      /// <param name="spawnPosition"></param>
      /// <param name="spawnRotation"></param>
      /// <returns></returns>
      public GameObject SpawnFromPool(string type, Vector3 spawnPosition, Quaternion spawnRotation)
      {
         if (PoolDictionary.ContainsKey(type) == false) { return null; }

         //Mark the object in the pool
         GameObject objectToSpawn = PoolDictionary[type].Dequeue();

         objectToSpawn.transform.position = spawnPosition;
         objectToSpawn.transform.rotation = spawnRotation;

         objectToSpawn.SetActive(true);

         //Mark the object in the pool
         PoolDictionary[type].Enqueue(objectToSpawn);
         return objectToSpawn;
      }
   
      public GameObject SpawnRandomFromPool(Vector3 spawnPosition, Quaternion spawnRotation, string[] poolParams)
      {
         var randomPool = new List<Pool>();
      
         poolParams.ToList().ForEach(x =>
         {
            randomPool.Add(Pools.Find(y => y.ObjectName == x));   
         });


         var randomIndex = Random.Range(0, randomPool.Count);
      
         string type = randomPool[randomIndex].ObjectName;
      
         if (PoolDictionary.ContainsKey(type) == false) { return null; }

         //Mark the object in the pool
         GameObject objectToSpawn = PoolDictionary[type].Dequeue();

         objectToSpawn.transform.position = spawnPosition;
         objectToSpawn.transform.rotation = spawnRotation;

         objectToSpawn.SetActive(true);

         //Mark the object in the pool
         PoolDictionary[type].Enqueue(objectToSpawn);
         return objectToSpawn;
      }

      public bool HasObject(string objectName)
      {
         return PoolDictionary.Keys.ToList().Contains(objectName);
      }
   
      /// <summary>
      /// Spawning the object as type of GameObject with given Transform data
      /// </summary>
      /// <param name="type"></param>
      /// <param name="spawnPosition"></param>
      /// <param name="spawnRotation"></param>
      /// <returns></returns>
      public  T SpawnFromPool<T>(string type, Vector3 spawnPosition, Quaternion spawnRotation) where T : MonoBehaviour
      {
         if (PoolDictionary.ContainsKey(type) == false) { return null; }

         //Mark the object in the pool
         GameObject objectToSpawn = PoolDictionary[type].Dequeue();

         objectToSpawn.transform.position = spawnPosition;
         objectToSpawn.transform.rotation = spawnRotation;

         objectToSpawn.gameObject.SetActive(true);

         //Mark the object in the pool
         PoolDictionary[type].Enqueue(objectToSpawn.gameObject);

         return objectToSpawn.GetComponent<T>();
      }

      /// <summary>
      /// Disables the object and returns it to the pool
      /// </summary>
      /// <param name="poolObject"></param>
      public void DestroyPoolObject<T>(T poolObject) where T : MonoBehaviour
      {
         poolObject.gameObject.SetActive(false);

         Pool pool = Pools.Find(x => x.Objects.Find(_ => poolObject.gameObject));

         PoolDictionary[pool.ObjectName].Enqueue(poolObject.gameObject);
      }
   }
}