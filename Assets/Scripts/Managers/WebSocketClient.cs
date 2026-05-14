using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class WebSocketClient : MonoBehaviour
{
    private ClientWebSocket _socket = new ClientWebSocket();

    async void Start()
    {
        await _socket.ConnectAsync(new Uri("ws://localhost:5000"), CancellationToken.None);

        byte[] buffer = new byte[1024];
        while (_socket.State == WebSocketState.Open)
        {
            var result = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            Debug.Log(message);
        }
    }
}
