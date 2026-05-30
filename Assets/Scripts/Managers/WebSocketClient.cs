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
        try
        {
            await _socket.ConnectAsync(new Uri("ws://localhost:5000"), CancellationToken.None);

            byte[] buffer = new byte[1024];
            while (_socket.State == WebSocketState.Open)
            {
                var result = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                string message = Encoding.UTF8.GetString(buffer, 0, result.Count).Trim();
                Debug.Log($"[WebSocket] {message}");
                HandleMessage(message);
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[WebSocket] Disconnected: {e.Message}");
        }
    }

    private void HandleMessage(string message)
    {
        // Expected format from gesture server: "ATTACK:<damage>" e.g. "ATTACK:15"
        if (message.StartsWith("ATTACK:", StringComparison.OrdinalIgnoreCase))
        {
            if (int.TryParse(message.Substring(7), out int damage))
            {
                // Marshal back to main thread
                UnityMainThreadDispatcher.Enqueue(() =>
                    BattleManager.Instance?.PlayerAttack(damage));
            }
        }
    }
}
