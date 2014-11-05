using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Web;
using SignalR.Reactive;

namespace Itera.RxDemo.Models
{
    public class Message
    {
        public string From { get; set; }
        public string Text { get; set; }
    }
    public class ChatHub : Hub
    {
        private static Dictionary<string, Subject<Message>> rooms = new Dictionary<string, Subject<Message>>();
        private static Dictionary<string, Dictionary<string, string>> users = new Dictionary<string, Dictionary<string, string>>();
        public void Join(string room, string name)
        {
            if (!rooms.ContainsKey(room))
            {
                var obs = new Subject<Message>();
                obs.ToClientside().Observable<ChatHub>("Message");
                rooms.Add(room, obs);
            }
            if (!users.ContainsKey(room))
            {
                users.Add(room, new Dictionary<string,string>());
            }
            if (!users[room].ContainsKey(Context.ConnectionId))
            {
                users[room].Add(Context.ConnectionId, name);
            }
        }
        public void Send(string room, string text)
        {
            rooms[room].OnNext(new Message{
                From = users[room][Context.ConnectionId],
                Text = text
            });
        }
    }
}