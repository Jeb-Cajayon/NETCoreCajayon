using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Helpers;
using SampleWebApiAspNetCore.Services;
using SampleWebApiAspNetCore.Models;
using SampleWebApiAspNetCore.Repositories;
using System.Text.Json;

namespace SampleWebApiAspNetCore.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GachaController : ControllerBase
    {
        private readonly IGachaRepository _rngRepository;
        private readonly IMapper _mapper;
        private readonly ILinkService<GachaController> _linkService;

        public GachaController(
            IGachaRepository rngRepository,
            IMapper mapper,
            ILinkService<GachaController> linkService)
        {
            _rngRepository = rngRepository;
            _mapper = mapper;
            _linkService = linkService;
        }

        [HttpGet(Name = nameof(GetAllGacha))]
        public ActionResult GetAllGacha(ApiVersion version, [FromQuery] QueryParameters queryParameters)
        {
            List<GachaEntity> rngItems = _rngRepository.GetAll(queryParameters).ToList();

            var allItemCount = _rngRepository.Count();

            var paginationMetadata = new
            {
                totalCount = allItemCount,
                pageSize = queryParameters.PageCount,
                currentPage = queryParameters.Page,
                totalPages = queryParameters.GetTotalPages(allItemCount)
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var links = _linkService.CreateLinksForCollection(queryParameters, allItemCount, version);
            var toReturn = rngItems.Select(x => _linkService.ExpandSingleGachaItem(x, x.Id, version));

            return Ok(new
            {
                value = toReturn,
                links = links
            });
        }

        [HttpGet]
        [Route("{id:int}", Name = nameof(GetSingleGacha))]
        public ActionResult GetSingleGacha(ApiVersion version, int id)
        {
            GachaEntity rngItem = _rngRepository.GetSingle(id);

            if (rngItem == null)
            {
                return NotFound();
            }

            GachaDto item = _mapper.Map<GachaDto>(rngItem);

            return Ok(_linkService.ExpandSingleGachaItem(item, item.Id, version));
        }

        [HttpPost(Name = nameof(AddGacha))]
        public ActionResult<GachaDto> AddGacha(ApiVersion version, [FromBody] GachaCreateDto rngCreateDto)
        {
            if (rngCreateDto == null)
            {
                return BadRequest();
            }

            GachaEntity toAdd = _mapper.Map<GachaEntity>(rngCreateDto);

            _rngRepository.Add(toAdd);

            if (!_rngRepository.Save())
            {
                throw new Exception("Creating a rngitem failed on save.");
            }

            GachaEntity newrngItem = _rngRepository.GetSingle(toAdd.Id);
            GachaDto GachaDto = _mapper.Map<GachaDto>(newrngItem);

            return CreatedAtRoute(nameof(GetSingleGacha),
                new { version = version.ToString(), id = newrngItem.Id },
                _linkService.ExpandSingleGachaItem(GachaDto, GachaDto.Id, version));
        }

        [HttpPatch("{id:int}", Name = nameof(PartiallyUpdateGacha))]
        public ActionResult<GachaDto> PartiallyUpdateGacha(ApiVersion version, int id, [FromBody] JsonPatchDocument<GachaUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            GachaEntity existingEntity = _rngRepository.GetSingle(id);

            if (existingEntity == null)
            {
                return NotFound();
            }

            GachaUpdateDto GachaUpdateDto = _mapper.Map<GachaUpdateDto>(existingEntity);
            patchDoc.ApplyTo(GachaUpdateDto);

            TryValidateModel(GachaUpdateDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(GachaUpdateDto, existingEntity);
            GachaEntity updated = _rngRepository.Update(id, existingEntity);

            if (!_rngRepository.Save())
            {
                throw new Exception("Updating a rngitem failed on save.");
            }

            GachaDto GachaDto = _mapper.Map<GachaDto>(updated);

            return Ok(_linkService.ExpandSingleGachaItem(GachaDto, GachaDto.Id, version));
        }

        [HttpDelete]
        [Route("{id:int}", Name = nameof(RemoveGacha))]
        public ActionResult RemoveGacha(int id)
        {
            GachaEntity rngItem = _rngRepository.GetSingle(id);

            if (rngItem == null)
            {
                return NotFound();
            }

            _rngRepository.Delete(id);

            if (!_rngRepository.Save())
            {
                throw new Exception("Deleting a rngitem failed on save.");
            }

            return NoContent();
        }

        [HttpPut]
        [Route("{id:int}", Name = nameof(UpdateGacha))]
        public ActionResult<GachaDto> UpdateGacha(ApiVersion version, int id, [FromBody] GachaUpdateDto GachaUpdateDto)
        {
            if (GachaUpdateDto == null)
            {
                return BadRequest();
            }

            var existingrngItem = _rngRepository.GetSingle(id);

            if (existingrngItem == null)
            {
                return NotFound();
            }

            _mapper.Map(GachaUpdateDto, existingrngItem);

            _rngRepository.Update(id, existingrngItem);

            if (!_rngRepository.Save())
            {
                throw new Exception("Updating a rngitem failed on save.");
            }

            GachaDto GachaDto = _mapper.Map<GachaDto>(existingrngItem);

            return Ok(_linkService.ExpandSingleGachaItem(GachaDto, GachaDto.Id, version));
        }

        [HttpGet("GetRandomNum", Name = nameof(GetRandomNum))]
        public ActionResult GetRandomNum()
        {
            ICollection<GachaEntity> rngItems = _rngRepository.GetRandomNum();

            IEnumerable<GachaDto> dtos = rngItems.Select(x => _mapper.Map<GachaDto>(x));

            var links = new List<LinkDto>();

            // self 
            links.Add(new LinkDto(Url.Link(nameof(GetRandomNum), null), "self", "GET"));

            return Ok(new
            {
                value = dtos,
                links = links
            });
        }
    }
}
