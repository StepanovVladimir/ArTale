using Assets.Scripts.ProppMorphology;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateScenes : MonoBehaviour
{
    CharacterFunctionsSequenceGenerator generator = new CharacterFunctionsSequenceGenerator();

    public void GenerateScenesHandler()
    {
        var characterFunctions = generator.Generate();
        characterFunctions.ForEach(f => Debug.Log($"{f.Name}\nМесто: {f.Place}"));
    }
}
