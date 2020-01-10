using System;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
public abstract class MASAgent: Agent{
    // Open conversations with different agents
    protected List<Conversation> conversations = new List<Conversation>();
    //Flags de resposta

    /// <summary>
    /// Create a new message
    /// </summary>
    /// <param name="performative">String type of the message </param>
    /// <param name="content">String message content</param>
    /// <param name="sender">String sender´s name</param>
    /// <param name="receiverName">String name of the receiving agent or "all" if is for everybody</param>
    /// <param name="id">string conversation id if it is a reply or -1 in another case</param>
    /// <returns>A message instance with the given values</returns>
    private Message CreateMesagge(string performative, string content, string sender, string receiverName, string id = "-1"){
        Message tmp = null;
        String validAnswers = null;
        bool isAnswer = id.Equals("-1") ? false : true;

        if (performative !=null){      
			switch(performative){
                case "inform":
					tmp = new Inform(content,sender,receiverName,id);
                    break;
				case "query":
					tmp =  new Query(content,sender,receiverName,id);
                    validAnswers = "inform";
                    break;
				case "request":
					tmp =  new Request(content,sender,receiverName,id);
                    validAnswers = "accept\\refuse";
                    break;
				case "accept":
					tmp = new Accept(sender,receiverName,id);
                    break;
                case "refuse":
                    tmp = new Refuse(sender, receiverName, id);
                    break;
            }
            //If the message "needs" a new conversation
            if (!isAnswer && validAnswers != null){
                //Se finalmente obtemos unha Message engadese unha conversa e enviase a id da mesma no Message
                //If we have a new message and is for another agent, we create a new conversation
                if (tmp != null && receiverName != gameObject.name) {
                    Conversation aux = new Conversation(tmp, validAnswers);
                    tmp.SetId(aux.GetId());
                    conversations.Add(aux);
                }
            }   
        }
		return tmp;
	}
    /// <summary>
    /// Crea e envia un Message a un axente
    /// </summary>
    /// <param name="performative">String type of the message </param>
    /// <param name="content">String message content</param>
    /// <param name="sender">String sender´s name</param>
    /// <param name="receiverName">String name of the receiving agent</param>
    /// <param name="receiver"> MASAgent component of the agent to whom the message is sent</param>
    /// <param name="id">string conversation id if it is a reply or -1 in another case</param>
    public void SendMessage(string performative, string content,string sender, string receiverName, MASAgent receiver, string id = "-1")
    {

        if (!sender.Equals(receiverName)) {
            Message Message = CreateMesagge(performative, content, sender, receiverName, id);
            if (Message != null){
                receiver.ReceiveMessage(Message);
            }
            else{
                throw new Exception("Message creation error");
            }
        }
		
		
	}

    /// <summary>
    /// Validate the received Message and delegate the processing of its content to the corresponding function
    /// </summary>
    /// <param name="Message">Message the received Message</param>
	public void  ReceiveMessage(Message Message){
        
        string sender = Message.GetSender();
        //He would only attend to the message if he did not send it himself and if the message was to the agent
        if (sender != gameObject.name){
            string receptor = Message.GetReceiver();
            if (receptor == gameObject.name || receptor == "all"){
                //If it is an answer, we validate that the agent is waiting for it
                if (Message.IsAnswer()){
					if (!ValidateAnswer(Message)){
						new Exception("Unexpected response");
					}
				}
                string performative = Message.GetPerformative();
                string content = Message.GetContent();
                string id = Message.GetConversationId();
                switch (performative){
					case "inform":
                        ProcessInform(content, sender, id);
						break;
					case "query":
                        ProcessQuery(content, sender, id);
						break;
					case "request":
                        ProcessRequest(content, sender, id);
						break;
                    case "accept":
                        ProcessAccept(sender, id);
                        break;
                    case "refuse":
                        ProcessRefuse(sender, id);
                        break;
                }
			}
		}
	}
    /// <summary>
    /// Auxiliary function that checks if the reply message responds to an existing conversation and matches the expected response type
    /// </summary>
    /// <param name="Message">message to be validated</param>
    /// <returns></returns>
    private bool ValidateAnswer(Message Message){
        string performative = Message.GetPerformative();
        bool valid = false;
        List<Conversation> conversationsAux = new List<Conversation>(conversations);
		foreach(Conversation conversation in conversationsAux){
            if ((conversation.GetId().Equals(Message.GetConversationId())) && (conversation.GetReceiver().Equals(Message.GetSender()))){
				string [] validAnswers = conversation.GetValidAnswers().Split('\\');
				foreach(string validAnswer in validAnswers){
					if (validAnswer.Equals(performative)){
					    valid = true;
                        conversations.Remove(conversation);
                        break;
                    }
				}
			}
		}
		return valid;
	}

    //Process the information received by a message of type inform
    abstract protected void ProcessInform(string content, string sender, string id);
    //Process the information received by a message of type query
    abstract protected void ProcessQuery(string content, string sender, string id);
    //Process the information received by a message of type request
    abstract protected void ProcessRequest(string content, string sender, string id);
    //Process the information received by a message of type accept
    abstract protected void ProcessAccept(string sender, string id);
    //Process the information received by a message of type refuse
    abstract protected void ProcessRefuse(string sender, string id);
}
