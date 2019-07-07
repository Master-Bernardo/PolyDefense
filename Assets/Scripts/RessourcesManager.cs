using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourcesManager : MonoBehaviour
{
    public static RessourcesManager Instance;

    Dictionary<RessourceType, HashSet<Ressource>>ressources = new Dictionary<RessourceType, HashSet<Ressource>>();
    

    void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(Instance);
        }
        else
        {
            Instance = this;
        }

        ressources.Add(RessourceType.fer, new HashSet<Ressource>());
        ressources.Add(RessourceType.mer, new HashSet<Ressource>());
        ressources.Add(RessourceType.rubith, new HashSet<Ressource>());


    }

    public void AddRessource(Ressource ressource)
    {
        ressources[ressource.type].Add(ressource);
    }

    public void RemoveRessource(Ressource ressource)
    {
        ressources[ressource.type].Remove(ressource);
    }

    public HashSet<Ressource> GetRessources(RessourceType type)
    {
        return ressources[type];
    }


}
