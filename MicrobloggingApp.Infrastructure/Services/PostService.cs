using MicrobloggingApp.Infrastructure.DTOs;
using MicrobloggingApp.Infrastructure.Interfacses;
using MicrobloggingApp.Infrastructure.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace MicrobloggingApp.Infrastructure.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IAzureBlobService _blobService;
        private readonly IQueueService _queueService;
        private readonly ILogger<PostService> _logger;

        public PostService(IPostRepository postRepository,
            IAzureBlobService blobService,
            IQueueService queueService,
            ILogger<PostService> logger)
        {
            _postRepository = postRepository;
            _blobService = blobService;
            _queueService = queueService;
            _logger = logger;
        }

        public async Task<Guid> CreatePostAsync(PostDto postDto, Guid userid)
        {
            try
            {
                var post = new Post
                {
                    Text = postDto.Text,
                    UserId = userid,
                    CreatedAt = DateTime.UtcNow,
                    Latitude = GetRandomCoordinate(),
                    Longitude = GetRandomCoordinate()
                };

                if (postDto.Image != null)
                {
                    var imageUrl = await _blobService.UploadImageAsync(postDto.Image, Guid.NewGuid().ToString());
                    post.OriginalImageUrl = imageUrl;

                    // Enqueue for background processing
                    await _queueService.SendMessageAsync(JsonConvert.SerializeObject(new
                    {
                        PostId = post.Id,
                        OriginalImageUrl = imageUrl
                    }));
                }

                var _post = await _postRepository.CreatePostAsync(post);
                return _post.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating posts.");
                throw;
            }
        }

        public async Task<IEnumerable<Post>> GetAllPostsAsync(Guid userId)
        {
            try
            {
                return await _postRepository.GetAllPostsAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting posts.");
                throw;
            }
        }

        private double GetRandomCoordinate()
        {
            Random rnd = new Random();
            return rnd.NextDouble() * 180.0 - 90.0;
        }
    }
}
