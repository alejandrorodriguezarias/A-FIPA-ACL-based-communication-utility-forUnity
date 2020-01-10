using System;
using System.Collections.Generic;
using UnityEngine;
//Store information about an active conversation
public class Conversation{

	private string validAnswers;
	private Message Message;
	private MASAgent agent;
    private string id;

    public Conversation( Message Message, string validAnswers)
    {
		this.validAnswers = validAnswers;
		this.Message = Message;
        id = DateTime.Now.TimeOfDay.ToString();
	}	


	public string GetReceiver(){
		return Message.GetReceiver();
	}
	public string GetValidAnswers(){
		return validAnswers;
	}

    public Message GetMessage() {
        return Message;
    }
    public string GetId(){

        return id;
    }
}
