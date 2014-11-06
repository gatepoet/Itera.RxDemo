using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Web;
using SignalR.Reactive;
using System.Reactive.Linq;
using System.Reactive;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace Itera.RxDemo.Models
{
    public class Message
    {
        public string From { get; set; }
        public string Text { get; set; }
    }
    public class User
    {
        public string ClientId { get; set; }
        public string Name { get; set; }
    }
    public class ChatHub : Hub
    {
        private static Dictionary<string, Subject<Message>> rooms = new Dictionary<string, Subject<Message>>();
        private static Dictionary<string, ObservableCollection<User>> users = new Dictionary<string, ObservableCollection<User>>();

        public void Join(string room, string name)
        {
            if (!rooms.ContainsKey(room))
            {
                var obs = new Subject<Message>();
                obs.ToClientside().Observable<ChatHub>(room + "-messages");
                rooms.Add(room, obs);
            }
            if (!users.ContainsKey(room))
            {
                var coll = new ObservableCollection<User>();
                Observable.FromEventPattern<NotifyCollectionChangedEventHandler,NotifyCollectionChangedEventArgs>(
                    c => coll.CollectionChanged += c,
                    c => coll.CollectionChanged -= c)

                    .Select(ch => coll.Select(u => u.Name).ToArray())
                    .ToClientside().Observable<ChatHub>(room + "-users");
                users.Add(room, coll);
            }
            var user = users[room].SingleOrDefault(u => u.ClientId == Context.ConnectionId);
            if (user != null)
            {
                users[room].Remove(user);
            }
            users[room].Add(new User
            {
                ClientId = Context.ConnectionId,
                Name = name
            });
        }
        public void Send(string room, string text)
        {
            rooms[room].OnNext(new Message{
                From = users[room].Single(u => u.ClientId == Context.ConnectionId).Name,
                Text = text
            });
        }
    }
    public static class ObservableCollectionExtensions
    {
        public static IObservable<NotifyCollectionChangedEventArgs>
            GetObservableChanges<T>(this ObservableCollection<T> collection)
        {
            return Observable.FromEvent<
                NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                    h => collection.CollectionChanged += h,
                    h => collection.CollectionChanged -= h);
        }

        public static IObservable<T> GetObservableAddedValues<T>(
            this ObservableCollection<T> collection)
        {
            return collection.GetObservableChanges()
                .Where(evnt => evnt.Action == NotifyCollectionChangedAction.Add)
                .SelectMany(evnt => evnt.NewItems.Cast<T>());
        }
    }
}