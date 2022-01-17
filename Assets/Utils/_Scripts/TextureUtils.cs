using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DarkFrontier.Utils
{
#if UNITY_EDITOR
    public class TextureUtils : EditorWindow
    {
        [MenuItem("Utils/Texture")]
        public static void OpenWindow()
        {
            var window = GetWindow<TextureUtils>();
            window.titleContent = new GUIContent("Texture Utils");
        }

        private TextField _prefix;
        private Toggle _mode;
        private ObjectField _albedo;
        private ObjectField _normal;
        private ObjectField _specular;
        private ObjectField _metallic;
        private ObjectField _emission;
        private ObjectField _occlusion;
        private ObjectField _hull;
        private ObjectField _icon;
        private Button _standardize;
        
        private void OnEnable()
        {
            var box = new Box();
            rootVisualElement.Add(box);

            box.Add(_prefix = new TextField("Prefix"));
            box.Add(_mode = EditorUtils.Toggle("Specular", true));
            box.Add(_albedo = EditorUtils.Texture2DField("Albedo"));
            box.Add(_normal = EditorUtils.Texture2DField("Normal"));
            box.Add(_specular = EditorUtils.Texture2DField("Specular"));
            box.Add(_metallic = EditorUtils.Texture2DField("Metallic"));
            box.Add(_emission = EditorUtils.Texture2DField("Emission"));
            box.Add(_occlusion = EditorUtils.Texture2DField("Occlusion"));
            box.Add(_hull = EditorUtils.Texture2DField("Hull"));
            box.Add(_icon = EditorUtils.Texture2DField("Icon"));
            box.Add(_standardize = EditorUtils.Button("Standardize", Standardize));
        }

        private void Update()
        {
            _specular.SetEnabled(_mode.value);
            _metallic.SetEnabled(!_mode.value);
        }

        private void Standardize()
        {
            var rename = !(string.IsNullOrEmpty(_prefix.value) || string.IsNullOrWhiteSpace(_prefix.value));
            
            if(_albedo.value != null)
            {
                if(rename)
                {
                    Rename(_albedo.value, $"{_prefix.value} Albedo");
                }
                Compress(Importer(_albedo.value), TextureImporterCompression.CompressedHQ, 100);
            }

            if(_normal.value != null)
            {
                if(rename)
                {
                    Rename(_normal.value, $"{_prefix.value} Normal");
                }
                Compress(Importer(_normal.value), TextureImporterCompression.CompressedHQ);
            }

            if(_mode.value)
            {
                if(_specular.value != null)
                {
                    if(rename)
                    {
                        Rename(_specular.value, $"{_prefix.value} Specular");
                    }
                    Compress(Importer(_specular.value), TextureImporterCompression.CompressedLQ, 0);
                }
            }
            else
            {
                if(_metallic.value != null)
                {
                    if(rename)
                    {
                        Rename(_metallic.value, $"{_prefix.value} Metallic");
                    }
                    
                    Compress(Importer(_metallic.value), TextureImporterCompression.CompressedLQ, 0);
                }
            }

            if(_emission.value != null)
            {
                if(rename)
                {
                    Rename(_emission.value, $"{_prefix.value} Emission");
                }
                Compress(Importer(_emission.value), TextureImporterCompression.Compressed, 50);
            }

            if(_occlusion.value != null)
            {
                if(rename)
                {
                    Rename(_occlusion.value, $"{_prefix.value} Occlusion");
                }
                Compress(Importer(_occlusion.value), TextureImporterCompression.CompressedLQ, 0);
            }

            if(_hull.value != null)
            {
                if(rename)
                {
                    Rename(_hull.value, $"{_prefix.value} Hull");
                }
                Compress(Importer(_hull.value), 256, TextureImporterCompression.CompressedLQ, 0);
            }
            
            if(_icon.value != null)
            {
                if(rename)
                {
                    Rename(_icon.value, $"{_prefix.value} Icon");
                }
                Compress(Importer(_icon.value), 256, TextureImporterCompression.CompressedLQ, 0);
            }
        }

        public static string Path(Object obj)
        {
            return AssetDatabase.GetAssetPath(obj);
        }

        public static void Rename(Object obj, string name)
        {
            AssetDatabase.RenameAsset(Path(obj), name);
        }
        
        public static TextureImporter Importer(Object texture)
        {
            return (TextureImporter)AssetImporter.GetAtPath(Path(texture));
        }

        public static void Compress(TextureImporter importer, TextureImporterCompression compression)
        {
            Compress(importer, 2048, compression);
        }

        public static void Compress(TextureImporter importer, short size, TextureImporterCompression compression)
        {
            importer.maxTextureSize = size;
            importer.textureCompression = compression;
            importer.crunchedCompression = false;
            SaveSettings(importer);
        }

        public static void Compress(TextureImporter importer, TextureImporterCompression compression, byte crunch)
        {
            Compress(importer, 2048, compression, crunch);
        }
        
        public static void Compress(TextureImporter importer, short size, TextureImporterCompression compression, byte crunch)
        {
            importer.maxTextureSize = size;
            importer.textureCompression = compression;
            importer.crunchedCompression = true;
            importer.compressionQuality = crunch;
            SaveSettings(importer);
        }

        public static void SaveSettings(TextureImporter importer)
        {
            EditorUtility.SetDirty(importer);
            importer.SaveAndReimport();
        }
    }
#endif
}