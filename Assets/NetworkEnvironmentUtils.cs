// MIT License
// 
// Copyright (c) 獏星(ばくすたー) 2019 
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Baku.VMagicMirrorConfig
{
    /// <summary> ネットワーク環境について何か情報提供してくれるユーティリティ </summary>
    static class NetworkEnvironmentUtils
    {
        private static readonly UdpClient _udpClient = new UdpClient();

        /// <summary>
        /// IPv4のローカルアドレスっぽいものを取得します。
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIpAddressAsString()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            //空ではないIPv4アドレスをてきとーに拾うだけ。これで十分じゃない場合は…多分それは人類には難しいやつ…
            return host.AddressList
                .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)
                .Select(ip => ip.ToString())
                .Where(s => !string.IsNullOrEmpty(s))
                .FirstOrDefault() 
                ?? "(unknown)";
        }

        /// <summary>
        /// iFacialMocapのデータ受信をiOS機器に対してリクエストします。
        /// 内部的には決め打ちのUDPメッセージを一発打つだけです。
        /// </summary>
        /// <param name="ipAddress">IPv4でiOS機器の端末を指定したLAN内のIPアドレス。</param>
        public static bool SendIFacialMocapDataReceiveRequest(string ipAddress)
        {
            if (IPAddress.TryParse(ipAddress, out var address))
            {
                var data = Encoding.UTF8.GetBytes("FACEMOTION3D_OtherStreaming");
                _udpClient.Send(data, data.Length, new IPEndPoint(address, 49993));
                return true;
            }
            return false;
        }
    }
}