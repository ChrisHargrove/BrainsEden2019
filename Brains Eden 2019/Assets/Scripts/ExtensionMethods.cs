using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static T Random<T>(this List<T> list) {
        if (list.Count < 1) throw new InvalidOperationException("The list is empty!");
        return list[UnityEngine.Random.Range(0, list.Count)];
    }
}
