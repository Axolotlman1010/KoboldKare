using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Localization;

[System.Serializable]
public class KoboldCreateObjective : DragonMailObjective {
    [SerializeField]
    private int maxKobolds = 1;

    [SerializeField]
    private LocalizedString kobold;
    private int kobolds = 0;
    public override void Register() {
        FarmSpawnEventHandler.AddListener(OnEntitySpawn);
    }

    public override void Unregister() {
        FarmSpawnEventHandler.RemoveListener(OnEntitySpawn);
    }

    private void OnEntitySpawn(GameObject obj) {
        if (obj.GetComponentInChildren<Kobold>() != null) {
            kobolds++;
        }
        if (kobolds >= maxKobolds) {
            TriggerComplete();
        }
    }

    public override string GetTextBody() {
        return $"{kobold.GetLocalizedString()} {kobolds.ToString()}/{maxKobolds.ToString()}";
    }

    public override void Save(BinaryWriter writer, string version) {
        writer.Write(kobolds);
    }

    public override void Load(BinaryReader reader, string version) {
        kobolds = reader.ReadInt32();
    }

    public override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.IsWriting) {
            stream.SendNext(kobolds);
        } else {
            kobolds = (int)stream.ReceiveNext();
        }
    }
}
