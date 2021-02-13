using UnityEngine;
using System.Collections.Generic;


public class Pool : MonoBehaviour
{
    public uint size = 1;
    public GameObject poolObject;

    private List<GameObject> m_items = new List<GameObject>();
    private List<GameObject> m_activeItems = new List<GameObject>();

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < size; ++i)
        {
            InstantiateObject();
        }
    }

    public List<GameObject> GetActiveObjects()
    {
        return m_activeItems;
    }

    public GameObject GetObject()
    {
        if (m_items.Count == 0)
        {
            Debug.Log("Pool depleted, consider boosting its size, this can cause performance issues.");
            InstantiateObject();
        }

        if (m_items.Count > 0)
        {
            GameObject item = (GameObject)m_items[0];
            m_items.RemoveAt(0);
            m_activeItems.Add(item);
            //Debug.Log (m_items.Count.ToString ());
            return item;
        }

        return null;
    }

    public void FreeObject(GameObject goToFree)
    {
        if (m_activeItems.Count > 0)
        {
            foreach (GameObject go in m_activeItems)
            {
                if (go.GetInstanceID() == goToFree.GetInstanceID())
                {
                    m_items.Add(go);
                    m_activeItems.Remove(go);
                    goToFree.SetActive(false);
                    //Debug.Log (m_items.Count.ToString ());
                    break;
                }
            }
        }
        else
        {
            Debug.Log("Nothing to free.");
        }
    }

    public void FreeAndDeactivateAllObjects()
    {
        foreach (GameObject go in m_activeItems)
        {
            go.SetActive(false);
            m_items.Add(go);
        }

        m_activeItems.Clear();
    }

    private void InstantiateObject()
    {
        GameObject newGo = Instantiate(poolObject, gameObject.transform);
        PoolLifeTime lifeTime = newGo.GetComponent<PoolLifeTime>();

        if (lifeTime != null)
        {
            lifeTime.SetPool(this);
        }
        newGo.SetActive(false);
        m_items.Add(newGo);
    }
}