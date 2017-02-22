using System;
using System.Reflection;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ReflectiveEnumerator
{
    static ReflectiveEnumerator() { }

    /// <summary>
    /// Looks for all objects of type T and returns a list of those objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="constructorArgs"></param>
    /// <returns></returns>
    public static IEnumerable<T> GetEnumerableOfType<T>(params object[] constructorArgs) where T : class, IComparable<T>
    {
        //Make a list to hold the objects in, then make a local query towards all objects of type T
        List<T> objects = new List<T>();
        foreach (Type type in
            Assembly.GetAssembly(typeof(T)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
        {
            //Creates an instance of that type of object, and stores it in a list.
            objects.Add((T)Activator.CreateInstance(type, constructorArgs));
        }
        objects.Sort();
        return objects;
    }
}
