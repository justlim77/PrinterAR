using UnityEngine;
using System.Collections;

public static class Extensions {
    public static Transform GetChildByName(this Transform trans, string name)
    {
        Transform[] children = trans.GetComponentsInChildren<Transform>();
        foreach(Transform child in children)
        { 
            if (child.name == name)
            {
                return child;
            }
        }
        return null;
    }

    public static Transform Clear(this Transform trans)
    {
        foreach (Transform child in trans)
        {
            GameObject.Destroy(child.gameObject);
        }
        return trans;
    }
}
