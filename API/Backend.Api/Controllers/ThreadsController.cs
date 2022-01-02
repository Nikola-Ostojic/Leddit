using Backend.Api.DTOs;
using Backend.Api.DTOs.Request;
using Backend.Api.DTOs.Response;
using Backend.Api.Mappers;
using Backend.Core.Interfaces;
using Backend.DAL.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ThreadsController : ControllerBase
    {
        private readonly IUserService _usersService;
        private readonly IThreadService _threadsService;
        private readonly ICommentService _commentService;
        private readonly ILogger<ThreadsController> _logger;
        private readonly IMapper<ThreadEntity, ThreadResponseDTO> _threadMapper;

        public ThreadsController(
            IUserService usersService,
            ILogger<ThreadsController> logger,
            IThreadService threadService,
            ICommentService commentService,
            IMapper<ThreadEntity, ThreadResponseDTO> threadMapper)
        {
            _usersService = usersService;
            _logger = logger;
            _threadsService = threadService;
            _commentService = commentService;
            _threadMapper = threadMapper;
        }

        [HttpGet]
        public async Task<ActionResult<PageableDTO<ThreadResponseDTO>>> GetThreads(string searchCriteria, int page = 1, int itemsPerPage = 25)
        {
            _logger.LogInformation("GetThreads");
            var result = await _threadsService.GetThreads(searchCriteria, page, itemsPerPage);

            var mappedResult = _threadMapper.ToPageableDto(result);

            foreach (var item in mappedResult.Data)
            {
                item.CommentsCount = await _commentService.GetCommentsCount(item.Id);
            }
            return mappedResult;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ThreadResponseDTO>> Get(int id)
        {
            var thread = await _threadsService.Get(id);
            if (thread == null)
            {
                return NotFound($"Thread with the id of {id} doesn't exist");
            }

            var threadDto = _threadMapper.ToDto(thread);
            threadDto.CommentsCount = await _commentService.GetCommentsCount(id);
            return threadDto;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ThreadResponseDTO>> Create([FromBody] ThreadRequestDTO threadRequest)
        {
            var createdThread = await _threadsService.Create(new ThreadEntity
            {
                Title = threadRequest.Title,
                Content = threadRequest.Content,
                Comments = new List<CommentEntity>(),
                Author = await GettingCurrentClient(),
                
            });
            return Created($"threads/{createdThread.Id}", _threadMapper.ToDto(createdThread));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<ThreadResponseDTO>> Update(int id, [FromBody] ThreadRequestDTO threadRequest)
        {
            var currentUser = await GettingCurrentClient();
            var threadInQuestion = await _threadsService.Get(threadRequest.Id);

            if (threadInQuestion == null)
            {
                return NotFound($"Couldn't find a thread with the id of {id} to update");
            }

            if (currentUser.Email != threadInQuestion.Author.Email)
            {
                return Forbid();
            }

            var updatedThread = await _threadsService.Update(new ThreadEntity
            {
                Id = id,
                Title = threadRequest.Title,
                Content = threadRequest.Content,
                Comments = new List<CommentEntity>(),
                Author = currentUser
            });
            return _threadMapper.ToDto(updatedThread);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            var currentUser = await GettingCurrentClient();
            var threadToDelete = await _threadsService.Get(id);

            if (threadToDelete == null)
            {
                return NotFound($"Couldn't find a thread with the id of {id} to delete");
            }

            if (currentUser.Id != threadToDelete.Author.Id)
            {
                if (currentUser.Role != Role.Admin)
                {
                    return Forbid();
                }
            }

            await _threadsService.Delete(id);
            return NoContent();
        }

        private async Task<UserEntity> GettingCurrentClient()
        {
            var accessToken = Request.Headers["Authorization"].ToString().Split(' ')[1];
            var decodedToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            var email = decodedToken.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;
            var user = await _usersService.GetUserByEmail(email);
            return user;
        }
    }
}
