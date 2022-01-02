using Backend.Api.DTOs;
using Backend.Api.DTOs.Request;
using Backend.Api.DTOs.Response;
using Backend.Api.Mappers;
using Backend.Core.Interfaces;
using Backend.DAL.Entities;
using Backend.DAL.Interfaces;
using Backend.DAL.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly IUserService _usersService;
        private readonly ICommentService _commentService;
        private readonly ILogger<CommentsController> _logger;
        private readonly IMapper<CommentEntity, CommentResponseDTO> _commentMapper;
        private readonly IThreadService _threadService;

        public CommentsController(
            ILogger<CommentsController> logger,
            ICommentService commentService,
            IMapper<CommentEntity, CommentResponseDTO> commentMapper,
            IUserService _userService,
            IThreadService threadService)
        {
            _usersService = _userService;
            _logger = logger;
            _commentService = commentService;
            _commentMapper = commentMapper;
            _threadService = threadService;
        }

        [HttpGet]
        public async Task<ActionResult<PageableDTO<CommentResponseDTO>>> GetComments(int threadId, int page, int itemsPerPage)
        {
            if (threadId == 0)
            {
                return BadRequest();
            }

            _logger.LogInformation("GetComments");
            var result = await _commentService.GetComments(threadId, page, itemsPerPage);
            var mappedResult = _commentMapper.ToPageableDto(result);
            return mappedResult;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CommentResponseDTO>> Get(int id)
        {
            var comment = await _commentService.Get(id);
            if (comment == null)
            {
                return NotFound($"Comment with the id of {id} doesn't exist");
            }

            return _commentMapper.ToDto(comment);
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult<CommentResponseDTO>> Create([FromBody] CommentRequestDTO commentRequest)
        {
            var thread = await _threadService.GetWithoutUser(commentRequest.ThreadId);
            if (thread == null)
            {
                return BadRequest($"Thread with {commentRequest.ThreadId} does not exist.");
            }

            var user = await GettingCurrentClient();

            var createdComment = await _commentService.Create(new CommentEntity
            {
                Thread = thread,
                Content = commentRequest.Content,
                Author = user
            });

            return Created($"comments/{createdComment.Id}", _commentMapper.ToDto(createdComment));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<CommentResponseDTO>> Update(int id, [FromBody] CommentRequestDTO commentRequest)
        {
            var currentUser = await GettingCurrentClient();
            var commentInQuestion = await _commentService.Get(commentRequest.Id);

            if (commentInQuestion == null)
            {
                return NotFound($"Couldn't find a comment with the id of {id} to update");
            }

            var thread = await _threadService.GetWithoutUser(commentRequest.ThreadId);

            if (thread == null)
            {
                return BadRequest($"Thread with {commentRequest.ThreadId} does not exist.");
            }

            if (currentUser.Email != commentInQuestion.Author.Email)
            {
                return Forbid();
            }

            var updatedComment = await _commentService.Update(new CommentEntity
            {
                Id = commentInQuestion.Id,
                Content = commentRequest.Content,
                Author = currentUser,
                Thread = thread
            });
            return _commentMapper.ToDto(updatedComment);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            var currentUser = await GettingCurrentClient();
            var commentToDelete = await _commentService.Get(id);

            if (commentToDelete == null)
            {
                return NotFound($"Couldn't find a comment with the id of {id} to delete");
            }

            if (currentUser.Email != commentToDelete.Author.Email)
            {
                if (currentUser.Role != Role.Admin)
                {
                    return Forbid();
                }
            }

            await _commentService.Delete(id);
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
