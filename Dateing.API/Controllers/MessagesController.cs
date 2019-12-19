using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Dateing.API.Data;
using Dateing.API.DTO;
using Dateing.API.Helpers;
using Dateing.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dateing.API.Controllers
{

    [ServiceFilter(typeof(LogUserActivity))]
    [ApiController]
    [Route("user/{userId}/[controller]")]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        public MessagesController(IDatingRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }
        [HttpGet("{id}", Name = "GetMessage")]
        public async Task<IActionResult> GetMessage(long userId, long id)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var messageFromRepo = await _repo.GetMessage(id);
            if (messageFromRepo == null)
                return NotFound();
            return Ok(messageFromRepo);
        }

        [HttpGet]
        public async Task<IActionResult> GetMessages(int userId, [FromQuery]MessagesParam param)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            param.UserId = userId;
            var messagesFromRepo = await _repo.GetMessagesForUser(param);

            var messages = _mapper.Map<IEnumerable<MessageToReturn>>(messagesFromRepo);
            Response.AddPagination(messagesFromRepo.CurrentPage, param.PageSize, messagesFromRepo.TotalCount, messagesFromRepo.TotalPage);
            return Ok(messages);
        }

        [HttpGet("/thread/{recipientId}")]
        public async Task<IActionResult> GetThreadMessage(long userId, long recipientId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            var messagesFromRepo = await _repo.GetMessageThread(userId, recipientId);
            var messages = _mapper.Map<IEnumerable<MessageToReturn>>(messagesFromRepo);
            return Ok(messages);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMessage(long userId, CreateMessageModel message)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            message.SenderId = userId;
            var recipient = await _repo.GetUser(message.RecipientId);
            if (recipient == null)
                return NotFound();
            var messageEntity = _mapper.Map<Message>(message);

            _repo.Add<Message>(messageEntity);
            if (await _repo.SaveAll())
            {
                var returnToUser = _mapper.Map<CreateMessageModel>(messageEntity);
                return CreatedAtRoute("GetMessage", new { userId, id = messageEntity.Id }, returnToUser);
            }

            throw new Exception("Create message failed on save.");
        }
    }
}