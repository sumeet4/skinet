using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Core.Interfaces;
using Core.Specification;
using API.DTOs;
using AutoMapper;
using API.Errors;
using API.Helpers;

namespace API.Controllers
{

    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _prodductsRepo;
        private readonly IGenericRepository<ProductType> _prodductTpeRepo;
        private readonly IGenericRepository<ProductBrand> _prodductBrandRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> prodductsRepo, IGenericRepository<ProductType> prodductTpeRepo,
        IGenericRepository<ProductBrand> prodductBrandRepo, IMapper mapper)
        {
            _mapper = mapper;
            _prodductBrandRepo = prodductBrandRepo;
            _prodductTpeRepo = prodductTpeRepo;
            _prodductsRepo = prodductsRepo;


        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams productParams)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(productParams);
            var countSpec = new ProductWithFiltersForCountSpecification(productParams);

            var totalItems = await _prodductsRepo.CountAsyn(countSpec);
            var products = await _prodductsRepo.ListAsync(spec);

            var data = _mapper.Map
                      < IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex,productParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);

            var product = await _prodductsRepo.GetEntityWithSpec(spec);

            if (product==null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Product, ProductToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _prodductBrandRepo.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _prodductTpeRepo.ListAllAsync());
        }

    }
}