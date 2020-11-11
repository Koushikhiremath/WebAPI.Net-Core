using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Commender.Data;
using Commender.DTOs;
using Commender.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Commender.Controllers
{
    [Route("api/commands")]
    [ApiController]
    public class CommandsContoller : ControllerBase
    {
        private readonly ICommenderRepo _repo;
        private readonly IMapper _mapper;

        public CommandsContoller(ICommenderRepo commenderRepo, IMapper mapper)
        {
            _repo = commenderRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetAllCommands()
        {
            var commandIteam = _repo.GetAllCommands();

            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandIteam));
        }

        [HttpGet("{id}", Name = "GetCommandByID")]
        public ActionResult<CommandReadDto> GetCommandByID(int id)
        {
            var result = _repo.GetCommandById(id);
            if (result != null)
            {
                return Ok(_mapper.Map<CommandReadDto>(result));
            }

            return NotFound();


        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand(CommandCreateDto commandCreate)
        {
            var commadModel = _mapper.Map<Command>(commandCreate);
            _repo.CreateCommand(commadModel);
            _repo.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(commadModel);

            //return Ok(commadModel);

            return CreatedAtRoute(nameof(GetCommandByID), new { Id = commandReadDto.Id }, commandReadDto);


        }

        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id,CommandUpdateDto commandUpdateDto)
        {
            var commandModelFromRepo = _repo.GetCommandById(id);
            if(commandModelFromRepo==null)
            {
                return NotFound();
            }

            _mapper.Map(commandUpdateDto, commandModelFromRepo);

            _repo.UpdateCommand(commandModelFromRepo);
            _repo.SaveChanges();

            return NoContent();

        }

        [HttpPatch("{id}")]

        public ActionResult PartialCommandUpdate(int id,JsonPatchDocument<CommandUpdateDto> jsonPatchDocument )
        {
            var commandModelFromRepo = _repo.GetCommandById(id);
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }

            var commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelFromRepo);

            jsonPatchDocument.ApplyTo(commandToPatch, ModelState);

            if(!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(commandToPatch, commandModelFromRepo);

            _repo.UpdateCommand(commandModelFromRepo);
            _repo.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult RemoveCommand(int id)
        {
            var commandModelFromRepo = _repo.GetCommandById(id);
            if (commandModelFromRepo == null)
            {
                return NotFound();
            }

            _repo.DeleteCommand(commandModelFromRepo);
            _repo.SaveChanges();

            return NoContent();
        }



    }
}
