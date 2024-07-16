using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

public class WebSocket {
    private Uri mUrl;

    public WebSocket(Uri url) {
        mUrl = url;

        string protocol = mUrl.Scheme;
        if (!protocol.Equals("ws") && !protocol.Equals("wss"))
            throw new ArgumentException("Unsupported protocol: " + protocol);
    }

    public void SendString(string str) {
        Send(Encoding.UTF8.GetBytes(str));
    }

    public string RecvString() {
        byte[] retval = Recv();
        if (retval == null)
            return null;
        return Encoding.UTF8.GetString(retval);
    }

    WebSocketSharp.WebSocket m_Socket;
    Queue<byte[]> m_Messages = new Queue<byte[]>();
    bool m_IsConnected = false;
    string m_Error = null;

    public IEnumerator Connect() {
        m_Socket = new WebSocketSharp.WebSocket(mUrl.ToString());
        m_Socket.OnMessage += (sender, e) => m_Messages.Enqueue(e.RawData);
        m_Socket.OnOpen += (sender, e) => m_IsConnected = true;
        m_Socket.OnError += (sender, e) => m_Error = e.Message;
        m_Socket.ConnectAsync();
        while (!m_IsConnected && m_Error == null)
            yield return 0;
    }

    public void Send(byte[] buffer) {
        m_Socket.Send(buffer);
    }

    public byte[] Recv() {
        if (m_Messages.Count == 0)
            return null;
        return m_Messages.Dequeue();
    }

    public bool IsAlive() {
        return m_Socket.IsAlive;
    }

    public void Close() {
        m_Socket.Close();
    }

    public string error {
        get {
            return m_Error;
        }
    }
}