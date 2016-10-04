using System;
using WebSocketSharp;
using WebSocketSharp.Server;

public class Link : WebSocketBehavior
{
    private string _suffix;

    public Link()
      : this(null)
    {
    }

    public Link(string suffix)
    {
        _suffix = suffix ?? String.Empty;
    }

    protected override void OnMessage(MessageEventArgs e)
    {
        Sessions.Broadcast(e.Data);
    }
}