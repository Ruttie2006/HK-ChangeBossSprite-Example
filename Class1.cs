using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Modding;
using UnityEngine;
using SFCore;
using SFCore.MonoBehaviours;
using UnityEngine.SceneManagement;
using UObject = UnityEngine.Object;
using USceneManager = UnityEngine.SceneManagement.SceneManager;

namespace TransHK
{


    public class TransMod : Mod
    {
        string Bossscene = "Room_Final_Boss_Core";
        string selectedBoss = "Hollow Knight Boss";

        public TransMod() : base("HKmod") { }
        public override string GetVersion() => "1";
        private Texture2D _tex;

        public override void Initialize()
        {
            Log("Starting");
            USceneManager.activeSceneChanged += SceneChanged;

            Assembly asm = Assembly.GetExecutingAssembly();
            
            foreach (string res in asm.GetManifestResourceNames())
            {
                Log("GetManifestRescourceNames");
                using (Stream s = asm.GetManifestResourceStream(res))
                {
                    if (s == null) continue;

                    byte[] buffer = new byte[s.Length];
                    s.Read(buffer, 0, buffer.Length);
                    
                    _tex = new Texture2D(2, 2);

                    _tex.LoadImage(buffer, true);
                }
            }
        }

        private void SceneChanged(Scene arg0, Scene arg1)
        {
            if (arg1.name != Bossscene) return;
            {
                GameManager.instance.StartCoroutine(SetSprite(arg1));
                Log("Scenechanged");
            }
        }

        private IEnumerator SetSprite(Scene arg1)
        {
            Log("Setting sprites");
            var boss = arg1.Find(selectedBoss);
            boss.GetComponent<tk2dSprite>().GetCurrentSpriteDef().material.mainTexture = _tex;
            yield return null;
        }
    }

    public static class Rareclass
    {
        public static void Log(string s)
        {
            Modding.Logger.Log($"[Thing] - {s}");
        }
        public static GameObject Find(this Scene scene, string name)
        {
            Log("NewMethodCalled");
            if (scene.IsValid())
            {
                Log("SceneIsValid");
                Transform retGo;
                foreach (var go in scene.GetRootGameObjects())
                {
                    if (go.name == name)
                    {
                        Log("if (go.name == name)");
                        return go;
                    }
                    retGo = go.transform.Find(name);
                    if (retGo != null)
                    {
                        Log("if retGo != null");
                        return retGo.gameObject;
                    }
                }
            }
            return null;
        }
    }
}