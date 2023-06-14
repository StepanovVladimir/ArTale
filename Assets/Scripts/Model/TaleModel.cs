using Assets.Scripts.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Siccity.GLTFUtility;
using UnityEngine.UI;
using System.IO.Compression;
using System.Net;

namespace Assets.Scripts
{
    class TaleModel
    {
        public ScriptScenes Script { get; set; }

        private TaleManager _taleManager;

        private string TaleNameId = Guid.NewGuid().ToString("N");

        public TaleModel(TaleManager taleManager)
        {
            _taleManager = taleManager;
        }

        public string TaleName
        {
            get
            {
                return _taleManager.TaleName;
            }

            set
            {
                _taleManager.TaleName = value;
            }
        }

        public bool IsViewMode
        {
            get
            {
                return _taleManager.IsViewMode;
            }

            set
            {
                _taleManager.IsViewMode = value;
            }
        }
        public List<string> LoadTaleList()
        {
            List<string> talesNames = new List<string>();
            var paths = Directory.GetDirectories(Utils.PathSaves);
            foreach (string path in paths)
            {
                talesNames.Add(Path.GetFileName(path));
            }
            return talesNames;
        }

        public void Create()
        {
            _taleManager.ClearTale();
            _taleManager.BtnAddOnClick();
            _taleManager.UpdateVisibleScenes();
        }

        public void Load()
        {
            Tale tale = ReadFile();
            Script = ReadScript();
            Unserialize(tale);
        }

        public void Save()
        {
            Tale tale = Serialize();
            ScriptScenes script = GetScript();
            WriteFile(tale, script);
        }

        public void Delete()
        {
            Directory.Delete(Utils.PathSaves + TaleName, true);
            _taleManager.ClearTale();
        }

        private Tale ReadFile()
        {
            string pathTaleRoot = Utils.PathSaves + TaleName + "/";
            string pathTale = pathTaleRoot + "tale.json";
            string json = File.ReadAllText(pathTale);
            return JsonUtility.FromJson<Tale>(json);
        }

        private ScriptScenes ReadScript()
        {
            string pathTaleRoot = Utils.PathSaves + TaleName + "/";
            string pathScript = pathTaleRoot + "script.json";
            string json = File.ReadAllText(pathScript);
            return JsonUtility.FromJson<ScriptScenes>(json);
        }

        private ScriptScenes GetScript()
        {
            ScriptScenes script = new ScriptScenes { scenes = new List<ScriptScene>() };
            script.wholeText = _taleManager.WholeTextInputField.text;
            for (int i = 0; i < _taleManager.SceneNames.Count; i++)
            {
                var scriptScene = new ScriptScene
                {
                    sceneId = i + 1,
                    title = _taleManager.SceneNames[i],
                    script = new List<ScriptPart> { new ScriptPart { text = _taleManager.SceneScripts[i] } }
                };

                if (_taleManager.SceneDescriptions.Count > 0)
                {
                    scriptScene.sceneDescription = _taleManager.SceneDescriptions[i];
                }

                script.scenes.Add(scriptScene);
            }

            return script;
        }

        private void WriteFile(Tale tale, ScriptScenes script)
        {
            string pathTaleRoot = Utils.PathSaves + TaleName + "/";
            Utils.TapDirectory(pathTaleRoot);
            string pathTale = pathTaleRoot + "tale.json";
            string pathModels = pathTaleRoot + "Models/";
            Utils.TapDirectory(pathModels);
            string json = JsonUtility.ToJson(tale, true);
            Debug.Log(pathTale);
            File.WriteAllText(pathTale, json);

            string pathScript = pathTaleRoot + "script.json";
            json = JsonUtility.ToJson(script, true);
            File.WriteAllText(pathScript, json);
        }

        public long ConvertToUnixTime(DateTime datetime)
        {
            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)(datetime - sTime).TotalSeconds;
        }

        public DateTime UnixTimeToDateTime(long unixtime)
        {
            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return sTime.AddSeconds(unixtime);
        }

