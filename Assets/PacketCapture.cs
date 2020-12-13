using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Newtonsoft.Json;
using ShellFileDialogs;

namespace uOSC
{

public class PacketCapture : MonoBehaviour
{
    public Text text;
    [SerializeField]
    int port = 39539;

#if NETFX_CORE
    Udp udp_ = new Uwp.Udp();
    Thread thread_ = new Uwp.Thread();
#else
    Udp udp_ = new DotNet.Udp();
    Thread thread_ = new DotNet.Thread();
#endif

    public class DataReceiveEvent : UnityEvent<Message> {};
    public DataReceiveEvent onDataReceived { get; private set; }
    
    class ReceivedMessage {
        public byte[] buffer;
        public double time;
    }
    
    List<ReceivedMessage> messages = new List<ReceivedMessage>();
    bool received = false;
    double baseTime = 0f;
    long count = 0;
    bool isOn = false;

    void Awake()
    {
        onDataReceived = new DataReceiveEvent();
    }

    void OnEnable()
    {
        if (isOn)
            OnDisable();
        isOn = true;
        messages.Clear();
        count = 0;
        received = false;
        udp_.StartServer(port);
        thread_.Start(UpdateMessage);
    }

    void OnDisable()
    {
        thread_.Stop();
        udp_.Stop();
        isOn = false;
    }

    void UpdateMessage()
    {
        while (udp_.messageCount > 0) 
        {
            if (!received) {
                received = true;
                baseTime = MonotonicTimestamp.Now().Seconds();
            }
            double now = MonotonicTimestamp.Now().Seconds();
            var buf = udp_.Receive();
            var message = new ReceivedMessage();
            message.buffer = buf;
            message.time = now - baseTime;
            messages.Add(message);
            count++;
        }
    }
    
    void Update() {
        text.text = "Packets: " + count.ToString();
    }
    
    void OnDestroy() {
        OnDisable();
    }
    
    public void Restart() {
        OnDisable();
        OnEnable();
    }
    
    public void Save() {
        OnDisable();
        Filter[] filters = new ShellFileDialogs.Filter[] { new ShellFileDialogs.Filter("JSON", "json"), new ShellFileDialogs.Filter("All files", "*") };
        string selection = FileSaveDialog.ShowDialog( System.IntPtr.Zero, "Save VMC protocol dump", initialDirectory: Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), defaultFileName: "packets.json", filters: filters, selectedFilterZeroBasedIndex: 0 );
        if (selection == null || selection == "") {
            OnEnable();
            return;
        }
        string json = JsonConvert.SerializeObject(messages, Formatting.Indented);
        File.WriteAllText(selection, json);
        OnEnable();
    }
}

}