using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CloudSystem : MonoBehaviour
{
    [SerializeField] Vector2 cloudAreaSize;
    [SerializeField] List<GameObject> cloudPrefabs;
    [SerializeField] int cloudOrderInLayer = 0;

    [System.Serializable]
    class CloudLayer
    {
        public Color tint;
        public Vector2Int rectangleCount;
        public float cloudSpeed;
    }

    [SerializeField] List<CloudLayer> cloudLayers;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, cloudAreaSize);
    }

    Queue<GameObject>[] clouds;
    List<GameObject> cloudParents;

    void Start()
    {
        clouds = new Queue<GameObject>[cloudLayers.Count];
        cloudParents = new List<GameObject>();

        for (int i = 0; i < cloudLayers.Count; i++)
        {
            clouds[i] = new Queue<GameObject>();
            GameObject parent = new GameObject("Layer"+(i+1));
            parent.transform.parent = gameObject.transform;

            cloudParents.Add(parent);

            Vector2 step = new Vector2(1.0f/ cloudLayers[i].rectangleCount.x, 1.0f/ cloudLayers[i].rectangleCount.y);
            for (int j = 0; j < cloudLayers[i].rectangleCount.y; j++)
            {
                for (int k = 0; k < cloudLayers[i].rectangleCount.x; k++)
                {
                    Vector2 pos = new Vector2(Random.Range((float)k / cloudLayers[i].rectangleCount.x, (float)(k + 1) / cloudLayers[i].rectangleCount.x), 
                                              Random.Range((float)j / cloudLayers[i].rectangleCount.y, (float)(j + 1) / cloudLayers[i].rectangleCount.y));
                    pos -= new Vector2(0.5f,0.5f);
                    pos *= cloudAreaSize;
                    pos += new Vector2(transform.position.x, transform.position.y);

                    GameObject cloudObj = Instantiate(cloudPrefabs[Random.Range(0, cloudPrefabs.Count)], pos, Quaternion.identity);
                    cloudObj.transform.parent = parent.transform;
                    SpriteRenderer renderer = cloudObj.GetComponent<SpriteRenderer>();
                    renderer.color = cloudLayers[i].tint;
                    renderer.sortingOrder = cloudOrderInLayer - i;
                    clouds[i].Enqueue(cloudObj);
                }
            }
            clouds[i] = new Queue<GameObject>(clouds[i].OrderByDescending(x => x.transform.position.x));
        }

        
    }

    const float layerResetVal = 10000;
    void Update()
    {
        for (int i = 0; i < cloudParents.Count; i++)
        {
            cloudParents[i].transform.position += new Vector3(cloudLayers[i].cloudSpeed * Time.deltaTime,0,0);
            if (cloudParents[i].transform.position.x > layerResetVal)
            {
                cloudParents[i].transform.position -= new Vector3(layerResetVal, 0, 0);
                for (int j = 0; j < cloudParents[i].transform.childCount; j++)
                {
                    cloudParents[i].transform.GetChild(j).transform.position += new Vector3(layerResetVal, 0, 0);
                }
            }
        }
        for (int i = 0; i < cloudLayers.Count; i++)
        {
            while (clouds[i].Count > 0 && clouds[i].Peek().transform.position.x > cloudAreaSize.x / 2)
            {
                GameObject obj = clouds[i].Dequeue();
                obj.transform.position -= new Vector3(cloudAreaSize.x, 0, 0);
                clouds[i].Enqueue(obj);
            }
        }
    }
}
