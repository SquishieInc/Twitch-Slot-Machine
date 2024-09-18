using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Net.Sockets;
using System.IO;

public class TwitchConnect : MonoBehaviour
{
    public UnityEvent<string, string> OnChatMessage;
    TcpClient Twitch;
    StreamReader Reader;
    StreamWriter Writer;

    const string URL = "irc.chat.twitch.tv";
    const int PORT = 6667;

    string user = "SquishieInc";
    //Get OAuth = https://twitchapps.com/tmi
    string OAuth = "oauth:6jk8mtlq9hnarocobk4jwxxccqe5bt";
    string Channel = "SquishieInc";

    float PingCounter = 0;

    // Start is called before the first frame update
    void ConnectToTwitch()
    {
        Twitch = new TcpClient(URL, PORT);
        Reader = new StreamReader(Twitch.GetStream());
        Writer = new StreamWriter(Twitch.GetStream());

        Writer.WriteLine("PASS " + OAuth);
        Writer.WriteLine("NICK " + user.ToLower());
        Writer.WriteLine("JOIN #" + Channel.ToLower());
        Writer.Flush();
    }

    private void Awake()
    {
        ConnectToTwitch();
    }

    // Update is called once per frame
    void Update()
    {
        PingCounter += Time.deltaTime;
        if(PingCounter >60)
        {
            Writer.WriteLine("PING " + URL);
            Writer.Flush();
        }
        if (!Twitch.Connected)
        {
            ConnectToTwitch();
        }
        if(Twitch.Available > 0)
        {
            string message = Reader.ReadLine();

            if(message.Contains("PRIVMSG"))
            {
                int splitPoint = message.IndexOf("!");
                string chatter = message.Substring(1, splitPoint - 1);

                splitPoint = message.IndexOf(":", 1);
                string msg = message.Substring(splitPoint + 1);

                OnChatMessage?.Invoke(chatter, msg);
            }
            print(message);
        }
    }
}
