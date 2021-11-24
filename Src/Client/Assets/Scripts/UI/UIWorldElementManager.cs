using Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldElementManager : MonoSingleton<UIWorldElementManager>
{
    public GameObject nameBarPerfab;

    private Dictionary<Transform, GameObject> elements = new Dictionary<Transform, GameObject>();//单例要记得start函数会被覆盖，要删掉

    public void AddCharacterNameBar(Transform owner,Character character)
    {
        GameObject goNameBar = Instantiate(nameBarPerfab, this.transform);
        goNameBar.name = "NameBar" + character.entityId;
        goNameBar.GetComponent<UIWorldElement>().owner = owner;
        goNameBar.GetComponent<UINameBar>().character = character;
        goNameBar.SetActive(true);
        this.elements[owner] = goNameBar;
    }
    public void RemoveCharacterNameBar(Transform owner)
    {
        if (this.elements.ContainsKey(owner))
        {
            Destroy(this.elements[owner]);
            this.elements.Remove(owner);
        }
        
    }
}