        private void Unserialize(Tale tale)
        {
            string pathTaleRoot = Utils.PathSaves + TaleName + "/";
            string pathModels = pathTaleRoot + "Models/";

            _taleManager.ClearTale();

            if (Script.wholeText != null)
            {
                _taleManager.WholeTextInputField.text = Script.wholeText;
            }

            foreach (Scene scene in tale.scenes)
            {
                ButtonScene bs = _taleManager.CreateScene(scene.name);
                bs.Scene.SetActive(false);
                bs.SceneId = scene.id;
                bs.gameObject.transform.position = scene.btnPosition;
                foreach (Obj obj in scene.objs)
                {
                    GameObject objModel = UnserializeObj(obj, pathModels);
                    objModel.transform.SetParent(bs.Scene.transform);
                }

                _taleManager.SceneNames.Add(Script.scenes[scene.id - 1].title);
                _taleManager.SceneScripts.Add(Script.scenes[scene.id - 1].script[0].text);
                if (Script.scenes[scene.id - 1].sceneDescription != null && !Script.scenes[scene.id - 1].sceneDescription.Equals(""))
                {
                    _taleManager.SceneDescriptions.Add(Script.scenes[scene.id - 1].sceneDescription);
                }
            }
            _taleManager.UpdateVisibleScenes();
            LoadModels(pathModels);
            // set links
            _taleManager.Links = new Dictionary<int, List<int>>();
            foreach (string kv in tale.Links)
            {
                string[] ab = kv.Split(new char[] { ':' });
                List<int> b = ab[1].Split(new char[] { ',' }).Select(x => Convert.ToInt32(x)).ToList();
                _taleManager.Links.Add(Convert.ToInt32(ab[0]), b);
            }
            _taleManager.RenderLinks();

            if (_taleManager.SceneDescriptions.Count > 0)
            {
                _taleManager.ActivateBtnGenerateText();
            }

            if (!_taleManager.WholeTextInputField.text.Equals(""))
            {
                _taleManager.ActivateBtnWholeText();
                _taleManager.SetOnWhichSceneMoveText();
            }
        }

        public string Download(string link)
        {
            string[] parts = link.Split(new char[] { '/' });
            string taleNameZip = parts[parts.Length - 1];
            //string taleName = taleNameZip.Split(new char[] { '.' })[0];
            string pathTaleZip = Utils.PathSaves + taleNameZip;
            WebClient Client = new WebClient();
            if (File.Exists(pathTaleZip))
            {
                File.Delete(pathTaleZip);
            }
            Client.DownloadFile(link, pathTaleZip);

            ZipArchive archive = ZipFile.OpenRead(pathTaleZip);
            ZipArchiveEntry entry = archive.GetEntry("tale.json");
            StreamReader reader = new StreamReader(entry.Open());
            string json = reader.ReadToEnd();
            Tale tale = JsonUtility.FromJson<Tale>(json);

            string pathTale = Utils.PathSaves + tale.TaleName + "/";
            if (Directory.Exists(pathTale))
            {
                Directory.Delete(pathTale, true);
            }
            ZipFile.ExtractToDirectory(pathTaleZip, pathTale);

            return tale.TaleName;
        }

        public ShareResponse Share()
        {
            Utils.DisableSSL();
            using (WebClient client = new WebClient())
            {
                string filepath = ZipTale(TaleName, TaleNameId);

                byte[] responseB = client.UploadFile(Utils.UploadUrl + "?taleName=" + TaleNameId, filepath);
                string response = Encoding.Default.GetString(responseB);
                Debug.Log(response);

                TaleNameId = Guid.NewGuid().ToString("N");

                return JsonUtility.FromJson<ShareResponse>(response);
            }
        }

        private string ZipTale(string taleName, string taleNameId)
        {
            string pathTale = Utils.PathSaves + taleName + "/";
            string pathTaleZip = Utils.PathSaves + taleNameId + ".zip";
            if (File.Exists(pathTaleZip))
            {
                File.Delete(pathTaleZip);
            }
            ZipFile.CreateFromDirectory(pathTale, pathTaleZip);
            return pathTaleZip;
        }

