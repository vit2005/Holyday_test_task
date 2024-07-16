using System;
using UnityEngine;

public class JsonHelper {

    public static T CreateFromJson<T>(string Json) where T : class {
        T classType = null;
        try {//If th Json IS correct
            if (!string.IsNullOrEmpty(Json)) {
                classType = JsonUtility.FromJson<T>(Json);
            }
        }
        catch (Exception e) {//Incase there is no Json File
            Debug.LogError(e.Message);
            classType = null;
        }

        return classType;
    }
}