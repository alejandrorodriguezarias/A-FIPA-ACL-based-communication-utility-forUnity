using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Message{

	protected bool ANSWER = true;
	protected bool FIRSTMESSAGE = false;

	protected string performative;
	protected string content; 
	protected string sender;
	protected string receiver;
    private string conversationId;


	public Message(string sender, string receiver, string conversationId){
		if (sender ==null){
			throw new Exception("Sender parameter can´t be null"); 
		}
		this.sender = sender;
		if (receiver ==null){
			throw new Exception("Receiver parameter can´t be null"); 
		}	
		this.receiver = receiver;
        this.conversationId = conversationId;
			
	}
    //Getters
	public string GetPerformative() {
		return this.performative;
	}

	public string GetContent() {
		return this.content;
	}
	public string GetSender() {
		return this.sender;
	}
	public string GetReceiver() {
		return this.receiver;
	}
    public bool IsAnswer()
    {
        return conversationId.Equals("-1") ? false : true;
    }
    public string GetConversationId()
    {
        return conversationId;
    }
    //Setters
    public void SetId(string id){
        this.conversationId = id;
    }
}

public class Inform : Message{

	public Inform (string content,string sender, string receiver, string id):base(sender,receiver,id){
		if (content ==null){
			throw new Exception("Content parameter can´t be null"); 
		}
        string[] contentDiv = content.Split('\\');
		if (contentDiv.Length < 2){ 
			throw new Exception("Invalid Format"); 
		}
		performative = "inform";
		this.content = content;
	}
	
}


public class Query: Message{

	public Query (string content,string sender, string receiver, string id):base(sender,receiver,id){
		if (content ==null){
			throw new Exception("Content parameter can´t be null"); 
		}
		performative = "query";
		this.content = content;
	}
	
}

public class Request : Message{

	public Request(string content,string sender, string receiver, string id):base(sender,receiver,id){
		if (content ==null){
			throw new Exception("Formato invalido"); 
		}
		performative = "request";
		this.content = content;
	}
	
}

public class Accept: Message{

	public Accept (string sender, string receiver, string id):base(sender,receiver, id){
		performative = "accept";
        content = null;
	}
	
}

public class Refuse : Message{

    public Refuse(string sender, string receiver, string id) : base(sender, receiver,  id)
    {
        content = null;
        performative = "refuse";
    }

}