        public void LoadModels(string modelDir)
        {
            DrawPreviewSceneObjects drawerPreview = _taleManager.GetComponent<DrawPreviewSceneObjects>();
            drawerPreview.ClearObjectsForScene();
            var paths = Directory.GetFiles(modelDir, "*.gltf", SearchOption.TopDirectoryOnly);
            foreach (string path in paths)
            {
                try
                {
                    GameObject model = CreateObjFromFile(path);
                    model.transform.SetParent(drawerPreview.ObjectsForScene.transform);
                    if (modelDir.Equals(Utils.CalcModelsLoadPath()))
                    {
                        AddModel(path);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log("1" + " " + ex.Message + " " + ex.Source + " " + ex.StackTrace);
                }
            }
            drawerPreview.RenderObjectsPreview();
        }

        private Tale Serialize()
        {
            Tale tale = new Tale();
            tale.TaleName = TaleName;
            tale.Edited = ConvertToUnixTime(DateTime.Now);
            foreach (Transform _scene in _taleManager.ImgTarget.transform)
            {
                Scene scene = new Scene();
                scene.objs = SerializeObj(_scene);
                ButtonScene bs = _taleManager.FindButtonByScene(_scene);
                scene.name = bs.GetComponentInChildren<Text>().text;
                scene.id = bs.SceneId;
                scene.btnPosition = bs.gameObject.transform.position;
                tale.scenes.Add(scene);
            }
            tale.Links = new List<string>();
            foreach (var kv in _taleManager.Links)
            {
                List<string> ls = kv.Value.Select(x => x.ToString()).ToList();
                tale.Links.Add(kv.Key + ":" + string.Join(",", ls));
            }
            return tale;
        }

        private GameObject UnserializeObj(Obj obj, string pathModels)
        {
            GameObject objModel;
            DrawPreviewSceneObjects drawerPreview = _taleManager.GetComponent<DrawPreviewSceneObjects>();
            if (drawerPreview.StandartObjectsDict.ContainsKey(obj.modelFilename))
            {
                objModel = _taleManager.InstantiateObj(drawerPreview.StandartObjectsDict[obj.modelFilename].gameObject);
            }
            else
            {
                objModel = CreateObjFromFile(pathModels + obj.modelFilename);
            }

            objModel.transform.localPosition = obj.position;
            objModel.transform.localRotation = obj.rotation;
            objModel.transform.localScale = obj.localScale;

            return objModel;
        }

        private List<Obj> SerializeObj(Transform _scene)
        {
            List<Obj> objs = new List<Obj>();
            foreach (Transform _obj in _scene.transform)
            {
                if (_obj.GetComponent<MoveObj>() == null)
                {
                    continue;
                }

                Obj obj = new Obj();

                // common
                obj.position = _obj.transform.localPosition;
                obj.rotation = _obj.transform.localRotation;
                obj.localScale = _obj.transform.localScale;

                obj.modelFilename = _obj.GetComponent<MoveObj>().ModelFilename;

                objs.Add(obj);
            }
            return objs;
        }

        private GameObject CreateObjFromFile(string path)
        {
            GameObject model = Importer.LoadFromFile(path);
            model.AddComponent<BoxCollider>();
            model.AddComponent<MoveObj>();
            model.GetComponent<MoveObj>().ModelFilename = Path.GetFileName(path);
            return model;
        }

        private void AddModel(string path)
        {
            string pathTaleRoot = Utils.PathSaves + TaleName + "/";
            Utils.TapDirectory(pathTaleRoot);

            string pathModels = pathTaleRoot + "Models/";
            Utils.TapDirectory(pathModels);

            string dest = pathModels + Path.GetFileName(path);
            if (File.Exists(dest))
            {
                File.Delete(dest);
            }
            File.Copy(path, dest);
        }

        public void ShowSceneById(int id)
        {
            _taleManager.ShowSceneById(id);
        }
    }
}