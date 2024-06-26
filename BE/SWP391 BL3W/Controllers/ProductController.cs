﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWP391_BL3W.DTO.Request;
using SWP391_BL3W.Services.Interface;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authorization;
using SWP391_BL3W.Database;

namespace SWP391_BL3W.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }
        [HttpGet("getAll")]
        public async Task<IActionResult> getAll(int? size, int? page)
        {
            var response = await _productService.getAll(size, page);
            return StatusCode((int)response.statusCode, new { data = response.Data, message = response.Errormessge });
        }

        [HttpGet("getDetailsById/{id}")]
        public async Task<IActionResult> getProductsDetailsById([FromRoute] int id)
        {
            var response = await _productService.getProductDetailbyId(id);
            return StatusCode((int)response.statusCode, new { data = response.Data, message = response.Errormessge });
        }

        [HttpGet("search")]
        public async Task<IActionResult> search(int? page, int? size, string? name, int? categoryId, int? watt, int? volt, string? producer)
        {
            var response = await _productService.search(page, size, name, categoryId, watt, volt, producer);
            return StatusCode((int)response.statusCode, new { data = response.Data, message = response.Errormessge });
        }

        [HttpGet("searchbyName/{name}")]
        public async Task<IActionResult> searchByName(int? page, int? size, [FromRoute] string name)
        {
            var response = await _productService.search(page, size, name);
            return StatusCode((int)response.statusCode, new { data = response.Data, message = response.Errormessge });
        }

        [Authorize(Roles = "1 , 3")]
        [HttpPost("create")]
        public async Task<IActionResult> create(CreateProductDTO dto)
        {
            var response = await _productService.create(dto);
            return StatusCode((int)response.statusCode, new { data = response.Data, message = response.Errormessge });
        }

        [Authorize(Roles = "1 , 3")]
        [HttpPut("update")]
        public async Task<IActionResult> update(UpdateProductsDTO dto)
        {
            var response = await _productService.updateProduct(dto);
            return StatusCode((int)response.statusCode, new { data = response.Data, message = response.Errormessge });
        }
    }
}
