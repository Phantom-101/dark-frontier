using System.Collections.Generic;
using System.IO;
using DarkFrontier.Files;
using DarkFrontier.SceneManagement;
using DarkFrontier.Serialization;
using DarkFrontier.UI.States;
using UnityEngine;

namespace DarkFrontier.UI {
    public class SaveSelectionUI : UIStateView {
        [SerializeField] private Transform _content;
        [SerializeField] private GameObject _universe;
        [SerializeField] private List<UniverseInfoUI> _instUniverses;
        [SerializeField] private GameObject _save;
        [SerializeField] private List<SaveInfoUI> _instSaves;
        [SerializeField] private List<string> _expanded = new List<string> ();

        public void SaveSelected (string universe, string name) {
            SceneUtils.Instance.LoadScene ("Game");
            SaveManager.GetInstance ().Load (universe, name);
        }

        protected override void OnStateChanged () {
            // Destroy instantiated prefabs
            while (_instUniverses.Count > 0) {
                Destroy (_instUniverses[0].gameObject);
                _instUniverses.RemoveAt (0);
            }
            while (_instSaves.Count > 0) {
                Destroy (_instSaves[0].gameObject);
                _instSaves.RemoveAt (0);
            }

            if (IsShown) {
                // Instantiate new prefabs
                DirectoryInfo[] universes = PathManager.GetUniverseDirectories ();
                foreach (DirectoryInfo universe in universes) {
                    GameObject ugo = Instantiate (_universe, _content);
                    UniverseInfoUI uinfo = ugo.GetComponent<UniverseInfoUI> ();
                    uinfo.Button.onClick.AddListener (() => {
                        if (_expanded.Contains (universe.Name)) _expanded.Remove (universe.Name);
                        else _expanded.Add (universe.Name);
                        OnStateChanged ();
                    });
                    uinfo.Name = universe.Name;
                    _instUniverses.Add (uinfo);
                    if (_expanded.Contains (universe.Name)) {
                        DirectoryInfo[] saves = PathManager.GetSaveDirectories (universe);
                        foreach (DirectoryInfo save in saves) {
                            GameObject sgo = Instantiate (_save, _content);
                            SaveInfoUI sinfo = sgo.GetComponent<SaveInfoUI> ();
                            sinfo.Button.onClick.AddListener (() => {
                                UIStateManager.RemoveState ();
                                SaveSelected (universe.Name, save.Name);
                            });
                            sinfo.Name = universe.Name;
                            sinfo.Time = save.Name;
                            _instSaves.Add (sinfo);
                        }
                    }
                }
            }
        }
    }
}
