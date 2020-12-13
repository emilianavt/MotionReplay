using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using ShellFileDialogs;

namespace uOSC
{

public class PacketReplay : MonoBehaviour
{
    public Text text;
    private const int BufferSize = 8192;

    [SerializeField]
    string address = "127.0.0.1";

    [SerializeField]
    int port = 39539;

#if NETFX_CORE
    Udp udp_ = new Uwp.Udp();
    Thread thread_ = new Uwp.Thread();
#else
    Udp udp_ = new DotNet.Udp();
    Thread thread_ = new DotNet.Thread();
#endif
    BlockingCollection<object> messages_ = new BlockingCollection<object>(new ConcurrentQueue<object>());
    object lockObject_ = new object();
    
    private long count = 0;
    
    string filename = "";
    
    class ReceivedMessage {
        public byte[] buffer;
        public double time;
    }

    public void Load()
    {
        Filter[] filters = new ShellFileDialogs.Filter[] { new ShellFileDialogs.Filter("JSON", "json"), new ShellFileDialogs.Filter("All files", "*") };
        filename = FileOpenDialog.ShowSingleSelectDialog( System.IntPtr.Zero, "Load VMC protocol dump", initialDirectory: Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), defaultFileName: "packets.json", filters: filters, selectedFilterZeroBasedIndex: 0 );
        thread_.Start(UpdateSend);
    }
    
    public void Cancel()
    {
        OnDestroy();
    }

    void UpdateSend()
    {
        if (filename == null || filename == "")
            return;
        string json = File.ReadAllText(filename);
        List<ReceivedMessage> messages = JsonConvert.DeserializeObject<List<ReceivedMessage>>(json);
        udp_.StartClient(address, port);
        count = 0;
        double start = MonotonicTimestamp.Now().Seconds();
        foreach (var message in messages) {
            double now;
            do {
                now = MonotonicTimestamp.Now().Seconds() - start;
            } while (now < message.time);
            /*if (now < message.time)
                System.Threading.Thread.Sleep(1000 * (int)(message.time - now));*/
            udp_.Send(message.buffer, message.buffer.Length);
            count++;
        }
        udp_.Stop();
        thread_.Stop();
    }
    
    void Update() {
        text.text = "Packets: " + count.ToString();
    }
    
    void OnDestroy() {
        thread_.Stop();
        udp_.Stop();
    }
    
    void OnDisable() {
        OnDestroy();
    }
}

}