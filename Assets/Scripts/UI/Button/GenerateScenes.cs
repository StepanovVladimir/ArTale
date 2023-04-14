using Assets.Scripts.ProppMorphology;
using Assets.Scripts.ProppMorphology.CharacterFunctions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateScenes : MonoBehaviour
{
    public GenerateText generateText;
    CharacterFunctionsSequenceGenerator generator = new CharacterFunctionsSequenceGenerator();

    public void GenerateScenesHandler()
    {
        var characterFunctions = generator.Generate();

        Camera camera = Camera.main;
        TaleManager taleManager = camera.GetComponent<TaleManager>();
        taleManager.ClearTaleWithoutClearObjectsForScene();
        int i = 1;
        float xPosition = 250;
        float yPosition = 615;
        bool down = true;

        generateText.Message = "Напиши текст с диалогами для русской народной сказки на основе данной сюжетной линии:\n";

        characterFunctions.ForEach(f => {
            string s = $"Место: {f.Place}\nДействующий персонаж: {f.ActingCharacter}";
            if (f.AdditionalCharacter1 != null)
            {
                s += $"\nДополнительный персонаж 1: {f.AdditionalCharacter1}";
            }
            if (f.AdditionalCharacter2 != null)
            {
                s += $"\nДополнительный персонаж 2: {f.AdditionalCharacter2}";
            }

            //Debug.Log(s);

            ButtonScene bs = taleManager.CreateScene($"{i}. {f.Name}", f.SceneIndex);
            bs.SceneId = i;
            bs.gameObject.transform.position = new Vector3(xPosition, yPosition, 0);

            taleManager.SceneNames.Add(f.Name);
            taleManager.SceneScripts.Add("");
            taleManager.SceneDescriptions.Add(f.StorylineString);

            generateText.Message += $"{f.StorylineString}\n";

            if (i > 1)
            {
                taleManager.Links.Add(i - 1, new List<int> { i });
            }

            //Debug.Log($"{xPosition}\n{yPosition}");

            if (i == 1 || i % 3 != 0 && (i - 1) % 3 != 0)
            {
                xPosition += 40;
            }
            else
            {
                xPosition += 250;
            }

            if (down)
            {
                yPosition -= 150;
            }
            else
            {
                yPosition += 150;
            }

            if (i % 3 == 0)
            {
                down = !down;
            }

            i++;
        });

        taleManager.UpdateVisibleScenes();
        taleManager.RenderLinks();
        taleManager.ActivateBtnGenerateText();

        //x: 174 236 324 477 619 738
        //y: 614 463 316 184 316 466
    }
}
