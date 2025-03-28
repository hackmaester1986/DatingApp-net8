using System;
using System.Security.Cryptography.X509Certificates;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[Authorize]
public class MessagesController(IMessageRepository messageRepository, IUserRepository userRepository,IMapper mapper) : BaseApiController
{
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteMessage(int id){
        var username = User.GetUsername();
        var message = await messageRepository.GetMessage(id);
        if(message == null) return BadRequest("Could not delete Message");

        if(message.SenderUsername != username && message.RecipientUsername != username){
            return Forbid();
        }
        if(message.SenderUsername == username){
            message.SenderDeleted = true;
        }
        if(message.RecipientUsername == username){
            message.RecipientDeleted = true;
        }

        if(message is {SenderDeleted: true, RecipientDeleted: true}){
            messageRepository.DeletedMessage(message);
        }
        if(await messageRepository.SaveAllAsync()) return Ok();

        return BadRequest("Message could not be deleted");
    }
    [HttpPost]
    public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessageDto)
    {
        var username = User.GetUsername();
        if(username == createMessageDto.RecipientUsername.ToLower()) return BadRequest("You can't send a message to yourself");

        var sender = await userRepository.GetUserByUserNameAsync(username);
        var recipient = await userRepository.GetUserByUserNameAsync(createMessageDto.RecipientUsername);

        if(recipient == null || sender == null) return BadRequest("Cannot send message");

        var message = new Message
        {
            Sender = sender,
            Recipient = recipient,
            SenderUsername = sender.UserName,
            RecipientUsername = recipient.UserName,
            Content = createMessageDto.Content
        };
        messageRepository.AddMessage(message);

        if(await messageRepository.SaveAllAsync()) return Ok(mapper.Map<MessageDto>(message));

        return BadRequest("Could not save message");

    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessagesForUser([FromQuery]MessageParams messageParams)
    {
        messageParams.UserName = User.GetUsername();

        var messages = await messageRepository.GetMessagesForUser(messageParams);

        Response.AddPaginationHeader(messages);

        return messages;
    }

    [HttpGet("thread/{username}")]
    public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string username){
        var currentUsername = User.GetUsername();

        return Ok(await messageRepository.GetMessageThread(currentUsername,username));
    }
}
