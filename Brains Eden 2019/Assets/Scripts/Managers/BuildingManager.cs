using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{

    public Building Tower;
    public List<Building> Houses = new List<Building>();

    void Update()
    {
        RemoveDeadBuildings();
    }

    public Building FindClosestHouse(Vector3 position)
    {
        if (Houses.Count > 0)
        {
            Building closestHouse = null;
            float closestDistance = float.PositiveInfinity;

            foreach (Building h in Houses)
            {
                var closest = Vector3.Distance(h.transform.position, position);
                if (closest < closestDistance)
                {
                    closestHouse = h;
                    closestDistance = closest;
                }
            }
            return closestHouse;
        }

        return Tower;
    }

    public void RemoveDeadBuildings() {
        Houses.RemoveAll(delegate (Building b) { return b.CurrentDestruction == DestructionLevel.DESTROYED; });
    }
}
