using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;
using System.Threading;

public class GenerateText : MonoBehaviour
{
    class Response
    {
        public class Choice
        {
            public class Message
            {
                public string content;
            }

            public Message message; 
        }

        public Choice[] choices;
    }

    public InputField inputField;
    public GameObject panelWait;

    public string Message { get; set; }

    public async void GenerateTextHandler()
    {
        var http = new HttpClient();

        var apiKey = "";
        http.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        var jsonContent = new
        {
            model = "gpt-3.5-turbo",
            messages = new[] { new { role = "user", content = Message } }
        };

        panelWait.SetActive(true);
        //var responseContent = await http.PostAsync("https://api.openai.com/v1/chat/completions", new StringContent(JsonConvert.SerializeObject(jsonContent), Encoding.UTF8, "application/json"));
        //var resContext = await responseContent.Content.ReadAsStringAsync();

        //var data = JsonConvert.DeserializeObject<Response>(resContext);
        //Debug.Log(data.choices[0].message.content);
        //inputField.text = data.choices[0].message.content;

        string text = "Во дворце царь и царевна жили долго и счастливо, пока однажды царевна не отлучилась в сад. Тут же на неё напал змей и унёс её в небеса. Царь был в отчаянии и решил сообщить об этом всему царству.\n\n";
        text += "- Надо что-то делать! - воскликнул царь. - Кто поможет мне вернуть мою дочь?\n\n";
        text += "- Я помогу, - ответил Иван, который был рядом.\n\n";
        text += "- Спасибо тебе, Иван! - сказал царь. - Буду очень благодарен, если ты вернёшь мне мою дочь.\n\n";
        text += "- Я постараюсь, - ответил Иван и отправился в лес.\n\n";
        text += "Там он встретил старца, который указал ему путь к логову змея. Иван смело пошёл вперёд и вскоре оказался перед змеем. Они начали бороться, но Иван оказался сильнее и победил змея.\n\n";
        text += "- Освободи царевну! - крикнул Иван.\n\n";
        text += "Царевна была свободна, но змеиха летела в погоню за ними. Иван и царевна прятались в кузнице, где нашли помощь у кузнеца.\n\n";
        text += "- Помогите нам, пожалуйста! - просил Иван.\n\n";
        text += "- Я помогу, - ответил кузнец и дал Ивану меч.\n\n";
        text += "С помощью этого меча Иван справился со змеихой и вернулся к царю во дворец. Царь был очень рад, что его дочь вернулась, и женил Ивана на царевне.\n\n";
        text += "- Ты настоящий герой, Иван! - сказал царь. - Теперь ты мой зять и будешь жить с нами во дворце.\n\n";

        inputField.text = text.Replace("\n\n", "\n");

        Camera camera = Camera.main;
        TaleManager taleManager = camera.GetComponent<TaleManager>();
        taleManager.ActivateBtnWholeText();
        taleManager.SetOnWhichSceneMoveText();

        Thread.Sleep(10000);

        taleManager.BtnShowPanelWholeTextOnClick();
    }
}
