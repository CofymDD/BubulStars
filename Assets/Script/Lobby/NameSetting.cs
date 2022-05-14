using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class NameSetting : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField nameField;

    public override void OnConnectedToMaster()
    {
        LoadNickName();
    }
    public void ChangeName()
    {
        PlayerPrefs.SetString("NickName", nameField.text);
        LoadNickName();
    }
    private void LoadNickName()
    {
        string playerName = PlayerPrefs.GetString("NickName");
        if(string.IsNullOrEmpty(playerName))
        {
            playerName = "Player" + Random.Range(0, 10000);
        }
        PhotonNetwork.NickName = playerName;
        nameField.text = playerName;
    }

}
