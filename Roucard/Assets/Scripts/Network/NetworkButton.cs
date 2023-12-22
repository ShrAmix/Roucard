using UnityEngine;
using Unity.Netcode;
using TMPro;
using System.Net;
using System.Net.Sockets;
using Unity.Networking.Transport;
using System.Net.NetworkInformation;

public class NetworkButton : MonoBehaviour
{
    public GameObject gameObject;
    public TMP_Text ipAddressText; // ������ �� ��������� TMP ��� ����������� IP-������
    public TMP_InputField ipAddressInput; // ������ �� ��������� InputField ��� ����� IP-������

    private void Start()
    {
        // ��������� ����� � IP-������� ��� �������
        UpdateIPAddressText(GetIPAddress());
    }

    public void Host()
    {
        NetworkManager.Singleton.StartHost();
        BlockPanel();
    }

    public void Server()
    {
        NetworkManager.Singleton.StartServer();
        UpdateIPAddressText(GetIPAddress());
        BlockPanel();
    }

    public void Client()
    {
        string ipAddress = ipAddressInput.text;
        byte[] ipAddressBytes = System.Text.Encoding.ASCII.GetBytes(ipAddress);
        NetworkManager.Singleton.NetworkConfig.ConnectionData = ipAddressBytes;
        NetworkManager.Singleton.StartClient();
        BlockPanel();
    }

    // ����� ��� ���������� ������ IP-������ � TMP
    private void UpdateIPAddressText(string ipAddress)
    {
        ipAddressText.text = "IP: " + ipAddress;
    }

    private string GetIPAddress()
    {
        string ipAddress = "";
        try
        {
            // �������� ��� ������� ���������� ����������
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var networkInterface in networkInterfaces)
            {
                if (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                    networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    // �������� IP-������ ��� �����������, ������� ���������� � ����
                    UnicastIPAddressInformationCollection ipInfo = networkInterface.GetIPProperties().UnicastAddresses;

                    foreach (var ip in ipInfo)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            // �������� ������ IPv4-�����, ������� ������
                            ipAddress = ip.Address.ToString();
                            return ipAddress;
                        }
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error getting IP address: " + ex.Message);
        }
        return ipAddress;
    }

    private void BlockPanel()
    {
        gameObject.SetActive(false);
    }
}
