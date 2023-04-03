using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.UI;

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

        var responseContent = await http.PostAsync("https://api.openai.com/v1/chat/completions", new StringContent(JsonConvert.SerializeObject(jsonContent), Encoding.UTF8, "application/json"));
        var resContext = await responseContent.Content.ReadAsStringAsync();

        var data = JsonConvert.DeserializeObject<Response>(resContext);
        Debug.Log(data.choices[0].message.content);
        inputField.text = data.choices[0].message.content;

        Camera camera = Camera.main;
        TaleManager taleManager = camera.GetComponent<TaleManager>();
        taleManager.ActivateBtnWholeText();
        taleManager.SetOnWhichSceneMoveText();
        taleManager.BtnShowPanelWholeTextOnClick();
    }
}
