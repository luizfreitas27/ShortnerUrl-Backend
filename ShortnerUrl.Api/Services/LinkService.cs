using Mapster;
using MapsterMapper;
using ShortnerUrl.Api.Dtos.Link.Request;
using ShortnerUrl.Api.Dtos.Link.Response;
using ShortnerUrl.Api.Exeptions;
using ShortnerUrl.Api.Models;
using ShortnerUrl.Api.Shared;
using ShortnerUrl.Api.Shared.Repositories;

namespace ShortnerUrl.Api.Services;

public class LinkService : ILinkService
{
    private readonly ILogger<LinkService> _logger;
    private readonly IUnityOfWork _unityOfWork;
    private IMapper _mapper;

    public LinkService(ILogger<LinkService> logger, IUnityOfWork unityOfWork, IMapper mapper)
    {
        _logger = logger;
        _unityOfWork = unityOfWork;
        _mapper = mapper;
    }

    public async Task<LinkResponseDto> CreateAsync(int userId, LinkCreateRequestDto dto, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Starting creating link...");
        
        var shortner = Utils.Utils.GenerateRandom();

        while (await _unityOfWork.Links.GetLinkByShortner(shortner, cancellationToken) != null)
        {
            shortner = Utils.Utils.GenerateRandom();
        }
        
        var link = _mapper.From(dto).AdaptToType<Link>();

        link.UserId = userId;
        link.Shortner = shortner;
        
        _unityOfWork.Links.Add(link);
        await _unityOfWork.CommitAsync(cancellationToken);
        
        return _mapper.From(link).AdaptToType<LinkResponseDto>();
    }

    public async Task<LinkResponseDto> GetByIdAsync(Guid id, int userId, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Starting getting link...");

        var link = await _unityOfWork.Links
            .GetLinkByIdAsync(id, userId, cancellationToken);

        if (link == null)
        {
            _logger.LogWarning("Link not found...");
            throw new AppException("Link not found...", 404);
        }
        
        _logger.LogInformation($"Finished getting link...");
        return _mapper.From(link).AdaptToType<LinkResponseDto>();
    }

    public async Task<List<LinkResponseDto>> GetAllAsync(int userId, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Starting getting all links...");

        var links = await _unityOfWork.Links.GetAllLinksAsync(userId ,cancellationToken);
        var list = links?.ToList() ?? [];

        if (list.Count == 0) 
        {
            _logger.LogWarning("No Links Created...");
            throw new AppException("No Links Created...", 404);
        }
        
        _logger.LogInformation($"Finished getting all links...");
        return _mapper.From(list).AdaptToType<List<LinkResponseDto>>();
    }

    public async Task<LinkResponseDto> UpdateAsync(Guid id, int userId, LinkUpdateRequestDto dto, CancellationToken cancellationToken)
    {
       _logger.LogInformation("Starting updating link..."); 
       
       var link = await _unityOfWork.Links.GetLinkByIdAsync(id, userId, cancellationToken);
       if (link == null)
       {
           _logger.LogWarning("Link not found...");
           throw new AppException("Link not found...", 404);
       }

       _mapper.From(dto).AdaptTo(link);
       
       _unityOfWork.Links.Update(link);
       
       await _unityOfWork.CommitAsync(cancellationToken);
       
       _logger.LogInformation($"Finished updating link...");
       
       return _mapper.From(link).AdaptToType<LinkResponseDto>();
        
    }

    public async Task DeleteAsync(Guid id, int userId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting deleting link...");

        var link = await _unityOfWork.Links.GetLinkByIdAsync(id, userId, cancellationToken);
        
        if (link == null)
        {
            _logger.LogWarning("Link not found...");
            throw new AppException("Link not found...", 404);
        }
        
        _unityOfWork.Links.Delete(link);
        await _unityOfWork.CommitAsync(cancellationToken);
        
        _logger.LogInformation("Finished deleting link...");
    }
}