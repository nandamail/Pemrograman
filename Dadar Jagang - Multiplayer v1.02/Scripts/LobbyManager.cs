using UnityEngine;
using UnityEngine.UI;
using TMPro; // Untuk Input Field TextMeshPro
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class LobbyManager : MonoBehaviour
{
    public Button hostButton;
    public Button clientButton;
    public TMP_InputField ipInputField; // Tambahkan Input Field untuk IP

    private void Start()
    {
        hostButton.onClick.AddListener(OnHostButtonClicked);
        clientButton.onClick.AddListener(OnClientButtonClicked);
    }

    private void OnHostButtonClicked()
    {
        GameManager.instance.StartHost(); // Mulai sebagai Host
    }

    private void OnClientButtonClicked()
    {
        string inputIP = ipInputField.text; // Ambil teks dari Input Field
        if (!string.IsNullOrEmpty(inputIP))
        {
            // Atur IP untuk Unity Transport
            UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            transport.ConnectionData.Address = inputIP;
            Debug.Log($"IP address set to: {inputIP}");
        }
        else
        {
            Debug.LogWarning("IP address is empty. Using default.");
        }

        GameManager.instance.StartClient(); // Mulai sebagai Klien
    }
}
