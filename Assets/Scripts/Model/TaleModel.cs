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
            ScriptScenes script = ReadScript();
            Unserialize(tale, script);
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
            for (int i = 0; i < _taleManager.SceneNames.Count; i++)
            {
                script.scenes.Add(new ScriptScene
                {
                    sceneId = i + 1,
                    title = _taleManager.SceneNames[i],
                    script = new List<ScriptPart> { new ScriptPart { text = _taleManager.SceneScripts[i] } }
                });
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

        private void Unserialize(Tale tale, ScriptScenes script)
        {
            string pathTaleRoot = Utils.PathSaves + TaleName + "/";
            string pathModels = pathTaleRoot + "Models/";

            _taleManager.ClearTale();

            foreach (Scene scene in tale.scenes)
            {
                ButtonScene bs = _taleManager.CreateScene(scene.name);
                bs.SceneId = scene.id;
                bs.gameObject.transform.position = scene.btnPosition;
                foreach (Obj obj in scene.objs)
                {
                    GameObject objModel = UnserializeObj(obj, pathModels);
                    objModel.transform.SetParent(bs.Scene.transform);
                }

                _taleManager.SceneNames.Add(script.scenes[scene.id - 1].title);
                _taleManager.SceneScripts.Add(script.scenes[scene.id - 1].script[0].text);
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
        }

        public string Download(string link)
        {
            string[] parts = link.Split(new char[] { '/' });
            string taleNameZip = parts[parts.Length - 1];
            string taleName = taleNameZip.Split(new char[] { '.' })[0];
            string pathTaleZip = Utils.PathSaves + taleNameZip;
            WebClient Client = new WebClient();
            if (File.Exists(pathTaleZip))
            {
                File.Delete(pathTaleZip);
            }
            Client.DownloadFile(link, pathTaleZip);

            string pathTale = Utils.PathSaves + taleName + "/";
            if (Directory.Exists(pathTale))
            {
                Directory.Delete(pathTale, true);
            }
            ZipFile.ExtractToDirectory(pathTaleZip, pathTale);

            return taleName;
        }

        public ShareResponse Share()
        {
            Utils.DisableSSL();
            using (WebClient client = new WebClient())
            {
                string filepath = ZipTale();

                byte[] responseB = client.UploadFile(Utils.UploadUrl + "?taleName=" + TaleName, filepath);
                string response = Encoding.Default.GetString(responseB);
                Debug.Log(response);

                return JsonUtility.FromJson<ShareResponse>(response);
            }
        }

        private string ZipTale()
        {
            string pathTale = Utils.PathSaves + TaleName + "/";
            string pathTaleZip = Utils.PathSaves + TaleName + ".zip";
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
            Debug.Log("YA TUTA");
            if (drawerPreview.StandartObjectsDict.ContainsKey(obj.modelFilename))
            {
                Debug.Log("YES");
                objModel = _taleManager.InstantiateObj(drawerPreview.StandartObjectsDict[obj.modelFilename].gameObject);
            }
            else
            {
                Debug.Log("NO");
                objModel = CreateObjFromFile(pathModels + obj.modelFilename);
            }

            objModel.transform.position = obj.position;
            objModel.transform.rotation = obj.rotation;
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
                obj.position = _obj.transform.position;
                obj.rotation = _obj.transform.rotation;
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

        public ScriptScenes LoadScript()
        {
            string pathTaleRoot = Utils.PathSaves + TaleName + "/";
            string pathTale = pathTaleRoot + "script.json";
            string json = File.ReadAllText(pathTale);
            return JsonUtility.FromJson<ScriptScenes>(json);
        }

        public void ShowSceneById(int id)
        {
            _taleManager.ShowSceneById(id);
        }
    }
}